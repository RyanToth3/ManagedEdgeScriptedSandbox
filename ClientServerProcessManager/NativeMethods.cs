//-----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//-----------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace ClientServerProcessManager
{
    internal static class NativeMethods
    {
        [DllImport("user32.dll", SetLastError = false)]
        public static extern IntPtr GetDesktopWindow();

        public const uint WM_SETTEXT = 0x000C;
        public const uint WM_QUIT = 0x0012;
        public const uint WM_SYSCOLORCHANGE = 0x0015;
        public const uint WM_SETTINGCHANGE = 0x001A;
        public const uint WM_WINDOWPOSCHANGED = 0x0047;
        public const uint WM_NCDESTROY = 0x0082;
        public const uint WM_CHAR = 0x0102;
        public const uint WM_DEADCHAR = 0x0103;
        public const uint WM_SYSCHAR = 0x0106;
        public const uint WM_SYSDEADCHAR = 0x0107;
        public const uint WM_TIMER = 0x0113;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_RBUTTONDOWN = 0x0204;
        public const uint WM_FONTDIALOGCLOSED = 0x1054;
        public const uint WM_FONTORCOLORCHANGED = 0x1057;
        public const uint WM_SETFOCUS = 0x0007;

        public const uint IDC_ARROW = 32512;

        public const int MAX_PATH = 260;
        public const int WH_MOUSE_LL = 14;
        public const int HC_ACTION = 0;

        public delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);
        public delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        public struct MouseHookStruct
        {
            public POINT pt;
            public IntPtr hwnd;
            public uint wHitTestCode;
            public UIntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct WNDCLASS
        {
            public uint style;
            public Delegate lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpszMenuName;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpszClassName;
        }

        public const int PM_NOREMOVE = 0x0000;
        public const int PM_REMOVE = 0x0001;

        [StructLayout(LayoutKind.Sequential)]
        public struct MSG
        {
            public IntPtr hwnd;
            public int message;
            public IntPtr wParam;
            public IntPtr lParam;
            public int time;
            public int pt_x;
            public int pt_y;
        }

        /// <summary>
        /// ShowWindow options
        /// </summary>
        internal enum SW
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

        /// <summary>
        /// SetWindowPos options
        /// </summary>
        [Flags]
        internal enum SWP
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

        /// <summary>
        /// WindowStyle values, WS_*
        /// </summary>
        [Flags]
        internal enum WS : uint
        {
            OVERLAPPED = 0x00000000,
            POPUP = 0x80000000,
            CHILD = 0x40000000,
            MINIMIZE = 0x20000000,
            VISIBLE = 0x10000000,
            DISABLED = 0x08000000,
            CLIPSIBLINGS = 0x04000000,
            CLIPCHILDREN = 0x02000000,
            MAXIMIZE = 0x01000000,
            BORDER = 0x00800000,
            DLGFRAME = 0x00400000,
            VSCROLL = 0x00200000,
            HSCROLL = 0x00100000,
            SYSMENU = 0x00080000,
            THICKFRAME = 0x00040000,
            GROUP = 0x00020000,
            TABSTOP = 0x00010000,

            MINIMIZEBOX = 0x00020000,
            MAXIMIZEBOX = 0x00010000,

            CAPTION = BORDER | DLGFRAME,
            TILED = OVERLAPPED,
            ICONIC = MINIMIZE,
            SIZEBOX = THICKFRAME,
            TILEDWINDOW = OVERLAPPEDWINDOW,

            OVERLAPPEDWINDOW = OVERLAPPED | CAPTION | SYSMENU | THICKFRAME | MINIMIZEBOX | MAXIMIZEBOX,
            POPUPWINDOW = POPUP | BORDER | SYSMENU,
            CHILDWINDOW = CHILD,
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class WINDOWPLACEMENT
        {
            public int length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
            public int flags;
            public SW showCmd;
            public POINT ptMinPosition;
            public POINT ptMaxPosition;
            public RECT rcNormalPosition;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public int X;
            public int Y;

            public Point(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MonitorInfo
        {
            public int Size;
            public RECT Monitor;
            public RECT WorkArea;
            public uint Flags;
        }

        public enum MonitorOptions : uint
        {
            MONITOR_DEFAULTTONULL = 0x00000000,
            MONITOR_DEFAULTTOPRIMARY = 0x00000001,
            MONITOR_DEFAULTTONEAREST = 0x00000002
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern ushort RegisterClass(ref WNDCLASS lpWndClass);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateWindowEx(int dwExStyle, IntPtr classAtom, string lpWindowName, WS dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

        [DllImport("user32.dll")]
        internal static extern IntPtr DefWindowProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr GetModuleHandle(string moduleName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnregisterClass(IntPtr classAtom, IntPtr hInstance);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyWindow(IntPtr hwnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PeekMessage(ref MSG msg, IntPtr hwnd, int nMsgFilterMin, int nMsgFilterMax, int wRemoveMsg);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetMessage(ref MSG msg, IntPtr hwnd, int nMsgFilterMin, int nMsgFilterMax);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool TranslateMessage(ref MSG msg);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr DispatchMessage(ref MSG msg);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PostMessage(IntPtr hWnd, uint nMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern void PostQuitMessage(int nExitCode);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SetParent(IntPtr hWnd, IntPtr hWndParent);

        public const int DUPLICATE_CLOSE_SOURCE = 0x00000001;
        public const int DUPLICATE_SAME_ACCESS = 0x00000002;

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DuplicateHandle(IntPtr hSourceProcessHandle, IntPtr hSourceHandle, IntPtr hTargetProcessHandle,
            out IntPtr lpTargetProcessHandle, int dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwOptions);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressMessage("Microsoft.Interoperability", "CA1415", Justification = "Overlapped unused")]
        public static extern bool ReadFile(SafeFileHandle hFile, byte[] buffer, uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, IntPtr lpOverlapped);

        [DllImport("kernel32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressMessage("Microsoft.Interoperability", "CA1415", Justification = "Overlapped unused")]
        public static extern bool WriteFile(SafeFileHandle hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, IntPtr lpOverlapped);

        [DllImport("kernel32.dll")]
        public static extern uint GetTickCount();

        public delegate void TimerProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetTimer(IntPtr hWnd, int nIDEvent, int uElapse, TimerProc lpTimerFunc);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool KillTimer(IntPtr hWnd, int nIDEvent);

        [DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, [MarshalAs(UnmanagedType.Bool)] bool attach);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CheckRemoteDebuggerPresent(IntPtr processHandle, [MarshalAs(UnmanagedType.Bool)] ref bool debuggerPresent);

        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern uint GetCurrentThreadId();

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hwnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern IntPtr GetFocus();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsChild(IntPtr wWndParent, IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowPlacement(IntPtr hwnd, WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SWP uFlags);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        public static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        public static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr LoadCursor(IntPtr hInst, IntPtr iconId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int GetDoubleClickTime();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr MonitorFromPoint(Point pt, MonitorOptions dwFlags);

        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromRect(ref RECT rect, MonitorOptions dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfo lpmi);

        [DllImport("user32.dll", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnableWindow(IntPtr hWnd, [MarshalAs(UnmanagedType.Bool)] bool bEnable);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern uint RegisterWindowMessage([MarshalAs(UnmanagedType.LPWStr)] string lpString);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hwnd, SW code);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ClientToScreen(IntPtr hWnd, ref POINT point);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr SetWindowsHookEx(int hookType, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool UnhookWindowsHookEx(IntPtr idHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetCursor();

        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(out POINT point);

        [StructLayout(LayoutKind.Sequential)]
        internal struct CURSORINFO
        {
            internal uint Size;
            internal uint Flags;
            internal IntPtr Cursor;
            internal POINT ScreenPosition;
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorInfo(ref CURSORINFO cursorInfo);

        [StructLayout(LayoutKind.Sequential)]
        internal struct ICONINFO
        {
            internal int fIcon;
            internal uint xHotspot;
            internal uint yHotspot;
            internal IntPtr hbmMask;
            internal IntPtr hbmColor;
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetIconInfo(IntPtr icon, out ICONINFO iconInfo);

        [StructLayout(LayoutKind.Sequential)]
        internal class BITMAP
        {
            internal int bmType;
            internal int bmWidth;
            internal int bmHeight;
            internal int bmWidthBytes;
            internal ushort bmPlanes;
            internal ushort bmBitsPixel;
            internal IntPtr bmBits;
        }

        [DllImport("Gdi32.dll", SetLastError = true)]
        internal static extern int GetObject(IntPtr handle, int bufferSize, [In, Out] BITMAP bm);

        [DllImport("Gdi32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern int GetBitmapBits(IntPtr handle, int bufferSize, byte[] buffer);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BringWindowToTop(IntPtr hwnd);

        internal enum GA
        {
            PARENT = 0x1,
            ROOT = 0x2,
            ROOTOWNER = 0x3
        }

        [DllImport("user32.dll")]
        internal static extern IntPtr GetAncestor(IntPtr hwnd, [MarshalAs(UnmanagedType.U4)] GA gaFlags);

        internal enum SystemParameterActions : uint
        {
            GetHighContrast = 0x0042
        }

        internal enum HighContrastFlags : uint
        {
            /// <summary>
            /// The high contrast feature is on.
            /// </summary>
            HighContrastOn = 0x00000001,

            /// <summary>
            /// //The high contrast feature is available.
            /// </summary>
            Available = 0x00000002,

            /// <summary>
            /// The user can turn the high contrast feature on and off by simultaneously pressing the left ALT, left SHIFT, and PRINT SCREEN keys.
            /// </summary>
            HotKeyActive = 0x00000004,

            /// <summary>
            /// confirmation dialog appears when the high contrast feature is activated by using the hot key.
            /// </summary>
            ConfirmHotKey = 0x00000008,

            /// <summary>
            /// A siren is played when the user turns the high contrast feature on or off by using the hot key.
            /// </summary>
            HotKeySound = 0x00000010,

            /// <summary>
            /// A visual indicator is displayed when the high contrast feature is on. This value is not currently used and is ignored. 
            /// </summary>
            Indicator = 0x00000020,

            /// <summary>
            /// The hot key associated with the high contrast feature can be enabled. An application can retrieve this value, but cannot set it.
            /// </summary>
            HotKeyAvailable = 0x00000040
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct HIGHCONTRAST
        {
            public uint Size;

            [MarshalAs(UnmanagedType.U4)]
            public HighContrastFlags Flags;

            public IntPtr DefaultScheme;
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SystemParametersInfo([MarshalAs(UnmanagedType.U4)] SystemParameterActions uiAction,
                                                         int uiParam,
                                                         IntPtr pInfo,
                                                         int fWinIni);

        internal static bool IsCurrentThemeHighContrast()
        {
            int structSize = Marshal.SizeOf(typeof(HIGHCONTRAST));
            IntPtr rawMem = Marshal.AllocHGlobal(structSize);
            if (rawMem == IntPtr.Zero)
            {
                // There is an event ordering problem with cache invalidation inside SystemParameters.HighContrast, which is why we have this method; however, if 
                // we can't even allocate memory we may as well just return whatever value it has. Who knows, it might even be right :)
                return System.Windows.SystemParameters.HighContrast;
            }

            try
            {
                // Fill in the size field
                Marshal.WriteInt32(rawMem, structSize);

                if (!SystemParametersInfo(SystemParameterActions.GetHighContrast, structSize, rawMem, fWinIni: 0))
                {
                    return System.Windows.SystemParameters.HighContrast;
                }

                HIGHCONTRAST val = (HIGHCONTRAST)Marshal.PtrToStructure(rawMem, typeof(HIGHCONTRAST));

                return ((val.Flags & HighContrastFlags.HighContrastOn) == HighContrastFlags.HighContrastOn);
            }
            finally
            {
                if (rawMem != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(rawMem);
                }
            }
        }

        internal enum ProcessCreationFlags : uint
        {
            /// <summary>
            /// The child processes of a process associated with a job are not associated with the job. If the calling process is not associated with a job, this constant has no effect. 
            /// If the calling process is associated with a job, the job must set the JOB_OBJECT_LIMIT_BREAKAWAY_OK limit.
            /// </summary>
            CREATE_BREAKAWAY_FROM_JOB = 0x01000000,

            /// <summary>
            /// The new process does not inherit the error mode of the calling process. Instead, the new process gets the default error mode. This feature is particularly useful for 
            /// multithreaded shell applications that run with hard errors disabled. The default behavior is for the new process to inherit the error mode of the caller. Setting this flag 
            /// changes that default behavior.
            /// </summary>
            CREATE_DEFAULT_ERROR_MODE = 0x04000000,

            /// <summary>
            /// The new process has a new console, instead of inheriting its parent's console (the default). This flag cannot be used with DETACHED_PROCESS.
            /// </summary>
            CREATE_NEW_CONSOLE = 0x00000010,

            /// <summary>
            /// The new process is the root process of a new process group. The process group includes all processes that are descendants of this root process. The process identifier of the new 
            /// process group is the same as the process identifier, which is returned in the lpProcessInformation parameter. Process groups are used by the GenerateConsoleCtrlEvent function to enable 
            /// sending a CTRL+BREAK signal to a group of console processes. If this flag is specified, CTRL+C signals will be disabled for all processes within the new process group.
            /// This flag is ignored if specified with CREATE_NEW_CONSOLE.
            /// </summary>
            CREATE_NEW_PROCESS_GROUP = 0x00000200,

            /// <summary>
            /// The process is a console application that is being run without a console window. Therefore, the console handle for the application is not set. This flag is ignored if the application is 
            /// not a console application, or if it is used with either CREATE_NEW_CONSOLE or DETACHED_PROCESS.
            /// </summary>
            CREATE_NO_WINDOW = 0x08000000,

            /// <summary>
            /// The process is to be run as a protected process. The system restricts access to protected processes and the threads of protected processes. To activate a protected process, the binary 
            /// must have a special signature. This signature is provided by Microsoft but not currently available for non-Microsoft binaries. There are currently four protected processes: media foundation, 
            /// audio engine, Windows error reporting, and system. Components that load into these binaries must also be signed. Multimedia companies can leverage the first two protected processes.
            /// Windows Server 2003 and Windows XP:  This value is not supported.
            /// </summary>
            CREATE_PROTECTED_PROCESS = 0x00040000,

            /// <summary>
            /// Allows the caller to execute a child process that bypasses the process restrictions that would normally be applied automatically to the process.
            /// </summary>
            CREATE_PRESERVE_CODE_AUTHZ_LEVEL = 0x02000000,

            /// <summary>
            /// This flag is valid only when starting a 16-bit Windows-based application. If set, the new process runs in a private Virtual DOS Machine (VDM). By default, all 16-bit Windows-based 
            /// applications run as threads in a single, shared VDM. The advantage of running separately is that a crash only terminates the single VDM; any other programs running in distinct VDMs 
            /// continue to function normally. Also, 16-bit Windows-based applications that are run in separate VDMs have separate input queues. That means that if one application stops responding 
            /// momentarily, applications in separate VDMs continue to receive input. The disadvantage of running separately is that it takes significantly more memory to do so. You should use this 
            /// flag only if the user requests that 16-bit applications should run in their own VDM.
            /// </summary>
            CREATE_SEPARATE_WOW_VDM = 0x00000800,

            /// <summary>
            /// The flag is valid only when starting a 16-bit Windows-based application. If the DefaultSeparateVDM switch in the Windows section of WIN.INI is TRUE, this flag overrides the switch. The 
            /// new process is run in the shared Virtual DOS Machine.
            /// </summary>
            CREATE_SHARED_WOW_VDM = 0x00001000,

            /// <summary>
            /// The primary thread of the new process is created in a suspended state, and does not run until the ResumeThread function is called.
            /// </summary>
            CREATE_SUSPENDED = 0x00000004,

            /// <summary>
            /// If this flag is set, the environment block pointed to by lpEnvironment uses Unicode characters. Otherwise, the environment block uses ANSI characters.
            /// </summary>
            CREATE_UNICODE_ENVIRONMENT = 0x00000400,

            /// <summary>
            /// The calling thread starts and debugs the new process. It can receive all related debug events using the WaitForDebugEvent function.
            /// </summary>
            DEBUG_ONLY_THIS_PROCESS = 0x00000002,

            /// <summary>
            /// The calling thread starts and debugs the new process and all child processes created by the new process. It can receive all related debug events using the WaitForDebugEvent function.
            /// A process that uses DEBUG_PROCESS becomes the root of a debugging chain. This continues until another process in the chain is created with DEBUG_PROCESS.
            /// If this flag is combined with DEBUG_ONLY_THIS_PROCESS, the caller debugs only the new process, not any child processes.
            /// </summary>
            DEBUG_PROCESS = 0x00000001,

            /// <summary>
            /// For console processes, the new process does not inherit its parent's console (the default). The new process can call the AllocConsole function at a later time to create a console.
            /// This value cannot be used with CREATE_NEW_CONSOLE.
            /// </summary>
            DETACHED_PROCESS = 0x00000008,

            /// <summary>
            /// The process is created with extended startup information; the lpStartupInfo parameter specifies a STARTUPINFOEX structure.
            /// Windows Server 2003 and Windows XP:  This value is not supported.
            /// </summary>
            EXTENDED_STARTUPINFO_PRESENT = 0x00080000,

            /// <summary>
            /// The process inherits its parent's affinity. If the parent process has threads in more than one processor group, the new process inherits the group-relative affinity of an arbitrary 
            /// group in use by the parent. Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:  This value is not supported.
            /// </summary>
            INHERIT_PARENT_AFFINITY = 0x00010000
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct PROCESS_INFORMATION
        {
            internal IntPtr hProcess;
            internal IntPtr hThread;
            internal uint dwProcessId;
            internal uint dwThreadId;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class STARTUPINFO : IDisposable
        {
            public readonly int cb;
            public readonly IntPtr lpReserved = IntPtr.Zero;
            public readonly IntPtr lpDesktop = IntPtr.Zero;
            public readonly IntPtr lpTitle = IntPtr.Zero;
            public readonly int dwX = 0;
            public readonly int dwY = 0;
            public readonly int dwXSize = 0;
            public readonly int dwYSize = 0;
            public readonly int dwXCountChars = 0;
            public readonly int dwYCountChars = 0;
            public readonly int dwFillAttribute = 0;
            public readonly int dwFlags = 0;
            public readonly short wShowWindow = 0;
            public readonly short cbReserved2 = 0;
            public readonly IntPtr lpReserved2 = IntPtr.Zero;
            public SafeFileHandle hStdInput = new SafeFileHandle(IntPtr.Zero, false);
            public SafeFileHandle hStdOutput = new SafeFileHandle(IntPtr.Zero, false);
            public SafeFileHandle hStdError = new SafeFileHandle(IntPtr.Zero, false);

            public STARTUPINFO()
            {
                this.cb = Marshal.SizeOf(this);
            }

            public void Dispose()
            {
                // close the handles created for child process
                if (this.hStdInput != null && !this.hStdInput.IsInvalid)
                {
                    this.hStdInput.Close();
                    this.hStdInput = null;
                }

                if (this.hStdOutput != null && !this.hStdOutput.IsInvalid)
                {
                    this.hStdOutput.Close();
                    this.hStdOutput = null;
                }

                if (this.hStdError != null && !this.hStdError.IsInvalid)
                {
                    this.hStdError.Close();
                    this.hStdError = null;
                }
            }
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CreateProcess(string lpApplicationName,
                                                  string lpCommandLine,
                                                  IntPtr lpProcessAttributes,
                                                  IntPtr lpThreadAttributes,
                                                  [MarshalAs(UnmanagedType.Bool)] bool bInheritHandles,
                                                  [MarshalAs(UnmanagedType.U4)] ProcessCreationFlags dwCreationFlags,
                                                  IntPtr lpEnvironment,
                                                  string lpCurrentDirectory,
                                                  STARTUPINFO lpStartupInfo,
                                                  ref PROCESS_INFORMATION lpProcessInformation);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CloseHandle(HandleRef handle);
    }
}
