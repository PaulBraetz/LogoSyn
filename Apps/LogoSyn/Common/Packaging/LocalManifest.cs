using System.Runtime.Serialization;

using Fort;

using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging.Abstractions;
using RhoMicro.Serialization.Attributes;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging;

/// <summary>
/// Default implementation of <see cref="ILocalManifest"/>.
/// </summary>
[DataContract]
[JsonContract(ImplementInstanceWriteMethod = true)]
public sealed partial class LocalManifest : ILocalManifest
{
	[DataMember]
	private IDictionary<String, String> Packages { get; set; } = new Dictionary<String, String>();

	private Object? _syncRoot;
	private Object SyncRoot => _syncRoot ??= new Object();

	/// <inheritdoc/>
	public Boolean TryAddPackageFile(FileInfo packageFile)
	{
		lock(SyncRoot)
		{
			using var stream = packageFile.OpenRead();
			var package = Package.Read(stream);
			var key = GetKey(package.PackageInfo.Name, package.PackageInfo.Version);
			var value = packageFile.FullName;
			return Packages.TryAdd(key, value);
		}
	}
	/// <inheritdoc/>
	public Boolean TryGetPackageFile(String name, String version, out FileInfo? packageFile)
	{
		var key = GetKey(name, version);
		lock(SyncRoot)
		{
			var result = Packages.TryGetValue(key, out var path);

			packageFile = result ?
				new FileInfo(path!) :
				null;

			return result;
		}
	}
	/// <inheritdoc/>
	public Boolean TryRemovePackage(String name, String version)
	{
		var key = GetKey(name, version);
		lock(SyncRoot)
		{
			var result = Packages.Remove(key);

			return result;
		}
	}
	/// <inheritdoc/>
	public FileInfo[] GetPackages()
	{
		lock(SyncRoot)
		{
			var result = Packages.Values.Select(v => new FileInfo(v)).ToArray();

			return result;
		}
	}
	/// <summary>
	/// Serializes an instance of <see cref="ILocalManifest"/> into a stream.
	/// </summary>
	/// <param name="manifest">The manifest to serialize.</param>
	/// <param name="stream">The stream into which to serialize <paramref name="manifest"/>.</param>
	public static void WriteJson(ILocalManifest manifest, Stream stream)
	{
		manifest.ThrowIfDefault(nameof(manifest));
		stream.ThrowIfDefault(nameof(stream));

		var clone = new LocalManifest();
		var packages = manifest.GetPackages();
		for(var i = 0; i < packages.Length; i++)
		{
			var package = packages[i];
			_ = clone.TryAddPackageFile(package);
		}

		clone.WriteJson(stream);
	}
	private static String GetKey(String? name, String? version)
	{
		name.ThrowIfDefault(nameof(name), $"Invalid package name provided.");
		version.ThrowIfDefault(nameof(version), $"Invalid package version provided.");

		var result = $"{name}_{version}";

		return result;
	}
}
