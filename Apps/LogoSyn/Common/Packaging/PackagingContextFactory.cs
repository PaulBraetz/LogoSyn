using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;

using Scli;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging;

internal sealed class PackagingContextFactory : ApplicationContextFactoryBase
{
	static PackagingContextFactory()
	{
		_parameters = Initialization.GetParameters();
		_ = _parameters.TryAdd("p", "package", "When set, packages may be managed.", s => s == null);
		_ = _parameters.TryAdd("ph", "packageHeadless", "When set, packages may be managed headlessly.", s => s == null);

		_ = _parameters.TryAdd("pi", "packageInfo", "Supplies the package information file for creating a package.", File.Exists);
		_ = _parameters.TryAdd("pd", "packageDirectory", "Supplies the package data directory containing all files required for creating a package.", Directory.Exists);
		_ = _parameters.TryAdd("pt", "packageTarget", "Supplies the target directory for the package created.");
		_ = _parameters.TryAdd("pm", "packageManifest", "Supplies the manifest file for registering the package created.", s => !String.IsNullOrEmpty(s));
	}

	public PackagingContextFactory(String[] args) : base(_parameters, args)
	{
	}

	private static readonly IParameterCollection _parameters;

	protected override Boolean CanCreate(IArgumentCollection arguments)
	{
		var result = arguments.TryGet("p", out var _);

		return result;
	}
	protected override IApplicationContext? CreateContext(IArgumentCollection arguments)
	{
		var helpInfo = _parameters.ToString();
		var context = new PackagingContext(helpInfo!);

		try
		{
			CreatePackage(arguments, context);

			var result = arguments.TryGet("ph", out var _) ? null : context;

			return result;
		} finally
		{
			context.Dispose();
		}
	}

	private static void CreatePackage(IArgumentCollection arguments, IPackagingContext context)
	{
		var hasInfo = arguments.TryGet("pi", s => new FileInfo(s!), out var packageInfoFile);
		var hasDirectory = arguments.TryGet("pd", d => new DirectoryInfo(d!), out var packageDirectory);
		if(hasInfo && hasDirectory)
		{
			var targetDirectoryPath = arguments.TryGet("pt", out var arg) ?
				arg!.Value :
				Directory.GetCurrentDirectory();
			var targetDirectory = new DirectoryInfo(targetDirectoryPath!);
			targetDirectory.Create();

			using var packageInfoStream = packageInfoFile!.OpenRead();
			var packageInfo = PackageInfo.ReadJson(packageInfoStream);
			context.CreatePackage(packageInfo, packageDirectory!, targetDirectory);

			if(arguments.TryGet("pm", s => new FileInfo(s!), out var manifestFile))
			{
				var manifest = context.ReadManifest(manifestFile!);

				var packageFilePath = packageInfo.GetPackageFileName(targetDirectory);
				var packageFile = new FileInfo(packageFilePath);
				_ = manifest.TryAddPackageFile(packageFile);

				context.WriteManifest(manifest, manifestFile!);
			}
		}
	}
}
