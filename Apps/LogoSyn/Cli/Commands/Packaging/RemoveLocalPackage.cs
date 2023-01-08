using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Cli.Commands.Packaging
{
	internal sealed class RemoveLocalPackage : PackagingCommandBase
	{
		public RemoveLocalPackage(String navigationKey, IPackagingContext context) : base("Remove Local Package", navigationKey, context)
		{
		}

		public override void Run()
		{
			var manifestFile = Read("Enter manifest path: ", s => new FileInfo(s));

			var manifest = Context.ReadManifest(manifestFile);

			var name = Read("Enter package name: ");
			var version = Read("Enter package version: ");

			var removed = manifest.TryRemovePackage(name, version);

			if (removed)
			{
				Context.WriteManifest(manifest, manifestFile);
			}

			var message = removed ?
				$"Removed package {name} {version} from manifest." :
				$"Unable to remove package {name} {version} from manifest (not found).";

			Console.WriteLine(message);
		}
	}
}
