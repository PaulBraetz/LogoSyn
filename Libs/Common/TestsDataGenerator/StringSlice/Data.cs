using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace RhoMicro.LogoSyn.Libs.Common.Tests.TestsDataGenerator.StringSlice;

internal class Data : IData
{
	#region Slice

	private static IEnumerable<Object[]> Slice_Value(IEnumerable<(String value, Int32 start, Int32 length)> slices)
	{
		return slices
			.Select(t => new Object[]
			{
				t.value
			});
	}
	private static IEnumerable<Object[]> Slice_Value_Length(IEnumerable<(String value, Int32 start, Int32 length)> slices)
	{
		return slices
			.Select(t => new Object[]
			{
				t.value, t.length
			});
	}
	private static IEnumerable<Object[]> Slice_Value_Start_Length(IEnumerable<(String value, Int32 start, Int32 length)> slices)
	{
		return slices
			.Select(t => new Object[]
			{
				t.value, t.start, t.length
			});
	}

	private static IEnumerable<(String value, Int32 start, Int32 length)> ValidSlices(Int32 count)
	{
		var data = Enumerable.Range(0, count)
			.Select(i => Random.Shared.Next(0, 50))
			.Select(Common.RandomValue)
			.Select(v => (value: v, start: Random.Shared.Next(0, v.Length)))
			.Select(t => (t.value, t.start, length: Random.Shared.Next(t.start, t.value.Length) - t.start));

		return data;
	}

	private static IEnumerable<Object[]> ValidSlices_Value_Start_Length(Int32 count)
	{
		var data = Slice_Value_Start_Length(ValidSlices(count));

		return data;
	}
	private static IEnumerable<Object[]> ValidSlices_Value_Length(Int32 count)
	{
		var data = Slice_Value_Length(ValidSlices(count));

		return data;
	}
	private static IEnumerable<Object[]> ValidSlices_Value(Int32 count)
	{
		var data = Slice_Value(ValidSlices(count));

		return data;
	}

	private static IEnumerable<(String value, Int32 start, Int32 length)> InvalidSlices(Int32 count)
	{
		var data = Enumerable.Range(0, count / 4)
			.Select(i => Random.Shared.Next(0, 50))
			.Select(Common.RandomValue)
			.SelectMany(v => new[]
			{
				(value: v, start: Random.Shared.Next(v.Length, Int32.MaxValue-1)),
				(value: v, start: Random.Shared.Next(Int32.MinValue+1, 0))
			})
			.Prepend((value: Common.RandomValue(count), start: Int32.MaxValue))
			.Prepend((value: Common.RandomValue(count), start: Int32.MinValue))
			.Prepend((value: Common.RandomValue(count), start: -1))
			.Prepend((value: Common.RandomValue(count), start: count + 1))
			.SelectMany(invalidate)
			.Prepend((value: null!, start: 0, length: 0));

		return data;

		static IEnumerable<(String value, Int32 start, Int32 length)> invalidate((String value, Int32 start) t)
		{
			var invalid = new (String value, Int32 start, Int32 length)[]
			{
				(t.value, t.start, length: Random.Shared.Next(t.value.Length - t.start, Int32.MaxValue)),
				(t.value, t.start, length: Random.Shared.Next(Int32.MinValue, -1)),
				(t.value, t.start, length: -1)
			};

			return invalid;
		}
	}

	private static IEnumerable<Object[]> InvalidSlices_Value_Start_Length(Int32 count)
	{
		var data = Slice_Value_Start_Length(InvalidSlices(count));

		return data;
	}
	private static IEnumerable<Object[]> InvalidSlices_Value_Length(Int32 count)
	{
		var data = Slice_Value_Length(InvalidSlices(count));

		return data;
	}
	private static IEnumerable<Object[]> InvalidSlices_Value()
	{
		var data = new Object[][] { new Object[] { null! } };

		return data;
	}
	#endregion

	#region Resize
	private static IEnumerable<Object[]> Resize_Value_NewLength(IEnumerable<(String value, Int32 start, Int32 length, Int32 newLength)> slices)
	{
		return slices
			.Select(t => new Object[]
			{
				t.value, t.newLength
			});
	}
	private static IEnumerable<Object[]> Resize_Value_Length_NewLength(IEnumerable<(String value, Int32 start, Int32 length, Int32 newLength)> slices)
	{
		return slices
			.Select(t => new Object[]
			{
				t.value, t.length, t.newLength
			});
	}
	private static IEnumerable<Object[]> Resize_Value_Start_Length_NewLength(IEnumerable<(String value, Int32 start, Int32 length, Int32 newLength)> slices)
	{
		return slices
			.Select(t => new Object[]
			{
				t.value, t.start, t.length, t.newLength
			});
	}

