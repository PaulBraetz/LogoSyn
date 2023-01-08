using Fort;
using System.Runtime.Serialization;

namespace RhoMicro.LogoSyn.Libs.Common.Strings
{
	/// <summary>
	/// Default implementation of <see cref="IStringSlice"/>.
	/// </summary>
	[DataContract]
	public struct StringSlice : IEquatable<IStringSlice>, IStringSlice
	{
		/// <inheritdoc/>
		[DataMember]
		public String Value { get; private set; }
		/// <inheritdoc/>
		[DataMember]
		public Int32 Length { get; private set; }
		/// <inheritdoc/>
		[DataMember]
		public Int32 Start { get; private set; }

		private Lazy<String>? _slice = null;
		private Lazy<String> Slice => _slice ??= new Lazy<String>(SubStringFactory);

		internal StringSlice(String value, Int32 start, Int32 length)
		{
			Value = value ?? throw new ArgumentNullException(nameof(value));

			if (start < 0 || start >= value.Length && start != 0)
			{
				throw new ArgumentOutOfRangeException(nameof(start), start, $"{nameof(start)} must be between 0 and {value.Length - 1} (inclusive).");
			}

			Start = start;

			if (length < 0 || start + length > value.Length)
			{
				throw new ArgumentOutOfRangeException(nameof(length), length, $"{nameof(length)} must be between 0 and {value.Length - start} (inclusive).");
			}

			Length = length;
		}
		internal StringSlice(String value, Int32 length) : this(value, 0, length)
		{

		}
		internal StringSlice(String value) : this(value, 0, value?.Length ?? -1)
		{

		}

		private String SubStringFactory()
		{
			var result = Value.Substring(Start, Length);

			return result;
		}

		/// <summary>
		/// Clones a provided instance of <see cref="IStringSlice"/>.
		/// </summary>
		/// <param name="slice">The instance to clone.</param>
		/// <returns>A new instance of </returns>
		public static IStringSlice Clone(IStringSlice slice)
		{
			slice.ThrowIfDefault(nameof(slice));

			var result = new StringSlice(slice.Value, slice.Start, slice.Length);

			return result;
		}

		/// <inheritdoc/>
		public override Boolean Equals(Object? obj)
		{
			return obj is IStringSlice slice && Equals(slice);
		}

		/// <inheritdoc/>
		public Boolean Equals(IStringSlice? other)
		{
			return StringSliceEqualityComparer.Instance.Equals(this, other);
		}

		/// <inheritdoc/>
		public override Int32 GetHashCode()
		{
			return StringSliceEqualityComparer.Instance.GetHashCode(this);
		}

		/// <inheritdoc/>
		public override String ToString()
		{
			var value = Slice.Value;

			return value;
		}

		/// <inheritdoc/>
		public static Boolean operator ==(StringSlice left, StringSlice right)
		{
			return left.Equals(right);
		}

		/// <inheritdoc/>
		public static Boolean operator !=(StringSlice left, StringSlice right)
		{
			return !(left == right);
		}
	}
}
