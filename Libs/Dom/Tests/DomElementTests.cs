using RhoMicro.LogoSyn.Libs.Common.Strings;
using RhoMicro.LogoSyn.Libs.Dom.Abstractions;
using RhoMicro.LogoSyn.Libs.Dom.Comparers;
using RhoMicro.LogoSyn.Libs.Dom.Tests.Mocks;

namespace RhoMicro.LogoSyn.Libs.Dom.Tests
{
	[TestClass]
	internal class DomElementTests
	{
		public static IEnumerable<Object[]> NotDisjuncts
		{
			get
			{
				return new Object[][]
				{
					new Object[] {1, 3, 1+3-2, 2},
					new Object[] {3, 5, 3+5-2, 3},
					new Object[] {2, 2, 2+2-1, 1},
					new Object[] {1, 7, 1+7-5, 5},
					new Object[] {3, 8, 3+8-3, 9},
					new Object[] {3, 0, 3+0-1, 4}
				};
			}
		}
		public static IEnumerable<Object[]> Disjuncts
		{
			get
			{
				return new Object[][]
				{
					new Object[] {1, 3, 1+3+2, 2},
					new Object[] {3, 5, 3+5+0, 3},
					new Object[] {2, 2, 2+2+10, 1},
					new Object[] {1, 7, 1+7+5, 5},
					new Object[] {3, 8, 3+8+3, 9},
					new Object[] {3, 0, 3+0+9, 4}
				};
			}
		}
		public static IEnumerable<Object[]> Subsets
		{
			get
			{
				return new Object[][]
				{
					new Object[] {"4875", 0, 4, 0, Discriminators.Default.Literal, "3478", 1, 2, 1, Discriminators.Default.Code},
					new Object[] {"9823", 0, 1, 3, Discriminators.Default.Literal, "3405", 1, 1, 3, Discriminators.Default.Literal},
					new Object[] {"fjfj", 0, 3, 5, Discriminators.Default.Literal, "9823", 0, 1, 6, Discriminators.Default.Literal},
					new Object[] {"3405", 2, 2, 2, Discriminators.Default.Literal, "029t", 2, 2, 2, Discriminators.Default.Literal},
					new Object[] {"982t", 3, 1, 7, Discriminators.Default.Literal, "3405", 0, 0, 7, Discriminators.Default.Literal},
					new Object[] {"dtkj", 1, 3, 8, Discriminators.Default.Literal, "tdfk", 0, 1, 9, Discriminators.Default.Literal},
					new Object[] {"dtkd", 0, 3, 0, Discriminators.Default.Literal, "ters", 1, 2, 0, Discriminators.Default.Literal}
				};
			}
		}
		public static IEnumerable<Object[]> EqualElements
		{
			get
			{
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

		[TestMethod]
		[DynamicData(nameof(Disjuncts))]
		public void Disjunct(Int32 positionX, Int32 lengthX, Int32 positionY, Int32 lengthY)
		{
			var value = "012345678901234567890123456789";
			var kind = Discriminators.Default.Literal;

			var x = (IDomElement<Discriminators.Default>)new DomElement(kind, value.Slice(lengthX), positionX);
			var y = (IDomElement<Discriminators.Default>)new DomElement(kind, value.Slice(lengthY), positionY);

			Assert.IsTrue(x.IsDisjunct(y));
			Assert.IsTrue(y.IsDisjunct(x));
		}
		[TestMethod]
		[DynamicData(nameof(NotDisjuncts))]
		public void NotDisjunct(Int32 positionX, Int32 lengthX, Int32 positionY, Int32 lengthY)
		{
			var value = "012345678901234567890123456789";
			var kind = Discriminators.Default.Literal;

			var x = (IDomElement<Discriminators.Default>)new DomElement(kind, value.Slice(lengthX), positionX);
			var y = (IDomElement<Discriminators.Default>)new DomElement(kind, value.Slice(lengthY), positionY);

			Assert.IsFalse(x.IsDisjunct(y));
			Assert.IsFalse(y.IsDisjunct(x));
		}

		[TestMethod]
		[DynamicData(nameof(Subsets))]
		public void SubsetOf(String valueX, Int32 startX, Int32 lengthX, Int32 positionX, Discriminators.Default kindX,
							 String valueY, Int32 startY, Int32 lengthY, Int32 positionY, Discriminators.Default kindY)
		{
			var x = (IDomElement<Discriminators.Default>)new DomElement(kindX, valueX.Slice(startX, lengthX), positionX);
			var y = (IDomElement<Discriminators.Default>)new DomElement(kindY, valueY.Slice(startY, lengthY), positionY);

			Assert.IsTrue(y.SubsetOf(x));
			if (DomElementEqualityComparer<Discriminators.Default>.Instance.Equals(x, y))
			{
				Assert.IsTrue(x.SubsetOf(y));
			}
		}
	}
}
