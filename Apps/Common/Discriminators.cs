namespace RhoMicro.LogoSyn.Apps.Common;

/// <summary>
/// Provides the <see cref="Default"/> enum for default element discrimination.
/// </summary>
public static class Discriminators
{
	/// <summary>
	/// Defines how a document element is to be handled by the interpreter.
	/// </summary>
	public enum Default : Byte
	{
		/// <summary>
		/// The elements value is to be included in the output as is.
		/// </summary>
		Literal,
		/// <summary>
		/// The element is to be evaluated but not included in the output.
		/// </summary>
		Code,
		/// <summary>
		/// The element is to be evaluated and included in the output.
		/// </summary>
		Display,
		/// <summary>
		/// The element is not to evaluated nor included in the output.
		/// </summary>
		Ignore
	}
}