using Fort;
using RhoMicro.LogoSyn.Libs.Common.Strings;
using RhoMicro.LogoSyn.Libs.Dom.Abstractions;
using RhoMicro.LogoSyn.Libs.Dom.Comparers;
using System.Collections;

namespace RhoMicro.LogoSyn.Libs.Dom
{
	/// <summary>
	/// Data structure for normalizing dom elements onto elements of a set size.
	/// </summary>
	/// <typeparam name="TDiscriminator">
	/// The discriminator by which to distinguish elements.
	/// </typeparam>
	internal sealed class Chunk<TDiscriminator> : IEquatable<Chunk<TDiscriminator>?>, IEnumerable<IDomElement<TDiscriminator>>
	{
		public Chunk(Int32 line, Int32 size)
		{
			line.ThrowIfNot(l => l >= 0, $"{nameof(line)} must be abovve or equal to 0.");
			size.ThrowIfNot(s => s >= 0, $"{nameof(size)} must be abovve or equal to 0.");

			Position = line;
			Size = size;
		}

		public readonly Int32 Position;
		public readonly Int32 Size;

		public Int32 AbsoluteLoad => _elements.Count;

		private readonly ISet<IDomElement<TDiscriminator>> _elements = new HashSet<IDomElement<TDiscriminator>>(DomElementEqualityComparer<TDiscriminator>.Instance);

		/// <summary>
		/// Adds an element to the chunk.
		/// </summary>
		/// <param name="element">The element to add to the chunk.</param>
		/// <exception cref="ArgumentNullException">if <paramref name="element"/> is null</exception>
		/// <exception cref="ArgumentException">if <paramref name="element"/> does not intersect the chunk</exception>
		public void Add(IDomElement<TDiscriminator> element)
		{
			if (element is null)
			{
				throw new ArgumentNullException(nameof(element));
			}

			if (element.Position > Position + Size || element.GetEnd() < Position)
			{
				throw new ArgumentException($"{nameof(element)} does not intersect this chunk.");
			}

			if (_elements.Contains(element))
			{
				_ = _elements.Remove(element);
				_ = _elements.Add(element);
			}
			else
			{
				Consolidate(element);
			}
		}

		/// <summary>
		/// Consolidates intersecting elements resulting from adding <paramref name="element"/> by creating new, disjunct elements.
		/// </summary>
		/// <param name="element">The element to add.</param>
		private void Consolidate(IDomElement<TDiscriminator> element)
		{
			if (element.Slice.Length == 0)
			{
				return;
			}

			//yields subsets, intersections and neighbours
			var conflicts = _elements.Where(e => e.Intersects(element) || e.IsKindNeighbour(element)).ToArray();

			if (conflicts.Length == 0)
			{
				_ = _elements.Add(element);
				return;
			}

			Int32 position;
			Int32 start;
			Int32 length;
			TDiscriminator kind;
			IStringSlice slice;
			IDomElement<TDiscriminator> reducedConflict;

			foreach (var conflict in conflicts)
			{
				_ = _elements.Remove(conflict);

				if (conflict.IsKindNeighbour(element))
				{
					//Neighbour of same kind
					element = DomElementComparer<TDiscriminator>.Instance.Compare(conflict, element) < 0
						? conflict.Concat(element)
						: element.Concat(conflict);
				}
				else if (!conflict.SubsetOf(element))
				{
					//Intersection
					kind = conflict.Kind;

					if (conflict.Position < element.Position)
					{
						//preceding intersection
						position = conflict.Position;
						length = element.Position - conflict.Position;
						start = conflict.Slice.Start;
						slice = conflict.Slice.Shift(start).Resize(length);
						reducedConflict = new DomElement<TDiscriminator>(kind, slice, position);
						_ = _elements.Add(reducedConflict);
					}

					if (conflict.GetEnd() > element.Position)
					{
						//succeding intersection
						position = element.GetEnd();
						length = conflict.GetEnd() - element.Position - element.Slice.Length;
						start = conflict.Slice.Start + conflict.Slice.Length - length;
						slice = conflict.Slice.Resize(length).Shift(start);
						reducedConflict = new DomElement<TDiscriminator>(kind, slice, position);
						_ = _elements.Add(reducedConflict);
					}
				}
			}

			Consolidate(element);
		}

		public Boolean TryGet(Int32 position, out IDomElement<TDiscriminator>? element)
		{
			element = _elements.SingleOrDefault(e => e.Position <= position && e.GetEnd() > position);

			return element != null;
		}

		public Boolean TryRemove(IDomElement<TDiscriminator> element)
		{
			var result = _elements.Remove(element);

			return result;
		}

		public override Boolean Equals(Object? obj)
		{
			return Equals(obj as Chunk<TDiscriminator>);
		}

		public Boolean Equals(Chunk<TDiscriminator>? other)
		{
			return other is not null &&
				   Position == other.Position;
		}

		public override Int32 GetHashCode()
		{
			return HashCode.Combine(Position);
		}

		public IEnumerator<IDomElement<TDiscriminator>> GetEnumerator()
		{
			return _elements.OrderBy(e => e, DomElementComparer<TDiscriminator>.Instance).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)_elements).GetEnumerator();
		}
	}
}
