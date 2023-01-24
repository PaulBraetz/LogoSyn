namespace RhoMicro.LogoSyn.Libs.Dom.Dom.Abstractions;

/// <summary>
/// A collection of sequential document elements.
/// </summary>
/// <typeparam name="TDiscriminator">
/// The discriminator by which to distinguish elements.
/// </typeparam>
public interface IDom<TDiscriminator> : IEnumerable<IDomElement<TDiscriminator>>
{
	/// <summary>
	/// Adds an element to the dom. If the element intersects existing elements, they shall be consolidated.<br/>
	/// This means that there is no guarantee that enumerating over this Object will yield elements equal to those passed to <see cref="Add"/>.
	/// </summary>
	/// <param name="element">The element to add.</param>
	void Add(IDomElement<TDiscriminator> element);
	/// <summary>
	/// Removes the elements intersecting a span of indices.
	/// </summary>
	/// <param name="position">The first character index of the elements to be removed.</param>
	/// <param name="length">The length of the span that elements to be removed must intersect.</param>
	/// <returns>The elements removed at <paramref name="position"/> to <paramref name="position"/> + <paramref name="length"/>.</returns>
	IEnumerable<IDomElement<TDiscriminator>> Remove(Int32 position, Int32 length = 0);
	/// <summary>
	/// Attempts to retrieve the <see cref="IDomElement{TDiscriminator}"/> at position <paramref name="position"/>.
	/// </summary>
	/// <param name="position">The position to look for an element at.</param>
	/// <param name="element">The element at <paramref name="position"/>, if was found; otherwise, <see langword="null"/>.</param>
	/// <returns><see langword="true"/> if there exists an element at <paramref name="position"/>; otherwise, <see langword="false"/>.</returns>
	Boolean TryGet(Int32 position, out IDomElement<TDiscriminator>? element);
	/// <summary>
	/// Returns the amount of elements in the document.
	/// </summary>
	/// <returns>The amount of elements in this document</returns>
	Int32 Count();
	/// <summary>
	/// Clears the document of all elements.
	/// </summary>
	void Clear();
	/// <summary>
	/// The size of chunks in this document.
	/// </summary>
	Int32 ChunkSize {
		get;
	}
}