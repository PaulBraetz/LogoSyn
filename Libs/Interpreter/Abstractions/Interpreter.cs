using RhoMicro.LogoSyn.Libs.Dom;
using RhoMicro.LogoSyn.Libs.Dom.Abstractions;

namespace RhoMicro.LogoSyn.Libs.Interpreter.Abstractions
{
	/// <summary>
	/// Base class for types implementing <see cref="IInterpreter"/>.
	/// </summary>
	/// <typeparam name="TDiscriminator">
	/// The discriminator by which to distinguish elements.
	/// </typeparam>
	public abstract class Interpreter<TDiscriminator> : IInterpreter
	{
		/// <inheritdoc/>
		public void Interpret(Stream document)
		{
			var dom = Dom<TDiscriminator>.ReadJson(document, false);

			Interpret(dom);
		}
		/// <summary>
		/// Interpretes a document.
		/// </summary>
		/// <param name="document">The document to interpret.</param>
		protected abstract void Interpret(IDom<TDiscriminator> document);

		/// <summary>
		/// Creates a strategy based interpreter.
		/// </summary>
		/// <param name="strategy">The strategy to base the interpreter on.</param>
		/// <returns>An interpreter based on the strategy passed.</returns>
		public static IInterpreter Create(Action<IDom<TDiscriminator>> strategy)
		{
			var result = new InterpreterStrategy<TDiscriminator>(strategy);

			return result;
		}
	}
}
