using Microsoft.VisualStudio.Threading;
using RpcContract;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using WebView2Sharp;

namespace ClientServerProcessManager
{
    internal class BrowserManager : IRpcContract
    {
        private JoinableTaskFactory jtf;
        private Func<Task> shutdownTask;
        private ImmutableDictionary<IntPtr, RemoteBrowserWindow> browsers = ImmutableDictionary<IntPtr, RemoteBrowserWindow>.Empty;
        private int size = 0;

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

        public async Task<(IntPtr browserHandle, IntPtr browserWindow)> CreateBrowserAsync(IntPtr parentHandle)
        {
            await this.jtf.SwitchToMainThreadAsync();

            if (parentHandle == IntPtr.Zero)
            {
                // Set parent handle to the desktop window if one hasn't been specified yet
                parentHandle = NativeMethods.GetDesktopWindow();
            }

            var window = new BrowserHolder(parentHandle);

            // This is just a key that helps with identifying each webview
            var referenceHandle = this.ToHandle(Interlocked.Increment(ref this.size));

            WebView2Wrapper webView = await WebView2Wrapper.CreateWebView2WrapperAsync(window.Handle, parentHandle, this.jtf);

            ImmutableInterlocked.TryAdd(ref this.browsers, referenceHandle, new RemoteBrowserWindow(window, webView, this.jtf));

            return (referenceHandle, window.Handle);
        }

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const uint shrowd = 0x01DECAC11;
        const int shift = 8;
        IntPtr ToHandle(int index)
        {
            return new IntPtr(((uint)index << shift) ^ shrowd);
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
            await browser.SetParentAsync(parent);
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

        public async Task SetWindowPostionAsync(IntPtr browserHandle, IntPtr hwndAfter, Rect position, RpcContract.NativeMethods.SWP flags)
        {
            await this.jtf.SwitchToMainThreadAsync();
            var browser = this.GetRemoteBrowserWindow(browserHandle);
            await browser.SetWindowPostionAsync(hwndAfter, position, (NativeMethods.SWP)(int)flags);
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
