using Microsoft.VisualStudio.Threading;
using RpcContract;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace ClientServerProcessManager
{
    internal class BrowserManager : IRpcContract
    {
        private JoinableTaskFactory jtf;
        private Func<Task> shutdownTask;
        private ImmutableDictionary<IntPtr, RemoteBrowserWindow> browsers = ImmutableDictionary<IntPtr, RemoteBrowserWindow>.Empty;

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

        public BrowserManager(JoinableTaskFactory jtf, Func<Task> shutdown)
        {
            this.jtf = jtf;
            this.shutdownTask = shutdown;
        }

        public async Task<string> DoSomethingAsync()
        {
            await this.jtf.SwitchToMainThreadAsync();
            return "This message is from the server";
        }

        public async Task DisposeAsync()
        {
            await this.jtf.SwitchToMainThreadAsync();

            var browserListCopy = this.browsers;
            this.browsers = ImmutableDictionary<IntPtr, RemoteBrowserWindow>.Empty;

            foreach (var browser in browserListCopy.Values)
            {
                await browser.DisposeAsync();
            }

            await this.shutdownTask();
        }

        public async Task PingHeartBeatAsync()
        {
            // If we can successfully switch to the main thread we shouldn't be stuck doing any work
            await this.jtf.SwitchToMainThreadAsync();
        }

        public async Task CreateBrowserAsync(IntPtr handle)
        {
            await this.jtf.SwitchToMainThreadAsync();

            // Do other browser create stuff

            ImmutableInterlocked.TryAdd(ref this.browsers, handle, new RemoteBrowserWindow(handle, this.jtf));
        }

        public async Task DestroyBrowserAsync(IntPtr handle)
        {
            await this.jtf.SwitchToMainThreadAsync();

            if (ImmutableInterlocked.TryRemove(ref this.browsers, handle, out RemoteBrowserWindow browser))
            {
                await browser.DisposeAsync();
            }
        }

        public async Task PostUnicodeStringAsync(IntPtr browserHandle, string message)
        {
            await this.jtf.SwitchToMainThreadAsync();
            var browser = this.GetRemoteBrowserWindow(browserHandle);
            await browser.ReceiveMessage(message);
        }

        public async Task NavigateToAsync(IntPtr browserHandle, string url)
        {
            await this.jtf.SwitchToMainThreadAsync();
            var browser = this.GetRemoteBrowserWindow(browserHandle);
            await browser.NavigateToAsync(url);
        }

        public async Task ReplaceBodyContentsAsync(IntPtr browserHandle, string contents)
        {
            await this.jtf.SwitchToMainThreadAsync();
            var browser = this.GetRemoteBrowserWindow(browserHandle);
            await browser.ReplaceBodyContentsAsync(contents);
        }

        public async Task SetParentAsync(IntPtr browserHandle, IntPtr parent)
        {
            await this.jtf.SwitchToMainThreadAsync();
            var browser = this.GetRemoteBrowserWindow(browserHandle);
            browser.Parent = parent;
        }

        public async Task<string> GetPluginInformationAsync(IntPtr browserHandle)
        {
            await this.jtf.SwitchToMainThreadAsync();
            var browser = this.GetRemoteBrowserWindow(browserHandle);
            return browser.PluginInformation;
        }

        public async Task<Version> GetHostVersionInfoAsync(IntPtr browserHandle)
        {
            await this.jtf.SwitchToMainThreadAsync();
            var browser = this.GetRemoteBrowserWindow(browserHandle);
            return browser.HostVersionInfo;
        }

        public async Task<Dictionary<string, FileAlias>> GetFileAliasInfoAsync(IntPtr browserHandle)
        {
            await this.jtf.SwitchToMainThreadAsync();
            var browser = this.GetRemoteBrowserWindow(browserHandle);
            return browser.FileAliases;
        }

        public async Task<string> GetPerfAnalyticsInfoAsync(IntPtr browserHandle)
        {
            await this.jtf.SwitchToMainThreadAsync();
            var browser = this.GetRemoteBrowserWindow(browserHandle);
            return browser.PerfAnalyticsInfo;
        }

        public async Task SetZoomAsync(IntPtr browserHandle, int zoom)
        {
            await this.jtf.SwitchToMainThreadAsync();
            var browser = this.GetRemoteBrowserWindow(browserHandle);
            await browser.SetZoomAsync(zoom);
        }

        public async Task SetHostVersionInfoAsync(IntPtr browserHandle, Version version)
        {
            await this.jtf.SwitchToMainThreadAsync();
            var browser = this.GetRemoteBrowserWindow(browserHandle);
            browser.HostVersionInfo = version;
        }

        public async Task SetPluginInformationAsync(IntPtr browserHandle, string info)
        {
            await this.jtf.SwitchToMainThreadAsync();
            var browser = this.GetRemoteBrowserWindow(browserHandle);
            browser.PluginInformation = info;
        }

        public async Task NavigateToStreamAsync(IntPtr browserHandle, string baseUrl, string contents)
        {
            await this.jtf.SwitchToMainThreadAsync();
            var browser = this.GetRemoteBrowserWindow(browserHandle);
            await browser.NavigateToStreamAsync(baseUrl, contents);
        }

        public async Task SetWindowPostionAsync(IntPtr browserHandle, IntPtr hwndAfter, Rect position, NativeMethods.ShowWindow flags)
        {
            await this.jtf.SwitchToMainThreadAsync();
            var browser = this.GetRemoteBrowserWindow(browserHandle);
            await browser.SetWindowPostionAsync(hwndAfter, position, flags);
        }

        public async Task SetFileAliasInfoAsync(IntPtr browserHandle, Dictionary<string, FileAlias> fileAliases)
        {
            await this.jtf.SwitchToMainThreadAsync();
            var browser = this.GetRemoteBrowserWindow(browserHandle);
            browser.FileAliases = fileAliases;
        }

        public async Task SetPerfAnalyticsInfo(IntPtr browserHandle, string initializationInfo)
        {
            await this.jtf.SwitchToMainThreadAsync();
            var browser = this.GetRemoteBrowserWindow(browserHandle);
            browser.PerfAnalyticsInfo = initializationInfo;
        }

        private RemoteBrowserWindow GetRemoteBrowserWindow(IntPtr handle)
        {
            if (this.browsers.TryGetValue(handle, out RemoteBrowserWindow browser))
            {
                return browser;
            }

            throw new ArgumentException($"Browser for handle '{handle}' was not found.");
        }
    }
}
