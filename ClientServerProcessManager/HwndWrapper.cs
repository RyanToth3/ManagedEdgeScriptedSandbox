//-----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//-----------------------------------------------------------------------------

using RpcContract;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace ClientServerProcessManager
{
    /// <summary>
    /// A class that helps create an HWND and simplifies implementing an WndProc for the HWND.
    /// It manages the creation and destruction of the window class and maps window creation
    /// and destruction to IDisposable.
    /// </summary>
    internal abstract class HwndWrapper : IDisposable
    {
        private IntPtr handle;
        private ushort wndClassAtom;
        private GCHandle windowReferenceProxy;

        // Used to keep the delegate alive (stored in the native window class structure)
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "This keeps the delegate alive.")]
        private Delegate wndProc;

        protected virtual void Dispose(bool finalizer)
        {
            this.DisposeNativeResources();
        }

        protected ushort WindowClassAtom
        {
            get
            {
                if (this.wndClassAtom == 0)
                    this.wndClassAtom = this.CreateWindowClassCore();
                return this.wndClassAtom;
            }
        }

        ~HwndWrapper()
        {
            this.Dispose(true);
        }

        protected virtual ushort CreateWindowClassCore()
        {
            return this.RegisterClass(Guid.NewGuid().ToString());
        }

        protected virtual void DestroyWindowClassCore()
        {
            if (this.wndClassAtom != 0)
            {
                IntPtr instance = NativeMethods.GetModuleHandle(null);
                NativeMethods.UnregisterClass(new IntPtr(this.wndClassAtom), instance);
                this.wndClassAtom = 0;
            }
        }

        [SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")]
        private static readonly IntPtr DefaultCursor = NativeMethods.LoadCursor(IntPtr.Zero, new IntPtr(NativeMethods.IDC_ARROW));

        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Windows API")]
        private static readonly IntPtr COLOR_BACKGROUND = new IntPtr(1);

        protected ushort RegisterClass(string className)
        {
            var wndClass = new NativeMethods.WNDCLASS();
            wndClass.cbClsExtra = 0;
            wndClass.cbWndExtra = 0;
            wndClass.hbrBackground = COLOR_BACKGROUND;
            wndClass.hCursor = DefaultCursor;
            wndClass.hIcon = IntPtr.Zero;
            wndClass.lpfnWndProc = this.wndProc = (NativeMethods.WndProc)this.WrapperWndProc;
            wndClass.lpszClassName = className;
            wndClass.lpszMenuName = null;
            wndClass.style = 0;

            // Create a proxy GC refernce to the window procedure delegate to stand in for the reference kept alive by the window itself.
            // We keep this alive because it keeps wndProc alive and we also need it to be alive.
            this.windowReferenceProxy = GCHandle.Alloc(this);

            return NativeMethods.RegisterClass(ref wndClass);
        }

        protected abstract IntPtr CreateWindowCore();

        protected virtual void DestroyWindowCore()
        {
            if (this.handle != IntPtr.Zero)
            {
                NativeMethods.DestroyWindow(this.handle);
                this.handle = IntPtr.Zero;
            }
        }

        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", MessageId = "hWnd", Justification = "Windows API")]
        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", MessageId = "wParam", Justification = "Windows API")]
        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", MessageId = "wParam", Justification = "Windows API")]
        private IntPtr WrapperWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (msg == NativeMethods.WM_NCDESTROY)
            {
                // WM_NCDESTROY is the last message that will be sent to the window. We no longer need the window
                // procedure delegate nor this object to be alive any longer. Release the handle that is a proxy 
                // representing the windows reference. This allows both to now be collected.
                if (this.windowReferenceProxy != null)
                    this.windowReferenceProxy.Free();
            }
            return this.WndProc(hWnd, msg, wParam, lParam);
        }

        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", MessageId = "hWnd", Justification = "Windows API")]
        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", MessageId = "wParam", Justification = "Windows API")]
        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", MessageId = "wParam", Justification = "Windows API")]
        protected virtual IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            return NativeMethods.DefWindowProc(hWnd, msg, wParam, lParam);
        }

        public IntPtr Handle
        {
            get
            {
                this.EnsureHandle();
                return this.handle;
            }
        }

        public void EnsureHandle()
        {
            if (this.handle == IntPtr.Zero)
                this.handle = this.CreateWindowCore();
        }

        protected IntPtr ProtectedHandle
        {
            get { return this.handle; }
        }

        private void DisposeNativeResources()
        {
            this.DestroyWindowCore();
            this.DestroyWindowClassCore();
        }

        public void Dispose()
        {
            this.Dispose(false);
            GC.SuppressFinalize(this);
        }
    }
}
