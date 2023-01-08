using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging.Abstractions
{
	/// <summary>
	/// A manifest mapping package names and versions onto their local file paths.
	/// </summary>
	public interface ILocalManifest
	{
		/// <summary>
		/// Attempts to retrieve a local package.
		/// </summary>
		/// <param name="name">The name of the package to retrieve.</param>
		/// <param name="version">The version of the package to retrieve.</param>
		/// <param name="packageFile">
		/// Will contain a reference to the file path of the package to retrieve, if 
		/// it was indexed; otherwise, <see langword="null"/>.
		/// </param>
		/// <returns>
		/// <see langword="true"/>, if a package matching <paramref name="name"/> and 
		/// <paramref name="version"/> could be found; otherwise, <see langword="false"/>.
		/// </returns>
		Boolean TryGetPackageFile(String name, String version, out FileInfo? packageFile);
		/// <summary>
		/// Attempts to index a local package.
		/// </summary>
		/// <param name="packageFile">The file path of the package to add.</param>
		/// <returns>
		/// <see langword="true"/>, if the package found at <paramref name="packageFile"/> was added successfully;
		/// otherwise, if the package has already been indexed, <see langword="false"/>.
		/// </returns>
		Boolean TryAddPackageFile(FileInfo packageFile);
		/// <summary>
		/// Attempts to remove a local package from the manifest.
		/// </summary>
		/// <param name="name">The name of the package to remove.</param>
		/// <param name="version">The version of the package to remove.</param>
		/// <returns>
		/// <see langword="true"/>, if a package matching <paramref name="name"/> and 
		/// <paramref name="version"/> could be removed; otherwise, if no matching package could be found, <see langword="false"/>.
		/// </returns>
		Boolean TryRemovePackage(String name, String version);
		/// <summary>
		/// Returns all indexed package files.
		/// </summary>
		/// <returns>All indexed package files.</returns>
		FileInfo[] GetPackages();
	}
}
