using Microsoft.VisualStudio.Threading;
using RpcContract;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DaytonaClient
{
    /// <summary>
    /// This class is responsible for starting the ScriptedSandbox process and acting as a proxy for a client to communicate with it.
    /// </summary>
    public class DaytonaClient : IRpcContract
    {
        private const string ScriptedSandBoxProcessPath = @"..\..\..\ClientServerProcessManager\bin\Debug\ClientServerProcessManager.exe";

        private bool disposed;
        private IRpcContract inner;
        private Process scriptedSandboxProcess;
        private AsyncSemaphore disposeSem = new AsyncSemaphore(initialCount: 1);

        public event EventHandler FocusChanged
        {
            add { }
            remove { }
        }

        public event EventHandler TabOut
        {
            add { }
            remove { }
        }

        public event EventHandler ModelStateChanged
        {
            add { }
            remove { }
        }

        private DaytonaClient(Process process, IRpcContract inner)
        {
            this.inner = inner;
            this.scriptedSandboxProcess = process;
        }

        public static async Task<IRpcContract> CreateDaytonaClientAsync(CancellationToken cancellationToken = default)
        {
            var pipeName = Guid.NewGuid().ToString();

            var startInfo = new ProcessStartInfo()
            {
                FileName = ScriptedSandBoxProcessPath,
                Arguments = pipeName,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
            };

            var process = new Process()
            {
                StartInfo = startInfo
            };
            process.Start();

            var npcs = new NamedPipeClientStream("." /*server name: local computer*/, pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
            await npcs.ConnectWithRetryAsync(cancellationToken);

            var rpcContract = JsonRpc.Attach<IRpcContract>(new HeaderDelimitedMessageHandler(npcs));

            return new DaytonaClient(process, rpcContract);
        }

        public async Task<string> DoSomethingAsync()
        {
            try
            {
                return await this.inner.DoSomethingAsync();
            }
            catch (ConnectionLostException)
            {
                Console.Out.WriteLine("ScriptedSandBox process terminated unexpectedly.");
                throw;
            }
        }

        public async Task DisposeAsync()
        {
            if (!this.disposed && !this.scriptedSandboxProcess.HasExited)
            {
                using (await this.disposeSem.EnterAsync())
                {
                    if (!this.disposed && !this.scriptedSandboxProcess.HasExited)
                    {
                        this.disposed = true;

                        try
                        {
                            await this.inner.DisposeAsync();
                        }
                        catch (ConnectionLostException)
                        {
                            // Ignore this exception because its possible for the process to exit before this call returns
                            Console.Out.WriteLine("ScriptedSandbox terminated before the DisposeAsync call returned.");
                        }

                        var cancellationTokenSource = new CancellationTokenSource();
                        cancellationTokenSource.CancelAfter(500);

                        try
                        {
                            await this.scriptedSandboxProcess.WaitForExitAsync(cancellationTokenSource.Token);
                        }
                        catch (OperationCanceledException)
                        {
                            Debug.Assert(this.scriptedSandboxProcess.HasExited, "Server process did not exit.");
                        }
                    }
                }
            }
        }

        public Task PingHeartBeatAsync()
        {
            return this.inner.PingHeartBeatAsync();
        }

        public Task<(IntPtr browserHandle, IntPtr browserWindow)> CreateBrowserAsync(IntPtr parentHandle)
        {
            return this.inner.CreateBrowserAsync(parentHandle);
        }

        public Task DestroyBrowserAsync(IntPtr handle)
        {
            return this.inner.DestroyBrowserAsync(handle);
        }

        public Task PostUnicodeStringAsync(IntPtr browserHandle, string message)
        {
            return this.inner.PostUnicodeStringAsync(browserHandle, message);
        }

        public Task NavigateToAsync(IntPtr browserHandle, string url)
        {
            return this.inner.NavigateToAsync(browserHandle, url);
        }

        public Task ReplaceBodyContentsAsync(IntPtr browserHandle, string contents)
        {
            return this.inner.ReplaceBodyContentsAsync(browserHandle, contents);
        }

        public Task SetParentAsync(IntPtr browserHandle, IntPtr parent)
        {
            return this.inner.SetParentAsync(browserHandle, parent);
        }

        public Task<Dictionary<string, FileAlias>> GetFileAliasInfoAsync(IntPtr browserHandle)
        {
            return this.inner.GetFileAliasInfoAsync(browserHandle);
        }

        public Task<string> GetPerfAnalyticsInfoAsync(IntPtr browserHandle)
        {
            return this.inner.GetPerfAnalyticsInfoAsync(browserHandle);
        }

        public Task SetZoomAsync(IntPtr browserHandle, int zoom)
        {
            return this.inner.SetZoomAsync(browserHandle, zoom);
        }

        public Task SetHostVersionInfoAsync(IntPtr browserHandle, Version version)
        {
            return this.inner.SetHostVersionInfoAsync(browserHandle, version);
        }

        public Task SetPluginInformationAsync(IntPtr browserHandle, string info)
        {
            return this.SetPluginInformationAsync(browserHandle, info);
        }

        public Task<string> GetPluginInformationAsync(IntPtr browserHandle)
        {
            return this.inner.GetPluginInformationAsync(browserHandle);
        }

        public Task<Version> GetHostVersionInfoAsync(IntPtr browserHandle)
        {
            return this.inner.GetHostVersionInfoAsync(browserHandle);
        }

        public Task NavigateToStreamAsync(IntPtr browserHandle, string baseUrl, string contents)
        {
            return this.inner.NavigateToStreamAsync(browserHandle, baseUrl, contents);
        }

        public Task SetWindowPostionAsync(IntPtr browserHandle, IntPtr hwndAfter, Rect position, NativeMethods.SWP flags)
        {
            return this.inner.SetWindowPostionAsync(browserHandle, hwndAfter, position, flags);
        }

        public Task SetFileAliasInfoAsync(IntPtr browserHandle, Dictionary<string, FileAlias> fileAliases)
        {
            return this.inner.SetFileAliasInfoAsync(browserHandle, fileAliases);
        }

        public Task SetPerfAnalyticsInfo(IntPtr browserHandle, string initializationInfo)
        {
            return this.inner.SetPerfAnalyticsInfo(browserHandle, initializationInfo);
        }
    }
}
