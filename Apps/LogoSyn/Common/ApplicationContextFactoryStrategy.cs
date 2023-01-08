using Fort;
using RhoMicro.Common.System.Abstractions;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common
{
	internal sealed class ApplicationContextFactoryStrategy : IFactory<IApplicationContext>
	{
		public ApplicationContextFactoryStrategy(Func<IApplicationContext> createStrategy)
		{
			createStrategy.ThrowIfDefault(nameof(createStrategy));

			_createStrategy = createStrategy;
		}

		private readonly Func<IApplicationContext> _createStrategy;

		public IApplicationContext Create()
		{
			var result = _createStrategy.Invoke();

			return result;
		}
	}
}
