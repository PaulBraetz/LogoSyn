using RhoMicro.Common.System.Security.Cryptography.Hashing.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging.Abstractions
{
	/// <summary>
	/// Loader responsible for retrieving packages and their information.
	/// </summary>
	public interface IPackageLoader
	{
		/// <summary>
		/// Loads package information.
		/// </summary>
		/// <param name="name">
		/// The name of the package whose information to load.
		/// </param>
		/// <param name="version">
		/// The version of the package whose information to load.
		/// </param>
		/// <param name="cancellationToken">
		/// Token used to cancel the operation.
		/// </param>
		/// <returns>A task that, upon completion, will contain package information for the package with the name and version requested, if one can be located; otherwise, <see langword="null"/>.</returns>
		Task<IPackageInfo?> Load(String name, String version, CancellationToken cancellationToken = default);
		/// <summary>
		/// Loads a package, optionally verifying it using a computed hash value.
		/// </summary>
		/// <param name="name">
		/// The name of the package to load.
		/// </param>
		/// <param name="version">
		/// The version of the package to load.
		/// </param>
		/// <param name="packageHash">
		/// The hash of the package to load. 
		/// The package loaded will be verified against this value.
		/// </param>
		/// <param name="cancellationToken">
		/// Token used to cancel the operation.
		/// </param>
		/// <returns>A task that, upon completion, will contain a package with the name and version requested if one can be located; otherwise, <see langword="null"/>.</returns>
		Task<IPackage?> Load(String name, String version, IHash<IPackage> packageHash, CancellationToken cancellationToken = default);
	}
}
