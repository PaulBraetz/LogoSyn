using Fort;

using RhoMicro.LogoSyn.Apps.LogoSyn.Cli.Commands.Packaging;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;

using Scli.Command;
using Scli.Menu;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Cli.Menus;

internal sealed class PackagingMenu : ExitableMenuBase
{
	public PackagingMenu(IPackagingContext context) : base("Packaging", "Exit")
	{
		context.ThrowIfDefault(nameof(context));

		_ = new CommandCollectionBuilder<ICommand>(1, new HashSet<UInt32> { 0 })
			.Append(k => new ReadManifest(k, context))
			.Append(k => new GetLocalPackage(k, context))
			.Append(k => new AddLocalPackage(k, context))
			.Append(k => new RemoveLocalPackage(k, context))
			.Append(k => new GetPackageInfo(k, context))
			.Append(k => new CreatePackage(k, context))
			.Append(k => new GetPackageHash(k, context))
			.Build(out var actions);

		Actions = actions;
	}
	public override void Run() => base.Run();
}
