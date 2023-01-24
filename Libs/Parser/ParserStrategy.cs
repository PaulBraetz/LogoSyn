using Fort;

using RhoMicro.LogoSyn.Libs.Dom.Dom.Abstractions;
using RhoMicro.LogoSyn.Libs.Parser.Abstractions;

namespace RhoMicro.LogoSyn.Libs.Parser;

/// <summary>
/// Strategy based implementation of <see cref="IParser"/>.
/// </summary>
/// <typeparam name="TDiscriminator">
/// The discriminator by which to distinguish elements.
/// </typeparam>
internal sealed class ParserStrategy<TDiscriminator> : Parser<TDiscriminator>
{
	private readonly Func<TextReader, IDom<TDiscriminator>> _strategy;

	public ParserStrategy(Func<TextReader, IDom<TDiscriminator>> strategy)
	{
		strategy.ThrowIfDefault(nameof(strategy));

		_strategy = strategy;
	}

	protected override IDom<TDiscriminator> Parse(TextReader reader)
	{
		reader.ThrowIfDefault(nameof(reader));

		var dom = _strategy.Invoke(reader);

		return dom;
	}
}
