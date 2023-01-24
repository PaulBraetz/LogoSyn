namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging.Abstractions;

/// <summary>
/// Represents an executable local package.
/// </summary>
public interface IPackage : IDisposable
{
	/// <summary>
	/// The directory at which package data is located.
	/// </summary>
	DirectoryInfo PackageDataDirectory {
		get;
	}
	/// <summary>
	/// Information on this package.
	/// </summary>
	IPackageInfo PackageInfo {
		get;
	}
}
