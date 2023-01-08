using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Cli.Commands.Packaging
{
	internal sealed class GetLocalPackage : PackagingCommandBase
	{
		public GetLocalPackage(String navigationKey, IPackagingContext context) : base("Get Local Package", navigationKey, context)
		{
		}
		public override void Run()
		{
			var manifestFile = Read("Enter manifest path: ", s => new FileInfo(s));
			var manifest = Context.ReadManifest(manifestFile);

			var name = Read("Enter package name: ");
			var version = Read("Enter package version: ");

			var retrieved = manifest.TryGetPackageFile(name, version, out var file);
			var message = retrieved ?
				$"Retrieved package {name} {version} from manifest: {file!.FullName}" :
				$"Unable to retrieve package {name} {version} from manifest (not found).";

			Console.WriteLine(message);
		}
	}
}
