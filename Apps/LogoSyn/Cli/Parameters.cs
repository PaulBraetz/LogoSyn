﻿using RhoMicro.LogoSyn.Apps.LogoSyn.Common;
using Scli;
using System.Text.RegularExpressions;

namespace RhoMicro.LogoSyn.Apps.LogoSyn
{
	internal partial class Program
	{
		static Program()
		{
			var parameters = Initialization.GetParameters();
			_ = parameters.TryAdd("h", "help", "Display help.", s => s == null);
			_ = parameters.TryAdd("v", "version", "Display version.", s => s == null);

			Parameters = Initialization.GetParameters();
			foreach (var parameter in parameters.OrderBy(p => p.ShortName))
			{
				_ = Parameters.TryAdd(parameter.ShortName[1..], parameter.LongName?[2..], parameter.Description, parameter.Validator);
			}
		}

		private static readonly IParameterCollection Parameters;
	}
}