	private static IEnumerable<(String value, Int32 startm, Int32 length, Int32 newLength)> ValidResize(Int32 count)
	{
		var data = ValidSlices(count)
			.Select(t => (t.value, t.start, t.length, newLength: Random.Shared.Next(0, t.value.Length - t.start)));

		return data;
	}

	private static IEnumerable<Object[]> ValidResize_Value_NewLength(Int32 count)
	{
		var data = Resize_Value_NewLength(ValidResize(count));

		return data;
	}
	private static IEnumerable<Object[]> ValidResize_Value_Length_NewLength(Int32 count)
	{
		var data = Resize_Value_Length_NewLength(ValidResize(count));

		return data;
	}
	private static IEnumerable<Object[]> ValidResize_Value_Start_Length_NewLength(Int32 count)
	{
		var data = Resize_Value_Start_Length_NewLength(ValidResize(count));

		return data;
	}

	private static IEnumerable<(String value, Int32 startm, Int32 length, Int32 newLength)> InvalidResize(Int32 count)
	{
		var data = ValidSlices(count)
			.SelectMany(t => new[]
			{
				(t.value, t.start, t.length, newLength: Random.Shared.Next(Int32.MinValue, -1)),
				(t.value, t.start, t.length, newLength: Random.Shared.Next(t.value.Length - t.start, Int32.MaxValue)),
				(t.value, t.start, t.length, newLength: -1)
			});

		return data;
	}

	private static IEnumerable<Object[]> InvalidResize_Value_NewLength(Int32 count)
	{
		var data = Resize_Value_NewLength(InvalidResize(count));

		return data;
	}
	private static IEnumerable<Object[]> InvalidResize_Value_Length_NewLength(Int32 count)
	{
		var data = Resize_Value_Length_NewLength(InvalidResize(count));

		return data;
	}
	private static IEnumerable<Object[]> InvalidResize_Value_Start_Length_NewLength(Int32 count)
	{
		var data = Resize_Value_Start_Length_NewLength(InvalidResize(count));

		return data;
	}
	#endregion

	#region Shift
	private static IEnumerable<Object[]> Shift_Value_NewStart(IEnumerable<(String value, Int32 start, Int32 length, Int32 newStart)> slices)
	{
		return slices
			.Select(t => new Object[]
			{
				t.value, t.newStart
			});
	}
	private static IEnumerable<Object[]> Shift_Value_Length_NewStart(IEnumerable<(String value, Int32 start, Int32 length, Int32 newStart)> slices)
	{
		return slices
			.Select(t => new Object[]
			{
				t.value, t.length, t.newStart
			});
	}
	private static IEnumerable<Object[]> Shift_Value_Start_Length_NewStart(IEnumerable<(String value, Int32 start, Int32 length, Int32 newStart)> slices)
	{
		return slices
			.Select(t => new Object[]
			{
				t.value, t.start, t.length, t.newStart
			});
	}

	private static IEnumerable<(String value, Int32 startm, Int32 length, Int32 newStart)> ValidShift(Int32 count)
	{
		var data = ValidSlices(count)
			.Select(t => (t.value, t.start, t.length, newStart: Random.Shared.Next(0, t.value.Length - t.length)));

		return data;
	}

	private static IEnumerable<Object[]> ValidShift_Value_NewStart(Int32 count)
	{
		var data = Shift_Value_NewStart(ValidShift(count));

		return data;
	}
	private static IEnumerable<Object[]> ValidShift_Value_Length_NewStart(Int32 count)
	{
		var data = Shift_Value_Length_NewStart(ValidShift(count));

		return data;
	}
	private static IEnumerable<Object[]> ValidShift_Value_Start_Length_NewStart(Int32 count)
	{
		var data = Shift_Value_Start_Length_NewStart(ValidShift(count));

		return data;
	}

	private static IEnumerable<(String value, Int32 startm, Int32 length, Int32 newStart)> InvalidShift(Int32 count)
	{
		var data = ValidSlices(count)
			.SelectMany(t => new[]
			{
				(t.value, t.start, t.length, newStart: Random.Shared.Next(Int32.MinValue, -1)),
				(t.value, t.start, t.length, newStart: Random.Shared.Next(t.value.Length - t.length, Int32.MaxValue)),
				(t.value, t.start, t.length, newStart: -1)
			});

		return data;
	}

	private static IEnumerable<Object[]> InvalidShift_Value_NewStart(Int32 count)
	{
		var data = Shift_Value_NewStart(InvalidShift(count));

		return data;
	}
	private static IEnumerable<Object[]> InvalidShift_Value_Length_NewStart(Int32 count)
	{
		var data = Shift_Value_Length_NewStart(InvalidShift(count));

		return data;
	}
	private static IEnumerable<Object[]> InvalidShift_Value_Start_Length_NewStart(Int32 count)
	{
		var data = Shift_Value_Start_Length_NewStart(InvalidShift(count));

		return data;
	}
	#endregion

