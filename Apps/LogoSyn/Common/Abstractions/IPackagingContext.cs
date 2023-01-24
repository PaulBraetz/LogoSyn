using RhoMicro.Common.System.Security.Cryptography.Hashing.Abstractions;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging.Abstractions;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;

/// <summary>
/// The context in which a packaging application is run.
/// </summary>
public interface IPackagingContext : IApplicationContext
{
	/// <summary>
	/// Creates a package and strores it in a directory.
	/// </summary>
	/// <param name="packageInfo">The packages metadata.</param>
	/// <param name="packageDirectory">The directory containing the package files.</param>
	/// <param name="targetDirectory">The directory which to write the resulting package file to.</param>
	void CreatePackage(IPackageInfo packageInfo, DirectoryInfo packageDirectory, DirectoryInfo targetDirectory);
	/// <summary>
	/// Retrieves a package and its associated metadata.
	/// </summary>
	/// <param name="packageFile">The file containing the package.</param>
	/// <returns>An instance of <see cref="IPackage"/>, containing the package data from <paramref name="packageFile"/>.</returns>
	IPackage GetPackage(FileInfo packageFile);
	/// <summary>
	/// Reads a local package manifest from a file.
	/// </summary>
	/// <param name="manifestFile">The file containing the manifest.</param>
	/// <returns>The manifest read from <paramref name="manifestFile"/>.</returns>
	ILocalManifest ReadManifest(FileInfo manifestFile);
	/// <summary>
	/// Writes a local package manifest to a file.
	/// </summary>
	/// <param name="manifest">The manifest to write.</param>
	/// <param name="manifestFile">The file which to write <paramref name="manifest"/> to.</param>
	void WriteManifest(ILocalManifest manifest, FileInfo manifestFile);
	/// <summary>
	/// Hashes a package using a built in algorithm.
	/// </summary>
	/// <param name="package">The package to hash.</param>
	/// <param name="algorithmName">The algorithm to use.</param>
	/// <returns>A hash of <paramref name="package"/>.</returns>
	IHash<IPackage> Hash(IPackage package, String algorithmName);
}
