using Fort;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Cli.Commands;

internal abstract class CommandBase<TContext> : Scli.Command.CommandBase
{
	protected CommandBase(String name, String navigationKey, TContext context) : base(name)
	{
		navigationKey.ThrowIfDefault(nameof(navigationKey));
		context.ThrowIfDefault(nameof(context));

		NavigationKey = navigationKey;
		Context = context;
	}

	protected TContext Context {
		get;
	}
}
