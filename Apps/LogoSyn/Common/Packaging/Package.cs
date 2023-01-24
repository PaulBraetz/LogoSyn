using System.IO.Compression;

using Fort;

using RhoMicro.Common.IO;
using RhoMicro.Common.System;
using RhoMicro.Common.System.IO;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging.Abstractions;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging;

/// <summary>
/// Default implementation of <see cref="IPackage"/>.
/// </summary>
public sealed partial class Package : DisposableBase, IPackage
{
	/// <inheritdoc/>
	public IPackageInfo PackageInfo {
		get;
	}
	/// <inheritdoc/>
	public DirectoryInfo PackageDataDirectory => _tempDirectory.Directory;

	private readonly TemporaryDirectory _tempDirectory;

	private Package(IPackageInfo packageInfo, DirectoryInfo packageDirectory)
	{
		packageInfo.ThrowIfDefault(nameof(packageInfo));
		packageDirectory.ThrowIfDefaultOrNot(d => d.Exists, $"{nameof(packageDirectory)} does not exist.", nameof(packageDirectory));

		PackageInfo = packageInfo;
		_tempDirectory = Directories.GetTempDirectory();
		packageDirectory.CopyRecursively(Path.Combine(_tempDirectory.Directory.FullName, packageDirectory.Name));
	}

	/// <summary>
	/// Finalizer.
	/// </summary>
	~Package()
	{
		FinalizeDispose();
	}
	/// <inheritdoc/>
	protected override void DisposeUnmanaged(Boolean disposing) => _tempDirectory.Dispose();

	/// <summary>
	/// Serializes the identifying properties of a package into a stream.
	/// </summary>
	/// <remarks>
	/// This function is intended to be used as the default hashing serialization 
	/// strategy for instances of <see cref="IPackage"/>. It is in no way intended 
	/// to be used as a textual or other serialization mechanism.
	/// </remarks>
	/// <param name="package">The package to serialize.</param>
	/// <returns>A stream, containing the identifying properties of <paramref name="package"/>.</returns>
	public static Stream SerializeIdentifiers(IPackage package)
	{
		package.ThrowIfDefault(nameof(package));

		var metaDataStream = Packaging.PackageInfo.SerializeIdentifiers(package.PackageInfo);
		var packageStream = GetPackageStream(package);

		var result = metaDataStream.Append(packageStream, true);

		return result;
	}
	private static Stream GetPackageStream(IPackage package)
	{
		var archiveFileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
		ZipFile.CreateFromDirectory(package.PackageDataDirectory.FullName, archiveFileName, CompressionLevel.NoCompression, false);
		var result = File.OpenRead(archiveFileName);

		return result;
	}
}
