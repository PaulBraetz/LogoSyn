using RhoMicro.LogoSyn.Libs.Common.Strings;

namespace RhoMicro.LogoSyn.Libs.Common.Tests.StringSlice;

[TestClass]
public class StringSliceTests
{
	#region ValidSlice
	public static Object[][] ValidSlices_Value => Data.ValidSlices_Value;
	[TestMethod]
	[DynamicData(nameof(ValidSlices_Value))]
	public void ValidSlice_Value(String value)
	{
		var slice = value.Slice();

		AssertSliceValid(slice, value, 0, value.Length);
	}
	public static Object[][] ValidSlices_Value_Length => Data.ValidSlices_Value_Length;
	[TestMethod]
	[DynamicData(nameof(ValidSlices_Value_Length))]
	public void ValidSlice_Value_Length(String value, Int32 length)
	{
		var slice = value.Slice(length);

		AssertSliceValid(slice, value, 0, length);
	}
	public static Object[][] ValidSlices_Value_Start_Length => Data.ValidSlices_Value_Start_Length;
	[TestMethod]
	[DynamicData(nameof(ValidSlices_Value_Start_Length))]
	public void ValidSlice_Value_Start_Length(String value, Int32 start, Int32 length)
	{
		var slice = value.Slice(start, length);

		AssertSliceValid(slice, value, start, length);
	}
	#endregion

	#region InvalidSlice
	public static Object[][] InvalidSlices_Value => Data.InvalidSlices_Value;
	[TestMethod]
	[DynamicData(nameof(InvalidSlices_Value))]
	public void InvalidSlice_Value(String value)
	{
		_ = value == null
			? (Exception)Assert.ThrowsException<ArgumentNullException>(() => value!.Slice())
			: Assert.ThrowsException<ArgumentOutOfRangeException>(() => value.Slice());
	}
	public static Object[][] InvalidSlices_Value_Length => Data.InvalidSlices_Value_Length;
	[TestMethod]
	[DynamicData(nameof(InvalidSlices_Value_Length))]
	public void InvalidSlice_Value_Length(String value, Int32 length)
	{
		_ = value == null
			? (Exception)Assert.ThrowsException<ArgumentNullException>(() => value!.Slice(length))
			: Assert.ThrowsException<ArgumentOutOfRangeException>(() => value.Slice(length));
	}
	public static Object[][] InvalidSlices_Value_Start_Length => Data.InvalidSlices_Value_Start_Length;
	[TestMethod]
	[DynamicData(nameof(InvalidSlices_Value_Start_Length))]
	public void InvalidSlice_Value_Start_Length(String value, Int32 start, Int32 length)
	{
		_ = value == null
			? (Exception)Assert.ThrowsException<ArgumentNullException>(() => value!.Slice(start, length))
			: Assert.ThrowsException<ArgumentOutOfRangeException>(() => value.Slice(start, length));
	}
	#endregion

	#region EqualSlice
	[TestMethod]
	public void EqualSlice()
	{
		var x = String.Empty.Slice();
		var y = String.Empty.Slice();

		AssertSlicesEqual(x, y);
	}
	[TestMethod]
	[DynamicData(nameof(ValidSlices_Value))]
	public void EqualSlice_Value(String value)
	{
		var x = value.Slice();
		var y = value.Slice();

		AssertSlicesEqual(x, y);
	}
	[TestMethod]
	[DynamicData(nameof(ValidSlices_Value_Length))]
	public void EqualSlice_Value_Length(String value, Int32 length)
	{
		var x = value.Slice(length);
		var y = value.Slice(length);

		AssertSlicesEqual(x, y);
	}
	[TestMethod]
	[DynamicData(nameof(ValidSlices_Value_Start_Length))]
	public void EqualSlice_Value_Start_Length(String value, Int32 start, Int32 length)
	{
		var x = value.Slice(start, length);
		var y = value.Slice(start, length);

		AssertSlicesEqual(x, y);
	}
	#endregion

