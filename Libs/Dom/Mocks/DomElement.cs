using RhoMicro.LogoSyn.Libs.Common.Strings;
using RhoMicro.LogoSyn.Libs.Dom.Dom.Abstractions;

namespace RhoMicro.LogoSyn.Libs.Dom.Tests.Mocks;

public readonly struct DomElement : IDomElement<Discriminators.Default>
{
	public DomElement(Discriminators.Default kind, IStringSlice slice, Int32 position)
	{
		Kind = kind;
		Slice = slice;
		Position = position;
	}

	public Discriminators.Default Kind {
		get;
	}
	public IStringSlice Slice {
		get;
	}
	public Int32 Position {
		get;
	}
}
