using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Cli.Commands.Packaging;

internal sealed class ReadManifest : PackagingCommandBase
{
	public ReadManifest(String navigationKey, IPackagingContext context) : base("Read Manifest", navigationKey, context)
	{
	}

	public override void Run()
	{
		var path = Read("Enter manifest path: ");
		var file = new FileInfo(path);

		var manifest = Context.ReadManifest(file);
		Context.WriteManifest(manifest, file);

		var packages = manifest.GetPackages();

		foreach(var package in packages)
		{
			Console.WriteLine(package.Name);
		}
	}
}
