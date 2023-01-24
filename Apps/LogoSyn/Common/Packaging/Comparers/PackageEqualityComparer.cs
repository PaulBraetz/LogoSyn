using System.Diagnostics.CodeAnalysis;

using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging.Abstractions;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging.Comparers;

/// <summary>
/// Defines methods to support the comparison of instances of <see cref="IPackage"/> for equality.
/// </summary>
public sealed class PackageEqualityComparer : IEqualityComparer<IPackage>
{
	private PackageEqualityComparer()
	{
	}

	/// <summary>
	/// Instance of <see cref="PackageEqualityComparer"/>.
	/// </summary>
	public static readonly PackageEqualityComparer Instance = new();

	/// <inheritdoc/>
	public Boolean Equals(IPackage? x, IPackage? y)
	{
		if(x == null)
		{
			return y == null;
		}

		if(y == null)
		{
			return x == null;
		}

		var result = x.PackageDataDirectory.FullName == y.PackageDataDirectory.FullName &&
			PackageInfoEqualityComparer.Instance.Equals(x.PackageInfo, y.PackageInfo);

		return result;
	}

	/// <inheritdoc/>
	public Int32 GetHashCode([DisallowNull] IPackage obj)
	{
		if(obj is null)
		{
			throw new ArgumentNullException(nameof(obj));
		}

		var hashCode = HashCode.Combine(obj.PackageDataDirectory,
			PackageInfoEqualityComparer.Instance.GetHashCode(obj.PackageInfo));

		return hashCode;
	}
}
