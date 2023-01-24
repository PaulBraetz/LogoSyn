using Fort;

using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Compilation;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common;

/// <summary>
/// Provides extension methods for LogoSyn applications
/// </summary>
public static class Extensions
{
	/// <summary>
	/// Adds a local package loader to an instance of <see cref="ICompilationContext"/>.
	/// </summary>
	/// <param name="context">The context to which to add the local package loader.</param>
	/// <param name="manifestFilePath">The path at which the manifest file indexing local packages can be found.</param>
	public static void AddLocalPackageLoader(this ICompilationContext context, String manifestFilePath)
	{
		context.ThrowIfDefault(nameof(context));
		manifestFilePath.ThrowIfDefault(nameof(manifestFilePath));

		var manifestFile = new FileInfo(manifestFilePath);
		var loader = LocalPackageLoader.Create(manifestFile);
		context.AddPackageLoader(loader);
	}
	/// <summary>
	/// Adds a factory for the built in packaging application context to the application.
	/// </summary>
	/// <param name="builder">The builder which to add the packaging context factory to.</param>
	/// <param name="args">The arguments to initialize the factory with.</param>
	/// <returns>A reference to <paramref name="builder"/>.</returns>
	public static IApplicationBuilder AddPackagingContext(this IApplicationBuilder builder, String[] args)
	{
		builder.ThrowIfDefault(nameof(builder));
		args.ThrowIfDefault(nameof(args));

		var factory = new PackagingContextFactory(args);
		_ = builder.AddContextFactory(factory!);

		return builder;
	}
	/// <summary>
	/// Adds a factory for the built in compiler application context to the application.
	/// </summary>
	/// <param name="builder">The builder which to add the compilation context factory to.</param>
	/// <param name="args">The arguments to initialize the factory with.</param>
	/// <returns>A reference to <paramref name="builder"/>.</returns>
	public static IApplicationBuilder AddCompilationContext(this IApplicationBuilder builder, String[] args)
	{
		builder.ThrowIfDefault(nameof(builder));
		args.ThrowIfDefault(nameof(args));

		var factory = new CompilationContextFactory(args);
		_ = builder.AddContextFactory(factory!);

		return builder;
	}
	/// <summary>
	/// Adds an application context factory to the builder.
	/// </summary>
	/// <param name="builder">The builder which to add the application context factory to.</param>
	/// <param name="factory">The factory to add.</param>
	/// <returns>A reference to this builder.</returns>
	public static IApplicationBuilder AddContextFactory(this IApplicationBuilder builder, Func<IApplicationContext> factory)
	{
		builder.ThrowIfDefault(nameof(builder));
		factory.ThrowIfDefault(nameof(factory));

		var wrapper = new ApplicationContextFactoryStrategy(factory);
		_ = builder.AddContextFactory(wrapper);

		return builder;
	}
}
