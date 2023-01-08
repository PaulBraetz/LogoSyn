using RhoMicro.Common.System.Security.Cryptography.Hashing.Abstractions;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging.Abstractions;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions
{
	/// <summary>
	/// Represents information required to invoke a package application.
	/// </summary>
	public interface IPackageInvocationInfo
	{
		/// <summary>
		/// The name of the package to invoke.
		/// </summary>
		String PackageName { get; }
		/// <summary>
		/// The version of the package to invoke.
		/// </summary>
		String PackageVersion { get; }
		/// <summary>
		/// The hash value of the package to invoke.
		/// </summary>
		IHash<IPackage> PackageHash { get; }
		/// <summary>
		/// The arguments to be passed to the package upon invokation.
		/// </summary>
		String[] Arguments { get; }
	}
}
