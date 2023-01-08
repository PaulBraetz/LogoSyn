namespace RhoMicro.LogoSyn.Libs.Common.Strings
{
	/// <summary>
	/// Represents a <see cref="String"/> slice (lazy substring).
	/// </summary>
	public interface IStringSlice
	{
		/// <summary>
		/// The slices length.
		/// </summary>
		Int32 Length { get; }
		/// <summary>
		/// The slices starting index.
		/// </summary>
		Int32 Start { get; }
		/// <summary>
		/// The value from which the slice is to be obtained.
		/// </summary>
		String Value { get; }
	}
}