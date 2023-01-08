namespace RhoMicro.LogoSyn.Libs.Interpreter.Abstractions
{
	/// <summary>
	/// Represents an interpreter able to interpret a document.
	/// </summary>
	public interface IInterpreter
	{
		/// <summary>
		/// Interpretes the document provided.
		/// </summary>
		/// <param name="document">The document to interpret.</param>
		void Interpret(Stream document);
	}
}
