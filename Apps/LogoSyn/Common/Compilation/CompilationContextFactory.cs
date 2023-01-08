using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;
using Scli;
using System.Reflection;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common.Compilation
{
	internal sealed class CompilationContextFactory : ApplicationContextFactoryBase
	{
		private const String DEFAULT_MANIFEST_FILE_NAME = "Manifest";
		private const String DEFAULT_MANIFEST_PATH = "Packages";

		static CompilationContextFactory()
		{
			_parameters = Initialization.GetParameters();
			_parameters.TryAdd("c", "compile", "When set, source documents may be compiled.", s => s == null);

			_parameters.TryAdd("cs", "compileSource", "Supplies the .ls source file path", File.Exists);
			_parameters.TryAdd("cm", "compileManifests",
				"Supplies a comma-delimited list of local manifest files. " +
				$"If none are provided, an attempt will be made to locate a file at \"{DEFAULT_MANIFEST_PATH}\" " +
				$"in the executing directory and named \"{DEFAULT_MANIFEST_FILE_NAME}\" and use it.", s => s != null && s.Split(',').All(File.Exists));
			_parameters.TryAdd("ct", "compileTarget", "Supplies the target file to which to write the compilation result.", s => !String.IsNullOrEmpty(s));
			_parameters.TryAdd("ce", "compileError", "Supplies the file to which to write error details, should any arise.", s => !String.IsNullOrEmpty(s));
		}

		public CompilationContextFactory(String[] args) : base(_parameters, args)
		{
		}

		private static readonly IParameterCollection _parameters;

		protected override Boolean CanCreate(IArgumentCollection arguments)
		{
			var result = arguments.TryGet("c", out var _);

			return result;
		}
		protected override IApplicationContext CreateContext(IArgumentCollection arguments)
		{
			var helpInfo = _parameters.ToString();
			var result = new CompilationContext(helpInfo!);

			Seed(result, arguments);

			return result;
		}

		private static void Seed(ICompilationContext context, IArgumentCollection arguments)
		{
			if (arguments.TryGet("cs", out var sourcePath))
			{
				var source = File.OpenRead(sourcePath!.Value!);
				context.SetDocument(source);
			}

			if (arguments.TryGet("cm", s => s!.Split(','), out var manifests))
			{
				foreach (var manifest in manifests!)
				{
					context.AddLocalPackageLoader(manifest);
				}
			}
			else if (TryGetDefaultManifest(out var defaultManifest))
			{
				context.AddLocalPackageLoader(defaultManifest!);
			}

			if (arguments.TryGet("ct", s => new FileInfo(s!), out var target))
			{
				target!.Delete();
				var standardOutput = target.Create();
				context.SetStandardOutput(standardOutput);
			}

			if (arguments.TryGet("ce", s => new FileInfo(s!), out var errors))
			{
				errors!.Delete();
				var standardError = errors!.Create();
				context.SetStandardError(standardError);
			}
		}

		private static Boolean TryGetDefaultManifest(out String? manifestPath)
		{
			manifestPath = null;

			var assemblyLocation = Assembly.GetEntryAssembly()?.Location;
			if (assemblyLocation == null)
			{
				return false;
			}

			var assemblyDirectoryPath = new FileInfo(assemblyLocation).Directory?.FullName;
			if (assemblyDirectoryPath == null)
			{
				return false;
			}

			var manifestDirectoryPath = Path.Combine(assemblyDirectoryPath, DEFAULT_MANIFEST_PATH);
			var manifestDirectory = new DirectoryInfo(manifestDirectoryPath);
			if (!manifestDirectory.Exists)
			{
				return false;
			}

			manifestPath = manifestDirectory.EnumerateFiles()
				.OrderByDescending(f => f.CreationTime)
				.FirstOrDefault(f => Path.GetFileNameWithoutExtension(f.Name) == DEFAULT_MANIFEST_FILE_NAME)?
				.FullName;

			return manifestPath != null;
		}
	}
}
