namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging.Abstractions;

/// <summary>
/// Represents metadata and details on the execution of a package.
/// </summary>
public interface IPackageInfo
{
	/// <summary>
	/// The entry point, pointing to an executable file in the packages data directory.
	/// </summary>
	String? EntryPoint {
		get;
	}
	/// <summary>
	/// The name of the package.
	/// </summary>
	String? Name {
		get;
	}
	/// <summary>
	/// The package version.
	/// </summary>
	String? Version {
		get;
	}
	/// <summary>
	/// The package description.
	/// </summary>
	String? Description {
		get;
	}
	/// <summary>
	/// The package author.
	/// </summary>
	String? Author {
		get;
	}
	/// <summary>
	/// The package website.
	/// </summary>
	String? Website {
		get;
	}
}
