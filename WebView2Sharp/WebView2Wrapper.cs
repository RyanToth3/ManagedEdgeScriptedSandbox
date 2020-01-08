using Microsoft.VisualStudio.Threading;
using System;
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
        public  JoinableTaskFactory JoinableTaskFactory;

        private AsyncManualResetEvent webViewCreationEvent = new AsyncManualResetEvent(initialState: false);

        public event EventHandler NavigationCompleted;

        private WebView2Wrapper(IntPtr hwndTarget, JoinableTaskFactory factory)
        {
            this.JoinableTaskFactory = factory;
            this.hwndTarget = hwndTarget;
            this.environmentCompletedHandler = new EnvironmentCompletedHandler(this);
            this.webViewCompletedHandler = new WebViewCompletedHandler(this);
        }

        public static async Task<WebView2Wrapper> CreateWebView2WrapperAsync(IntPtr hwndTarget, JoinableTaskFactory factory)
        {
            var wrapper = new WebView2Wrapper(hwndTarget, factory);

            // string userDataFolder = Path.Combine(Path.GetTempPath(), "WebView2Cache");
            string userDataFolder = null;
            int result = NativeMethods.CreateWebView2EnvironmentWithDetails(null, userDataFolder, null, wrapper.environmentCompletedHandler);

            // No point in returning until the webview is actually initialized
            await wrapper.webViewCreationEvent.WaitAsync();

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

        public void OnWindowSizeChanged()
        {
            if (this.webView != null)
            {
                NativeMethods.RECT bounds = new NativeMethods.RECT();
                NativeMethods.GetClientRect(this.hwndTarget, out bounds);
                this.webView.put_Bounds(bounds);
            }
        }

        private void OnWebViewCreated(NativeMethods.IWebView2WebView webView)
        {
            this.webView = webView;
            this.webView.add_NavigationCompleted(new NavigationCompletedEventHandler(this), out _);

            this.OnWindowSizeChanged();

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
            this.NavigationCompleted?.Invoke(this, EventArgs.Empty);
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
    }
}
