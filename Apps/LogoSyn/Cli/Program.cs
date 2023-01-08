using RhoMicro.LogoSyn.Apps.LogoSyn.Cli.Menus;
using RhoMicro.LogoSyn.Apps.LogoSyn.Cli.Visitors;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common;
using Scli;
using System.Diagnostics;
using System.Reflection;

namespace RhoMicro.LogoSyn.Apps.LogoSyn
{
	internal partial class Program
	{

		static async Task Main(String[] args)
		{
			var packagingArgs = args;
			var compilationArgs = args;

			var visitorParameters = Initialization.GetParameters();
			visitorParameters.TryAdd("h", "help", "When set, ignores all other arguments and displays help instead", s => s == null);
			visitorParameters.TryAdd("c", "compile", validator: s => s == null);
			visitorParameters.TryAdd("p", "package", validator: s => s == null);

			var visitorArguments = visitorParameters.MatchArguments(Array.Empty<String>());
			if (args.Length == 0 || args.Length == 1 && args[0] == "-h")
			{
				packagingArgs = new[] { "-p" };
				compilationArgs = new[] { "-c" };
				visitorArguments = visitorParameters.MatchArguments(new[] { "-c", "-p", "-h" });
			}
			else if (args.Length == 1 && !args[0].StartsWith("-"))
			{
				packagingArgs = Array.Empty<String>();
				compilationArgs = new[] { "-c", "-cs", args[0] };
			}

			await ApplicationBuilder.Create()
				.AddPackagingContext(packagingArgs)
				.AddCompilationContext(compilationArgs)
				.AddContextVisitor(new HelpVisitor(visitorArguments))
				.AddContextVisitor(new PackagingVisitor(visitorArguments))
				.AddContextVisitor(new CompilationVisitor(visitorArguments))
				.Build()
				.RunAsync(CancellationToken.None);
		}
	}
}