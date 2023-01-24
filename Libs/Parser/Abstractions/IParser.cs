using RhoMicro.LogoSyn.Libs.Dom.Dom.Abstractions;

namespace RhoMicro.LogoSyn.Libs.Parser.Abstractions;

/// <summary>
/// A parser that produces a <see cref="IDom{TDiscriminator}"/> based on a <see cref="TextReader"/>.
/// </summary>
public interface IParser
{
	/// <summary>
	/// Parses a document based on a source and writes the resulting document to a stream.
	/// </summary>
	/// <param name="sourceStream">The source input to parse the document from.</param>
	/// <param name="interpreterStream">The output stream to which the resulting serialized dom will be written.</param>
	public void Parse(Stream sourceStream, Stream interpreterStream);
}
