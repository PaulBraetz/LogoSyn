using System.Text;

using Fort;

using RhoMicro.LogoSyn.Apps.Common;
using RhoMicro.LogoSyn.Libs.Common.Strings;
using RhoMicro.LogoSyn.Libs.Dom;
using RhoMicro.LogoSyn.Libs.Dom.Dom;
using RhoMicro.LogoSyn.Libs.Dom.Dom.Abstractions;
using RhoMicro.LogoSyn.Libs.Parser.Abstractions;

namespace RhoMicro.LogoSyn.Apps.Parser.CSharpParser;

internal static class Parser
{
	public const Int32 CHUNK_SIZE = 80;
	public const Char TOKEN_ESCAPE = '\\';
	public const Char TOKEN_OPEN_CODE = '{';
	public const Char TOKEN_CLOSE_CODE = '}';
	public const Char TOKEN_OPEN_DISPLAY = '[';
	public const Char TOKEN_CLOSE_DISPLAY = ']';

	private static IDom<Discriminators.Default> Parse(TextReader stream, Int32 lineOffset)
	{
		stream.ThrowIfDefault(nameof(stream));

		var dom = new Dom<Discriminators.Default>(CHUNK_SIZE);
		var builder = new StringBuilder();
		var escaped = false;
		var lineCount = 0;
		var position = 0;
		var openParentheses = 0;
		var kind = Discriminators.Default.Literal;

		String? line;

		if(lineOffset > 0)
		{
			while(lineOffset > 0)
			{
				_ = builder.Append('\n');
				lineCount++;
				lineOffset--;
			}
			
			var offsetSlice = builder.ToString().Slice();
			var offsetElement = new DomElement<Discriminators.Default>(Discriminators.Default.Ignore, offsetSlice, 0);
			dom.Add(offsetElement);

			_ = builder.Clear();

			position = lineCount;
		}

		while((line = stream.ReadLine()) != null)
		{
			lineCount++;

			for(var col = 0; col < line.Length; col++)
			{
				var c = line[col];

				switch(c)
				{
					case TOKEN_OPEN_CODE:
						if(!appendEscaped(c))
						{
							openParentheses++;

							if(openParentheses == 1)
							{
								commit();
							} else
							{
								append(c);
							}
						}

						break;
					case TOKEN_CLOSE_CODE:
						if(!appendEscaped(c))
						{
							openParentheses--;

							if(openParentheses == 0)
							{
								commit();
							} else if(openParentheses < 0)
							{
								throw new Exception($"Too many closing parentheses encountered on line {lineCount + 1}, column {col + 1}.");
							} else
							{
								append(c);
							}
						}

						break;
					case TOKEN_ESCAPE:
						escaped = !appendEscaped(c);
						break;
					case Char.MinValue:
						continue;
					default:
						_ = appendEscaped(TOKEN_ESCAPE);
						append(c);
						break;
				}
			}

			if(stream.Peek() != -1)
			{
				_ = builder.Append('\n');
			}
		}

		if(openParentheses > 0)
		{
			throw new Exception($"Too few closing parentheses encountered.");
		}

		commit();

		return dom;

		void append(Char c)
		{
			_ = builder!.Append(c);
			escaped = false;
		}

		Boolean appendEscaped(Char c)
		{
			if(escaped)
			{
				append(c);
				return true;
			}

			return false;
		}

		void commit()
		{
			var value = builder!.ToString();

			if(value.Length > 0)
			{
				var slice = value.Slice();
				var element = new DomElement<Discriminators.Default>(kind, slice, position);
				dom.Add(element);
			}

			position += value.Length;
			_ = builder.Clear();
			kind = kind == Discriminators.Default.Literal ?
				Discriminators.Default.Code :
				Discriminators.Default.Literal;
		}
	}
	private static Func<TextReader, IDom<Discriminators.Default>> GetStrategy(Int32 lineOffset)
	{
		return partialApplication;

		IDom<Discriminators.Default> partialApplication(TextReader reader)
		{
			var result = Parse(reader, lineOffset);

			return result;
		}
	}

	public static IParser Create(Int32 lineOffset)
	{
		var strategy = GetStrategy(lineOffset);
		var result = Parser<Discriminators.Default>.Create(strategy);

		return result;
	}
}
