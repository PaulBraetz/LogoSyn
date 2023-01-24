using RhoMicro.LogoSyn.Libs.Common.Tests.TestsDataGenerator;
using RhoMicro.LogoSyn.Libs.Common.Tests.TestsDataGenerator.StringSlice;

var count = 25;

var dataSources = new IData[]
{
			new Data()
};

foreach(var source in dataSources)
{
	source.WriteToFile(count);
}
