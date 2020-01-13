using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WpfWebView2Client
{
    public static class NativeMethods
    {
        #region WebView2 APIs

        #region Initialization Interfaces
        [DllImport("WebView2Loader.dll")]
        public static extern int CreateWebView2EnvironmentWithDetails(string browserExecutableFolder, string userDataFolder, WEBVIEW2_RELEASE_CHANNEL_PREFERENCE releaseChannelPreference, string additionalBrowserArguments, IWebView2CreateWebView2EnvironmentCompletedHandler environment_created_handler);

        [Guid("A8346945-51C2-4CE6-8B4C-6F3C4391828B"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2CreateWebView2EnvironmentCompletedHandler
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            void Invoke([MarshalAs(UnmanagedType.Error)] int result, [MarshalAs(UnmanagedType.Interface)] IWebView2Environment webViewEnvironment);
        }

        [Guid("33D17ECE-82FA-47D9-8978-CD17FF3C3CC6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2Environment
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            void CreateWebView(IntPtr parentWindow, [MarshalAs(UnmanagedType.Interface)] IWebView2CreateWebViewCompletedHandler handler);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void CreateWebResourceResponse([MarshalAs(UnmanagedType.Interface)] IStream content, int statusCode, [MarshalAs(UnmanagedType.LPWStr)] string reasonPhrase, [MarshalAs(UnmanagedType.LPWStr)] string headers, [MarshalAs(UnmanagedType.Interface)] ref IWebView2WebResourceResponse response);
        }

        [Guid("E0618CDD-4947-4F58-802C-FC1F20BD4274"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2CreateWebViewCompletedHandler
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            void Invoke([MarshalAs(UnmanagedType.Error)] int result, [MarshalAs(UnmanagedType.Interface)] IWebView2WebView webview);
        }

        [Guid("76711B9E-8D56-4806-8485-35250BB2384F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2WebView
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            [return: MarshalAs(UnmanagedType.Interface)]
            IWebView2Settings get_Settings();
            [MethodImpl(MethodImplOptions.InternalCall)]
            [return: MarshalAs(UnmanagedType.LPWStr)]
            string get_Source();
            [MethodImpl(MethodImplOptions.InternalCall)]
            void Navigate([MarshalAs(UnmanagedType.LPWStr)] [In] string uri);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void MoveFocus([In] WEBVIEW2_MOVE_FOCUS_REASON reason);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void NavigateToString([MarshalAs(UnmanagedType.LPWStr)] [In] string htmlContent);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void add_NavigationStarting([MarshalAs(UnmanagedType.Interface)] [In] IWebView2NavigationStartingEventHandler eventHandler, out EventRegistrationToken token);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void remove_NavigationStarting([In] EventRegistrationToken token);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void add_DocumentStateChanged([MarshalAs(UnmanagedType.Interface)] [In] IWebView2DocumentStateChangedEventHandler eventHandler, out EventRegistrationToken token);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void remove_DocumentStateChanged([In] EventRegistrationToken token);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void add_NavigationCompleted([MarshalAs(UnmanagedType.Interface)] [In] IWebView2NavigationCompletedEventHandler eventHandler, out EventRegistrationToken token);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void remove_NavigationCompleted([In] EventRegistrationToken token);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void add_FrameNavigationStarting([MarshalAs(UnmanagedType.Interface)] [In] IWebView2NavigationStartingEventHandler eventHandler, out EventRegistrationToken token);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void remove_FrameNavigationStarting([In] EventRegistrationToken token);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void add_MoveFocusRequested([MarshalAs(UnmanagedType.Interface)] [In] IWebView2MoveFocusRequestedEventHandler eventHandler, out EventRegistrationToken token);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void remove_MoveFocusRequested([In] EventRegistrationToken token);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void add_GotFocus([MarshalAs(UnmanagedType.Interface)] [In] IWebView2FocusChangedEventHandler eventHandler, out EventRegistrationToken token);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void remove_GotFocus([In] EventRegistrationToken token);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void add_LostFocus([MarshalAs(UnmanagedType.Interface)] [In] IWebView2FocusChangedEventHandler eventHandler, out EventRegistrationToken token);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void remove_LostFocus([In] EventRegistrationToken token);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void add_WebResourceRequested([MarshalAs(UnmanagedType.LPWStr)] [In] ref string urlFilter, [In] ref WEBVIEW2_WEB_RESOURCE_CONTEXT resourceContextFilter, [ComAliasName("MyLibrary.ULONG_PTR")] [In] uint filterLength, [MarshalAs(UnmanagedType.Interface)] [In] IWebView2WebResourceRequestedEventHandler eventHandler, out EventRegistrationToken token);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void remove_WebResourceRequested([In] EventRegistrationToken token);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void add_ScriptDialogOpening([MarshalAs(UnmanagedType.Interface)] [In] IWebView2ScriptDialogOpeningEventHandler eventHandler, out EventRegistrationToken token);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void remove_ScriptDialogOpening([In] EventRegistrationToken token);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void add_ZoomFactorChanged([MarshalAs(UnmanagedType.Interface)] [In] IWebView2ZoomFactorChangedEventHandler eventHandler, out EventRegistrationToken token);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void remove_ZoomFactorChanged([In] EventRegistrationToken token);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void add_PermissionRequested([MarshalAs(UnmanagedType.Interface)] [In] IWebView2PermissionRequestedEventHandler eventHandler, out EventRegistrationToken token);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void remove_PermissionRequested([In] EventRegistrationToken token);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void add_ProcessFailed([MarshalAs(UnmanagedType.Interface)] [In] IWebView2ProcessFailedEventHandler eventHandler, out EventRegistrationToken token);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void remove_ProcessFailed([In] EventRegistrationToken token);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void AddScriptToExecuteOnDocumentCreated([MarshalAs(UnmanagedType.LPWStr)] [In] string javaScript, [MarshalAs(UnmanagedType.Interface)] [In] IWebView2AddScriptToExecuteOnDocumentCreatedCompletedHandler handler);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void RemoveScriptToExecuteOnDocumentCreated([MarshalAs(UnmanagedType.LPWStr)] [In] string id);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void ExecuteScript([MarshalAs(UnmanagedType.LPWStr)] [In] string javaScript, [MarshalAs(UnmanagedType.Interface)] [In] IWebView2ExecuteScriptCompletedHandler handler);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void CapturePreview([In] WEBVIEW2_CAPTURE_PREVIEW_IMAGE_FORMAT imageFormat, [MarshalAs(UnmanagedType.Interface)] [In] IStream imageStream, [MarshalAs(UnmanagedType.Interface)] [In] IWebView2CapturePreviewCompletedHandler handler);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void Reload();
            [MethodImpl(MethodImplOptions.InternalCall)]
            RECT get_Bounds();
            [MethodImpl(MethodImplOptions.InternalCall)]
            void put_Bounds([In] RECT bounds);
            [MethodImpl(MethodImplOptions.InternalCall)]
            double get_ZoomFactor();
            [MethodImpl(MethodImplOptions.InternalCall)]
            void put_ZoomFactor([In] double zoomFactor);
            [MethodImpl(MethodImplOptions.InternalCall)]
            int get_IsVisible();
            [MethodImpl(MethodImplOptions.InternalCall)]
            void put_IsVisible([In] int isVisible);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void PostWebMessageAsJson([MarshalAs(UnmanagedType.LPWStr)] [In] string webMessageAsJson);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void PostWebMessageAsString([MarshalAs(UnmanagedType.LPWStr)] [In] string webMessageAsString);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void add_WebMessageReceived([MarshalAs(UnmanagedType.Interface)] [In] IWebView2WebMessageReceivedEventHandler handler, out EventRegistrationToken token);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void remove_WebMessageReceived([In] EventRegistrationToken token);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void Close();
            [MethodImpl(MethodImplOptions.InternalCall)]
            void CallDevToolsProtocolMethod([MarshalAs(UnmanagedType.LPWStr)] [In] string methodName, [MarshalAs(UnmanagedType.LPWStr)] [In] string parametersAsJson, [MarshalAs(UnmanagedType.Interface)] [In] IWebView2CallDevToolsProtocolMethodCompletedHandler handler);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void add_DevToolsProtocolEventReceived([MarshalAs(UnmanagedType.LPWStr)] [In] string eventName, [MarshalAs(UnmanagedType.Interface)] [In] IWebView2DevToolsProtocolEventReceivedEventHandler handler, out EventRegistrationToken token);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void remove_DevToolsProtocolEventReceived([MarshalAs(UnmanagedType.LPWStr)] [In] string eventName, [In] EventRegistrationToken token);
            [MethodImpl(MethodImplOptions.InternalCall)]
            uint get_BrowserProcessId();
            [MethodImpl(MethodImplOptions.InternalCall)]
            int get_CanGoBack();
            [MethodImpl(MethodImplOptions.InternalCall)]
            int get_CanGoForward();
            [MethodImpl(MethodImplOptions.InternalCall)]
            void GoBack();
            [MethodImpl(MethodImplOptions.InternalCall)]
            void GoForward();
        }

        [Guid("A28CD108-3234-4B45-B390-7E871B504A96"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2Settings
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            int get_IsScriptEnabled();
            [MethodImpl(MethodImplOptions.InternalCall)]
            void put_IsScriptEnabled([In] int isScriptEnabled);
            [MethodImpl(MethodImplOptions.InternalCall)]
            int get_IsWebMessageEnabled();
            [MethodImpl(MethodImplOptions.InternalCall)]
            void put_IsWebMessageEnabled([In] int isWebMessageEnabled);
            [MethodImpl(MethodImplOptions.InternalCall)]
            int get_AreDefaultScriptDialogsEnabled();
            [MethodImpl(MethodImplOptions.InternalCall)]
            void put_AreDefaultScriptDialogsEnabled([In] int areDefaultScriptDialogsEnabled);
            [MethodImpl(MethodImplOptions.InternalCall)]
            int get_IsFullscreenAllowed();
            [MethodImpl(MethodImplOptions.InternalCall)]
            void put_IsFullscreenAllowed([In] int isFullscreenAllowed);
            [MethodImpl(MethodImplOptions.InternalCall)]
            int get_IsStatusBarEnabled();
            [MethodImpl(MethodImplOptions.InternalCall)]
            void put_IsStatusBarEnabled([In] int isStatusBarEnabled);
            [MethodImpl(MethodImplOptions.InternalCall)]
            int get_AreDevToolsEnabled();
            [MethodImpl(MethodImplOptions.InternalCall)]
            void put_AreDevToolsEnabled([In] int areDevToolsEnabled);
        }
        #endregion

        #region Enums
        public enum WEBVIEW2_CAPTURE_PREVIEW_IMAGE_FORMAT
        {
            WEBVIEW2_CAPTURE_PREVIEW_IMAGE_FORMAT_PNG = 0,
            WEBVIEW2_CAPTURE_PREVIEW_IMAGE_FORMAT_JPEG = (WEBVIEW2_CAPTURE_PREVIEW_IMAGE_FORMAT_PNG + 1)
        }

        public enum WEBVIEW2_MOVE_FOCUS_REASON
        {
            WEBVIEW2_MOVE_FOCUS_REASON_PROGRAMMATIC = 0,
            WEBVIEW2_MOVE_FOCUS_REASON_NEXT = (WEBVIEW2_MOVE_FOCUS_REASON_PROGRAMMATIC + 1),
            WEBVIEW2_MOVE_FOCUS_REASON_PREVIOUS = (WEBVIEW2_MOVE_FOCUS_REASON_NEXT + 1)
        }

        public enum WEBVIEW2_PERMISSION_STATE
        {
            WEBVIEW2_PERMISSION_STATE_DEFAULT,
            WEBVIEW2_PERMISSION_STATE_ALLOW,
            WEBVIEW2_PERMISSION_STATE_DENY
        }

        public enum WEBVIEW2_PERMISSION_TYPE
        {
            WEBVIEW2_PERMISSION_TYPE_UNKNOWN_PERMISSION,
            WEBVIEW2_PERMISSION_TYPE_MICROPHONE,
            WEBVIEW2_PERMISSION_TYPE_CAMERA,
            WEBVIEW2_PERMISSION_TYPE_GEOLOCATION,
            WEBVIEW2_PERMISSION_TYPE_NOTIFICATIONS,
            WEBVIEW2_PERMISSION_TYPE_OTHER_SENSORS,
            WEBVIEW2_PERMISSION_TYPE_CLIPBOARD_READ
        }

        public enum WEBVIEW2_PROCESS_FAILED_KIND
        {
            WEBVIEW2_PROCESS_FAILED_KIND_BROWSER_PROCESS_EXITED,
            WEBVIEW2_PROCESS_FAILED_KIND_RENDER_PROCESS_EXITED,
            WEBVIEW2_PROCESS_FAILED_KIND_RENDER_PROCESS_UNRESPONSIVE
        }

        public enum WEBVIEW2_RELEASE_CHANNEL_PREFERENCE
        {
            WEBVIEW2_RELEASE_CHANNEL_PREFERENCE_STABLE = 0,
            WEBVIEW2_RELEASE_CHANNEL_PREFERENCE_CANARY = (WEBVIEW2_RELEASE_CHANNEL_PREFERENCE_STABLE + 1)
        }

        public enum WEBVIEW2_SCRIPT_DIALOG_KIND
        {
            WEBVIEW2_SCRIPT_DIALOG_KIND_ALERT,
            WEBVIEW2_SCRIPT_DIALOG_KIND_CONFIRM,
            WEBVIEW2_SCRIPT_DIALOG_KIND_PROMPT
        }

        public enum WEBVIEW2_WEB_ERROR_STATUS
        {
            WEBVIEW2_WEB_ERROR_STATUS_UNKNOWN,
            WEBVIEW2_WEB_ERROR_STATUS_CERTIFICATE_COMMON_NAME_IS_INCORRECT,
            WEBVIEW2_WEB_ERROR_STATUS_CERTIFICATE_EXPIRED,
            WEBVIEW2_WEB_ERROR_STATUS_CLIENT_CERTIFICATE_CONTAINS_ERRORS,
            WEBVIEW2_WEB_ERROR_STATUS_CERTIFICATE_REVOKED,
            WEBVIEW2_WEB_ERROR_STATUS_CERTIFICATE_IS_INVALID,
            WEBVIEW2_WEB_ERROR_STATUS_SERVER_UNREACHABLE,
            WEBVIEW2_WEB_ERROR_STATUS_TIMEOUT,
            WEBVIEW2_WEB_ERROR_STATUS_ERROR_HTTP_INVALID_SERVER_RESPONSE,
            WEBVIEW2_WEB_ERROR_STATUS_CONNECTION_ABORTED,
            WEBVIEW2_WEB_ERROR_STATUS_CONNECTION_RESET,
            WEBVIEW2_WEB_ERROR_STATUS_DISCONNECTED,
            WEBVIEW2_WEB_ERROR_STATUS_CANNOT_CONNECT,
            WEBVIEW2_WEB_ERROR_STATUS_HOST_NAME_NOT_RESOLVED,
            WEBVIEW2_WEB_ERROR_STATUS_OPERATION_CANCELED,
            WEBVIEW2_WEB_ERROR_STATUS_REDIRECT_FAILED,
            WEBVIEW2_WEB_ERROR_STATUS_UNEXPECTED_ERROR
        }

        public enum WEBVIEW2_WEB_RESOURCE_CONTEXT
        {
            WEBVIEW2_WEB_RESOURCE_CONTEXT_ALL = 0,
            WEBVIEW2_WEB_RESOURCE_CONTEXT_DOCUMENT = (WEBVIEW2_WEB_RESOURCE_CONTEXT_ALL + 1),
            WEBVIEW2_WEB_RESOURCE_CONTEXT_STYLESHEET = (WEBVIEW2_WEB_RESOURCE_CONTEXT_DOCUMENT + 1),
            WEBVIEW2_WEB_RESOURCE_CONTEXT_IMAGE = (WEBVIEW2_WEB_RESOURCE_CONTEXT_STYLESHEET + 1),
            WEBVIEW2_WEB_RESOURCE_CONTEXT_MEDIA = (WEBVIEW2_WEB_RESOURCE_CONTEXT_IMAGE + 1),
            WEBVIEW2_WEB_RESOURCE_CONTEXT_FONT = (WEBVIEW2_WEB_RESOURCE_CONTEXT_MEDIA + 1),
            WEBVIEW2_WEB_RESOURCE_CONTEXT_SCRIPT = (WEBVIEW2_WEB_RESOURCE_CONTEXT_FONT + 1),
            WEBVIEW2_WEB_RESOURCE_CONTEXT_XML_HTTP_REQUEST = (WEBVIEW2_WEB_RESOURCE_CONTEXT_SCRIPT + 1),
            WEBVIEW2_WEB_RESOURCE_CONTEXT_FETCH = (WEBVIEW2_WEB_RESOURCE_CONTEXT_XML_HTTP_REQUEST + 1)
        }
        #endregion

        #region Event Args Interfaces
        [Guid("BF0F875F-8EB0-4211-9B80-2892F7276BB9"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2DevToolsProtocolEventReceivedEventArgs
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            [return: MarshalAs(UnmanagedType.LPWStr)]
            string get_ParameterObjectAsJson();
        }

        [Guid("3A38CB7F-EFC1-41B4-87FC-5AFCEE27C8ED"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2DocumentStateChangedEventArgs
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            int get_IsNewDocument();
            [MethodImpl(MethodImplOptions.InternalCall)]
            int get_IsErrorPage();
        }

        [Guid("64AF5AE3-27A1-47E0-8901-95119C1BA95B"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2MoveFocusRequestedEventArgs
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            WEBVIEW2_MOVE_FOCUS_REASON get_Reason();
            [MethodImpl(MethodImplOptions.InternalCall)]
            int get_Handled();
            [MethodImpl(MethodImplOptions.InternalCall)]
            void put_Handled([In] int value);
        }

        [Guid("48655B1F-3F52-4835-B7AA-7D95F7D7587E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2NavigationCompletedEventArgs
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            int get_IsSuccess();
            [MethodImpl(MethodImplOptions.InternalCall)]
            WEBVIEW2_WEB_ERROR_STATUS get_WebErrorStatus();
        }

        [Guid("9D7A1F73-8211-48C0-9119-686D1FB1AE02"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2NavigationStartingEventArgs
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            [return: MarshalAs(UnmanagedType.LPWStr)]
            string get_Uri();
            [MethodImpl(MethodImplOptions.InternalCall)]
            int get_IsUserInitiated();
            [MethodImpl(MethodImplOptions.InternalCall)]
            int get_IsRedirected();
            [MethodImpl(MethodImplOptions.InternalCall)]
            [return: MarshalAs(UnmanagedType.Interface)]
            IWebView2HttpRequestHeaders get_RequestHeaders();
            [MethodImpl(MethodImplOptions.InternalCall)]
            int get_Cancel();
            [MethodImpl(MethodImplOptions.InternalCall)]
            void put_Cancel([In] int cancel);
        }

        [Guid("8D8DA0E4-A071-486F-85AA-31B4B2BADC61"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2PermissionRequestedEventArgs
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            [return: MarshalAs(UnmanagedType.LPWStr)]
            string get_Uri();
            [MethodImpl(MethodImplOptions.InternalCall)]
            WEBVIEW2_PERMISSION_TYPE get_PermissionType();
            [MethodImpl(MethodImplOptions.InternalCall)]
            int get_IsUserInitiated();
            [MethodImpl(MethodImplOptions.InternalCall)]
            WEBVIEW2_PERMISSION_STATE get_State();
            [MethodImpl(MethodImplOptions.InternalCall)]
            void put_State([In] WEBVIEW2_PERMISSION_STATE value);
            [MethodImpl(MethodImplOptions.InternalCall)]
            [return: MarshalAs(UnmanagedType.Interface)]
            IWebView2Deferral GetDeferral();
        }

        [Guid("6DABCFB8-8C7D-4515-893B-9766766900DA"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2ProcessFailedEventArgs
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            WEBVIEW2_PROCESS_FAILED_KIND get_ProcessFailedKind();
        }

        [Guid("ABB0484E-8D4F-4BEA-9058-B0287221A976"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2ScriptDialogOpeningEventArgs
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            [return: MarshalAs(UnmanagedType.LPWStr)]
            string get_Uri();
            [MethodImpl(MethodImplOptions.InternalCall)]
            WEBVIEW2_SCRIPT_DIALOG_KIND get_Kind();
            [MethodImpl(MethodImplOptions.InternalCall)]
            [return: MarshalAs(UnmanagedType.LPWStr)]
            string get_Message();
            [MethodImpl(MethodImplOptions.InternalCall)]
            void Accept();
            [MethodImpl(MethodImplOptions.InternalCall)]
            [return: MarshalAs(UnmanagedType.LPWStr)]
            string get_DefaultText();
            [MethodImpl(MethodImplOptions.InternalCall)]
            [return: MarshalAs(UnmanagedType.LPWStr)]
            string get_ResultText();
            [MethodImpl(MethodImplOptions.InternalCall)]
            void put_ResultText([MarshalAs(UnmanagedType.LPWStr)] [In] string resultText);
            [MethodImpl(MethodImplOptions.InternalCall)]
            [return: MarshalAs(UnmanagedType.Interface)]
            IWebView2Deferral GetDeferral();
        }

        [Guid("E32C6167-14F1-42EA-8743-B014EF6AD27F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2WebMessageReceivedEventArgs
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            [return: MarshalAs(UnmanagedType.LPWStr)]
            string get_Source();
            [MethodImpl(MethodImplOptions.InternalCall)]
            [return: MarshalAs(UnmanagedType.LPWStr)]
            string get_WebMessageAsJson();
            [MethodImpl(MethodImplOptions.InternalCall)]
            [return: MarshalAs(UnmanagedType.LPWStr)]
            string get_WebMessageAsString();
        }

        [Guid("D8B1DD71-B9AD-4EEB-ABE3-87E7EFC5D37F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2WebResourceRequestedEventArgs
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            [return: MarshalAs(UnmanagedType.Interface)]
            IWebView2WebResourceRequest get_Request();
            [MethodImpl(MethodImplOptions.InternalCall)]
            [return: MarshalAs(UnmanagedType.Interface)]
            IWebView2WebResourceResponse get_Response();
            [MethodImpl(MethodImplOptions.InternalCall)]
            void put_Response([MarshalAs(UnmanagedType.Interface)] [In] IWebView2WebResourceResponse response);
            [MethodImpl(MethodImplOptions.InternalCall)]
            [return: MarshalAs(UnmanagedType.Interface)]
            IWebView2Deferral GetDeferral();
        }
        #endregion

        #region Event Handler Interfaces
        [Guid("EE07AA7F-5DAF-4C00-9C0B-5F736213C92D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2AddScriptToExecuteOnDocumentCreatedCompletedHandler
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            void Invoke([MarshalAs(UnmanagedType.Error)] [In] int errorCode, [MarshalAs(UnmanagedType.LPWStr)] [In] string id);
        }

        [Guid("6EA28F62-FEC5-48EA-9669-67979B50579E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2CallDevToolsProtocolMethodCompletedHandler
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            void Invoke([MarshalAs(UnmanagedType.Error)] [In] int errorCode, [MarshalAs(UnmanagedType.LPWStr)] [In] string returnObjectAsJson);
        }

        [Guid("5755B27A-3FCD-4E01-B368-06834A5AFCDC"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2CapturePreviewCompletedHandler
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            void Invoke([MarshalAs(UnmanagedType.Error)] [In] int result);
        }

        [Guid("37D087EA-12F6-4856-81D8-5596C708CA59"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2DevToolsProtocolEventReceivedEventHandler
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            void Invoke([MarshalAs(UnmanagedType.Interface)] [In] IWebView2WebView webview, [MarshalAs(UnmanagedType.Interface)] [In] IWebView2DevToolsProtocolEventReceivedEventArgs args);
        }

        [Guid("88E66305-3A5A-4E7F-9C76-2EBFC138CAFD"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2DocumentStateChangedEventHandler
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            void Invoke([MarshalAs(UnmanagedType.Interface)] [In] IWebView2WebView webview, [MarshalAs(UnmanagedType.Interface)] [In] IWebView2DocumentStateChangedEventArgs args);
        }

        [Guid("F5AC0E3B-8B92-45E5-ABEF-DB8518EFFF27"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2ExecuteScriptCompletedHandler
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            void Invoke([MarshalAs(UnmanagedType.Error)] [In] int errorCode, [MarshalAs(UnmanagedType.LPWStr)] [In] string resultObjectAsJson);
        }

        [Guid("76BDBECE-02CC-4E56-AD81-5F808E8572A6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2FocusChangedEventHandler
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            void Invoke([MarshalAs(UnmanagedType.Interface)] [In] IWebView2WebView webview, [MarshalAs(UnmanagedType.IUnknown)] [In] object args);
        }

        [Guid("F3A49DD0-EA49-469C-8B7A-8CC5E8E4EF27"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2MoveFocusRequestedEventHandler
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            void Invoke([MarshalAs(UnmanagedType.Interface)] [In] IWebView2WebView webview, [MarshalAs(UnmanagedType.Interface)] [In] IWebView2MoveFocusRequestedEventArgs args);
        }

        [Guid("DCEB3A27-C8C0-4DE7-889D-AF3DE80EDB3C"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2NavigationCompletedEventHandler
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            void Invoke([MarshalAs(UnmanagedType.Interface)] [In] IWebView2WebView webview, [MarshalAs(UnmanagedType.Interface)] [In] IWebView2NavigationCompletedEventArgs args);
        }

        [Guid("34896570-DC04-40F9-A2DA-8582551A707D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2NavigationStartingEventHandler
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            void Invoke([MarshalAs(UnmanagedType.Interface)] [In] IWebView2WebView webview, [MarshalAs(UnmanagedType.Interface)] [In] IWebView2NavigationStartingEventArgs args);
        }

        [Guid("C5DA3C20-95AC-4345-B3C9-5FCA3B92C9DB"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2PermissionRequestedEventHandler
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            void Invoke([MarshalAs(UnmanagedType.Interface)] [In] IWebView2WebView webview, [MarshalAs(UnmanagedType.Interface)] [In] IWebView2PermissionRequestedEventArgs args);
        }

        [Guid("011EC830-5DAF-4767-A099-C43DE1A925F4"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2ProcessFailedEventHandler
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            void Invoke([MarshalAs(UnmanagedType.Interface)] [In] IWebView2WebView webview, [MarshalAs(UnmanagedType.Interface)] [In] IWebView2ProcessFailedEventArgs args);
        }

        [Guid("8EAF9A50-2AF9-45DA-9AC5-F80F4147180E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2ScriptDialogOpeningEventHandler
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            void Invoke([MarshalAs(UnmanagedType.Interface)] [In] IWebView2WebView webview, [MarshalAs(UnmanagedType.Interface)] [In] IWebView2ScriptDialogOpeningEventArgs args);
        }

        [Guid("0E682B9A-B686-4327-9A56-E0305705A3DB"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2WebMessageReceivedEventHandler
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            void Invoke([MarshalAs(UnmanagedType.Interface)] [In] IWebView2WebView webview, [MarshalAs(UnmanagedType.Interface)] [In] IWebView2WebMessageReceivedEventArgs args);
        }

        [Guid("E2AE08C1-4F67-4348-AE05-C89CB14C2ADD"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2WebResourceRequestedEventHandler
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            void Invoke([MarshalAs(UnmanagedType.Interface)] [In] IWebView2WebView webview, [MarshalAs(UnmanagedType.Interface)] [In] IWebView2WebResourceRequestedEventArgs args);
        }

        [Guid("A5C0B08B-25D7-4BAC-AD06-11783393088E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2ZoomFactorChangedEventHandler
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            void Invoke([MarshalAs(UnmanagedType.Interface)] [In] IWebView2WebView webview, [MarshalAs(UnmanagedType.IUnknown)] [In] object args);
        }
        #endregion

        #region Other Types
        [StructLayout(LayoutKind.Sequential)]
        public class WINDOWPLACEMENT
        {
            public int length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
            public int flags;
            public SW showCmd;
            public POINT ptMinPosition;
            public POINT ptMaxPosition;
            public RECT rcNormalPosition;
        }

        /// <summary>
        /// SetWindowPos options
        /// </summary>
        [Flags]
        public enum SWP
        {
            ASYNCWINDOWPOS = 0x4000,
            DEFERERASE = 0x2000,
            DRAWFRAME = 0x0020,
            FRAMECHANGED = 0x0020,
            HIDEWINDOW = 0x0080,
            NOACTIVATE = 0x0010,
            NOCOPYBITS = 0x0100,
            NOMOVE = 0x0002,
            NOOWNERZORDER = 0x0200,
            NOREDRAW = 0x0008,
            NOREPOSITION = 0x0200,
            NOSENDCHANGING = 0x0400,
            NOSIZE = 0x0001,
            NOZORDER = 0x0004,
            SHOWWINDOW = 0x0040,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }

        /// <summary>
        /// ShowWindow options
        /// </summary>
        public enum SW
        {
            HIDE = 0,
            SHOWNORMAL = 1,
            NORMAL = 1,
            SHOWMINIMIZED = 2,
            SHOWMAXIMIZED = 3,
            MAXIMIZE = 3,
            SHOWNOACTIVATE = 4,
            SHOW = 5,
            MINIMIZE = 6,
            SHOWMINNOACTIVE = 7,
            SHOWNA = 8,
            RESTORE = 9,
            SHOWDEFAULT = 10,
            FORCEMINIMIZE = 11,
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowPlacement(IntPtr hwnd, WINDOWPLACEMENT lpwndpl);

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct EventRegistrationToken
        {
            public long value;
        }

        [Guid("0C733A30-2A1C-11CE-ADE5-00AA0044773D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface ISequentialStream
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            void RemoteRead(out byte pv, [In] uint cb, out uint pcbRead);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void RemoteWrite([In] ref byte pv, [In] uint cb, out uint pcbWritten);
        }

        [Guid("0000000C-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IStream : ISequentialStream
        {
            //[MethodImpl(MethodImplOptions.InternalCall)]
            //void RemoteRead(out byte pv, [In] uint cb, out uint pcbRead);
            //[MethodImpl(MethodImplOptions.InternalCall)]
            //void RemoteWrite([In] ref byte pv, [In] uint cb, out uint pcbWritten);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void RemoteSeek([In] _LARGE_INTEGER dlibMove, [In] uint dwOrigin, out _ULARGE_INTEGER plibNewPosition);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void SetSize([In] _ULARGE_INTEGER libNewSize);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void RemoteCopyTo([MarshalAs(UnmanagedType.Interface)] [In] IStream pstm, [In] _ULARGE_INTEGER cb, out _ULARGE_INTEGER pcbRead, out _ULARGE_INTEGER pcbWritten);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void Commit([In] uint grfCommitFlags);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void Revert();
            [MethodImpl(MethodImplOptions.InternalCall)]
            void LockRegion([In] _ULARGE_INTEGER libOffset, [In] _ULARGE_INTEGER cb, [In] uint dwLockType);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void UnlockRegion([In] _ULARGE_INTEGER libOffset, [In] _ULARGE_INTEGER cb, [In] uint dwLockType);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void Stat(out tagSTATSTG pstatstg, [In] uint grfStatFlag);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void Clone([MarshalAs(UnmanagedType.Interface)] out IStream ppstm);
        }

        [Guid("BD478C19-4706-4B1D-88B6-76DD39ACB7B1"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2Deferral
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            void Complete();
        }

        [Guid("66A215E4-CA41-490B-884A-411FFB17CD1C"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2HttpHeadersCollectionIterator
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            void GetCurrentHeader([MarshalAs(UnmanagedType.LPWStr)] out string name, [MarshalAs(UnmanagedType.LPWStr)] out string value);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void MoveNext(out int has_next);
        }

        [Guid("982BE490-0252-44F3-9F33-376C04885A6D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2HttpRequestHeaders
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            void GetHeader([MarshalAs(UnmanagedType.LPWStr)] [In] string name, [MarshalAs(UnmanagedType.LPWStr)] out string value);
            [MethodImpl(MethodImplOptions.InternalCall)]
            int Contains([MarshalAs(UnmanagedType.LPWStr)] [In] string name);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void SetHeader([MarshalAs(UnmanagedType.LPWStr)] [In] string name, [MarshalAs(UnmanagedType.LPWStr)] [In] string value);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void RemoveHeader([MarshalAs(UnmanagedType.LPWStr)] [In] string name);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void GetIterator([MarshalAs(UnmanagedType.Interface)] out IWebView2HttpHeadersCollectionIterator iterator);
        }

        [Guid("6D1A13A6-C677-41AA-852F-827B53F35301"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2HttpResponseHeaders
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            void AppendHeader([MarshalAs(UnmanagedType.LPWStr)] [In] string name, [MarshalAs(UnmanagedType.LPWStr)] [In] string value);
            [MethodImpl(MethodImplOptions.InternalCall)]
            int Contains([MarshalAs(UnmanagedType.LPWStr)] [In] string name);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void GetHeaders([MarshalAs(UnmanagedType.LPWStr)] [In] string name, [MarshalAs(UnmanagedType.Interface)] out IWebView2HttpHeadersCollectionIterator iterator);
            [MethodImpl(MethodImplOptions.InternalCall)]
            void GetIterator([MarshalAs(UnmanagedType.Interface)] out IWebView2HttpHeadersCollectionIterator iterator);
        }

        [Guid("1B3F4122-34A0-4F5D-9089-AF63C3AFE375"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2WebResourceRequest
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            [return: MarshalAs(UnmanagedType.LPWStr)]
            string get_Uri();
            [MethodImpl(MethodImplOptions.InternalCall)]
            void put_Uri([MarshalAs(UnmanagedType.LPWStr)] [In] string uri);
            [MethodImpl(MethodImplOptions.InternalCall)]
            [return: MarshalAs(UnmanagedType.LPWStr)]
            string get_Method();
            [MethodImpl(MethodImplOptions.InternalCall)]
            void put_Method([MarshalAs(UnmanagedType.LPWStr)] [In] string method);
            [MethodImpl(MethodImplOptions.InternalCall)]
            [return: MarshalAs(UnmanagedType.Interface)]
            IStream get_Content();
            [MethodImpl(MethodImplOptions.InternalCall)]
            void put_Content([MarshalAs(UnmanagedType.Interface)] [In] IStream content);
            [MethodImpl(MethodImplOptions.InternalCall)]
            [return: MarshalAs(UnmanagedType.Interface)]
            IWebView2HttpRequestHeaders get_Headers();
        }

        [Guid("297886A6-5FDF-472D-A97A-E336ECFE1352"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface IWebView2WebResourceResponse
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            [return: MarshalAs(UnmanagedType.Interface)]
            IStream get_Content();
            [MethodImpl(MethodImplOptions.InternalCall)]
            void put_Content([MarshalAs(UnmanagedType.Interface)] [In] IStream content);
            [MethodImpl(MethodImplOptions.InternalCall)]
            [return: MarshalAs(UnmanagedType.Interface)]
            IWebView2HttpResponseHeaders get_Headers();
            [MethodImpl(MethodImplOptions.InternalCall)]
            int get_StatusCode();
            [MethodImpl(MethodImplOptions.InternalCall)]
            void put_StatusCode([In] int statusCode);
            [MethodImpl(MethodImplOptions.InternalCall)]
            [return: MarshalAs(UnmanagedType.LPWStr)]
            string get_ReasonPhrase();
            [MethodImpl(MethodImplOptions.InternalCall)]
            void put_ReasonPhrase([MarshalAs(UnmanagedType.LPWStr)] [In] string reasonPhrase);
        }
        #endregion
        #endregion

        #region Win32 APIs
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left, Top, Right, Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public RECT(System.Drawing.Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }

            public int X
            {
                get { return Left; }
                set { Right -= (Left - value); Left = value; }
            }

            public int Y
            {
                get { return Top; }
                set { Bottom -= (Top - value); Top = value; }
            }

            public int Height
            {
                get { return Bottom - Top; }
                set { Bottom = value + Top; }
            }

            public int Width
            {
                get { return Right - Left; }
                set { Right = value + Left; }
            }

            public System.Drawing.Point Location
            {
                get { return new System.Drawing.Point(Left, Top); }
                set { X = value.X; Y = value.Y; }
            }

            public System.Drawing.Size Size
            {
                get { return new System.Drawing.Size(Width, Height); }
                set { Width = value.Width; Height = value.Height; }
            }

            public static implicit operator System.Drawing.Rectangle(RECT r)
            {
                return new System.Drawing.Rectangle(r.Left, r.Top, r.Width, r.Height);
            }

            public static implicit operator RECT(System.Drawing.Rectangle r)
            {
                return new RECT(r);
            }

            public static bool operator ==(RECT r1, RECT r2)
            {
                return r1.Equals(r2);
            }

            public static bool operator !=(RECT r1, RECT r2)
            {
                return !r1.Equals(r2);
            }

            public bool Equals(RECT r)
            {
                return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
            }

            public override bool Equals(object obj)
            {
                if (obj is RECT)
                    return Equals((RECT)obj);
                else if (obj is System.Drawing.Rectangle)
                    return Equals(new RECT((System.Drawing.Rectangle)obj));
                return false;
            }

            public override int GetHashCode()
            {
                return ((System.Drawing.Rectangle)this).GetHashCode();
            }

            public override string ToString()
            {
                return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
            }
        }

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        public enum WindowStyles : uint
        {
            WS_BORDER = 0x800000,
            WS_CAPTION = 0xc00000,
            WS_CHILD = 0x40000000,
            WS_CLIPCHILDREN = 0x2000000,
            WS_CLIPSIBLINGS = 0x4000000,
            WS_DISABLED = 0x8000000,
            WS_DLGFRAME = 0x400000,
            WS_GROUP = 0x20000,
            WS_HSCROLL = 0x100000,
            WS_MAXIMIZE = 0x1000000,
            WS_MAXIMIZEBOX = 0x10000,
            WS_MINIMIZE = 0x20000000,
            WS_MINIMIZEBOX = 0x20000,
            WS_OVERLAPPED = 0x0,
            WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_SIZEFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
            WS_POPUP = 0x80000000u,
            WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
            WS_SIZEFRAME = 0x40000,
            WS_SYSMENU = 0x80000,
            WS_TABSTOP = 0x10000,
            WS_VISIBLE = 0x10000000,
            WS_VSCROLL = 0x200000
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct _LARGE_INTEGER
        {
            public long QuadPart;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct _ULARGE_INTEGER
        {
            public ulong QuadPart;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct _FILETIME
        {
            public uint dwLowDateTime;
            public uint dwHighDateTime;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct tagSTATSTG
        {
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pwcsName;
            public uint type;
            public _ULARGE_INTEGER cbSize;
            public _FILETIME mtime;
            public _FILETIME ctime;
            public _FILETIME atime;
            public uint grfMode;
            public uint grfLocksSupported;
            public Guid clsid;
            public uint grfStateBits;
            public uint reserved;
        }
        #endregion
    }
}
