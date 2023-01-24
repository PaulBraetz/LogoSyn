using RhoMicro.LogoSyn.Libs.Common.Strings;
using RhoMicro.LogoSyn.Libs.Dom.Dom.Abstractions;
using RhoMicro.LogoSyn.Libs.Dom.Dom.Comparers;
using RhoMicro.LogoSyn.Libs.Dom.Tests.Mocks;

namespace RhoMicro.LogoSyn.Libs.Dom.Tests;

[TestClass()]
public class DomElementComparerTests
{
	public static IEnumerable<Object[]> EqualElements {
		get {
			return new Object[][]
			{
				new Object[] {"4875", 0, 4, 0, Discriminators.Default.Literal, "3478", 0, 4, 0, Discriminators.Default.Code},
				new Object[] {"9823", 0, 1, 3, Discriminators.Default.Literal, "3405", 1, 1, 3, Discriminators.Default.Literal},
				new Object[] {"fjfj", 0, 3, 5, Discriminators.Default.Literal, "9823", 0, 3, 5, Discriminators.Default.Literal},
				new Object[] {"3405", 2, 2, 2, Discriminators.Default.Literal, "029t", 2, 2, 2, Discriminators.Default.Literal},
				new Object[] {"982t", 3, 1, 7, Discriminators.Default.Literal, "3405", 0, 1, 7, Discriminators.Default.Literal},
				new Object[] {"dtkj", 1, 3, 8, Discriminators.Default.Literal, "tdfk", 0, 3, 8, Discriminators.Default.Literal},
				new Object[] {"dtkd", 0, 3, 0, Discriminators.Default.Literal, "ters", 1, 3, 0, Discriminators.Default.Literal}
			};
		}
	}
	public static IEnumerable<Object[]> PrecedingElements {
		get {
			return new Object[][]
			{
				new Object[] {"4875", 0, 4, 0, Discriminators.Default.Literal, "3478", 0, 4, 5, Discriminators.Default.Code},
				new Object[] {"9823", 0, 1, 3, Discriminators.Default.Literal, "3405", 1, 3, 5, Discriminators.Default.Literal},
				new Object[] {"fjfj", 0, 3, 5, Discriminators.Default.Literal, "9823", 0, 3, 9, Discriminators.Default.Literal},
				new Object[] {"3405", 2, 2, 2, Discriminators.Default.Literal, "029t", 2, 2, 6, Discriminators.Default.Literal},
				new Object[] {"982t", 3, 1, 7, Discriminators.Default.Literal, "3405", 0, 1, 9, Discriminators.Default.Literal},
				new Object[] {"dtkj", 1, 3, 8, Discriminators.Default.Literal, "tdfk", 0, 3, 12, Discriminators.Default.Literal},
				new Object[] {"dtkd", 0, 3, 0, Discriminators.Default.Literal, "ters", 1, 3, 4, Discriminators.Default.Literal}
			};
		}
	}
	[TestMethod()]
	[DynamicData(nameof(EqualElements))]
	public void Equality(String valueX, Int32 startX, Int32 lengthX, Int32 positionX, Discriminators.Default kindX,
						 String valueY, Int32 startY, Int32 lengthY, Int32 positionY, Discriminators.Default kindY)
	{
		var x = (IDomElement<Discriminators.Default>)new DomElement(kindX, valueX.Slice(startX, lengthX), positionX);
		var y = (IDomElement<Discriminators.Default>)new DomElement(kindY, valueY.Slice(startY, lengthY), positionY);

		var equalityComparer = DomElementEqualityComparer<Discriminators.Default>.Instance;
		Assert.IsTrue(equalityComparer.Equals(y, y));
		Assert.IsTrue(equalityComparer.Equals(y, x));
		Assert.IsTrue(equalityComparer.Equals(x, y));
		Assert.IsTrue(equalityComparer.Equals(x, x));

		var comparer = DomElementComparer<Discriminators.Default>.Instance;
		Assert.AreEqual(0, comparer.Compare(y, y));
		Assert.AreEqual(0, comparer.Compare(y, x));
		Assert.AreEqual(0, comparer.Compare(x, y));
		Assert.AreEqual(0, comparer.Compare(x, x));
	}
	[TestMethod()]
	[DynamicData(nameof(PrecedingElements))]
	public void Precedence(String valueX, Int32 startX, Int32 lengthX, Int32 positionX, Discriminators.Default kindX,
						   String valueY, Int32 startY, Int32 lengthY, Int32 positionY, Discriminators.Default kindY)
	{
		var x = (IDomElement<Discriminators.Default>)new DomElement(kindX, valueX.Slice(startX, lengthX), positionX);
		var y = (IDomElement<Discriminators.Default>)new DomElement(kindY, valueY.Slice(startY, lengthY), positionY);

		var comparer = DomElementComparer<Discriminators.Default>.Instance;
		Assert.AreEqual(-1, comparer.Compare(x, y));
		Assert.AreEqual(1, comparer.Compare(y, x));
	}
	[TestMethod()]
	[DynamicData(nameof(PrecedingElements))]
	public void Inequality(String valueX, Int32 startX, Int32 lengthX, Int32 positionX, Discriminators.Default kindX,
						   String valueY, Int32 startY, Int32 lengthY, Int32 positionY, Discriminators.Default kindY)
	{
		var x = (IDomElement<Discriminators.Default>)new DomElement(kindX, valueX.Slice(startX, lengthX), positionX);
		var y = (IDomElement<Discriminators.Default>)new DomElement(kindY, valueY.Slice(startY, lengthY), positionY);

		var comparer = DomElementComparer<Discriminators.Default>.Instance;
		Assert.AreNotEqual(0, comparer.Compare(x, y));
		Assert.AreNotEqual(0, comparer.Compare(y, x));

		var equalityComparer = DomElementEqualityComparer<Discriminators.Default>.Instance;
		Assert.IsFalse(equalityComparer.Equals(x, y));
		Assert.IsFalse(equalityComparer.Equals(y, x));
	}
}