	#region Extensions
	#region Slice
	[TestMethod]
	[DynamicData(nameof(ValidSlices_Value))]
	public void Slice_Value(String value)
	{
		var x = value.Slice();
		var y = value.Slice();

		AssertSlicesEqual(x, y);
	}
	[TestMethod]
	[DynamicData(nameof(ValidSlices_Value_Length))]
	public void Slice_Value_Length(String value, Int32 length)
	{
		var x = value.Slice(length);
		var y = value.Slice(length);

		AssertSlicesEqual(x, y);
	}
	[TestMethod]
	[DynamicData(nameof(ValidSlices_Value_Start_Length))]
	public void Slice_Value_Start_Length(String value, Int32 start, Int32 length)
	{
		var x = value.Slice(start, length);
		var y = value.Slice(start, length);

		AssertSlicesEqual(x, y);
	}
	#endregion
	#region Resize
	public static IEnumerable<Object[]> ValidResize_Value_NewLength => Data.ValidResize_Value_NewLength;
	[TestMethod]
	[DynamicData(nameof(ValidResize_Value_NewLength))]
	public void ValidResize_Value(String value, Int32 newLength)
	{
		var x = value.Slice();
		var y = x.Resize(newLength);

		AssertSliceValid(y, x.Value, x.Start, newLength);
	}
	public static IEnumerable<Object[]> ValidResize_Value_Length_NewLength => Data.ValidResize_Value_Length_NewLength;
	[TestMethod]
	[DynamicData(nameof(ValidResize_Value_Length_NewLength))]
	public void ValidResize_Value_Length(String value, Int32 length, Int32 newLength)
	{
		var x = value.Slice(length);
		var y = x.Resize(newLength);

		AssertSliceValid(y, x.Value, x.Start, newLength);
	}
	public static IEnumerable<Object[]> ValidResize_Value_Start_Length_NewLength => Data.ValidResize_Value_Start_Length_NewLength;
	[TestMethod]
	[DynamicData(nameof(ValidResize_Value_Start_Length_NewLength))]
	public void ValidResize_Value_Start_Length(String value, Int32 start, Int32 length, Int32 newLength)
	{
		var x = value.Slice(start, length);
		var y = x.Resize(newLength);

		AssertSliceValid(y, x.Value, x.Start, newLength);
	}

