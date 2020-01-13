using Microsoft.VisualStudio.Threading;
using RpcContract;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
        private BrowserHolder browserHolder;

        public RemoteBrowserWindow(BrowserHolder browserHolder, WebView2Wrapper webView, JoinableTaskFactory jtf)
        {
            this.jtf = jtf;
            this.webView = webView;
            this.browserHolder = browserHolder;
        }

        public IntPtr Handle
        {
            get
            {
                return this.browserHolder.Handle;
            }
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
            this.webView?.Navigate(url);
        }

        public async Task NavigateToStreamAsync(string baseUrl, string contents)
        {
            await this.jtf.SwitchToMainThreadAsync();
        }

        public async Task ReceiveMessage(string message)
        {
            await this.jtf.SwitchToMainThreadAsync();

            // TODO: no clue if this is the right way to do this
            this.webView?.ExecuteScript(message);
        }

        public async Task ReplaceBodyContentsAsync(string contents)
        {
            await this.jtf.SwitchToMainThreadAsync();
        }

        public async Task SetParentAsync(IntPtr parentHandle)
        {
            await this.jtf.SwitchToMainThreadAsync();
            this.browserHolder.ParentHandle = parentHandle;
            this.webView.SetParent(parentHandle);
            this.webView.OnWindowSizeChanged(parentHandle);
        }

        public async Task SetWindowPostionAsync(IntPtr hwndAfter, Rect position, NativeMethods.SWP flags)
        {
            await this.jtf.SwitchToMainThreadAsync();
            if (this.webView == null)
            {
                NativeMethods.SetWindowPos(this.browserHolder.Handle, hwndAfter, (int)position.X, (int)position.Y, (int)position.Width, (int)position.Height, flags);
            }
        }

        public async Task SetZoomAsync(int zoom)
        {
            await this.jtf.SwitchToMainThreadAsync();
        }

        private static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 8)
                return NativeMethods.SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            else
                return new IntPtr(NativeMethods.SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
        }
    }
}
