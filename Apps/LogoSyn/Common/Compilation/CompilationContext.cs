using Fort;

using RhoMicro.Common.System;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging.Abstractions;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common.Compilation;

internal sealed class CompilationContext : DisposableBase, ICompilationContext
{
	public CompilationContext(String helpInfo)
	{
		helpInfo.ThrowIfDefault(nameof(helpInfo));

		_helpInfo = helpInfo;
	}

	private readonly String _helpInfo;
	private readonly SemaphoreSlim _gate = new(1, 1);
	private IDocumentInfo? Document {
		get; set;
	}
	private PackageLoaderQueue PackageLoader { get; } = new PackageLoaderQueue();
	private Stream? StandardOutput {
		get; set;
	}
	private Stream? StandardError {
		get; set;
	}

	public String GetHelpInfo() => _helpInfo;
	public void AddPackageLoader(IPackageLoader loader)
	{
		ThrowIfDisposed(nameof(CompilationContext));
		loader.ThrowIfDefault(nameof(loader));

		_gate.Wait();
		try
		{
			_ = PackageLoader.Set(loader);
		} finally
		{
			_ = _gate.Release();
		}
	}
	public void SetDocument(Stream source)
	{
		ThrowIfDisposed(nameof(CompilationContext));
		source.ThrowIfDefaultOrNot(s => s.CanRead && s.CanSeek, $"{nameof(source)} must be read-enabled and seek-enabled.", nameof(source));

		_gate.Wait();
		try
		{
			Document = DocumentInfo.Read(source);
		} finally
		{
			_ = _gate.Release();
		}
	}
	public void SetStandardOutput(Stream standardOutput)
	{
		ThrowIfDisposed(nameof(CompilationContext));
		standardOutput.ThrowIfDefaultOrNot(s => s.CanWrite, $"{nameof(standardOutput)} must be write-enabled.", nameof(standardOutput));

		_gate.Wait();
		try
		{
			StandardOutput = standardOutput;
		} finally
		{
			_ = _gate.Release();
		}
	}
	public void SetStandardError(Stream standardError)
	{
		ThrowIfDisposed(nameof(CompilationContext));
		standardError.ThrowIfDefaultOrNot(s => s.CanWrite, $"{nameof(standardError)} must be write-enabled.", nameof(standardError));

		_gate.Wait();
		try
		{
			StandardError = standardError;
		} finally
		{
			_ = _gate.Release();
		}
	}
	public async Task Compile(CancellationToken cancellationToken)
	{
		ThrowIfDisposed(nameof(CompilationContext));

		_gate.Wait(cancellationToken);
		try
		{
			if(Document == null)
			{
				throw new InvalidOperationException($"{nameof(Document)} was not provided for compilation.");
			}

			await Document.Compile(PackageLoader, StandardOutput, StandardError, cancellationToken);
		} finally
		{
			_ = _gate.Release();
		}
	}

	protected override void DisposeManaged(Boolean disposing)
	{
		_gate.Wait();
		_gate.Dispose();
		Document?.Dispose();
		PackageLoader.Dispose();
		base.DisposeManaged(disposing);
	}
}
