using RhoMicro.LogoSyn.Libs.Dom.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace RhoMicro.LogoSyn.Libs.Dom.Comparers
{
	/// <summary>
	/// Defines methods to support the comparison of <see cref="IDomElement{TDiscriminator}"/> objects for equality.
	/// </summary>
	/// <typeparam name="TDiscriminator">
	/// The discriminator by which to distinguish elements.
	/// </typeparam>
	public sealed class DomElementEqualityComparer<TDiscriminator> : IEqualityComparer<IDomElement<TDiscriminator>>
	{
		private DomElementEqualityComparer()
		{

		}

		/// <summary>
		/// Instance of <see cref="DomElementEqualityComparer{TDiscriminator}"/>.
		/// </summary>
		public static readonly DomElementEqualityComparer<TDiscriminator> Instance = new();

		/// <summary>
		/// Determines whether two objects of type <see cref="IDomElement{TDiscriminator}"/> are equal.
		/// </summary>
		/// <param name="x">The first Object to compare.</param>
		/// <param name="y">The second Object to compare.</param>
		/// <returns><see langword="true"/> if the elements provided share the same bounds; otherwise, <see langword="false"/>.</returns>
		public Boolean Equals(IDomElement<TDiscriminator>? x, IDomElement<TDiscriminator>? y)
		{
			if (x == null)
			{
				return y == null;
			}

			if (y == null)
			{
				return x == null;
			}

			var result = x.Position == y.Position &&
						 x.Slice.Length == y.Slice.Length;

			return result;
		}

		/// <summary>
		/// Serves as a hash function for the specified Object for hashing algorithms and data structures, such as a hash table.
		/// </summary>
		/// <param name="obj">The Object for which to get a hash code.</param>
		/// <returns>A hash code for the specified Object.</returns>
		/// <exception cref="ArgumentNullException">The type of obj is a reference type and obj is null.</exception>
		public Int32 GetHashCode([DisallowNull] IDomElement<TDiscriminator> obj)
		{
			if (obj is null)
			{
				throw new ArgumentNullException(nameof(obj));
			}

			var hashCode = HashCode.Combine(obj.Position, obj.Slice.Length);

			return hashCode;
		}
	}
}
