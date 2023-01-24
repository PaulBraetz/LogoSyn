using System.Diagnostics.CodeAnalysis;

namespace RhoMicro.LogoSyn.Libs.Common.Strings;

/// <summary>
/// Defines methods to support the comparison of <see cref="IStringSlice"/> objects for equality.
/// </summary>
public sealed class StringSliceEqualityComparer : IEqualityComparer<IStringSlice>
{
	private StringSliceEqualityComparer()
	{
	}
	/// <summary>
	/// Instance of <see cref="StringSliceEqualityComparer"/>.
	/// </summary>
	public static readonly StringSliceEqualityComparer Instance = new();
	/// <summary>
	/// Determines whether two objects of type <see cref="IStringSlice"/> are equal.
	/// </summary>
	/// <param name="x">The first Object to compare.</param>
	/// <param name="y">The second Object to compare.</param>
	/// <returns><see langword="true"/> if the elements provided share the same value, start and length; otherwise, <see langword="false"/>.</returns>
	public Boolean Equals(IStringSlice? x, IStringSlice? y)
	{
		if(x == null)
		{
			return y == null;
		}

		if(y == null)
		{
			return x == null;
		}

		var result = x.Start == y.Start &&
			x.Length == y.Length &&
			x.Value == y.Value;

		return result;
	}

	/// <summary>
	/// Serves as a hash function for the specified Object for hashing algorithms and data structures, such as a hash table.
	/// </summary>
	/// <param name="obj">The Object for which to get a hash code.</param>
	/// <returns>A hash code for the specified Object.</returns>
	/// <exception cref="ArgumentNullException">The type of obj is a reference type and obj is null.</exception>
	public Int32 GetHashCode([DisallowNull] IStringSlice obj)
	{
		if(obj is null)
		{
			throw new ArgumentNullException(nameof(obj));
		}

		var hashCode = HashCode.Combine(obj.Start, obj.Length, obj.Value);

		return hashCode;
	}
}
