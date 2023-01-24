namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;

/// <summary>
/// Represents a LogoSyn application.
/// </summary>
public interface IApplication : IDisposable
{
	/// <summary>
	/// Runs the application.
	/// </summary>
	/// <param name="cancellationToken">
	/// Token used to signal the application to exit execution.
	/// </param>
	/// <returns>A task that will complete upon the application exiting.</returns>
	Task RunAsync(CancellationToken cancellationToken);
}
