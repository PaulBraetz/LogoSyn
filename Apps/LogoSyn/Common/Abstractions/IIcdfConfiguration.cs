namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;

/// <summary>
/// Configuration used to parse and interpret an .ls source file.
/// </summary>
public interface IDocumentInfo : IDisposable
{
	/// <summary>
	/// Information on the interpreter to be used.
	/// </summary>
	IPackageInvocationInfo InterpreterInfo {
		get;
	}
	/// <summary>
	/// Information on the parser to be used.
	/// </summary>
	IPackageInvocationInfo ParserInfo {
		get;
	}
	/// <summary>
	/// The stream containing the document source.
	/// </summary>
	Stream Source {
		get;
	}
	/// <summary>
	/// The offset up to which <see cref="Source"/> contains only metadata, that is, the stream position after which source text can be found.
	/// </summary>
	Int32 SourceOffset {
		get;
	}
}