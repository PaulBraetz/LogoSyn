using RhoMicro.Common.System.Abstractions;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common
{
	/// <summary>
	/// Builder used to instantiate instances of <see cref="IApplication"/>.
	/// </summary>
	public interface IApplicationBuilder : IBuilder<IApplication>
	{
		/// <summary>
		/// Adds a factory for instances of <see cref="IApplicationContext"/> to the builder, to be used by the resulting application.
		/// </summary>
		/// <param name="factory">The factory to add.</param>
		/// <returns>A reference to the builder, for chaining further operations.</returns>
		IApplicationBuilder AddContextFactory(IFactory<IApplicationContext> factory);
		/// <summary>
		/// Adds an asynchronous visitor for instances of <see cref="IApplicationContext"/> to the builder, to visit the resulting applications contexts.
		/// </summary>
		/// <param name="visitor">The visitor to add.</param>
		/// <returns>A reference to the builder, for chaining further operations.</returns>
		IApplicationBuilder AddContextVisitor(IAsyncVisitor<IApplicationContext> visitor);
		/// <summary>
		/// Adds a visitor for instances of <see cref="IApplicationContext"/> to the builder, to visit the resulting applications contexts.
		/// </summary>
		/// <param name="visitor">The visitor to add.</param>
		/// <returns>A reference to the builder, for chaining further operations.</returns>
		IApplicationBuilder AddContextVisitor(IVisitor<IApplicationContext> visitor);
	}
}