using System.Text.Json;

using Fort;

using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging.Abstractions;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging;

internal static class Extensions
{
	public static void WriteJson(this IPackageInfo packageInfo, Stream target)
	{
		JsonSerializer.Serialize(target, packageInfo, new JsonSerializerOptions()
		{
			WriteIndented = true
		});
	}
	public static void Write(this IPackage package, Stream target) => Package.Write(target, package);

	public static String GetPackageFileName(this IPackageInfo packageInfo, DirectoryInfo? containingDirectory = null)
	{
		packageInfo.ThrowIfDefault(nameof(packageInfo));

		var result = $"{packageInfo.Name}_{packageInfo.Version?.Replace('.', '_') ?? "v0"}.lspkg";

		if(containingDirectory != null)
		{
			result = Path.Combine(containingDirectory.FullName, result);
		}

		return result;
	}

	public static IPackageLoader Append(this IPackageLoader loader, IPackageLoader next)
	{
		loader.ThrowIfDefault(nameof(loader));
		next.ThrowIfDefault(nameof(next));

		var result = new PackageLoaderQueue().Set(0, loader).Set(1, next);

		return result;
	}
	public static IPackageLoader AppendRange(this IPackageLoader loader, IEnumerable<IPackageLoader> next)
	{
		loader.ThrowIfDefault(nameof(loader));
		next.ThrowIfDefault(nameof(next));

		var result = new PackageLoaderQueue().Set(0, loader);

		var i = 1;
		foreach(var nextLoader in next)
		{
			_ = result.Set(i, nextLoader);
			i++;
		}

		return result;
	}
	public static IPackageLoader AppendRange(this IPackageLoader loader, params IPackageLoader[] next)
	{
		loader.ThrowIfDefault(nameof(loader));
		next.ThrowIfDefault(nameof(next));

		var result = new PackageLoaderQueue().Set(0, loader);

		for(var i = 0; i < next.Length; i++)
		{
			_ = result.Set(i + 1, next[i]);
		}

		return result;
	}
}
