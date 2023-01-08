using Fort;
using RhoMicro.Common.System;
using RhoMicro.Common.System.Abstractions;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common
{
	/// <summary>
	/// Default implementation of <see cref="IApplication"/>.
	/// </summary>
	internal sealed class Application : DisposableBase, IApplication
	{
		/// <summary>
		/// Initializes a new instance with the context provided.
		/// </summary>
		/// <param name="contexts">The context in which the application is to be executed.</param>
		/// <param name="visitors">Visitors used to handle the applications context.</param>
		/// <param name="asyncVisitors">Visitors used to handle the applications context asynchronously.</param>
		public Application(IEnumerable<IApplicationContext> contexts, IEnumerable<IVisitor<IApplicationContext>> visitors, IEnumerable<IAsyncVisitor<IApplicationContext>> asyncVisitors)
		{
			contexts.ThrowIfDefault(nameof(contexts));
			visitors.ThrowIfDefault(nameof(visitors));
			asyncVisitors.ThrowIfDefault(nameof(asyncVisitors));

			_contexts = contexts;
			_visitors = visitors;
			_asyncVisitors = asyncVisitors;
		}

		private readonly SemaphoreSlim _gate = new(1, 1);
		private readonly IEnumerable<IVisitor<IApplicationContext>> _visitors;
		private readonly IEnumerable<IAsyncVisitor<IApplicationContext>> _asyncVisitors;
		private readonly IEnumerable<IApplicationContext> _contexts;

		/// <inheritdoc/>
		public async Task RunAsync(CancellationToken cancellationToken)
		{
			ThrowIfDisposed(nameof(Application));

			await _gate.WaitAsync(cancellationToken);
			try
			{
				var exceptions = await VisitContexts(cancellationToken);

				if (exceptions.Count > 0)
				{
					throw new AggregateException(exceptions);
				}
			}
			finally
			{
				_gate.Release();
			}
		}

		private async Task<List<Exception>> VisitContexts(CancellationToken cancellationToken)
		{
			var exceptions = new List<Exception>();
			foreach (var context in _contexts)
			{
				VisitContext(context, exceptions, cancellationToken);
				await VisitContextAsync(context, exceptions, cancellationToken);
			}

			return exceptions;
		}

		private async Task VisitContextAsync(IApplicationContext context, List<Exception> exceptions, CancellationToken cancellationToken)
		{
			foreach (var asyncVisitor in _asyncVisitors)
			{
				try
				{
					await asyncVisitor.VisitAsync(context, cancellationToken);
				}
				catch (Exception ex)
				{
					exceptions.Add(ex);
				}
			}
		}
		private void VisitContext(IApplicationContext context, List<Exception> exceptions, CancellationToken cancellationToken)
		{
			foreach (var visitor in _visitors)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return;
				}

				try
				{
					visitor.Visit(context);
				}
				catch (Exception ex)
				{
					exceptions.Add(ex);
				}
			}
		}
	}
}
