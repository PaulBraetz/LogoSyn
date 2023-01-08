using Fort;
using RhoMicro.LogoSyn.Libs.Common.Strings;
using RhoMicro.LogoSyn.Libs.Dom.Abstractions;

namespace RhoMicro.LogoSyn.Libs.Dom
{
	/// <summary>
	/// Extension methods for types in the <c>Libs.Dom</c> namespace.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Returns wether or not two <see cref="IDomElement{TDiscriminator}"/> share the same discriminator.
		/// </summary>
		/// <typeparam name="TDiscriminator">
		/// The discriminator by which to distinguish elements.
		/// </typeparam>
		/// <param name="first">The first element.</param>
		/// <param name="second">The second element.</param>
		/// <returns><see langword="true"/> if the elements provided share the same discriminator; otherwise, <see langword="false"/>.</returns>
		public static Boolean KindEquals<TDiscriminator>(this IDomElement<TDiscriminator> first, IDomElement<TDiscriminator> second)
		{
			first.ThrowIfDefault(nameof(first));
			second.ThrowIfDefault(nameof(second));

			var result = EqualityComparer<TDiscriminator>.Default.Equals(first.Kind, second.Kind);

			return result;
		}

		/// <summary>
		/// Returns the position of an <see cref="IDomElement{TDiscriminator}"/>s last character.
		/// </summary>
		/// <param name="element">
		/// The element.
		/// </param>
		/// <typeparam name="TDiscriminator">
		/// The discriminator by which to distinguish elements.
		/// </typeparam>
		/// <returns>
		/// The position of <paramref name="element"/>s last character.
		/// </returns>
		public static Int32 GetEnd<TDiscriminator>(this IDomElement<TDiscriminator> element)
		{
			element.ThrowIfDefault(nameof(element));

			var end = element.Position + element.Slice.Length;

			return end;
		}

		/// <summary>
		/// Calculates the distance of two disjunct <see cref="IDomElement{TDiscriminator}"/>s.
		/// </summary>
		/// <param name="first">The first element.</param>
		/// <param name="second">The second element.</param>
		/// <typeparam name="TDiscriminator">
		/// The discriminator by which to distinguish elements.
		/// </typeparam>
		/// <returns>The distance between <paramref name="first"/> and <paramref name="second"/>.</returns>
		public static Int32 DistanceTo<TDiscriminator>(this IDomElement<TDiscriminator> first, IDomElement<TDiscriminator> second)
		{
			first.ThrowIfDefault(nameof(first));
			second.ThrowIfDefaultOrNot(e => !e.Intersects(first), $"{nameof(second)} cannot intersect {nameof(first)}.", nameof(second));

			var distance = first.Position - second.GetEnd();
			if (distance < 0)
			{
				distance = second.Position - first.GetEnd();
			}

			return distance;
		}
		/// <summary>
		/// Returns wether or not two <see cref="IDomElement{TDiscriminator}"/>s intersect.
		/// </summary>
		/// <param name="first">The first element.</param>
		/// <param name="second">The second element.</param>
		/// <typeparam name="TDiscriminator">
		/// The discriminator by which to distinguish elements.
		/// </typeparam>
		/// <returns><see langword="true"/> if <paramref name="first"/> intersects <paramref name="second"/>; otherwise, <see langword="false"/>.</returns>
		public static Boolean Intersects<TDiscriminator>(this IDomElement<TDiscriminator> first, IDomElement<TDiscriminator> second)
		{
			var intersects = !first.IsDisjunct(second);

			return intersects;
		}

