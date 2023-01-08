using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging.Abstractions;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions
{
	/// <summary>
	/// The context in which a compiler application is run.
	/// </summary>
	public interface ICompilationContext : IApplicationContext
	{
		/// <summary>
		/// Reads the document to compile from a stream.
		/// </summary>
		/// <param name="source">The stream containing source data.</param>
		void SetDocument(Stream source);
		/// <summary>
		/// Adds a package loader to the context.
		/// </summary>
		/// <param name="loader"></param>
		void AddPackageLoader(IPackageLoader loader);
		/// <summary>
		/// Sets the standard output stream for compilation output.
		/// </summary>
		/// <param name="standardOutput">The standard output stream for compilation output.</param>
		void SetStandardOutput(Stream standardOutput);
		/// <summary>
		/// Sets the standard error stream for compilation errors.
		/// </summary>
		/// <param name="standardError">The standard error  stream for compilation errors.</param>
		void SetStandardError(Stream standardError);
		/// <summary>
		/// Compiles the source using the resources set.
		/// </summary>
		/// <param name="cancellationToken">Token used to signal the compilation to cancel.</param>
		/// <returns>A Task that will complete upon the compilation ending.</returns>
		public Task Compile(CancellationToken cancellationToken);
	}
}
