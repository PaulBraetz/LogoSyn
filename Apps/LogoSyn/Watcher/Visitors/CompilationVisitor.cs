using Fort;
using RhoMicro.Common.IO;
using RhoMicro.Common.System.Abstractions;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;
using Scli;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Watcher.Visitors
{
	internal sealed class CompilationVisitor : VisitorBase<IApplicationContext>
	{
		public CompilationVisitor(String sourcePath)
		{
			sourcePath.ThrowIfDefault(nameof(sourcePath));

			var info = new FileInfo(sourcePath);
			_sourceFilePath = info.FullName;
			_sourceFileName = info.Name;
			_sourceDirectory = info.Directory?.FullName ?? String.Empty;
		}

		private readonly String _sourceFilePath;
		private readonly String _sourceDirectory;
		private readonly String _sourceFileName;

		private const Int32 STATE_IDLE = 0;
		private const Int32 STATE_COMPILING = 1;
		private Int32 _compilationState = STATE_IDLE;

		private Func<Task>? _waitingHandler;

		private const Int32 COMPILE_READ_ACCESS_TRIES = 10;
		private const Int32 COMPILE_READ_ACCESS_TIMEOUT = 2500;
		private const Int32 COMPILE_READ_ACCESS_DELAY = COMPILE_READ_ACCESS_TIMEOUT / COMPILE_READ_ACCESS_TRIES;

		protected override void Receive(IApplicationContext obj)
		{
			var context = (ICompilationContext)obj;
			using var watcher = GetWatcher(context);
			PrintExitPrompt();
			Console.ReadKey();
			Print("Stopping");
		}

		private FileSystemWatcher GetWatcher(ICompilationContext context)
		{
			var watcher = new FileSystemWatcher()
			{
				NotifyFilter = NotifyFilters.LastWrite,
				Path = _sourceDirectory,
				Filter = _sourceFileName
			};

			watcher.Changed += async (o, a) => await OnChange(context);
			watcher.EnableRaisingEvents = true;

			return watcher;
		}

		private async Task OnChange(ICompilationContext context)
		{
			if (Interlocked.CompareExchange(ref _compilationState, STATE_COMPILING, STATE_IDLE) == STATE_IDLE)
			{
				if (_waitingHandler != null)
				{
					await _waitingHandler.Invoke();
				}

				await RunHandler(context);

				_compilationState = STATE_IDLE;
			}
			else
			{
				_waitingHandler = () => RunHandler(context);
			}
		}

		private async Task RunHandler(ICompilationContext context)
		{
			var start = DateTimeOffset.Now;
			Print($"Compiling", ConsoleColor.DarkGreen);

			await Compile(context);

			var delta = DateTimeOffset.Now - start;
			Print($"Compilation Time: {delta:mm':'ss'.'fff}:", ConsoleColor.DarkGreen);
			PrintExitPrompt();
		}

		private void PrintExitPrompt()
		{
			Print($"Watching {_sourceFileName}, press any key to exit.");
		}

		private async Task Compile(ICompilationContext context)
		{
			try
			{
				using (var copyStream = new MemoryStream())
				{
					var sourceStream = await GetSourceStream();
					if (sourceStream == null)
					{
						return;
					}

					using (sourceStream)
					{
						await sourceStream.CopyToAsync(copyStream);
					}

					context.SetDocument(copyStream);
					await context.Compile(default);
				}
			}
			catch (Exception ex)
			{
				Print(ex.Message, ConsoleColor.DarkRed);
			}
		}

		private async Task<Stream?> GetSourceStream()
		{
			Stream? sourceStream = null;

			for (var i = 0; i < COMPILE_READ_ACCESS_TRIES; i++)
			{
				try
				{
					sourceStream = File.OpenRead(_sourceFilePath);
					break;
				}
				catch (IOException ex)
				when (ex.Message == @$"The process cannot access the file '{_sourceFilePath}' because it is being used by another process.")
				{
				}
				Print($"Attempt {i + 1}: Unable aquire read-lock, retrying...", ConsoleColor.DarkYellow);
				await Task.Delay(COMPILE_READ_ACCESS_DELAY);
			}

			if (sourceStream == null)
			{
				Print($"Timeout while attempting to aquire read-lock.", ConsoleColor.DarkRed);
			}
			else
			{
				Print($"Aquired read-lock", ConsoleColor.DarkGreen);
			}

			return sourceStream;
		}

		private void Print(String info, ConsoleColor? color = null)
		{
			Console.ResetColor();
			Console.Write("[LsWatch] >> ");
			if (color.HasValue)
			{
				Console.ForegroundColor = color.Value;
			}
			Console.WriteLine(info);
			Console.ResetColor();
		}

		protected override Boolean CanReceive(IApplicationContext obj)
		{
			var result = obj is ICompilationContext;

			return result;
		}
	}
}
