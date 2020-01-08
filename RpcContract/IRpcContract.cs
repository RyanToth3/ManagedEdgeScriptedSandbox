using Microsoft.VisualStudio.Threading;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace RpcContract
{
    public interface IRpcContract : IAsyncDisposable
    {
        #region DummyCode
        Task<string> DoSomethingAsync();
        #endregion

        #region ActualInterface

        event EventHandler ModelStateChanged;
        event EventHandler FocusChanged;
        event EventHandler TabOut;

        Task PingHeartBeatAsync();
        Task CreateBrowserAsync(IntPtr handle);
        Task DestroyBrowserAsync(IntPtr handle);
        Task PostUnicodeStringAsync(IntPtr browserHandle, string message);
        Task NavigateToAsync(IntPtr browserHandle, string url);
        Task NavigateToStreamAsync(IntPtr browserHandle, string baseUrl, string contents);
        Task ReplaceBodyContentsAsync(IntPtr browserHandle, string contents);
        Task SetParentAsync(IntPtr browserHandle, IntPtr parent);
        Task SetWindowPostionAsync(IntPtr browserHandle, IntPtr hwndAfter, Rect position, NativeMethods.ShowWindow flags);
        Task SetHostVersionInfoAsync(IntPtr browserHandle, Version version);
        Task SetPluginInformationAsync(IntPtr browserHandle, string info);
        Task<string> GetPluginInformationAsync(IntPtr browserHandle);
        Task<Version> GetHostVersionInfoAsync(IntPtr browserHandle);
        Task SetFileAliasInfoAsync(IntPtr browserHandle, Dictionary<string, FileAlias> fileAliases);
        Task<Dictionary<string, FileAlias>> GetFileAliasInfoAsync(IntPtr browserHandle);
        Task SetPerfAnalyticsInfo(IntPtr browserHandle, string initializationInfo);
        Task<string> GetPerfAnalyticsInfoAsync(IntPtr browserHandle);
        Task SetZoomAsync(IntPtr browserHandle, int zoom);

        // IncomingMessageFromServiceHubService 
        // ServiceHubServicePipeDisconnected 

        #endregion
    }
}
