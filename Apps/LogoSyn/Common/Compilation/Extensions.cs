using Fort;
using RhoMicro.Common.IO;
using RhoMicro.Common.System.IO;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Abstractions;
using RhoMicro.LogoSyn.Apps.LogoSyn.Common.Packaging.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhoMicro.LogoSyn.Apps.LogoSyn.Common.Compilation
{
	internal static class Extensions
	{
		public static async Task Compile(this IDocumentInfo documentInfo,
									  IPackageLoader loader,
									  Stream? standardOutput = null,
									  Stream? standardError = null,
									  CancellationToken cancellationToken = default)
		{
			documentInfo.ThrowIfDefault(nameof(documentInfo));
			loader.ThrowIfDefault(nameof(loader));

			documentInfo.Source.Seek(documentInfo.SourceOffset, SeekOrigin.Begin);

			var redirectError = standardError != null;
			var redirectOutput = standardOutput != null;

			using var parserProcess = await GetProcess(documentInfo.ParserInfo, loader, true, redirectError, cancellationToken);
			var parserStarted = parserProcess.Start();
			if (!parserStarted)
			{
				throw new Exception($"Unable to invoke package {documentInfo.ParserInfo.PackageName} {documentInfo.ParserInfo.PackageVersion}");
			}

			using var interpreterProcess = await GetProcess(documentInfo.InterpreterInfo, loader, redirectOutput, redirectError, cancellationToken);
			var interpreterStarted = interpreterProcess.Start();
			if (!interpreterStarted)
			{
				throw new Exception($"Unable to invoke package {documentInfo.InterpreterInfo.PackageName} {documentInfo.InterpreterInfo.PackageVersion}");
			}

			await documentInfo.Source.CopyToAsync(parserProcess.StandardInput.BaseStream, cancellationToken);
			parserProcess.StandardInput.Close();

			await parserProcess.StandardOutput.BaseStream.CopyToAsync(interpreterProcess.StandardInput.BaseStream);
			interpreterProcess.StandardInput.Close();

			if (redirectOutput)
			{
				await interpreterProcess.StandardOutput.BaseStream.CopyToAsync(standardOutput!);
			}
			if (redirectError)
			{
				await parserProcess.StandardError.BaseStream.CopyToAsync(standardError!);
				await interpreterProcess.StandardError.BaseStream.CopyToAsync(standardError!);
			}

			await parserProcess.WaitForExitAsync(cancellationToken);
			await interpreterProcess.WaitForExitAsync(cancellationToken);
		}
		private static async Task<Process> GetProcess(IPackageInvocationInfo info,
			IPackageLoader loader,
			Boolean redirectOutput,
			Boolean redirectError,
			CancellationToken cancellationToken)
		{
			var package = await loader.Load(info.PackageName, info.PackageVersion, info.PackageHash, cancellationToken);
			VerifyPackage(info, package);

			var entryPoint = Path.Combine(package!.PackageDataDirectory.FullName, package.PackageInfo.EntryPoint!);
			var processInfo = new ProcessStartInfo()
			{
				UseShellExecute = false,
				RedirectStandardInput = true,
				RedirectStandardOutput = redirectOutput,
				RedirectStandardError = redirectError,
				FileName = entryPoint
			};
			foreach (var argument in info.Arguments ?? Array.Empty<String>())
			{
				processInfo.ArgumentList.Add(argument);
			}

			var result = new Process()
			{
				StartInfo = processInfo
			};

			return result;
		}
		private static void VerifyPackage(IPackageInvocationInfo info, IPackage? package)
		{
			if (package == null)
			{
				throw new InvalidOperationException($"Unable to load package {info.PackageName} ({info.PackageVersion}).");
			}

			if (package.PackageDataDirectory == null || !package.PackageDataDirectory.Exists)
			{
				throw new InvalidOperationException($"{nameof(package)}.{nameof(package.PackageDataDirectory)} is null or does not exist.");
			}

			if (package.PackageInfo?.EntryPoint == null || !File.Exists(Path.Combine(package.PackageDataDirectory.FullName, package.PackageInfo.EntryPoint)))
			{
				throw new InvalidOperationException($"Unable to locate entry point file.");
			}
		}
	}
}
