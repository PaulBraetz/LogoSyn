using System.Reflection;
using System.Runtime.Loader;
using System.Text;

using Fort;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Emit;

using RhoMicro.LogoSyn.Apps.Common;
using RhoMicro.LogoSyn.Libs.Dom.Dom.Abstractions;
using RhoMicro.LogoSyn.Libs.Interpreter.Abstractions;

using Scli;

namespace RhoMicro.LogoSyn.Apps.Interpreter.CSharpInterpreter;

/// <summary>
/// A C#-based interpreter, able to interpret simplified documents..
/// </summary>
internal sealed class Interpreter
{
	public static IInterpreter Create(IArgumentCollection arguments)
	{
		var enableDump = arguments.TryGet("d", out var _);

		var result = Interpreter<Discriminators.Default>.Create(s => Interpret(s, enableDump));

		return result;
	}

	private static readonly String _generatedAssemblyName = $"Document_{Guid.NewGuid()}";
	private static readonly String _dumpFileName = $"{_generatedAssemblyName}_dump.cs";

	#region Template
	private const String PRINT_LOCAL = "print";
	private const String PRINT_PRIVATE = "Print";
	private const String NAMESPACE = "MyDocument";
	private const String TYPE = "Document";
	private const String EXECUTE_NAME = "Execute";
	#region Context
	private const String CONTEXT_TYPE = "Context";
	private const String CONTEXT_NAME = "DocumentContext";
	private const String TARGET_FILE_PATH_NAME = "TargetFilePath";
	private const String INTERMEDIATE_FILE_PATH_NAME = "IntermediateResultFilePath";
	#endregion
	private const String FORMAT_0 =
@$"using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace {NAMESPACE}
{{
	public sealed class {TYPE}
	{{
		public sealed class {CONTEXT_TYPE}
		{{
			public {CONTEXT_TYPE}()
			{{

			}}

			public String {TARGET_FILE_PATH_NAME} {{ get; set; }}

			public String {INTERMEDIATE_FILE_PATH_NAME} {{ get; set; }}
		}}

		public Document(Action<String> {PRINT_LOCAL})
		{{
			if({PRINT_LOCAL} == null)
			{{
				throw new ArgumentNullException(""{PRINT_LOCAL}"");
			}}

			{PRINT_PRIVATE} = o => {PRINT_LOCAL}.Invoke(o?.ToString() ?? String.Empty); 
		}}

		private readonly Action<Object> {PRINT_PRIVATE};
		public {CONTEXT_TYPE} {CONTEXT_NAME} {{ get; }} = new {CONTEXT_TYPE}();

		public void {EXECUTE_NAME}()
		{{
";

	private const String FORMAT_1 =
@"
		}
	}
}";
	#endregion

	private static void Interpret(IDom<Discriminators.Default> dom, Boolean enableDump)
	{
		dom.ThrowIfDefault(nameof(dom));

		var syntaxTree = BuildSyntaxTree(dom);

		CompileAndExecute(syntaxTree, enableDump);
	}

	private static String BuildSyntaxTree(IDom<Discriminators.Default> dom)
	{
		var builder = new StringBuilder(FORMAT_0);
		var line = 1;

		foreach(var element in dom)
		{
			var slice = element.Slice.ToString() ?? String.Empty;

			switch(element.Kind)
			{
				case Discriminators.Default.Literal:
					//TODO: strengthen escape
					append($"{PRINT_PRIVATE}.Invoke(@\"{slice.Replace(@"""", @"""""")}\");");

					break;
				case Discriminators.Default.Code:
					append(slice);

					break;
				case Discriminators.Default.Display:
					var expression = element.Slice.ToString();
					if(!String.IsNullOrWhiteSpace(expression))
					{
						append($"{PRINT_PRIVATE}.Invoke({expression});");
					}

					break;
			}

			line += slice.Count(c => c == '\n');

			void append(String text)
			{
				_ = builder!.Append($"#line {line} \"Intermediate\"\r\n")
					.Append(text)
					.Append($"\r\n#line default\r\n#line hidden\r\n");
			}
		}

		var result = builder.Append(FORMAT_1).ToString();

		return result;
	}

	private static EmitResult Compile(String syntaxTreeText, Stream peStream)
	{
		var syntaxTree = SyntaxFactory.ParseSyntaxTree(syntaxTreeText);

		var basePath = Path.GetDirectoryName(typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly.Location)!;
		var root = syntaxTree.GetRoot() as CompilationUnitSyntax;
		var references = root!.Usings;
		var referencePaths = new List<String>
		{
			typeof(Object).GetTypeInfo().Assembly.Location,
			typeof(Console).GetTypeInfo().Assembly.Location,
			Path.Combine(basePath, "System.Runtime.dll"),
			Path.Combine(basePath, "System.Runtime.Extensions.dll"),
			Path.Combine(basePath, "mscorlib.dll")
		};
		referencePaths.AddRange(references.Select(x => Path.Combine(basePath, $"{x.Name}.dll")));
		var executableReferences = new List<PortableExecutableReference>();
		foreach(var reference in referencePaths)
		{
			if(File.Exists(reference))
			{
				executableReferences.Add(MetadataReference.CreateFromFile(reference));
			}
		}

		var compilation = CSharpCompilation.Create(
			_generatedAssemblyName,
			new[] { syntaxTree },
			executableReferences,
			new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, optimizationLevel: OptimizationLevel.Release));

		var result = compilation.Emit(peStream);

		return result;
	}

	private static void CompileAndExecute(String syntaxTreeText, Boolean enableDump)
	{
		using var peStream = new MemoryStream();
		var result = Compile(syntaxTreeText, peStream);

		if(result.Success)
		{
			_ = peStream.Seek(0, SeekOrigin.Begin);

			var assemblyLoadContext = AssemblyLoadContext.Default;
			var assembly = assemblyLoadContext.LoadFromStream(peStream);

			var documentType = assembly!.DefinedTypes.Single(t => t.Name == TYPE);

			var targetBuilder = new StringBuilder();
			Object? document = null;
			Action<String> print = s => targetBuilder.Append(s);
			document = Activator.CreateInstance(documentType, print);
			var executeMethod = documentType.GetMethod(EXECUTE_NAME)!;
			_ = executeMethod.Invoke(document, Array.Empty<Object>());

			var context = documentType
				.GetProperty(CONTEXT_NAME)!
				.GetGetMethod()!
				.Invoke(document, Array.Empty<Object>());
			var contextType = assembly!.DefinedTypes.Single(t => t.Name == CONTEXT_TYPE);

			var targetFilePath = contextType
				.GetProperty(TARGET_FILE_PATH_NAME)!
				.GetGetMethod()!
				.Invoke(context, Array.Empty<Object>())?
				.ToString();

			if(targetFilePath != null)
			{
				File.Delete(targetFilePath);
				using var targetFile = File.CreateText(targetFilePath);
				targetFile.Write(targetBuilder.ToString());
			}

			var intermediateFilePath = contextType
				.GetProperty(INTERMEDIATE_FILE_PATH_NAME)!
				.GetGetMethod()!
				.Invoke(context, Array.Empty<Object>())?
				.ToString();

			if(intermediateFilePath != null)
			{
				File.Delete(intermediateFilePath);
				using var targetFile = File.CreateText(intermediateFilePath);
				targetFile.Write(syntaxTreeText);
			}
		} else
		{
			if(enableDump)
			{
				using var dump = File.CreateText(_dumpFileName);
				dump.Write(syntaxTreeText);
			}

			var message = String.Join("\n", result.Diagnostics);
			throw new Exception(message);
		}
	}
}
