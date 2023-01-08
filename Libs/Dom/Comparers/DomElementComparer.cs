using RhoMicro.LogoSyn.Libs.Dom.Abstractions;

namespace RhoMicro.LogoSyn.Libs.Dom.Comparers
{
	/// <summary>
	/// Defines a method to compare two <see cref="IDomElement{TDiscriminator}"/> objects.
	/// </summary>
	/// <typeparam name="TDiscriminator">
	/// The discriminator by which to distinguish elements.
	/// </typeparam>
	public sealed class DomElementComparer<TDiscriminator> : IComparer<IDomElement<TDiscriminator>>
	{
		private DomElementComparer()
		{

		}

		/// <summary>
		/// Instance of <see cref="DomElementComparer{TDiscriminator}"/>.
		/// </summary>
		public static readonly DomElementComparer<TDiscriminator> Instance = new();

		/// <summary>
		/// Performs a comparison of two objects of type <see cref="IDomElement{TDiscriminator}"/> and returns a value 
		/// indicating whether one Object is less than, equal to, or greater than the other.
		/// </summary>
		/// <param name="x">The first Object to compare.</param>
		/// <param name="y">The second Object to compare.</param>
		/// <returns>
		///     A signed integer that indicates the relative values of x and y, as shown in the
		///     following table.<br/>
		///     <list type="table">
		///			<item>
		///				<term>Value</term> <description>Meaning</description>
		///			</item>
		///			<item>
		///				<term>Less than zero</term> <description>x is less than y.</description>
		///			</item>
		///			<item>
		///				<term>Zero</term> <description>x equals y.</description>
		///			</item>
		///			<item>
		///				<term>Greater</term> <description>x is greater than y.</description>
		///			</item>
		///		</list>
		/// </returns>
		public Int32 Compare(IDomElement<TDiscriminator>? x, IDomElement<TDiscriminator>? y)
		{
			var equalityComparer = DomElementEqualityComparer<TDiscriminator>.Instance;
			var result = x == null ?
								y == null ?
									0 :
									-1 :
							y == null ?
								x == null ?
									0 :
									1 :
							equalityComparer.Equals(x, y) ?
								0 :
							x.Intersects(y) ?
								throw new InvalidOperationException("Cannot compare intersecting elements.") :
							x.Position > y.Position ?
								1 :
							x.Position < y.Position ?
								-1 :
							x.Slice.Length > y.Slice.Length ?
								1 :
							x.Slice.Length < y.Slice.Length ?
								-1 :
								0;

			return result;
		}
	}
}
