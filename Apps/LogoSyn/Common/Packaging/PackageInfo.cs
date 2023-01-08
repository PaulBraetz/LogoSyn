using Fort;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging.Abstractions;
using System.Text;
using System.Text.Json;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging
{
	/// <summary>
	/// Default implementation of <see cref="IPackageInfo"/>.
	/// </summary>
	public sealed class PackageInfo : IPackageInfo
	{
		/// <inheritdoc/>
		public String? EntryPoint { get; set; }
		/// <inheritdoc/>
		public String? Name { get; set; }
		/// <inheritdoc/>
		public String? Version { get; set; }

		/// <inheritdoc/>
		public String? Website { get; set; }
		/// <inheritdoc/>
		public String? Description { get; set; }
		/// <inheritdoc/>
		public String? Author { get; set; }
		/// <inheritdoc/>
		public String? Company { get; set; }

		/// <summary>
		/// Deserializes an instance of <see cref="IPackageInfo"/> from a json stream.
		/// </summary>
		/// <param name="source">The stream containing serialization data.</param>
		/// <returns>A new instance of <see cref="IPackageInfo"/>.</returns>
		public static IPackageInfo ReadJson(Stream source)
		{
			PackageInfo? result = null;

			try
			{
				result = JsonSerializer.Deserialize(source, typeof(PackageInfo)) as PackageInfo;
			}
			catch
			{
			}

			result ??= new PackageInfo();

			return result;
		}

		/// <summary>
		/// Serializes the identifying properties of a package info object into a stream.
		/// </summary>
		/// <remarks>
		/// This function is intended to be used as the default hashing serialization 
		/// strategy for instances of <see cref="IPackageInfo"/>. It is in no way intended 
		/// to be used as a textual or other serialization mechanism.
		/// </remarks>
		/// <param name="packageInfo">The package info to serialize.</param>
		/// <returns>A stream, containing the identifying properties of <paramref name="packageInfo"/>.</returns>
		public static Stream SerializeIdentifiers(IPackageInfo packageInfo)
		{
			packageInfo.ThrowIfDefault(nameof(packageInfo));

			var metaData = String.Concat(packageInfo.Name, packageInfo.Version, packageInfo.EntryPoint, packageInfo.Description, packageInfo.Author, packageInfo.Description, packageInfo.Website);
			var metaDataBytes = Encoding.Unicode.GetBytes(metaData);
			var result = new MemoryStream(metaDataBytes);

			return result;
		}
	}
}
