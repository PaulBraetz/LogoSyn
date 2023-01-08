using System.Runtime.Serialization;

namespace RhoMicro.LogoSyn.Libs.Parser.Tests.Mocks
{
	/// <summary>
	/// Provides enums for element discrimination.
	/// </summary>
	public static class Discriminators
	{
		/// <summary>
		/// Defines how a document element is to be handled by the interpreter.
		/// </summary>
		[DataContract]
		public enum Default : Byte
		{
			/// <summary>
			/// The elements value is to be included in the output as is.
			/// </summary>
			[EnumMember]
			Literal,
			/// <summary>
			/// The element is to be evaluated but not included in the output.
			/// </summary>
			[EnumMember]
			Code,
			/// <summary>
			/// The element is to be evaluated and included in the output.
			/// </summary>
			[EnumMember]
			Display,
			/// <summary>
			/// The element contains interpreter directives.
			/// </summary>
			[EnumMember]
			Directive
		}
		/// <summary>
		/// Defines how a document element is to be handled by the interpreter.
		/// </summary>
		[DataContract]
		public enum Simplified : Byte
		{
			/// <summary>
			/// The elements value is to be included in the output as is.
			/// </summary>
			[EnumMember]
			Literal,
			/// <summary>
			/// The element is to be evaluated but not included in the output.
			/// </summary>
			[EnumMember]
			Code
		}
	}
}
