using Microsoft.VisualStudio.Threading;
using RpcContract;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace WpfWebView2Client
{
    public class WebView2 : Control
    {
        JoinableTaskFactory joinableTaskFactory;
        private WebView2HwndHost hwndHost;
        private IRpcContract client;
        private IntPtr browserHandle;

        public WebView2()
        {
            this.joinableTaskFactory = 
                new JoinableTaskFactory(
                    new JoinableTaskCollection(
                        new JoinableTaskContext()));
        }

        public Uri Source
        {
            get { return (Uri)this.GetValue(SourceProperty); }
            set { this.SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(Uri), typeof(WebView2), new PropertyMetadata(null, OnSourceChanged));

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WebView2 webView = (WebView2)d;
            webView.Navigate(webView.Source);
        }

        static WebView2()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WebView2), new FrameworkPropertyMetadata(typeof(WebView2)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.GetTemplateChild("insertHwndHostHere") is Border hostElement &&
                !DesignerProperties.GetIsInDesignMode(this))
            {
                this.hwndHost = new WebView2HwndHost();
                hostElement.Child = this.hwndHost;
                this.hwndHost.Loaded += OnHwndHostLoaded;
            }
        }

        public void OnHwndHostLoaded(object sender, RoutedEventArgs e)
        {
            this.hwndHost.Loaded -= OnHwndHostLoaded;

            this.joinableTaskFactory.RunAsync(async () =>
            {
                this.client = await DaytonaClient.DaytonaClient.CreateDaytonaClientAsync();
                var handles = await this.client.CreateBrowserAsync(this.hwndHost.Handle);
                this.browserHandle = handles.browserHandle;
                await this.UpdateChildPlacementAsync();
                await this.client.NavigateToAsync(browserHandle, "https://reddit.com");
            });
        }

        private async Task UpdateChildPlacementAsync()
        {
            if (this.hwndHost.Handle != IntPtr.Zero)
            {
                NativeMethods.WINDOWPLACEMENT placement = new NativeMethods.WINDOWPLACEMENT();
                if (NativeMethods.GetWindowPlacement(this.hwndHost.Handle, placement))
                {
                    var position = placement.rcNormalPosition;
                    await this.client.SetWindowPostionAsync(this.browserHandle, /*hwndAfter*/ IntPtr.Zero, new Rect(position.Left, position.Top, position.Right - position.Left, position.Bottom - position.Top), RpcContract.NativeMethods.SWP.NOMOVE);
                }
            }
        }

        public void Close()
        {
            if (this.client != null)
            {
                this.joinableTaskFactory.Run(async () =>
                {
                    await this.client.DisposeAsync();
                });
            }
        }

        public void Navigate(Uri source)
        {
            this.Navigate(WebView2.UriToString(source));
        }

        public void Navigate(string source)
        {
            if (this.client != null)
            {
                this.joinableTaskFactory.RunAsync(async () =>
                {
                    await this.client.NavigateToAsync(browserHandle, source);
                }).Task.Forget();
            }
        }

        /// <summary>
        /// Copied from BindUriHelper.UriToString.
        ///
        /// Uri-toString does 3 things over the standard .toString()
        ///
        ///  1) We don't unescape special control characters. The default Uri.ToString()
        ///     will unescape a character like ctrl-g, or ctrl-h so the actual char is emitted.
        ///     However it's considered safer to emit the escaped version.
        ///
        ///  2) We truncate urls so that they are always <= MAX_URL_LENGTH
        ///
        /// This method should be called whenever you are taking a Uri
        /// and performing a p-invoke on it.
        /// </summary>
        internal static string UriToString(Uri uri)
        {
            const int MAX_PATH_LENGTH = 2048;
            const int MAX_SCHEME_LENGTH = 32;
            const int MAX_URL_LENGTH = MAX_PATH_LENGTH + MAX_SCHEME_LENGTH + 3; /*=sizeof("://")*/

            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            return new StringBuilder(
                uri.GetComponents(
                    uri.IsAbsoluteUri ? UriComponents.AbsoluteUri : UriComponents.SerializationInfoString,
                    UriFormat.SafeUnescaped),
                MAX_URL_LENGTH).ToString();
        }

        public class WebView2HwndHost : HwndHost
        {
            private HwndSource hwndSource;

            protected override HandleRef BuildWindowCore(HandleRef hwndParent)
            {
                this.hwndSource = new HwndSource(0, (int)NativeMethods.WindowStyles.WS_CHILD, 0, 0, 0, 300, 300, "WebView2Host", new WindowInteropHelper(Application.Current.MainWindow).Handle);
                return new HandleRef(this, this.hwndSource.Handle);
            }

            protected override void DestroyWindowCore(HandleRef hwnd)
            {
                if (this.hwndSource != null)
                {
                    this.hwndSource.Dispose();
                    this.hwndSource = null;
                }
            }
        }
    }
}
