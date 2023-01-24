namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;

/// <summary>
/// The context in which an application is run.
/// </summary>
public interface IApplicationContext : IDisposable
{
	/// <summary>
	/// Returns information on the application context suitable for displaying to the user.
	/// </summary>
	/// <returns></returns>
	String GetHelpInfo();
}
