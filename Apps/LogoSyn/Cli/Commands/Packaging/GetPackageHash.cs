using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BuiltIns = RhoMicro.Common.System.Security.Cryptography.Hashing.Abstractions.DefaultAlgorithmBase<RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging.Abstractions.IPackage>.BuiltinAlgorithm;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Cli.Commands.Packaging
{
	internal sealed class GetPackageHash : PackagingCommandBase
	{
		public GetPackageHash(String navigationKey, IPackagingContext context) : base("Get Local Package Hash", navigationKey, context)
		{
		}

		public override void Run()
		{
			var manifestFile = Read("Enter manifest path: ", s => new FileInfo(s));
			if (!manifestFile.Exists)
			{
				throw new FileNotFoundException($"Unable to locate manifest file at: {manifestFile.FullName}");
			}
			var manifest = Context.ReadManifest(manifestFile);

			var name = Read("Enter package name: ");
			var version = Read("Enter package version: ");

			var retrieved = manifest.TryGetPackageFile(name, version, out var packageFile);
			if (!retrieved)
			{
				throw new Exception($"Unable to retrieve package {name} {version} from manifest (not found).");
			}

			using var package = Context.GetPackage(packageFile!);
			var algorithms = Enum.GetValues<BuiltIns>();
			foreach (var algorithm in algorithms)
			{
				var algorithmName = algorithm.ToString();
				var hash = Context.Hash(package, algorithmName);
				var hashValue = Convert.ToBase64String(hash.Value);
				Console.WriteLine($"{algorithmName}: {hashValue}");
			}
		}
	}
}
