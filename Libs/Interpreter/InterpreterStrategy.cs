using Fort;

using RhoMicro.LogoSyn.Libs.Dom.Dom.Abstractions;
using RhoMicro.LogoSyn.Libs.Interpreter.Abstractions;

namespace RhoMicro.LogoSyn.Libs.Interpreter;

internal sealed class InterpreterStrategy<TDiscriminator> : Interpreter<TDiscriminator>
{
	private readonly Action<IDom<TDiscriminator>> _strategy;

	public InterpreterStrategy(Action<IDom<TDiscriminator>> strategy)
	{
		strategy.ThrowIfDefault(nameof(strategy));

		_strategy = strategy;
	}

	protected override void Interpret(IDom<TDiscriminator> document)
	{
		document.ThrowIfDefault(nameof(document));

		_strategy.Invoke(document);
	}
}
