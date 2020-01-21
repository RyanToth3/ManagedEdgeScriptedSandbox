using Microsoft.VisualStudio.Threading;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace WebView2Sharp
{
    public class WebView2Wrapper
    {
        private readonly IntPtr hwndTarget;
        private readonly EnvironmentCompletedHandler environmentCompletedHandler;
        private readonly WebViewCompletedHandler webViewCompletedHandler;
        private NativeMethods.IWebView2WebView webView;
        private string pendingNavigate;
        private bool pendingNavigateIsHtml;
        private IntPtr parentHandle;
        public  JoinableTaskFactory JoinableTaskFactory;

        private AsyncManualResetEvent webViewCreationEvent = new AsyncManualResetEvent(initialState: false);

        private WebView2Wrapper(IntPtr hwndTarget, IntPtr parentHandle, JoinableTaskFactory factory)
        {
            this.JoinableTaskFactory = factory;
            this.hwndTarget = hwndTarget;
            this.parentHandle = parentHandle;
            this.environmentCompletedHandler = new EnvironmentCompletedHandler(this);
            this.webViewCompletedHandler = new WebViewCompletedHandler(this);
        }

        public static async Task<WebView2Wrapper> CreateWebView2WrapperAsync(IntPtr hwndTarget, IntPtr parentHandle, JoinableTaskFactory factory)
        {
            var wrapper = new WebView2Wrapper(hwndTarget, parentHandle, factory);

            // string userDataFolder = Path.Combine(Path.GetTempPath(), "WebView2Cache");
            string userDataFolder = null;
            int result = NativeMethods.CreateWebView2EnvironmentWithDetails(null, userDataFolder, null, wrapper.environmentCompletedHandler);

            if (result != 0)
            {
                throw new Win32Exception("Failed to create the web view environment with details.");
            }

            // No point in returning until the webview is actually initialized
            await wrapper.webViewCreationEvent.WaitAsync();

            wrapper.EnsureVisible();

            if (parentHandle != NativeMethods.GetDesktopWindow())
            {
                wrapper.OnWindowSizeChanged(parentHandle);
            }

            return wrapper;
        }

        public void Navigate(string uri)
        {
            if (this.webView == null)
            {
                this.pendingNavigate = uri;
                this.pendingNavigateIsHtml = false;
                return;
            }

            this.webView.Navigate(uri);
        }

        public void NavigateToString(string text)
        {
            if (this.webView == null)
            {
                this.pendingNavigate = text;
                this.pendingNavigateIsHtml = true;
                return;
            }

            this.webView.NavigateToString(text);
        }

        public void ExecuteScript(string javascript)
        {
            if (this.webView != null)
            {
                this.webView.ExecuteScript(javascript, new ExecuteScriptCompletedEventHandler(this));
                
                // Maybe wait for the script to finish executing here?
            }
        }

        public void SetZoom(int zoom)
        {
            this.webView?.put_ZoomFactor(zoom);
        }

        public void OnWindowSizeChanged(IntPtr parentHandle)
        {
            if (this.webView != null && parentHandle != IntPtr.Zero)
            {
                NativeMethods.RECT bounds = new NativeMethods.RECT();
                NativeMethods.GetClientRect(parentHandle, out bounds);
                this.UpdateWindowSize(bounds, NativeMethods.SWP.NOMOVE);
            }
        }

        public void UpdateWindowSize(NativeMethods.RECT bounds, NativeMethods.SWP flags)
        {
            if (this.webView != null && parentHandle != NativeMethods.GetDesktopWindow())
            {
                this.webView.put_Bounds(bounds);
                NativeMethods.SetWindowPos(this.hwndTarget, IntPtr.Zero, bounds.X, bounds.Y, bounds.Width, bounds.Height, flags);
                this.EnsureVisible();
            }
        }

        public void SetParent(IntPtr parentHandle)
        {
            this.parentHandle = parentHandle;
            NativeMethods.ShowWindow(parentHandle, NativeMethods.SW.SHOW);
        }

        private void EnsureVisible()
        {
            if (this.webView != null && this.parentHandle != NativeMethods.GetDesktopWindow())
            {
                var visible = this.webView.get_IsVisible();
                if (visible == 0)
                {
                    this.webView.put_IsVisible(1);
                }

                NativeMethods.ShowWindow(this.parentHandle, NativeMethods.SW.SHOW);
            }
        }

        private void OnWebViewCreated(NativeMethods.IWebView2WebView webView)
        {
            this.webView = webView;
            this.webView.add_NavigationCompleted(new NavigationCompletedEventHandler(this), out _);

            var settings = this.webView.get_Settings();
            settings.put_IsScriptEnabled(1);
            settings.put_AreDefaultScriptDialogsEnabled(1);
            settings.put_IsWebMessageEnabled(1);

            this.webView.add_WebMessageReceived(new WebMessageReceivedEventHandler(this), out _);

            NativeMethods.ShowWindow(this.parentHandle, NativeMethods.SW.SHOW);

            if (!string.IsNullOrEmpty(this.pendingNavigate))
            {
                if (this.pendingNavigateIsHtml)
                {
                    this.NavigateToString(this.pendingNavigate);
                }
                else
                {
                    this.Navigate(this.pendingNavigate);
                }
            }

            webViewCreationEvent.Set();
        }

        private void OnNavigationCompleted()
        {
            // Do something?
        }

        private void OnScriptExecutionCompleted()
        {
            // Do something?
        }

        private void OnWebMessageReceived()
        {
            // Do something?
        }

        private class WebViewCompletedHandler : NativeMethods.IWebView2CreateWebViewCompletedHandler
        {
            private readonly WebView2Wrapper wrapper;

            public WebViewCompletedHandler(WebView2Wrapper wrapper)
            {
                this.wrapper = wrapper;
            }

            public void Invoke(int result, NativeMethods.IWebView2WebView webView)
            {
                this.wrapper.OnWebViewCreated(webView);
            }
        }

        private class EnvironmentCompletedHandler : NativeMethods.IWebView2CreateWebView2EnvironmentCompletedHandler
        {
            private readonly WebView2Wrapper wrapper;

            public EnvironmentCompletedHandler(WebView2Wrapper wrapper)
            {
                this.wrapper = wrapper;
            }

            public void Invoke(int result, NativeMethods.IWebView2Environment webViewEnvironment)
            {
                Task.Run(async () => 
                {
                    await this.wrapper.JoinableTaskFactory.SwitchToMainThreadAsync();
                    webViewEnvironment.CreateWebView(this.wrapper.hwndTarget, this.wrapper.webViewCompletedHandler);
                });
            }
        }

        private class NavigationCompletedEventHandler : NativeMethods.IWebView2NavigationCompletedEventHandler
        {
            private readonly WebView2Wrapper wrapper;

            public NavigationCompletedEventHandler(WebView2Wrapper wrapper)
            {
                this.wrapper = wrapper;
            }

            public void Invoke([In, MarshalAs(UnmanagedType.Interface)] NativeMethods.IWebView2WebView webview, [In, MarshalAs(UnmanagedType.Interface)] NativeMethods.IWebView2NavigationCompletedEventArgs args)
            {
                this.wrapper.OnNavigationCompleted();
            }
        }

        private class ExecuteScriptCompletedEventHandler : NativeMethods.IWebView2ExecuteScriptCompletedHandler
        {
            private readonly WebView2Wrapper wrapper;

            public ExecuteScriptCompletedEventHandler(WebView2Wrapper wrapper)
            {
                this.wrapper = wrapper;
            }

            public void Invoke([In, MarshalAs(UnmanagedType.Error)] int errorCode, [In, MarshalAs(UnmanagedType.LPWStr)] string resultObjectAsJson)
            {
                this.wrapper.OnScriptExecutionCompleted();
            }
        }

        private class WebMessageReceivedEventHandler : NativeMethods.IWebView2WebMessageReceivedEventHandler
        {
            private readonly WebView2Wrapper wrapper;

            public WebMessageReceivedEventHandler(WebView2Wrapper wrapper)
            {
                this.wrapper = wrapper;
            }

            public void Invoke([In, MarshalAs(UnmanagedType.Interface)] NativeMethods.IWebView2WebView webview, [In, MarshalAs(UnmanagedType.Interface)] NativeMethods.IWebView2WebMessageReceivedEventArgs args)
            {
                this.wrapper.OnWebMessageReceived();
            }
        }
    }
}
