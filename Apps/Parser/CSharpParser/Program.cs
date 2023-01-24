using Scli;

namespace RhoMicro.LogoSyn.Apps.Parser.CSharpParser;

internal class Program
{
	static Program()
	{
		_parameters = Initialization.GetParameters();
		_ = _parameters.TryAdd("o", "offset", "Supplies the number of offset lines to add before the document.", s => Int32.TryParse(s, out _));
		_ = _parameters.TryAdd("s", "source", "Supplies the source document path.", File.Exists);
		_ = _parameters.TryAdd("t", "target", "Supplies the target file to which to write the parsed document.", s => !String.IsNullOrWhiteSpace(s));
	}

	private static readonly IParameterCollection _parameters;

	public static void Main(String[] args)
	{
		var arguments = _parameters.MatchArguments(args);
		_ = arguments.TryGet("o", Int32.Parse!, out var offset);
		using var source = arguments.TryGet("s", s => new FileInfo(s!), out var sourceFile) ?
			sourceFile!.OpenRead() :
			Console.OpenStandardInput();
		using var target = arguments.TryGet("t", t => new FileInfo(t!), out var targetFile) ?
			targetFile!.Create() :
			Console.OpenStandardOutput();

		Parser.Create(offset).Parse(source, target);
	}
}
