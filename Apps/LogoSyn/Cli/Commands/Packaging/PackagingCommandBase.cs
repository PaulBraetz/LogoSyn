using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;
using Scli.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Cli.Commands.Packaging
{
	internal abstract class PackagingCommandBase : CommandBase<IPackagingContext>
	{
		protected PackagingCommandBase(String name, String navigationKey, IPackagingContext context) : base(name, navigationKey, context)
		{
		}
	}
}
