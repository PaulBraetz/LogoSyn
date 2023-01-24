using Fort;

using RhoMicro.Common.System.Abstractions;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;

using Scli;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Cli.Visitors;

internal sealed class CompilationVisitor : AsyncVisitorBase<IApplicationContext>
{
	private readonly Boolean _canReceive;

	public CompilationVisitor(IArgumentCollection arguments)
	{
		arguments.ThrowIfDefault(nameof(arguments));

		_canReceive = !arguments.TryGet("h", out var _);
	}

	protected override async Task Receive(IApplicationContext obj, CancellationToken cancellationToken = default)
	{
		if(obj is ICompilationContext compilationContext)
		{
			await compilationContext.Compile(cancellationToken);
		}
	}

	protected override Task<Boolean> CanReceive(IApplicationContext obj, CancellationToken cancellationToken = default)
	{
		var result = _canReceive && obj is ICompilationContext;

		return Task.FromResult(result);
	}
}
