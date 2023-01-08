using RhoMicro.LogoSyn.Libs.Common.Strings;

namespace RhoMicro.LogoSyn.Libs.Dom.Tests
{
	[TestClass]
	public class StringSliceTests
	{
		public static IEnumerable<Object[]> Slices
		{
			get
			{
				return new Object[][]
				{
					new Object[] {"4875623490mcnbnv34285619567m788", 0, 4},
					new Object[] {"347958837rejrnluiriv65655489568", 4, 4},
					new Object[] {"9823xcm457ujrjrjnjrew98m47587vn", 6, 1},
					new Object[] {"34059b8hn5hzr1000jrj6n65m43nc23", 3, 2 },
					new Object[] {"fjfjrftjrtzj8u65567u576ujjzzrkj", 1, 3},
					new Object[] {"9823xcziöl6tzzu0cvßiuölum4587vn", 6, 3},
					new Object[] {"3405kfgtk,gfk9tktkt0rkb8m43nc23", 2, 7 },
					new Object[] {"029tzdijtz73v498n753478cmn48723", 5, 7},
					new Object[] {"982tgfkt4059386m3vc5d3xcm4587vn", 2, 13},
					new Object[] {"3405568ß098mce823nb4xu5678u9b23", 7, 8 },
					new Object[] {"dtkjdt34589c2n4327bvc7843mcktz6", 2, 3},
					new Object[] {"tdfkdtmifdsgmybnkmj0473vnkdtkdt", 4, 6},
					new Object[] {"dtkdtmc43890mxrnnv2mrnv87nmghj7", 3, 5 },
					new Object[] {"tershes908523x2384mnc7n9hbetnmq", 2, 9},
				};
			}
		}
		public static IEnumerable<Object[]> Shifts
		{
			get
			{
				return new Object[][]
				{
					new Object[] {"4875623490mcnbnv34285619567m788", 0, 0, 3},
					new Object[] {"347958837rejrnluiriv65655489568", 4, 4, 1},
					new Object[] {"9823xcm457ujrjrjnjrew98m47587vn", 6, 6, 0},
					new Object[] {"34059b8hn5hzr1000jrj6n65m43nc23", 3, 3, 3},
					new Object[] {"fjfjrftjrtzj8u65567u576ujjzzrkj", 1, 1, 5},
					new Object[] {"9823xcziöl6tzzu0cvßiuölum4587vn", 6, 6, 5},
					new Object[] {"3405kfgtk,gfk9tktkt0rkb8m43nc23", 2, 2, 1},
					new Object[] {"029tzdijtz73v498n753478cmn48723", 5, 5, 4},
					new Object[] {"982tgfkt4059386m3vc5d3xcm4587vn", 2, 2, 0},
					new Object[] {"3405568ß098mce823nb4xu5678u9b23", 7, 7, 3},
					new Object[] {"dtkjdt34589c2n4327bvc7843mcktz6", 2, 2, 4},
					new Object[] {"tdfkdtmifdsgmybnkmj0473vnkdtkdt", 4, 4, 4},
					new Object[] {"dtkdtmc43890mxrnnv2mrnv87nmghj7", 3, 3, 5},
					new Object[] {"tershes908523x2384mnc7n9hbetnmq", 2, 2, 3},
				};
			}
		}
		public static IEnumerable<Object[]> Resizes
		{
			get
			{
				return new Object[][]
				{
					new Object[] {"4875623490mcnbnv34285619567m788", 0, 0, 3},
					new Object[] {"347958837rejrnluiriv65655489568", 4, 4, 1},
					new Object[] {"9823xcm457ujrjrjnjrew98m47587vn", 6, 6, 0},
					new Object[] {"34059b8hn5hzr1000jrj6n65m43nc23", 3, 3, 3},
					new Object[] {"fjfjrftjrtzj8u65567u576ujjzzrkj", 1, 1, 5},
					new Object[] {"9823xcziöl6tzzu0cvßiuölum4587vn", 6, 6, 5},
					new Object[] {"3405kfgtk,gfk9tktkt0rkb8m43nc23", 2, 2, 1},
					new Object[] {"029tzdijtz73v498n753478cmn48723", 5, 5, 4},
					new Object[] {"982tgfkt4059386m3vc5d3xcm4587vn", 2, 2, 0},
					new Object[] {"3405568ß098mce823nb4xu5678u9b23", 7, 7, 3},
					new Object[] {"dtkjdt34589c2n4327bvc7843mcktz6", 2, 2, 4},
					new Object[] {"tdfkdtmifdsgmybnkmj0473vnkdtkdt", 4, 4, 4},
					new Object[] {"dtkdtmc43890mxrnnv2mrnv87nmghj7", 3, 3, 5},
					new Object[] {"tershes908523x2384mnc7n9hbetnmq", 2, 2, 3},
				};
			}
		}
		public static IEnumerable<Object[]> ImplicitSliceValues
		{
			get
			{
				return new Object[][]
				{
					new Object[] {String.Empty},
					new Object[] {""},
					new Object[] {"487nbnv34285619567m788"},
					new Object[] {"347958837rejririv65655489568"},
					new Object[] {"9823xcm457ujrjrjnjrew98m47587vn"},
					new Object[] {"34059b8hn5h00jrj6n65m43nc23"},
					new Object[] {"fjfjrftjrtzj8u65567u57jjzzrkj"},
					new Object[] {"9826tzzu0cvßiuölum4587vn"},
					new Object[] {"3405kfgtk,gfk9tkrkb8m43nc23"},
					new Object[] {"029tzdijtz73v498753478cmn48723"},
					new Object[] {"982tgfkt4059386m3vc5d3xcm4587vn"},
					new Object[] {"3405568ß098mce823nb4xu5678u9b23"},
					new Object[] {"dtkjdt34589c2n4327bvc7843mj0473vnkdtkdt"},
					new Object[] {"dtkdtmc43890mxrnnv2mrnv87nmghj7tershes908523x2384mnc7n9hbetnmq"}
				};
			}
		}

