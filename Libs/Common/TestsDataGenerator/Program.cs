namespace RhoMicro.LogoSyn.Libs.Common.Tests.TestsDataGenerator
{
	internal class Program
	{
		static void Main(String[] args)
		{
			var count = 25;

			var dataSources = new IData[]
			{
				new StringSlice.Data()
			};

			foreach (var source in dataSources)
			{
				source.WriteToFile(count);
			}
		}
	}
}