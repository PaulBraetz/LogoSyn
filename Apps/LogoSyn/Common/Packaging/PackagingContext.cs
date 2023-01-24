using Fort;

using RhoMicro.Common.System;
using RhoMicro.Common.System.Security.Cryptography.Hashing.Abstractions;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging.Abstractions;

using BuiltIns = RhoMicro.Common.System.Security.Cryptography.Hashing.Abstractions.DefaultAlgorithmBase<RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging.Abstractions.IPackage>.BuiltinAlgorithm;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging;

internal sealed class PackagingContext : DisposableBase, IPackagingContext
{
	public PackagingContext(String helpInfo)
	{
		helpInfo.ThrowIfDefault(nameof(helpInfo));

		_helpInfo = helpInfo;
	}

	private readonly String _helpInfo;

	public String GetHelpInfo() => _helpInfo;

	public void CreatePackage(IPackageInfo packageInfo, DirectoryInfo packageDirectory, DirectoryInfo targetDirectory)
	{
		packageInfo.ThrowIfDefaultOrNot(
			i => !String.IsNullOrWhiteSpace(i.Name) && !String.IsNullOrWhiteSpace(i.Version),
			$"{nameof(packageInfo)} must provide both a name and a version.",
			nameof(packageInfo));
		packageDirectory.ThrowIfDefaultOrNot(d => d.Exists, $"{nameof(packageDirectory)} does not exist.", nameof(packageDirectory));
		targetDirectory.ThrowIfDefault(nameof(targetDirectory));

		var packageFilePath = packageInfo.GetPackageFileName(targetDirectory);
		using var package = Package.Create(packageInfo, packageDirectory);
		using var packageFile = File.Create(packageFilePath);
		package.Write(packageFile);
	}

	public ILocalManifest ReadManifest(FileInfo manifestFile)
	{
		manifestFile.ThrowIfDefault(nameof(manifestFile));

		using var manifestStream = manifestFile.Open(FileMode.OpenOrCreate);
		var result = LocalManifest.ReadJson(manifestStream) ?? new LocalManifest();
		return result;
	}

	public void WriteManifest(ILocalManifest manifest, FileInfo file)
	{
		manifest.ThrowIfDefault(nameof(manifest));
		file.ThrowIfDefault(nameof(file));

		file.Create().Close();
		using var stream = file.Open(FileMode.OpenOrCreate);
		LocalManifest.WriteJson(manifest, stream);
	}
	public IPackage GetPackage(FileInfo packageFile)
	{
		packageFile.ThrowIfDefaultOrNot(f => f.Exists, $"{nameof(packageFile)} does not exist.", nameof(packageFile));

		using var packageStream = packageFile.OpenRead();
		var result = Package.Read(packageStream);

		return result;
	}
	public IHash<IPackage> Hash(IPackage package, String algorithmName)
	{
		if(!Enum.TryParse<BuiltIns>(algorithmName, false, out var algorithmType))
		{
			var validAlgorithms = Enum.GetValues<BuiltIns>().Select(a => a.ToString());
			throw new Exception($"Unable to hash using {algorithmName}. Valid algorithms are: {String.Join(',', validAlgorithms)}.");
		}

		var algorithm = DefaultAlgorithmBase<IPackage>.Create(Package.SerializeIdentifiers, algorithmType);
		var result = algorithm.Hash(package);

		return result;
	}
}
