using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace FBN_COVID
{
    public static class VueConnection
    {
        private static TimeSpan Timeout { get; } = TimeSpan.FromSeconds(60);

        private static string DoneMessage { get; } = "DONE  Compiled successfully in";

        public static void UseVueDevelopmentServer(this ISpaBuilder spa, IConfiguration Configuration)
        {
            int Port = Configuration.GetValue<int>("AppSettings:VuePort");
            Uri DevelopmentServerEndpoint = new Uri(Configuration.GetValue<string>("AppSettings:VueUri"));

            spa.UseProxyToSpaDevelopmentServer(async () =>
            {
                var loggerFactory = spa.ApplicationBuilder.ApplicationServices.GetService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger("Vue");

                if (IsRunning(Port))
                {
                    return DevelopmentServerEndpoint;
                }
                ///c 
                var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
                var processInfo = new ProcessStartInfo
                {
                    FileName = isWindows ? "cmd" : "npm",
                    Arguments = $"{(isWindows ? "npm " : "")}run serve",
                    WorkingDirectory = "ClientApp",
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                };
                var process = Process.Start(processInfo);
                var tcs = new TaskCompletionSource<int>();
                await Task.Run(() =>
                {
                    try
                    {
                        string line;
                        while ((line = process?.StandardOutput.ReadLine()) != null)
                        {
                            logger.LogInformation(line);
                            if (!tcs.Task.IsCompleted && line.Contains(DoneMessage, StringComparison.OrdinalIgnoreCase))
                            {
                                tcs.SetResult(1);
                            }
                        }
                    }
                    catch (EndOfStreamException ex)
                    {
                        logger.LogError(ex.ToString());
                        tcs.SetException(new InvalidOperationException("'npm run serve' failed.", ex));
                    }
                }).ConfigureAwait(true);
                await Task.Run(() =>
                {
                    try
                    {
                        string line;
                        while ((line = process?.StandardError.ReadLine()) != null)
                        {
                            logger.LogError(line);
                        }
                    }
                    catch (EndOfStreamException ex)
                    {
                        logger.LogError(ex.ToString());
                        tcs.SetException(new InvalidOperationException("'npm run serve' failed.", ex));
                    }
                }).ConfigureAwait(true);

                var timeout = Task.Delay(Timeout);
                if (await Task.WhenAny(timeout, tcs.Task).ConfigureAwait(false) == timeout)
                {
                    throw new TimeoutException();
                }

                return DevelopmentServerEndpoint;
            });
        }

        private static bool IsRunning(int port) => IPGlobalProperties.GetIPGlobalProperties()
                .GetActiveTcpListeners()
                .Select(x => x.Port)
                .Contains(port);
    }
}
