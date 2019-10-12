using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WinFinderbar
{
    //https://www.codeproject.com/Articles/6741/AppBar-using-C
    public class AppBarWindow : Form
    {
        [StructLayout(LayoutKind.Sequential)]
        struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct APPBARDATA
        {
            public int cbSize;
            public IntPtr hWnd;
            public int uCallbackMessage;
            public int uEdge;
            public RECT rc;
            public IntPtr lParam;
        }

        enum ABMsg : int
        {
            ABM_NEW=0,
            ABM_REMOVE=1,
            ABM_QUERYPOS=2,
            ABM_SETPOS=3,
            ABM_GETSTATE=4,
            ABM_GETTASKBARPOS=5,
            ABM_ACTIVATE=6,
            ABM_GETAUTOHIDEBAR=7,
            ABM_SETAUTOHIDEBAR=8,
            ABM_WINDOWPOSCHANGED=9,
            ABM_SETSTATE=10
        }

        enum ABNotify : int
        {
            ABN_STATECHANGE=0,
            ABN_POSCHANGED,
            ABN_FULLSCREENAPP,
            ABN_WINDOWARRANGE
        }

        enum ABEdge : int
        {
            ABE_LEFT=0,
            ABE_TOP,
            ABE_RIGHT,
            ABE_BOTTOM
        }

        private bool IsAppBarRegistered = false;
        
        private const string SHELL_DLL_NAME = "shell32.dll";
        private const string WINAPI_DLL_NAME = "user32.dll";
        
        private const int WS_CAPTION = 0x00C00000;
        private const int WS_BORDER = 0x00800000;
        private const int WS_EX_TOOLWINDOW = 0x00000080;
        private const int WS_EX_TOPMOST = 0x00000008;

        [DllImport(SHELL_DLL_NAME, CallingConvention = CallingConvention.StdCall)]
        private static extern uint SHAppBarMessage(int dwMessage, ref APPBARDATA pData);
        
        [DllImport(WINAPI_DLL_NAME)]
        private static extern int GetSystemMetrics(int Index);
        
        [DllImport(WINAPI_DLL_NAME, ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int cx, int cy, bool repaint);
        
        [DllImport(WINAPI_DLL_NAME, CharSet=CharSet.Auto)]
        private static extern int RegisterWindowMessage(string msg);
        
        private int uCallBack;

        public AppBarWindow()
        {
            this.Load+=(sender, args) => RegisterAppBar();
            this.Closed+=(sender, args) => UnregisterAppBar();
        }

        private void RegisterAppBar()
        {
            APPBARDATA abd = new APPBARDATA();
            abd.cbSize = Marshal.SizeOf(abd);
            abd.hWnd = this.Handle;
            if (!IsAppBarRegistered)
            {
                uCallBack = RegisterWindowMessage("AppBarMessage");
                abd.uCallbackMessage = uCallBack;

                uint ret = SHAppBarMessage((int)ABMsg.ABM_NEW, ref abd);
                IsAppBarRegistered = true;

                ABSetPos();
            }
        }
        
        private void UnregisterAppBar()
        {
            APPBARDATA abd = new APPBARDATA();
            abd.cbSize = Marshal.SizeOf(abd);
            abd.hWnd = this.Handle;
            if (IsAppBarRegistered)
            {
                SHAppBarMessage((int)ABMsg.ABM_REMOVE, ref abd);
                IsAppBarRegistered = false;
            }
        }

        private void ABSetPos()
        {
            APPBARDATA abd = new APPBARDATA();
            abd.cbSize = Marshal.SizeOf(abd);
            abd.hWnd = this.Handle;
            abd.uEdge = (int)ABEdge.ABE_TOP;

            if (abd.uEdge == (int)ABEdge.ABE_LEFT || abd.uEdge == (int)ABEdge.ABE_RIGHT) 
            {
                abd.rc.top = 0;
                abd.rc.bottom = SystemInformation.PrimaryMonitorSize.Height;
                if (abd.uEdge == (int)ABEdge.ABE_LEFT) 
                {
                    abd.rc.left = 0;
                    abd.rc.right = Size.Width;
                }
                else 
                {
                    abd.rc.right = SystemInformation.PrimaryMonitorSize.Width;
                    abd.rc.left = abd.rc.right - Size.Width;
                }

            }
            else 
            {
                abd.rc.left = 0;
                abd.rc.right = SystemInformation.PrimaryMonitorSize.Width;
                if (abd.uEdge == (int)ABEdge.ABE_TOP) 
                {
                    abd.rc.top = 0;
                    abd.rc.bottom = Size.Height;
                }
                else 
                {
                    abd.rc.bottom = SystemInformation.PrimaryMonitorSize.Height;
                    abd.rc.top = abd.rc.bottom - Size.Height;
                }
            }

            // Query the system for an approved size and position. 
            SHAppBarMessage((int)ABMsg.ABM_QUERYPOS, ref abd); 

            // Adjust the rectangle, depending on the edge to which the 
            // appbar is anchored. 
            switch (abd.uEdge) 
            { 
                case (int)ABEdge.ABE_LEFT: 
                    abd.rc.right = abd.rc.left + Size.Width;
                    break; 
                case (int)ABEdge.ABE_RIGHT: 
                    abd.rc.left= abd.rc.right - Size.Width;
                    break; 
                case (int)ABEdge.ABE_TOP: 
                    abd.rc.bottom = abd.rc.top + Size.Height;
                    break; 
                case (int)ABEdge.ABE_BOTTOM: 
                    abd.rc.top = abd.rc.bottom - Size.Height;
                    break; 
            }

            // Pass the final bounding rectangle to the system. 
            SHAppBarMessage((int)ABMsg.ABM_SETPOS, ref abd); 

            // Move and size the appbar so that it conforms to the 
            // bounding rectangle passed to the system. 
            MoveWindow(abd.hWnd, abd.rc.left, abd.rc.top, 
                abd.rc.right - abd.rc.left, abd.rc.bottom - abd.rc.top, true); 
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == uCallBack)
            {
                switch(m.WParam.ToInt32())
                {
                    case (int)ABNotify.ABN_POSCHANGED:
                        ABSetPos();
                        break;
                }
            }

            base.WndProc(ref m);
        }

        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style &= (~WS_CAPTION); //remove window caption
                cp.Style &= (~WS_BORDER); //remove window border
                cp.ExStyle = WS_EX_TOOLWINDOW | WS_EX_TOPMOST; //remove from Alt+Tab menu, make always on top
                return cp;
            }
        }
    }
}
