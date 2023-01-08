using Fort;

namespace RhoMicro.LogoSyn.Libs.Common.Strings
{
	/// <summary>
	/// Extensions for slicing instances of <see cref="String"/>.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Slices a <see cref="String"/>.
		/// </summary>
		/// <param name="value">The <see cref="String"/> to slice.</param>
		/// <returns>A slice of <paramref name="value"/> with start 0 and length <paramref name="value"/>.Length.</returns>
		public static IStringSlice Slice(this String value)
		{
			var slice = new StringSlice(value);

			return slice;
		}
		/// <summary>
		/// Slices a <see cref="String"/>.
		/// </summary>
		/// <param name="value">The <see cref="String"/> to slice.</param>
		/// <param name="length">The length of the resulting slice.</param>
		/// <returns>A slice of <paramref name="value"/> with start 0 and length <paramref name="length"/>.</returns>
		public static IStringSlice Slice(this String value, Int32 length)
		{
			var slice = new StringSlice(value, length);

			return slice;
		}
		/// <summary>
		/// Slices a <see cref="String"/>.
		/// </summary>
		/// <param name="value">The <see cref="String"/> to slice.</param>
		/// <param name="start">The start of the resulting slice.</param>
		/// <param name="length">The length of the resulting slice.</param>
		/// <returns>A slice of <paramref name="value"/> with start <paramref name="start"/> and length <paramref name="length"/>.</returns>
		public static IStringSlice Slice(this String value, Int32 start, Int32 length)
		{
			var slice = new StringSlice(value, start, length);

			return slice;
		}
		/// <summary>
		/// Resizes the slice provided.
		/// </summary>
		/// <param name="slice">The slice to resize.</param>
		/// <param name="length">The length of the resulting slice.</param>
		/// <returns>A new instance of <see cref="IStringSlice"/> with start <paramref name="slice"/>.Start and length <paramref name="length"/>.</returns>
		public static IStringSlice Resize(this IStringSlice slice, Int32 length)
		{
			slice.ThrowIfDefault(nameof(slice));
			var resizedSlice = new StringSlice(slice.Value, slice.Start, length);

			return resizedSlice;
		}
		/// <summary>
		/// Shifts the slice provided.
		/// </summary>
		/// <param name="slice">The slice to shift.</param>
		/// <param name="start">The start of the resulting slice.</param>
		/// <returns>A new instance of <see cref="IStringSlice"/> with start <paramref name="start"/> and length <paramref name="slice"/>.Start.</returns>
		public static IStringSlice Shift(this IStringSlice slice, Int32 start)
		{
			slice.ThrowIfDefault(nameof(slice));
			var shiftedSlice = new StringSlice(slice.Value, start, slice.Length);

			return shiftedSlice;
		}
		/// <summary>
		/// Slices the value contained in an instance of <see cref="IStringSlice"/>.
		/// </summary>
		/// <param name="slice">The slice whose value to slice.</param>
		/// <param name="start">The start of the resulting slice.</param>
		/// <param name="length">The length of the resulting slice.</param>
		/// <returns>A slice of <paramref name="slice"/>.Value with start <paramref name="start"/> and length <paramref name="length"/>.</returns>
		public static IStringSlice ReSlice(this IStringSlice slice, Int32 start, Int32 length)
		{
			slice.ThrowIfDefault(nameof(slice));
			var reslicedSlice = new StringSlice(slice.Value, start, length);

			return reslicedSlice;
		}
	}
}
