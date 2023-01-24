using System.Text.Json;

using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Cli.Commands.Packaging;

internal sealed class GetPackageInfo : PackagingCommandBase
{
	public GetPackageInfo(String navigationKey, IPackagingContext context) : base("Get Package Info", navigationKey, context)
	{
	}

	public override void Run()
	{
		var manifestFile = Read("Enter manifest path: ", s => new FileInfo(s));
		var manifest = Context.ReadManifest(manifestFile);

		var name = Read("Enter package name: ");
		var version = Read("Enter package version: ");

		var retrieved = manifest.TryGetPackageFile(name, version, out var packageFile);
		if(!retrieved)
		{
			throw new Exception($"Unable to retrieve package {name} {version} from manifest (not found).");
		}

		using var package = Context.GetPackage(packageFile!);
		var infoJson = JsonSerializer.Serialize(package.PackageInfo);
		Console.WriteLine(infoJson);
	}
}
