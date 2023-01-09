using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhoMicro.LogoSyn.Libs.Parser.Abstractions
{
	public interface IToken
	{
		String Escape { get; }
		String Value { get; }
	}
	public interface ITokenPair
	{
		IToken Opening { get; }
		IToken Closing { get; }
	}
}
