using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Cli.Commands.Packaging;

internal abstract class PackagingCommandBase : CommandBase<IPackagingContext>
{
	protected PackagingCommandBase(String name, String navigationKey, IPackagingContext context) : base(name, navigationKey, context)
	{
	}
}
