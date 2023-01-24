using RhoMicro.Common.System;
using RhoMicro.Common.System.Security.Cryptography.Hashing.Abstractions;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging.Abstractions;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging;

internal sealed class PackageLoaderQueue : DisposableBase, IPackageLoader
{
	private readonly SemaphoreSlim _gate = new(1, 1);
	private readonly IDictionary<Int32, IPackageLoader> _loaders = new Dictionary<Int32, IPackageLoader>();

	public Int32 Count => _loaders.Count;

	public PackageLoaderQueue Set(Int32 priority, IPackageLoader loader)
	{
		ThrowIfDisposed(nameof(PackageLoaderQueue));
		_gate.Wait();
		try
		{
			Add(priority, loader);
		} finally
		{
			_ = _gate.Release();
		}

		return this;
	}

	private void Add(Int32 priority, IPackageLoader loader)
	{
		if(!_loaders.TryAdd(priority, loader))
		{
			_ = _loaders.Remove(priority);
			_loaders.Add(priority, loader);
		}
	}

	public PackageLoaderQueue Set(IPackageLoader loader)
	{
		ThrowIfDisposed(nameof(PackageLoaderQueue));
		_gate.Wait();
		try
		{
			Add(Count, loader);
		} finally
		{
			_ = _gate.Release();
		}

		return this;
	}

	public async Task<IPackageInfo?> Load(String name, String version, CancellationToken cancellationToken = default)
	{
		ThrowIfDisposed(nameof(PackageLoaderQueue));
		await _gate.WaitAsync(cancellationToken);
		try
		{
			var loadingTasks = _loaders
				.OrderBy(kvp => kvp.Key)
				.Select(kvp => kvp.Value)
				.Select(l => l.Load(name, version, cancellationToken));

			foreach(var loadingTask in loadingTasks)
			{
				var result = await loadingTask;

				if(result != null)
				{
					return result;
				}
			}
		} finally
		{
			_ = _gate.Release();
		}

		return null;
	}

	public async Task<IPackage?> Load(String name, String version, IHash<IPackage> packageHash, CancellationToken cancellationToken = default)
	{
		ThrowIfDisposed(nameof(PackageLoaderQueue));
		await _gate.WaitAsync(cancellationToken);
		try
		{
			var loadingTasks = _loaders
				.OrderBy(kvp => kvp.Key)
				.Select(kvp => kvp.Value)
				.Select(l => l.Load(name, version, packageHash, cancellationToken));

			foreach(var loadingTask in loadingTasks)
			{
				var result = await loadingTask;

				if(result != null)
				{
					return result;
				}
			}
		} finally
		{
			_ = _gate.Release();
		}

		return null;
	}

	protected override void DisposeManaged(Boolean disposing)
	{
		_gate.Wait();
		_gate.Dispose();
		base.DisposeManaged(disposing);
	}
}
