using Fort;

using RhoMicro.Common.System.Abstractions;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common;

/// <summary>
/// The default implementation of <see cref="IApplicationBuilder"/>.
/// </summary>
public sealed class ApplicationBuilder : IApplicationBuilder
{
	private ApplicationBuilder()
	{
	}

	private readonly ICollection<IFactory<IApplicationContext>> _factories = new List<IFactory<IApplicationContext>>();
	private readonly ICollection<IVisitor<IApplicationContext>> _visitors = new List<IVisitor<IApplicationContext>>();
	private readonly ICollection<IAsyncVisitor<IApplicationContext>> _asyncVisitors = new List<IAsyncVisitor<IApplicationContext>>();

	/// <summary>
	/// Creates a new instance of <see cref="IApplicationBuilder"/>.
	/// </summary>
	/// <returns>A new instance of <see cref="IApplicationBuilder"/>.</returns>
	public static IApplicationBuilder Create()
	{
		var result = new ApplicationBuilder();

		return result;
	}

	/// <summary>
	/// Adds an application context factory to the builder.
	/// </summary>
	/// <param name="factory">The factory toadd.</param>
	/// <returns>A reference to this builder.</returns>
	public IApplicationBuilder AddContextFactory(IFactory<IApplicationContext> factory)
	{
		factory.ThrowIfDefault(nameof(factory));

		_factories.Add(factory);

		return this;
	}
	/// <summary>
	/// Adds a context handler to the application.
	/// </summary>
	/// <param name="visitor">The handler to add.</param>
	public IApplicationBuilder AddContextVisitor(IAsyncVisitor<IApplicationContext> visitor)
	{
		visitor.ThrowIfDefault(nameof(visitor));

		_asyncVisitors.Add(visitor);

		return this;
	}
	/// <summary>
	/// Adds a context handler to the application.
	/// </summary>
	/// <param name="visitor">The handler to add.</param>
	public IApplicationBuilder AddContextVisitor(IVisitor<IApplicationContext> visitor)
	{
		visitor.ThrowIfDefault(nameof(visitor));

		_visitors.Add(visitor);

		return this;
	}

	/// <inheritdoc/>
	public IApplication Build()
	{
		var contexts = _factories.Select(f => f.Create()).Where(c => c != null).ToArray();
		var result = new Application(contexts, _visitors, _asyncVisitors);

		return result;
	}

	/// <inheritdoc/>
	public void Reset()
	{
		_factories.Clear();
		_visitors.Clear();
		_asyncVisitors.Clear();
	}
}
