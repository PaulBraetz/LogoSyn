using Fort;

using RhoMicro.Common.System.Abstractions;
using RhoMicro.LogoSyn.Apps.LogoSyn.Cli.Menus;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;

using Scli;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Cli.Visitors;

internal sealed class PackagingVisitor : VisitorBase<IApplicationContext>
{
	private readonly Boolean _canReceive;

	public PackagingVisitor(IArgumentCollection arguments)
	{
		arguments.ThrowIfDefault(nameof(arguments));

		_canReceive = !arguments.TryGet("h", out var _);
	}

	protected override Boolean CanReceive(IApplicationContext obj)
	{
		var result = _canReceive && obj is IPackagingContext;

		return result;
	}
	protected override void Receive(IApplicationContext obj)
	{
		if(obj is IPackagingContext packagingContext)
		{
			new PackagingMenu(packagingContext).Run();
		}
	}
}
