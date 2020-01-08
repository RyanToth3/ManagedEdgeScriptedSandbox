using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpcContract
{
    public class NativeMethods
    {
        public enum ShowWindow
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
    }
}
