using Fort;
using RhoMicro.LogoSyn.Libs.Common.Strings;
using RhoMicro.LogoSyn.Libs.Dom.Abstractions;
using System.Runtime.Serialization;

namespace RhoMicro.LogoSyn.Libs.Dom
{
	/// <summary>
	/// Default implementation of <see cref="IDomElement{TDiscriminator}"/>.
	/// </summary>
	/// <typeparam name="TDiscriminator">
	/// The discriminator by which to distinguish elements.
	/// </typeparam>
	[DataContract]
	public struct DomElement<TDiscriminator> : IDomElement<TDiscriminator>
	{
		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		/// <param name="kind">The new instances kind.</param>
		/// <param name="slice">The new instances value.</param>
		/// <param name="position">The new instance position in the document.</param>
		public DomElement(TDiscriminator kind, IStringSlice slice, Int32 position)
		{
			position.ThrowIfNot(p => p >= 0, $"{nameof(position)} must be abovve or equal to 0.");

			Kind = kind;
			Slice = slice;
			Position = position;
		}

		/// <inheritdoc/>
		[DataMember]
		public TDiscriminator Kind { get; private set; }
		/// <inheritdoc/>
		[DataMember]
		public IStringSlice Slice { get; private set; }
		/// <inheritdoc/>
		[DataMember]
		public Int32 Position { get; private set; }

		internal static IDomElement<TDiscriminator> Clone(IDomElement<TDiscriminator> instance)
		{
			instance.ThrowIfDefault(nameof(instance));

			var result = new DomElement<TDiscriminator>(instance.Kind, instance.Slice, instance.Position);

			return result;
		}
	}
}
