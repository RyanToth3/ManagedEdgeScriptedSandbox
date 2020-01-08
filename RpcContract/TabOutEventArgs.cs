using System;

namespace RpcContract
{
    public class TabOutEventArgs : EventArgs
    {
        public TabOutEventArgs(TabDirection direction)
        {
            this.Direction = direction;
        }

        public TabDirection Direction { get; }
    }

    public enum TabDirection
    {
        Forward = 0,
        Backward = 1,
    }
}
