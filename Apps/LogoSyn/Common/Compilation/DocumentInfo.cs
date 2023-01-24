using Fort;

using RhoMicro.Common.System;
using RhoMicro.Common.System.IO;
using RhoMicro.Common.System.Security.Cryptography.Hashing;
using RhoMicro.Common.System.Security.Cryptography.Hashing.Abstractions;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging.Abstractions;

using BuiltIns = RhoMicro.Common.System.Security.Cryptography.Hashing.Abstractions.DefaultAlgorithmBase<RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging.Abstractions.IPackage>.BuiltinAlgorithm;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common.Compilation;

internal sealed class DocumentInfo : DisposableBase, IDocumentInfo
{
	public DocumentInfo(IPackageInvocationInfo interpreterInfo, IPackageInvocationInfo parserInfo, Stream source, Int32 sourceOffset)
	{
		InterpreterInfo = interpreterInfo;
		ParserInfo = parserInfo;
		Source = source;
		SourceOffset = sourceOffset;
	}

	private static class Algorithm
	{
		private const String KEY_NAME = "Name";

		public static IAlgorithm<IPackage> Read(IDictionary<String, String> dictionary, String prefix)
		{
			var key = GetKey(prefix, KEY_NAME);
			var name = ReadValue(dictionary, key);

			if(!Enum.TryParse<BuiltIns>(name, false, out var algorithmType))
			{
				var validAlgorithms = Enum.GetValues<BuiltIns>().Select(a => a.ToString());
				throw new Exception($"Invalid value provided for {key}. Valid values are: {String.Join(',', validAlgorithms)}.");
			}

			var result = DefaultAlgorithmBase<IPackage>.Create(Package.SerializeIdentifiers, algorithmType);

			return result;
		}
	}
	private readonly struct PackageInvocationInfo : IPackageInvocationInfo
	{
		private PackageInvocationInfo(String packageName, String packageVersion, IHash<IPackage> packageHash, String[] arguments)
		{
			PackageName = packageName;
			PackageVersion = packageVersion;
			PackageHash = packageHash;
			Arguments = arguments;
		}

		public String PackageName {
			get;
		}
		public String PackageVersion {
			get;
		}
		public IHash<IPackage> PackageHash {
			get;
		}
		public String[] Arguments {
			get;
		}

		public static IPackageInvocationInfo Read(IDictionary<String, String> dictionary, String prefix)
		{
			var algorithmPrefix = GetKey(prefix, nameof(PackageHash), nameof(IHash<IPackage>.Algorithm));
			var algorithm = Algorithm.Read(dictionary, algorithmPrefix);

			read(nameof(PackageHash), out var hashString);

			var value = GetHashBytes(hashString);
			var hash = new Hash<IPackage>(value, algorithm);

			read(nameof(PackageName), out var packageName);
			read(nameof(PackageVersion), out var packageVersion);
			read(nameof(Arguments), out var argumentsString);

			var arguments = argumentsString.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
			var result = new PackageInvocationInfo(packageName, packageVersion, hash, arguments);

			return result;

			void read(String name, out String value)
			{
				var key = GetKey(prefix, name);
				value = ReadValue(dictionary, key);
			}
		}

		private static Byte[] GetHashBytes(String hashString)
		{
			try
			{
				var result = Convert.FromBase64String(hashString);

				return result;
			} catch(Exception ex)
			{
				throw new Exception($"Unable to convert hash to bytes.", ex);
			}
		}
	}

	public IPackageInvocationInfo InterpreterInfo {
		get;
	}
	public IPackageInvocationInfo ParserInfo {
		get;
	}
	public Stream Source {
		get;
	}
	public Int32 SourceOffset {
		get;
	}

	protected override void DisposeManaged(Boolean disposing)
	{
		Source.Dispose();
		base.DisposeManaged(disposing);
	}

	public static IDocumentInfo Read(Stream source)
	{
		source.ThrowIfDefaultOrNot(s => s.CanSeek && s.CanRead, $"{nameof(source)} must be seek- and read-enabled.", nameof(source));

		_ = source.Seek(0, SeekOrigin.Begin);
		var dictionary = ReadKeyValuePairs(source, out var sourceOffset);

		var interpreterInfo = PackageInvocationInfo.Read(dictionary, nameof(InterpreterInfo));
		var parserInfo = PackageInvocationInfo.Read(dictionary, nameof(ParserInfo));

		var result = new DocumentInfo(interpreterInfo, parserInfo, source, sourceOffset);

		return result;
	}

	private static IDictionary<String, String> ReadKeyValuePairs(Stream source, out Int32 sourceOffset)
	{
		var reader = new StreamReader(source);
		var result = new Dictionary<String, String>();
		sourceOffset = 0;

		while(true)
		{
			var fullLine = reader.ReadFullLine();
			var line = fullLine.Replace("\r\n", String.Empty).Replace("\n", String.Empty);
			var lineParts = line?.Split(':') ?? Array.Empty<String>();
			if(lineParts.Length > 1)
			{
				result.Add(lineParts[0].ToLower(), String.Join(':', lineParts[1..]));
			} else if(line != String.Empty)
			{
				break;
			}

			sourceOffset += reader.CurrentEncoding.GetBytes(fullLine!).Length;
		}

		return result;
	}
	private static String GetKey(params String[] parts)
	{
		var result = String.Join('.', parts.Select(p => p.ToLower()));

		return result;
	}
	private static String ReadValue(IDictionary<String, String> dictionary, String key)
	{
		var result = dictionary.TryGetValue(key, out var r1) ?
			r1 :
			throw new Exception($"No value provided for {key}.");
		return result;
	}
}
