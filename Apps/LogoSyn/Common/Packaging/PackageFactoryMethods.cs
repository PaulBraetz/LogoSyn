using Fort;
using RhoMicro.Common.IO;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging.Abstractions;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging
{
	public sealed partial class Package
	{
		private const String PACKAGE_INFO_FILE_NAME = "PackageInfo.json";

		/// <summary>
		/// Initializes an instance of <see cref="IPackage"/> from a zip archive stream.
		/// </summary>
		/// <param name="source">The stream containing archive data.</param>
		/// <returns>A new instance of <see cref="IPackage"/>.</returns>
		public static IPackage Read(Stream source)
		{
			source.ThrowIfDefault(nameof(source));

			Package result;

			using (var archive = OpenPackageFile(source))
			{
				var packageInfo = GetPackageInfo(archive);
				using var packageDirectory = GetPackageDirectory(archive);
				result = new Package(packageInfo, packageDirectory.Directory.EnumerateDirectories().Single());
			}

			return result;
		}
		private static ZipArchive OpenPackageFile(Stream source)
		{
			var archive = new ZipArchive(source);
			var result = archive.GetEntry(PACKAGE_INFO_FILE_NAME) == null
							? throw new InvalidDataException($"Package does not contain a package info entry.")
							: archive;

			return result;
		}
		private static IPackageInfo GetPackageInfo(ZipArchive archive)
		{
			using var entryStream = archive.GetEntry(PACKAGE_INFO_FILE_NAME)!.Open();
			var result = Packaging.PackageInfo.ReadJson(entryStream);

			return result;
		}
		private static TemporaryDirectory GetPackageDirectory(ZipArchive archive)
		{
			var workingDirectory = Directories.GetTempDirectory();
			archive.ExtractToDirectory(workingDirectory.Directory.FullName);

			return workingDirectory;
		}

		/// <summary>
		/// Creates a new instance of <see cref="IPackage"/>.
		/// </summary>
		/// <param name="packageInfo">The metadata of the package to create.</param>
		/// <param name="packageDirectory">The directory containing the package files.</param>
		/// <returns>A new </returns>
		public static IPackage Create(IPackageInfo packageInfo, DirectoryInfo packageDirectory)
		{
			packageInfo.ThrowIfDefault(nameof(packageInfo));
			packageDirectory.ThrowIfDefaultOrNot(d => d.Exists, $"{nameof(packageDirectory)} does not exist.", nameof(packageDirectory));

			var result = new Package(packageInfo, packageDirectory);

			return result;
		}
		/// <summary>
		/// Serializes a package into a stream.
		/// </summary>
		/// <param name="target">The stream to serialize into.</param>
		/// <param name="package">The package to serialize.</param>
		public static void Write(Stream target, IPackage package)
		{
			target.ThrowIfDefault(nameof(target));
			package.ThrowIfDefault(nameof(package));

			using var workingDirectory = Directories.GetTempDirectory();

			//create package info file in working dir
			var workingPackageInfoFileName = Path.Combine(workingDirectory.Directory.FullName, PACKAGE_INFO_FILE_NAME);
			using (var copyStream = File.Create(workingPackageInfoFileName))
			{
				package.PackageInfo.WriteJson(copyStream);
			}

			package.PackageDataDirectory.CopyRecursively(workingDirectory.Directory.FullName);

			using var resultDirectory = Directories.GetTempDirectory();

			//create (package + info)  zip in result dir
			var packageName = $"{package.PackageInfo.Name ?? $"Package-{Guid.NewGuid()}"}.lspkg";
			var packageFileName = Path.Combine(resultDirectory.Directory.FullName, packageName);
			ZipFile.CreateFromDirectory(workingDirectory.Directory.FullName, packageFileName, CompressionLevel.SmallestSize, false);

			//copy (package + info)  zip from result dir to target stream
			using var packageStream = File.OpenRead(packageFileName);
			packageStream.CopyTo(target);
		}
	}
}
