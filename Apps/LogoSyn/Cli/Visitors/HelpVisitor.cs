using Fort;
using RhoMicro.Common.System.Abstractions;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;
using Scli;
using System.Linq;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Cli.Visitors
{
	internal sealed class HelpVisitor : VisitorBase<IApplicationContext>
	{
		private readonly Boolean _canReceive;

		public HelpVisitor(IArgumentCollection arguments)
		{
			arguments.ThrowIfDefault(nameof(arguments));

			_canReceive = arguments.TryGet("h", out var _);
		}

		protected override Boolean CanReceive(IApplicationContext obj)
		{
			return _canReceive;
		}

		protected override void Receive(IApplicationContext obj)
		{
			var help = obj.GetHelpInfo();
			var helpTitle = $"|Help info for {obj.GetType().Name}:";
			var stars = String.Concat(Enumerable.Range(0, helpTitle.Length).Select(i => '-'));
			Console.WriteLine(stars);
			Console.WriteLine(helpTitle);
			Console.WriteLine(stars);
			Console.Write("|\t");
			Console.WriteLine(help.Replace("\n", "\n|\t"));
		}
	}
}
