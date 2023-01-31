using Scli;

namespace RhoMicro.LogoSyn.Apps.Interpreter.CSharpInterpreter;

internal class Program
{
	static Program()
	{
		_parameters = Initialization.GetParameters();
		_ = _parameters.TryAdd("s", "source", "Supplies the source document path.", File.Exists);
		_ = _parameters.TryAdd("d", "dump", "When set, forces the interpreter to dump the contents of the intermediate file on compilation errors.", s => s == null);
	}

	private static readonly IParameterCollection _parameters;

	private static void Main(String[] args)
	{
		var arguments = _parameters.MatchArguments(args);

		using var source = arguments.TryGet("s", s => new FileInfo(s!), out var sourceFile) ?
			sourceFile!.OpenRead() :
			Console.OpenStandardInput();
		Interpreter.Create(arguments).Interpret(source);
	}
}