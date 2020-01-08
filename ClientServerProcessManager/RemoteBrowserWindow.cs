using Microsoft.VisualStudio.Threading;
using RpcContract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using WebView2Sharp;
using IAsyncDisposable = Microsoft.VisualStudio.Threading.IAsyncDisposable;

namespace ClientServerProcessManager
{
    internal class RemoteBrowserWindow : IAsyncDisposable
    {
        private JoinableTaskFactory jtf;
        private WebView2Wrapper webView;

        public RemoteBrowserWindow(IntPtr handle, WebView2Wrapper webView, JoinableTaskFactory jtf)
        {
            this.Handle = handle;
            this.jtf = jtf;
            this.webView = webView;
        }

        public IntPtr Handle
        {
            get;
        }

        public IntPtr Parent 
        { 
            get;
            set; 
        }

        public Version HostVersionInfo
        {
            get;
            set;
        }

        public string PluginInformation
        {
            get;
            set;
        }

        public Dictionary<string, FileAlias> FileAliases
        {
            get;
            set;
        }

        public string PerfAnalyticsInfo
        {
            get;
            set;
        }

        public bool Disposed
        {
            get;
            private set;
        }

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

        public async Task DisposeAsync()
        {
            await this.jtf.SwitchToMainThreadAsync();
            this.Disposed = true;
        }

        public async Task NavigateToAsync(string url)
        {
            await this.jtf.SwitchToMainThreadAsync();
            this.webView.Navigate(url);
        }

        public async Task NavigateToStreamAsync(string baseUrl, string contents)
        {
            await this.jtf.SwitchToMainThreadAsync();
        }

        public async Task ReceiveMessage(string message)
        {
            await this.jtf.SwitchToMainThreadAsync();
        }

        public async Task ReplaceBodyContentsAsync(string contents)
        {
            await this.jtf.SwitchToMainThreadAsync();
        }

        public async Task SetWindowPostionAsync(IntPtr hwndAfter, Rect position, RpcContract.NativeMethods.ShowWindow flags)
        {
            await this.jtf.SwitchToMainThreadAsync();
        }

        public async Task SetZoomAsync(int zoom)
        {
            await this.jtf.SwitchToMainThreadAsync();
        }
    }
}
