namespace RhoMicro.LogoSyn.Libs.Common.Tests.TestsDataGenerator;

internal class Common
{
	public static String RandomValue(Int32 length)
	{
		return String.Concat(Enumerable.Range(0, length)
				.Select(i => Random.Shared.Next(36, 127))
				.Select(i => (Char)i))
			.Replace("\"\"", "\'");
	}
}
