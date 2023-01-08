using Fort;
using RhoMicro.LogoSyn.Libs.Dom;
using RhoMicro.LogoSyn.Libs.Dom.Abstractions;

namespace RhoMicro.LogoSyn.Libs.Parser.Abstractions
{
	/// <summary>
	/// Abstract base class for types implementing <see cref="IParser"/>.
	/// </summary>
	/// <typeparam name="TDiscriminator">
	/// The discriminator by which to distinguish elements.
	/// </typeparam>
	public abstract class Parser<TDiscriminator> : IParser
	{
		/// <summary>
		/// Parses a document based on a source.
		/// </summary>
		/// <param name="sourceStream">The source input to parse the document from.</param>
		/// <returns>An instance of <see cref="IDom{TDiscriminator}"/> based on the content read from <paramref name="sourceStream"/>.</returns>
		protected abstract IDom<TDiscriminator> Parse(TextReader sourceStream);

		/// <summary>
		/// Creates a strategy based parser.
		/// </summary>
		/// <param name="strategy">The strategy to base the parser on.</param>
		/// <returns>A parser based on the strategy passed.</returns>
		public static IParser Create(Func<TextReader, IDom<TDiscriminator>> strategy)
		{
			var parser = new ParserStrategy<TDiscriminator>(strategy);

			return parser;
		}

		/// <inheritdoc/>
		public void Parse(Stream sourceStream, Stream interpreterStream)
		{
			sourceStream.ThrowIfDefault(nameof(sourceStream));
			interpreterStream.ThrowIfDefault(nameof(interpreterStream));

			IDom<TDiscriminator>? dom = null;
			using (var reader = new StreamReader(sourceStream))
			{
				dom = Parse(reader);
			}

			Dom<TDiscriminator>.WriteJson(interpreterStream, dom);
		}
	}
}