using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ClientServerProcessManager
{
    internal class BrowserHolder : HwndWrapper
    {
        public BrowserHolder(IntPtr parentHandle)
        {
            this.parentHandle = parentHandle;
        }

        private IntPtr parentHandle;
        public IntPtr ParentHandle
        {
            get
            {
                return this.parentHandle;
            }
            set
            {
                this.parentHandle = value;
                NativeMethods.SetParent(this.Handle, this.parentHandle);
            }
        }

        protected override IntPtr CreateWindowCore()
        {
            RpcContract.NativeMethods.WindowStyles windowStyle =
                RpcContract.NativeMethods.WindowStyles.WS_VISIBLE |
                RpcContract.NativeMethods.WindowStyles.WS_CLIPCHILDREN |
                RpcContract.NativeMethods.WindowStyles.WS_CLIPSIBLINGS |
                RpcContract.NativeMethods.WindowStyles.WS_CHILD;

            IntPtr hwnd = RpcContract.NativeMethods.CreateWindowEx(
                0, // Extended Style
                new IntPtr(this.WindowClassAtom), // Class type
                this.GetType().Name, // Window Name
                windowStyle, // Style
                0, // X
                0, // Y
                1, // Width
                1, // Height
                this.parentHandle, // Parent
                IntPtr.Zero, // Menu
                RpcContract.NativeMethods.GetModuleHandle(null), // hInstance
                IntPtr.Zero); // lpData

            if (hwnd == IntPtr.Zero)
            {
                // TODO: Put string into a resource.
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Window creation failed.");
            }

            return hwnd;
        }
    }
}
