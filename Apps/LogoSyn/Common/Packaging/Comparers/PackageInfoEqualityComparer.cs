using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging.Comparers
{
	/// <summary>
	/// Defines methods to support the comparison of instances of <see cref="IPackageInfo"/> for equality.
	/// </summary>
	public sealed class PackageInfoEqualityComparer : IEqualityComparer<IPackageInfo>
	{
		private PackageInfoEqualityComparer() { }

		/// <summary>
		/// Instance of <see cref="PackageInfoEqualityComparer"/>.
		/// </summary>
		public static readonly PackageInfoEqualityComparer Instance = new();

		/// <inheritdoc/>
		public Boolean Equals(IPackageInfo? x, IPackageInfo? y)
		{
			if (x == null)
			{
				return y == null;
			}

			if (y == null)
			{
				return x == null;
			}

			var result = x.Name == y.Name &&
				x.Version == y.Version;

			return result;
		}

		/// <inheritdoc/>
		public Int32 GetHashCode([DisallowNull] IPackageInfo obj)
		{
			if (obj is null)
			{
				throw new ArgumentNullException(nameof(obj));
			}

			var hashCode = HashCode.Combine(obj.Name, obj.Version);

			return hashCode;
		}
	}
}
