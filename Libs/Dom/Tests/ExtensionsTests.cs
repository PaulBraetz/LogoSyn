using RhoMicro.LogoSyn.Libs.Common.Strings;
using RhoMicro.LogoSyn.Libs.Dom.Abstractions;
using RhoMicro.LogoSyn.Libs.Dom.Tests.Mocks;

namespace RhoMicro.LogoSyn.Libs.Dom.Tests
{
	[TestClass]
	public class ExtensionsTests
	{
		public static IEnumerable<Object[]> Intersecting
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
		public static IEnumerable<Object[]> NotIntersecting
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
		public static IEnumerable<Object[]> KindEqualsElements
		{
			get
			{
				return new Object[][]
				{
					new Object[]{ Discriminators.Default.Literal , Discriminators.Default.Literal},
					new Object[]{ Discriminators.Default.Code , Discriminators.Default.Code},
					new Object[]{ Discriminators.Default.Directive, Discriminators.Default.Directive},
					new Object[]{ Discriminators.Default.Display, Discriminators.Default.Display }
				};
			}
		}
		public static IEnumerable<Object[]> KindNotEqualsElements
		{
			get
			{
				return new Object[][]
				{
					new Object[]{Discriminators.Default.Literal, Discriminators.Default.Code},
					new Object[]{ Discriminators.Default.Literal, Discriminators.Default.Directive},
					new Object[]{ Discriminators.Default.Literal, Discriminators.Default.Display },
					new Object[]{ Discriminators.Default.Code, Discriminators.Default.Literal},
					new Object[]{ Discriminators.Default.Code, Discriminators.Default.Display },
					new Object[]{ Discriminators.Default.Code, Discriminators.Default.Directive},
					new Object[]{ Discriminators.Default.Display, Discriminators.Default.Code },
					new Object[]{ Discriminators.Default.Display, Discriminators.Default.Directive},
					new Object[]{ Discriminators.Default.Display, Discriminators.Default.Literal },
					new Object[]{ Discriminators.Default.Directive, Discriminators.Default.Code },
					new Object[]{ Discriminators.Default.Directive, Discriminators.Default.Display},
					new Object[]{ Discriminators.Default.Directive, Discriminators.Default.Literal }
				};
			}
		}
		public static IEnumerable<Object[]> Distances
		{
			get
			{
				return new Object[][]
				{
					new Object[]{0, 0, 1, 0},
					new Object[]{0, 5, 6, 5},
					new Object[]{9, 4, 7, 1},
					new Object[]{6, 3, 1, 4}
				};
			}
		}

		[TestMethod]
		[DynamicData(nameof(Intersecting))]
		public void IntersectsTest(Int32 positionX, Int32 lengthX, Int32 positionY, Int32 lengthY)
		{
			var value = "012345678901234567890123456789";
			var kind = Discriminators.Default.Literal;

			var x = (IDomElement<Discriminators.Default>)new DomElement(kind, value.Slice(lengthX), positionX);
			var y = (IDomElement<Discriminators.Default>)new DomElement(kind, value.Slice(lengthY), positionY);

			Assert.IsTrue(x.Intersects(y));
		}
		[TestMethod]
		[DynamicData(nameof(NotIntersecting))]
		public void NotIntersectsTest(Int32 positionX, Int32 lengthX, Int32 positionY, Int32 lengthY)
		{
			var value = "012345678901234567890123456789";
			var kind = Discriminators.Default.Literal;

			var x = (IDomElement<Discriminators.Default>)new DomElement(kind, value.Slice(lengthX), positionX);
			var y = (IDomElement<Discriminators.Default>)new DomElement(kind, value.Slice(lengthY), positionY);

			Assert.IsFalse(x.Intersects(y));
		}
		[TestMethod]
		[DynamicData(nameof(KindEqualsElements))]
		public void KindEqualsTest(Discriminators.Default kindX, Discriminators.Default kindY)
		{
			var x = new DomElement(kindX, String.Empty.Slice(), 0);
			var y = new DomElement(kindY, String.Empty.Slice(), 0);

			Assert.IsTrue(x.KindEquals(y));
		}

		[TestMethod]
		[DynamicData(nameof(KindNotEqualsElements))]
		public void KindNotEqualsTest(Discriminators.Default kindX, Discriminators.Default kindY)
		{
			var x = new DomElement(kindX, String.Empty.Slice(), 0);
			var y = new DomElement(kindY, String.Empty.Slice(), 0);

			Assert.IsFalse(x.KindEquals(y));
		}

		[TestMethod]
		public void GetEndTest()
		{
			var value = "1234567890";

			for (var position = 0; position < 10; position++)
			{
				for (var length = 0; length < 10; length++)
				{
					var slice = value.Slice(length);
					var element = new RhoMicro.LogoSyn.Libs.Dom.Tests.Mocks.DomElement(Discriminators.Default.Literal, slice, position);

					var expected = position + length;
					Assert.AreEqual(expected, element.GetEnd());
				}
			}
		}

		[TestMethod]
		[DynamicData(nameof(Distances))]
		public void DistanceToTest(Int32 positionX, Int32 lengthX, Int32 positionY, Int32 lengthY)
		{
			var value = "1234567890";
			var x = new DomElement(Discriminators.Default.Literal, value.Slice(lengthX), positionX);
			var y = new DomElement(Discriminators.Default.Literal, value.Slice(lengthY), positionY);

			var expected = Math.Max(positionX - (positionY + lengthY), positionY - (positionX + lengthX));
			var actual = x.DistanceTo(y);

			Assert.AreEqual(expected, actual);
			Assert.AreEqual(actual, y.DistanceTo(x));
		}
	}
}
