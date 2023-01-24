using RhoMicro.LogoSyn.Libs.Common.Strings;
using RhoMicro.LogoSyn.Libs.Dom.Dom.Comparers;

namespace RhoMicro.LogoSyn.Libs.Dom.Dom.Abstractions;

/// <summary>
/// Describes a LogoSyn element.
/// </summary>
/// <typeparam name="TDiscriminator">
/// The discriminator by which to distinguish elements.
/// </typeparam>
public interface IDomElement<TDiscriminator>
{
	/// <summary>
	/// The elements kind.
	/// </summary>
	TDiscriminator Kind {
		get;
	}
	/// <summary>
	/// The elements value.
	/// </summary>
	IStringSlice Slice {
		get;
	}
	/// <summary>
	/// The elements position in the document.
	/// </summary>
	Int32 Position {
		get;
	}
	/// <summary>
	/// Returns whether or not this element is a subset of <paramref name="element"/>, that is, it is contained within <paramref name="element"/>.
	/// </summary>
	/// <param name="element">The element to compare against.</param>
	/// <returns><see langword="true"/> if this element is a subset of <paramref name="element"/>; otherwise, <see langword="false"/>.</returns>
	sealed Boolean SubsetOf(IDomElement<TDiscriminator> element)
	{
		if(element == null)
			return false;

		var isSubset = Position >= element.Position &&
			Position + Slice.Length <= element.GetEnd();

		return isSubset;
	}
	/// <summary>
	/// Returns whether or not this element is disjunct from <paramref name="element"/>.
	/// </summary>
	/// <param name="element">The element to compare against.</param>
	/// <returns><see langword="true"/> if this element is disjunct from <paramref name="element"/>; otherwise, <see langword="false"/>.</returns>
	sealed Boolean IsDisjunct(IDomElement<TDiscriminator> element)
	{
		if(element == null)
			return true;

		var isDisjunct = Position >= element.GetEnd() ||
			element.Position >= this.GetEnd();

		return isDisjunct;
	}
	/// <summary>
	/// Indicates wether or an element precedes a second element.
	/// </summary>
	/// <param name="left">The lefthand operand.</param>
	/// <param name="right">The righthand operand.</param>
	/// <returns><see langword="true"/> if <paramref name="left"/> precedes <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
	static Boolean operator <(IDomElement<TDiscriminator> left, IDomElement<TDiscriminator> right)
	{
		var result = DomElementComparer<TDiscriminator>.Instance.Compare(left, right) < 0;

		return result;
	}
	/// <summary>
	/// Indicates wether or an element precedes or is equal to a second element.
	/// </summary>
	/// <param name="left">The lefthand operand.</param>
	/// <param name="right">The righthand operand.</param>
	/// <returns><see langword="true"/> if <paramref name="left"/> precedes or is equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
	static Boolean operator <=(IDomElement<TDiscriminator> left, IDomElement<TDiscriminator> right)
	{
		var result = DomElementComparer<TDiscriminator>.Instance.Compare(left, right) <= 0;

		return result;
	}
	/// <summary>
	/// Indicates wether or an element succedes a second element.
	/// </summary>
	/// <param name="left">The lefthand operand.</param>
	/// <param name="right">The righthand operand.</param>
	/// <returns><see langword="true"/> if <paramref name="left"/> succedes <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
	static Boolean operator >(IDomElement<TDiscriminator> left, IDomElement<TDiscriminator> right)
	{
		var result = DomElementComparer<TDiscriminator>.Instance.Compare(left, right) > 0;

		return result;
	}
	/// <summary>
	/// Indicates wether or an element succedes or is equal to a second element.
	/// </summary>
	/// <param name="left">The lefthand operand.</param>
	/// <param name="right">The righthand operand.</param>
	/// <returns><see langword="true"/> if <paramref name="left"/> succedes or is equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
	static Boolean operator >=(IDomElement<TDiscriminator> left, IDomElement<TDiscriminator> right)
	{
		var result = DomElementComparer<TDiscriminator>.Instance.Compare(left, right) >= 0;

		return result;
	}
}