		/// <summary>
		/// Merges two intersecting <see cref="IDomElement{TDiscriminator}"/> of the same discriminator.
		/// </summary>
		/// <param name="first">The element to merge into.</param>
		/// <param name="second">The element to merge.</param>
		/// <typeparam name="TDiscriminator">
		/// The discriminator by which to distinguish elements.
		/// </typeparam>
		/// <returns>A new, merged element.</returns>
		public static IDomElement<TDiscriminator> Merge<TDiscriminator>(this IDomElement<TDiscriminator> first, IDomElement<TDiscriminator> second)
		{
			first.ThrowIfDefault(nameof(first));
			second.ThrowIfDefaultOrNot(e => e.KindEquals(first), $"{nameof(second)} was of kind {second.Kind}, but {first.Kind} was expected.", nameof(second));
			second.ThrowIfNot(e => e.Intersects(first), $"{nameof(second)} was expected to intersect {nameof(first)}.");

			//TODO: implement conditions
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns wether or not two <see cref="IDomElement{TDiscriminator}"/>s are neighbours and share the same <typeparamref name="TDiscriminator"/>.
		/// </summary>
		/// <typeparam name="TDiscriminator">The discriminator by which to distinguish elements.</typeparam>
		/// <param name="first">The first element.</param>
		/// <param name="second">The second element.</param>
		/// <returns><see langword="true"/> if <paramref name="first"/> is neighbouring <paramref name="second"/> and shares the same <typeparamref name="TDiscriminator"/>; otherwise, <see langword="false"/>.</returns>
		public static Boolean IsKindNeighbour<TDiscriminator>(this IDomElement<TDiscriminator> first, IDomElement<TDiscriminator> second)
		{
			first.ThrowIfDefault(nameof(first));
			second.ThrowIfDefault(nameof(second));

			var result = first.IsNeighbour(second) && first.KindEquals(second);

			return result;
		}
		/// <summary>
		/// Returns wether or not two <see cref="IDomElement{TDiscriminator}"/>s are neighbours.
		/// </summary>
		/// <typeparam name="TDiscriminator">The discriminator by which to distinguish elements.</typeparam>
		/// <param name="first">The first element.</param>
		/// <param name="second">The second element.</param>
		/// <returns><see langword="true"/> if <paramref name="first"/> is neighbouring <paramref name="second"/>; otherwise, <see langword="false"/>.</returns>
		public static Boolean IsNeighbour<TDiscriminator>(this IDomElement<TDiscriminator> first, IDomElement<TDiscriminator> second)
		{
			first.ThrowIfDefault(nameof(first));
			second.ThrowIfDefault(nameof(second));

			var result = first.IsDisjunct(second) && first.DistanceTo(second) == 0;

			return result;
		}

		/// <summary>
		/// Concatenates a neighbouring <see cref="IDomElement{TDiscriminator}"/> of the same kind with this one.
		/// </summary>
		/// <param name="element">The first element to concatenate.</param>
		/// <param name="other">The second element to concatenate.</param>
		/// <typeparam name="TDiscriminator">
		/// The discriminator by which to distinguish elements.
		/// </typeparam>
		/// <returns>A new, concatenated element.</returns>
		public static IDomElement<TDiscriminator> Concat<TDiscriminator>(this IDomElement<TDiscriminator> element, IDomElement<TDiscriminator> other)
		{
			element.ThrowIfDefault(nameof(element));
			other.ThrowIfDefaultOrNot(e => e.KindEquals(element), $"{nameof(other)} was of kind {other.Kind}, but {element.Kind} was expected.", nameof(other));
			other.ThrowIfNot(e => e.IsKindNeighbour(element), $"{nameof(other)} must be neighbour of {nameof(element)}.", nameof(other));

			IStringSlice slice;

			if (element.Slice.Value == other.Slice.Value)
			{
				var start = Math.Min(element.Slice.Start, other.Slice.Start);
				var length = Math.Max(element.Slice.Start + element.Slice.Length, other.Slice.Start + other.Slice.Length) - start;
				slice = element.Slice.ReSlice(start, length);
			}
			else
			{
				slice = String.Concat(element.Slice, other.Slice).Slice();
			}

			var position = Math.Min(element.Position, other.Position);
			var kind = element.Kind;
			var result = new DomElement<TDiscriminator>(kind, slice, position);

			return result;
		}
		/// <summary>
		/// Normalizes an instance of <see cref="IDom{TDiscriminator}"/> to its <see cref="IDom{TDiscriminator}.ChunkSize"/>.
		/// </summary>
		/// <typeparam name="TDiscriminator">
		/// The discriminator by which to distinguish elements.
		/// </typeparam>
		/// <param name="dom">The instance to normalize.</param>
		/// <returns>A new, normalized instance of <paramref name="dom"/>.</returns>
		public static IDom<TDiscriminator> Normalize<TDiscriminator>(this IDom<TDiscriminator> dom)
		{
			var result = dom.Normalize(dom.ChunkSize);

			return result;
		}
		/// <summary>
		/// Normalizes an instance of <see cref="IDom{TDiscriminator}"/> to <paramref name="chunkSize"/>.
		/// </summary>
		/// <typeparam name="TDiscriminator">
		/// The discriminator by which to distinguish elements.
		/// </typeparam>
		/// <param name="dom">The instance to normalize.</param>
		/// <param name="chunkSize">The chunk size to normalize to.</param>
		/// <returns>A new, normalized instance of <paramref name="dom"/>.</returns>
		public static IDom<TDiscriminator> Normalize<TDiscriminator>(this IDom<TDiscriminator> dom, Int32 chunkSize)
		{
			chunkSize.ThrowIfNot(s => s > 0, $"{nameof(chunkSize)} must be > 0", nameof(chunkSize));

			var result = new Dom<TDiscriminator>(chunkSize);
			var position = 0;

			foreach (var element in dom)
			{
				var subElements = Enumerable.Range(element.Position, element.Slice.Length)
					.GroupBy(i => i / chunkSize)
					.Select(g => g.ToArray())
					.Select(g => (position: g[0], length: g[^1] - g[0] + 1))
					.Select(t => (t.position, start: element.Slice.Start + t.position - element.Position, t.length));

				foreach (var subElement in subElements)
				{
					var normalized = new DomElement<TDiscriminator>(element.Kind, element.Slice.ReSlice(subElement.start, subElement.length), subElement.position);
					result.Add(normalized);
					position = normalized.GetEnd();
				}
			}

			return result;
		}

		/// <summary>
		/// Shifts an elements position by a specified value.
		/// </summary>
		/// <typeparam name="TDiscriminator">
		/// The discriminator by which to distinguish elements.
		/// </typeparam>
		/// <param name="element">The element to shift.</param>
		/// <param name="offset">The offset by which to shift <paramref name="element"/>.</param>
		/// <returns>A new element based on <paramref name="element"/> and shifted b y <paramref name="offset"/>.</returns>
		public static IDomElement<TDiscriminator> Shift<TDiscriminator>(this IDomElement<TDiscriminator> element, Int32 offset)
		{
			element.ThrowIfDefault(nameof(element));

			var result = new DomElement<TDiscriminator>(element.Kind, element.Slice, element.Position + offset);

			return result;
		}
	}
}