		public static IEnumerable<Object[]> NewSliceValues
		{
			get
			{
				return new Object[][]
				{
					new Object[] {String.Empty},
					new Object[] {""},
					new Object[] {"487nbnv34285619567m788", 4, 10},
					new Object[] {"347958837rejririv65655489568"},
					new Object[] {"9823xcm457ujrjrjnjrew98m47587vn"},
					new Object[] {"34059b8hn5h00jrj6n65m43nc23"},
					new Object[] {"fjfjrftjrtzj8u65567u57jjzzrkj"},
					new Object[] {"9826tzzu0cvßiuölum4587vn"},
					new Object[] {"3405kfgtk,gfk9tkrkb8m43nc23"},
					new Object[] {"029tzdijtz73v498753478cmn48723"},
					new Object[] {"982tgfkt4059386m3vc5d3xcm4587vn"},
					new Object[] {"3405568ß098mce823nb4xu5678u9b23"},
					new Object[] {"dtkjdt34589c2n4327bvc7843mj0473vnkdtkdt"},
					new Object[] {"dtkdtmc43890mxrnnv2mrnv87nmghj7tershes908523x2384mnc7n9hbetnmq"}
				};
			}
		}

		[TestMethod]
		[DynamicData(nameof(ImplicitSliceValues))]
		public void ImplicitSlice(String value)
		{
			var slice = value.Slice();
			AssertSlice(slice, value, 0, value.Length);
		}
		[TestMethod]
		[DynamicData(nameof(Slices))]
		public void NewSlice(String value, Int32 start, Int32 length)
		{
			var slice = value.Slice(start, length);
			AssertSlice(slice, value, start, length);
		}
		private void AssertSlice(IStringSlice slice, String value, Int32 start, Int32 length)
		{
			var expected = value.Substring(start, length);

			Assert.AreEqual(length, slice.Length);
			Assert.AreEqual(start, slice.Start);
			Assert.AreEqual(value, slice.Value);
			Assert.AreEqual(expected, slice.ToString());
		}
		[TestMethod]
		[DynamicData(nameof(Shifts))]
		public void Shift(String value, Int32 start, Int32 length, Int32 newStart)
		{
			var slice = value.Slice(start, length).Shift(newStart).ToString();
			var expected = value.Substring(newStart, length);
			Assert.AreEqual(expected, slice);
		}
		[TestMethod]
		[DynamicData(nameof(Resizes))]
		public void Resize(String value, Int32 start, Int32 length, Int32 newLength)
		{
			var slice = value.Slice(start, length).Resize(newLength).ToString();
			var expected = value.Substring(start, newLength);
			Assert.AreEqual(expected, slice);
		}
	}
}
