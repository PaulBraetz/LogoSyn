using RhoMicro.LogoSyn.Apps.LogoSyn.Common;
using RhoMicro.LogoSyn.Apps.LogoSyn.Watcher.Visitors;
using Scli;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Watcher
{
	internal class Program
	{
		static async Task Main(String[] args)
		{
			if (args.Length != 1)
			{
				Console.WriteLine("Incorrect number of arguments supplied, only the source path is expected.");
				return;
			}

			var path = args[0];

			await ApplicationBuilder.Create()
				.AddCompilationContext(new[] { "-c" })
				.AddContextVisitor(new CompilationVisitor(path))
				.Build()
				.RunAsync(CancellationToken.None);
		}
	}
}