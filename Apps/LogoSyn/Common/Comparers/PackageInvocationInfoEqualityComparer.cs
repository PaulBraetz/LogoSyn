using RhoMicro.Common.System.Security.Cryptography.Hashing.Comparers;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;
using System.Diagnostics.CodeAnalysis;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging.Abstractions;
using RhoMicro.Common.System.Comparers;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common.Comparers
{
	/// <summary>
	/// Defines methods to support the comparison of instances of <see cref="IPackageInvocationInfo"/> for equality.
	/// </summary>
	public sealed class PackageInvocationInfoEqualityComparer : IEqualityComparer<IPackageInvocationInfo>
	{
		private PackageInvocationInfoEqualityComparer() { }

		/// <summary>
		/// Instance of <see cref="PackageInvocationInfoEqualityComparer"/>.
		/// </summary>
		public static readonly PackageInvocationInfoEqualityComparer Instance = new();

		/// <inheritdoc/>
		public Boolean Equals(IPackageInvocationInfo? x, IPackageInvocationInfo? y)
		{
			if (x == null)
			{
				return y == null;
			}

			if (y == null)
			{
				return x == null;
			}

			var result = x.PackageName == y.PackageName &&
				x.PackageVersion == y.PackageVersion &&
				ArrayEqualityComparer<String>.Instance.Equals(x.Arguments, y.Arguments) &&
				HashEqualityComparer<IPackage>.Instance.Equals(x.PackageHash, y.PackageHash);

			return result;
		}

		/// <inheritdoc/>
		public Int32 GetHashCode([DisallowNull] IPackageInvocationInfo obj)
		{
			if (obj is null)
			{
				throw new ArgumentNullException(nameof(obj));
			}

			var hashCode = new HashCode();
			hashCode.Add(obj.PackageName);
			hashCode.Add(obj.PackageVersion);
			hashCode.Add(obj.Arguments, ArrayEqualityComparer<String>.Instance);
			hashCode.Add(obj.PackageHash, HashEqualityComparer<IPackage>.Instance);

			var result = hashCode.ToHashCode();

			return result;
		}
	}
}