	private static String GetPropertyDeclaration(IEnumerable<Object[]> data, String name)
	{
		var backingFieldDeclaration = $"private static Object[][] _{name} = new Object[][]{{\t{String.Join(",\t", data.Select(d => $"new Object[] {{ {String.Join(", ", d.Select(d => d is null ? "null" : d is String ? $"@\"{d}\"" : d))}}}"))}}};";
		var propertyDeclaration = $"public static Object[][] {name} => _{name}.Select(v => (Object[])v.Clone()).ToArray();";

		return backingFieldDeclaration + propertyDeclaration;
	}
	private static IEnumerable<String> GetPropertyDeclarations(Int32 count)
	{
		return new String[]
		{
			GetPropertyDeclaration(ValidSlices_Value(count), nameof(ValidSlices_Value)),
			GetPropertyDeclaration(ValidSlices_Value_Length(count), nameof(ValidSlices_Value_Length)),
			GetPropertyDeclaration(ValidSlices_Value_Start_Length(count), nameof(ValidSlices_Value_Start_Length)),
			GetPropertyDeclaration(InvalidSlices_Value(), nameof(InvalidSlices_Value)),
			GetPropertyDeclaration(InvalidSlices_Value_Length(count), nameof(InvalidSlices_Value_Length)),
			GetPropertyDeclaration(InvalidSlices_Value_Start_Length(count), nameof(InvalidSlices_Value_Start_Length)),
			GetPropertyDeclaration(ValidResize_Value_NewLength(count), nameof(ValidResize_Value_NewLength)),
			GetPropertyDeclaration(ValidResize_Value_Length_NewLength(count), nameof(ValidResize_Value_Length_NewLength)),
			GetPropertyDeclaration(ValidResize_Value_Start_Length_NewLength(count), nameof(ValidResize_Value_Start_Length_NewLength)),
			GetPropertyDeclaration(InvalidResize_Value_NewLength(count), nameof(InvalidResize_Value_NewLength)),
			GetPropertyDeclaration(InvalidResize_Value_Length_NewLength(count), nameof(InvalidResize_Value_Length_NewLength)),
			GetPropertyDeclaration(InvalidResize_Value_Start_Length_NewLength(count), nameof(InvalidResize_Value_Start_Length_NewLength)),
			GetPropertyDeclaration(ValidShift_Value_NewStart(count), nameof(ValidShift_Value_NewStart)),
			GetPropertyDeclaration(ValidShift_Value_Length_NewStart(count), nameof(ValidShift_Value_Length_NewStart)),
			GetPropertyDeclaration(ValidShift_Value_Start_Length_NewStart(count), nameof(ValidShift_Value_Start_Length_NewStart)),
			GetPropertyDeclaration(InvalidShift_Value_NewStart(count), nameof(InvalidShift_Value_NewStart)),
			GetPropertyDeclaration(InvalidShift_Value_Length_NewStart(count), nameof(InvalidShift_Value_Length_NewStart)),
			GetPropertyDeclaration(InvalidShift_Value_Start_Length_NewStart(count), nameof(InvalidShift_Value_Start_Length_NewStart)),
			GetPropertyDeclaration(ValidSlices_Value(count), "ValidReSlices_Value"),
			GetPropertyDeclaration(ValidSlices_Value_Length(count), "ValidReSlices_Value_Length"),
			GetPropertyDeclaration(ValidSlices_Value_Start_Length(count), "ValidReSlices_Value_Start_Length"),
			GetPropertyDeclaration(InvalidSlices_Value(), "InvalidReSlices_Value"),
			GetPropertyDeclaration(InvalidSlices_Value_Length(count), "InvalidReSlices_Value_Length"),
			GetPropertyDeclaration(InvalidSlices_Value_Start_Length(count), "InvalidReSlices_Value_Start_Length"),
		};
	}
	private static String GetClassDeclaration(Int32 count) => $"namespace RhoMicro.LogoSyn.Libs.Common.Tests{{public static class Data{{{String.Join("", GetPropertyDeclarations(count))}}}}}";

	private static String GetData(Int32 count)
	{
		var result = SyntaxFactory.ParseSyntaxTree(GetClassDeclaration(count)).GetRoot().NormalizeWhitespace().ToFullString();

		return result;
	}

	public void WriteToFile(Int32 minimumCount)
	{
		using var file = new StreamWriter(File.Create(@"C:\Users\Personal\Documents\Projects\PaulBraetz\LogoSyn\CommonTests\StringSlice\Data.cs"));
		file.Write(GetData(minimumCount));
	}
}