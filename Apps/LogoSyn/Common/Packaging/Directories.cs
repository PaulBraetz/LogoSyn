using System.Reflection;

using RhoMicro.Common.IO;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging;

internal static class Directories
{
	private static readonly String _tempSubDirectoryFormat = $"Process-{Environment.ProcessId}-{{0}}";
	private const String TEMP_DIRECTORY_NAME = "LogoSyn Packages";
	private const String LOCAL_PACKAGES_DIRECTORY = "Local Packages";

	public static TemporaryDirectory GetTempDirectory()
	{
		var path = Path.Combine(TEMP_DIRECTORY_NAME, String.Format(_tempSubDirectoryFormat, Guid.NewGuid().ToString()));
		var result = TemporaryDirectory.CreateInTempPath(path);

		return result;
	}

	public static DirectoryInfo GetDefaultLocalPackageDirectory()
	{
		var assemblyLocation = Assembly.GetExecutingAssembly().Location;
		var parentDirectory = new FileInfo(assemblyLocation).DirectoryName ??
			throw new Exception($"Unable to infer local package directory from assembly location: {assemblyLocation}");

		var resultPath = Path.Combine(parentDirectory, LOCAL_PACKAGES_DIRECTORY);
		var result = new DirectoryInfo(resultPath);
		result.Create();

		return result;
	}
}
