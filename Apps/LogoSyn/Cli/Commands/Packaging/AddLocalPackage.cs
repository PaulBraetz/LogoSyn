using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Cli.Commands.Packaging
{
	internal sealed class AddLocalPackage : PackagingCommandBase
	{
		public AddLocalPackage(String navigationKey, IPackagingContext context) : base("Add Local Package", navigationKey, context)
		{
		}

		public override void Run()
		{
			var manifestFile = Read("Enter manifest path: ", s => new FileInfo(s));

			var manifest = Context.ReadManifest(manifestFile);

			var packageFile = Read("Enter package file path: ", s => new FileInfo(s));
			var added = manifest.TryAddPackageFile(packageFile);

			if(added)
			{
				Context.WriteManifest(manifest, manifestFile);
			}

			var message = added ?
				$"Added package {packageFile.Name} to manifest." :
				$"Unable to add duplicate package {packageFile.Name} to manifest.";

			Console.WriteLine(message);
		}
	}
}
