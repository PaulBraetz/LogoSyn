using Fort;

using RhoMicro.Common.System.Security.Cryptography.Hashing;
using RhoMicro.Common.System.Security.Cryptography.Hashing.Abstractions;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging.Abstractions;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging;

/// <summary>
/// Implements a package loader whose source is a local directory of packages.
/// </summary>
internal sealed partial class LocalPackageLoader : IPackageLoader
{
	private readonly ILocalManifest _manifest;

	/// <summary>
	/// Initializes a new instance with the manifest provided.
	/// </summary>
	/// <param name="manifest">The manifest indexing local packages.</param>
	public LocalPackageLoader(ILocalManifest manifest)
	{
		manifest.ThrowIfDefault(nameof(manifest));

		_manifest = manifest;
	}

	/// <summary>
	/// Creates a new local package loader based on a json manifest file.
	/// </summary>
	/// <param name="manifestFile">The file containg manifest data.</param>
	/// <returns>A new instance of <see cref="IPackageLoader"/>, ready to load local packages indexed in <paramref name="manifestFile"/>.</returns>
	public static IPackageLoader Create(FileInfo manifestFile)
	{
		manifestFile.ThrowIfDefault(nameof(manifestFile));

		ILocalManifest? manifest = null;
		using(var manifestStream = manifestFile.Open(FileMode.OpenOrCreate))
		{
			manifest = LocalManifest.ReadJson(manifestStream) ?? new LocalManifest();
		}

		var result = new LocalPackageLoader(manifest);

		return result;
	}

	/// <inheritdoc/>
	public Task<IPackageInfo?> Load(String name, String version, CancellationToken cancellationToken = default)
	{
		name.ThrowIfDefault(nameof(name));
		version.ThrowIfDefault(nameof(version));

		cancellationToken.ThrowIfCancellationRequested();

		IPackageInfo? result = null;
		if(TryLoad(name, version, out var package))
		{
			using(package)
			{
				result = package!.PackageInfo;
			}
		}

		return Task.FromResult(result);
	}
	/// <inheritdoc/>
	public Task<IPackage?> Load(String name, String version, IHash<IPackage> packageHash, CancellationToken cancellationToken = default)
	{
		name.ThrowIfDefault(nameof(name));
		version.ThrowIfDefault(nameof(version));
		packageHash.ThrowIfDefault(nameof(packageHash));

		cancellationToken.ThrowIfCancellationRequested();

		if(TryLoad(name, version, out var result))
		{
			result!.AsHashed(packageHash).ThrowIfInvalid($"package {name} {version}");
		}

		return Task.FromResult(result);
	}
	private Boolean TryLoad(String name, String version, out IPackage? package)
	{
		package = null;

		if(_manifest.TryGetPackageFile(name, version, out var packageFile))
		{
			using var packageStream = packageFile!.OpenRead();
			package = Package.Read(packageStream);
		}

		return package != null;
	}
}
