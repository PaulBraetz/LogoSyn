using Fort;

using RhoMicro.Common.System.Abstractions;

using Scli;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;

internal abstract class ApplicationContextFactoryBase : IFactory<IApplicationContext?>
{
	public ApplicationContextFactoryBase(IParameterCollection parameters, String[] args)
	{
		parameters.ThrowIfDefault(nameof(parameters));
		args.ThrowIfDefault(nameof(args));

		try
		{
			_arguments = parameters.MatchArguments(args);
		} catch(Exception ex)
		  when(ex.Message.Contains("Unknown argument", StringComparison.InvariantCultureIgnoreCase))
		{
			_arguments = parameters.MatchArguments(Array.Empty<String>());
		}
	}

	private readonly IArgumentCollection _arguments;

	protected abstract Boolean CanCreate(IArgumentCollection arguments);
	protected abstract IApplicationContext? CreateContext(IArgumentCollection arguments);
	public IApplicationContext? Create()
	{
		IApplicationContext? result = null;

		if(CanCreate(_arguments))
		{
			result = CreateContext(_arguments);
		}

		return result;
	}
}
