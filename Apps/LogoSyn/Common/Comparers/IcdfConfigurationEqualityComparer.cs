using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common.Comparers
{
	/// <summary>
	/// Defines methods to support the comparison of instances of <see cref="IDocumentInfo"/> for equality.
	/// </summary>
	public sealed class LogoSynConfigurationEqualityComparer : IEqualityComparer<IDocumentInfo>
	{
		private LogoSynConfigurationEqualityComparer() { }

		/// <summary>
		/// Instance of <see cref="LogoSynConfigurationEqualityComparer"/>.
		/// </summary>
		public static readonly LogoSynConfigurationEqualityComparer Instance = new();

		/// <inheritdoc/>
		public Boolean Equals(IDocumentInfo? x, IDocumentInfo? y)
		{
			if (x == null)
			{
				return y == null;
			}

			if (y == null)
			{
				return x == null;
			}

			var result = PackageInvocationInfoEqualityComparer.Instance.Equals(x.InterpreterInfo, y.InterpreterInfo) &&
				PackageInvocationInfoEqualityComparer.Instance.Equals(x.ParserInfo, y.ParserInfo);

			return result;
		}
		/// <inheritdoc/>
		public Int32 GetHashCode([DisallowNull] IDocumentInfo obj)
		{
			if (obj is null)
			{
				throw new ArgumentNullException(nameof(obj));
			}

			var hashCode = HashCode.Combine(PackageInvocationInfoEqualityComparer.Instance.GetHashCode(obj.InterpreterInfo),
											PackageInvocationInfoEqualityComparer.Instance.GetHashCode(obj.ParserInfo));

			return hashCode;
		}
	}
}