	public static IEnumerable<Object[]> InvalidResize_Value_NewLength => Data.InvalidResize_Value_NewLength;
	[TestMethod]
	[DynamicData(nameof(InvalidResize_Value_NewLength))]
	public void InvalidResize_Value(String value, Int32 newLength)
	{
		var x = value.Slice();
		_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => x.Resize(newLength));
	}
	public static IEnumerable<Object[]> InvalidResize_Value_Length_NewLength => Data.InvalidResize_Value_Length_NewLength;
	[TestMethod]
	[DynamicData(nameof(InvalidResize_Value_Length_NewLength))]
	public void InvalidResize_Value_Length(String value, Int32 length, Int32 newLength)
	{
		var x = value.Slice(length);
		_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => x.Resize(newLength));
	}
	public static IEnumerable<Object[]> InvalidResize_Value_Start_Length_NewLength => Data.InvalidResize_Value_Start_Length_NewLength;
	[TestMethod]
	[DynamicData(nameof(InvalidResize_Value_Start_Length_NewLength))]
	public void InvalidResize_Value_Start_Length(String value, Int32 start, Int32 length, Int32 newLength)
	{
		var x = value.Slice(start, length);
		_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => x.Resize(newLength));
	}
	#endregion
	#region Shift
	public static IEnumerable<Object[]> ValidShift_Value_NewStart => Data.ValidShift_Value_NewStart;
	[TestMethod]
	[DynamicData(nameof(ValidShift_Value_NewStart))]
	public void ValidShift_Value(String value, Int32 newStart)
	{
		var x = value.Slice(value.Length - newStart);
		var y = x.Shift(newStart);

		AssertSliceValid(y, x.Value, newStart, x.Length);
	}
	public static IEnumerable<Object[]> ValidShift_Value_Length_NewStart => Data.ValidShift_Value_Length_NewStart;
	[TestMethod]
	[DynamicData(nameof(ValidShift_Value_Length_NewStart))]
	public void ValidShift_Value_Length(String value, Int32 length, Int32 newStart)
	{
		var x = value.Slice(length);
		var y = x.Shift(newStart);

		AssertSliceValid(y, x.Value, newStart, x.Length);
	}
	public static IEnumerable<Object[]> ValidShift_Value_Start_Length_NewStart => Data.ValidShift_Value_Start_Length_NewStart;
	[TestMethod]
	[DynamicData(nameof(ValidShift_Value_Start_Length_NewStart))]
	public void ValidShift_Value_Start_Length(String value, Int32 start, Int32 length, Int32 newStart)
	{
		var x = value.Slice(start, length);
		var y = x.Shift(newStart);

		AssertSliceValid(y, x.Value, newStart, x.Length);
	}

	public static IEnumerable<Object[]> InvalidShift_Value_NewStart => Data.InvalidShift_Value_NewStart;
	[TestMethod]
	[DynamicData(nameof(InvalidShift_Value_NewStart))]
	public void InvalidShift_Value(String value, Int32 newStart)
	{
		var x = value.Slice();
		_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => x.Shift(newStart));
	}
	public static IEnumerable<Object[]> InvalidShift_Value_Length_NewStart => Data.InvalidShift_Value_Length_NewStart;
	[TestMethod]
	[DynamicData(nameof(InvalidShift_Value_Length_NewStart))]
	public void InvalidShift_Value_Length(String value, Int32 length, Int32 newStart)
	{
		var x = value.Slice(length);
		_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => x.Shift(newStart));
	}
	public static IEnumerable<Object[]> InvalidShift_Value_Start_Length_NewStart => Data.InvalidShift_Value_Start_Length_NewStart;
	[TestMethod]
	[DynamicData(nameof(InvalidShift_Value_Start_Length_NewStart))]
	public void InvalidShift_Value_Start_Length(String value, Int32 start, Int32 length, Int32 newStart)
	{
		var x = value.Slice(start, length);
		_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => x.Shift(newStart));
	}
	#endregion
	#region ReSlice
	public static IEnumerable<Object[]> ValidReSlices_Value => Data.ValidReSlices_Value;
	[TestMethod]
	[DynamicData(nameof(ValidReSlices_Value))]
	public void ValidReSlice_Value(String value)
	{
		var x = value.Slice();
		var y = x.ReSlice(0, value.Length);

		AssertSliceValid(y, value, 0, value.Length);
	}
	public static IEnumerable<Object[]> ValidReSlices_Value_Length => Data.ValidReSlices_Value_Length;
	[TestMethod]
	[DynamicData(nameof(ValidReSlices_Value_Length))]
	public void ValidReSlice_Value_Length(String value, Int32 length)
	{
		var x = value.Slice();
		var y = x.ReSlice(0, length);

		AssertSliceValid(y, value, 0, length);
	}
	public static IEnumerable<Object[]> ValidReSlices_Value_Start_Length => Data.ValidReSlices_Value_Start_Length;
	[TestMethod]
	[DynamicData(nameof(ValidReSlices_Value_Start_Length))]
	public void ValidReSlice_Value_Start_Length(String value, Int32 start, Int32 length)
	{
		var x = value.Slice();
		var y = x.ReSlice(start, length);

		AssertSliceValid(y, value, start, length);
	}

	public static IEnumerable<Object[]> InvalidReSlices_Value => Data.InvalidReSlices_Value;
	[TestMethod]
	[DynamicData(nameof(InvalidReSlices_Value))]
	public void InvalidReSlice_Value_Length(String value)
	{
		if(value != null)
		{
			var x = value.Slice();
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => x.ReSlice(0, value.Length));
		}
	}
	public static IEnumerable<Object[]> InvalidReSlices_Value_Length => Data.InvalidReSlices_Value_Length;
	[TestMethod]
	[DynamicData(nameof(InvalidReSlices_Value_Length))]
	public void InvalidReSlice_Value_Length(String value, Int32 length)
	{
		if(value != null)
		{
			var x = value.Slice();
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => x.ReSlice(0, length));
		}
	}
	public static IEnumerable<Object[]> InvalidReSlices_Value_Start_Length => Data.InvalidReSlices_Value_Start_Length;
	[TestMethod]
	[DynamicData(nameof(InvalidReSlices_Value_Start_Length))]
	public void InvalidReSlice_Value_Start_Length(String value, Int32 start, Int32 length)
	{
		if(value != null)
		{
			var x = value.Slice();
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => x.ReSlice(start, length));
		}
	}
	#endregion
	#endregion

	private static void AssertSlicesEqual(IStringSlice x, IStringSlice y)
	{
		Assert.IsTrue(x.Equals(x));
		Assert.IsTrue(x.Equals(y));
		Assert.IsTrue(y.Equals(x));
		Assert.IsTrue(y.Equals(y));
	}
	private static void AssertSliceValid(IStringSlice slice, String value, Int32 start, Int32 length)
	{
		var expected = value.Substring(start, length);
		var actual = slice.ToString();

		Assert.AreEqual(expected, actual);
		Assert.AreEqual(value, slice.Value);
		Assert.AreEqual(start, slice.Start);
		Assert.AreEqual(length, slice.Length);
	}
}