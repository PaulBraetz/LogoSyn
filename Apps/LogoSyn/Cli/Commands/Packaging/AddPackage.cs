using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Cli.Commands.Packaging
{
	internal sealed class CreatePackage : PackagingCommandBase
	{
		public CreatePackage(String navigationKey, IPackagingContext context) : base("Create Package", navigationKey, context)
		{
		}

		private record PackageInfo(String Name,
								   String Version,
								   String EntryPoint,
								   String Description,
								   String Author,
								   String Website) : IPackageInfo
		{ }

		public override void Run()
		{
			var name = Read("Enter package name: ");
			var version = Read("Enter package version: ");
			var entryPoint = Read("Enter package entry point: ");
			var description = Read("Enter package description: ");
			var author = Read("Enter package author: ");
			var website = Read("Enter package website: ");
			var packageInfo = new PackageInfo(name, version, entryPoint, description, author, website);

			var packageDirectoryPath = Read("Enter package directory path: ");
			var packageDirectory = new DirectoryInfo(packageDirectoryPath);

			var targetDirectoryPath = Read("Enter target directory path: ");
			var targetDirectory = new DirectoryInfo(targetDirectoryPath);

			Context.CreatePackage(packageInfo, packageDirectory, targetDirectory);
		}
	}
}
