using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Finderbar
{
    public partial class Form1 : Form
    {
        [DllImport ("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern IntPtr GetActiveWindow ();

        [DllImport ("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern IntPtr GetForegroundWindow ();

        [DllImport ("user32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr GetMenu (HandleRef hWnd);

        [DllImport ("user32", CharSet=CharSet.Auto, SetLastError=true)]
        internal static extern int GetWindowText (IntPtr hWnd, [Out, MarshalAs (UnmanagedType.LPTStr)] StringBuilder lpString, int nMaxCount);

        [DllImport ("user32.dll", SetLastError=true, CharSet=CharSet.Auto)]
        static extern int GetWindowTextLength (IntPtr hWnd);

        [DllImport ("user32.dll", SetLastError = true)]
        [return: MarshalAs (UnmanagedType.Bool)]
        static extern bool GetWindowRect (IntPtr hWnd, ref RECT lpRect);

        [DllImport ("user32.dll")]
        static extern bool GetMenuItemRect (IntPtr hWnd, IntPtr hMenu, uint uItem, ref RECT lprcItem);

        [DllImport ("user32.dll")]
        static extern int GetMenuItemCount(IntPtr hMenu);

        [DllImport ("user32.dll")]
        static extern int GetClassName (IntPtr hWnd, [Out, MarshalAs (UnmanagedType.LPTStr)] StringBuilder lpClassName, int nMaxCount);

        [DllImport ("user32.dll")]
        static extern long GetWindowThreadProcessId(IntPtr hWnd, ref ulong lpdwProcessId);

        [DllImport ("user32.dll")]
        static extern bool SetForegroundWindow (IntPtr hWnd);

        [DllImport ("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow (string lpClassName, string lpWindowName);

        [DllImport ("user32.dll", SetLastError = true)]
        public static extern bool ShowWindow (IntPtr hWnd, int nCmdShow);

        [DllImport ("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow (IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);

        [StructLayout (LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public IntPtr ActiveWindow;
        public Bitmap MenuScreenshot;
        public Rectangle MenuRegion;

        public Form1 ()
        {
            InitializeComponent ();
            ActiveWindow=this.Handle;
            this.TopMost=true;
            this.Size=new Size (Screen.PrimaryScreen.Bounds.Width, 18);
            this.Location=new Point (0, 0);
            timer1.Start ();
        }

        public static string GetWindowTitle (IntPtr hWnd)
        {
            int length=GetWindowTextLength (hWnd);
            StringBuilder sb=new StringBuilder (length + 1);
            GetWindowText (hWnd, sb, sb.Capacity);
            return sb.ToString ();
        }

        private string GetWindowName (IntPtr hWnd)
        {
            ulong pid=0;
            GetWindowThreadProcessId (hWnd, ref pid);
            Process p=Process.GetProcessById ((int)pid);
            if (p!=null)
                return p.ProcessName;
            return "finder";
        }

        private void timer1_Tick (object sender, EventArgs e)
        {
            IntPtr w2=GetForegroundWindow ();
            if(ActiveWindow!=w2)
            {
                if(w2==this.Handle)
                {
                    SetForegroundWindow (ActiveWindow);
                }
                else
                {
                    ActiveWindow=w2;
                    label1.Text=GetWindowName (ActiveWindow);
                    if(label1.Text=="Idle")
                    {
                        label1.Text="Finderbar";

                    }
                    else
                    {
                        IntPtr menu=GetMenu (new HandleRef (this, ActiveWindow));
                        if(menu!=IntPtr.Zero)
                        {
                            RECT menuRect=new RECT ();
                            MenuRegion=new Rectangle ();

                            for(int i=0; i<GetMenuItemCount (menu); i++)
                            {
                                GetMenuItemRect (ActiveWindow, menu, 0, ref menuRect);
                                if(i==0) MenuRegion=new Rectangle (menuRect.Left, menuRect.Top, menuRect.Right-menuRect.Left, menuRect.Bottom-menuRect.Top);
                                else MenuRegion.Width+=menuRect.Right-menuRect.Left+12;
                            }
                        }
                        else MenuRegion=new Rectangle ();
                    }
                }
            }

            if(MenuRegion.Width==0&&MenuRegion.Height==0)
            {
                this.MenuScreenshot=new Bitmap (1, 1);
            }
            else
            {
                this.MenuScreenshot=new Bitmap (MenuRegion.Width, MenuRegion.Height);
                Graphics g=Graphics.FromImage (MenuScreenshot);
                g.CopyFromScreen (MenuRegion.Left, MenuRegion.Top, 0, 0, this.MenuScreenshot.Size, CopyPixelOperation.SourceCopy);
            }
            pictureBox1.Image=MenuScreenshot;
        }

        //aplication name click
        private void label1_Click (object sender, EventArgs e)
        {
            if(label1.Text!="finder")
            {
                //window contextual menu
            }
        }

        [StructLayout (LayoutKind.Sequential)]
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
            ABM_NEW = 0,
            ABM_REMOVE = 1,
            ABM_QUERYPOS = 2,
            ABM_SETPOS = 3,
            ABM_GETSTATE = 4,
            ABM_GETTASKBARPOS = 5,
            ABM_ACTIVATE = 6,
            ABM_GETAUTOHIDEBAR = 7,
            ABM_SETAUTOHIDEBAR = 8,
            ABM_WINDOWPOSCHANGED = 9,
            ABM_SETSTATE = 10
        }

        enum ABNotify : int
        {
            ABN_STATECHANGE = 0,
            ABN_POSCHANGED,
            ABN_FULLSCREENAPP,
            ABN_WINDOWARRANGE
        }

        enum ABEdge : int
        {
            ABE_LEFT = 0,
            ABE_TOP,
            ABE_RIGHT,
            ABE_BOTTOM
        }

        private bool fBarRegistered = false;

        [DllImport ("SHELL32", CallingConvention = CallingConvention.StdCall)]
        static extern uint SHAppBarMessage (int dwMessage, ref APPBARDATA pData);
        [DllImport ("USER32")]
        static extern int GetSystemMetrics (int Index);
        [DllImport ("User32.dll", CharSet = CharSet.Auto)]
        private static extern int RegisterWindowMessage (string msg);
        private int uCallBack;

        private void RegisterBar ()
        {
            APPBARDATA abd = new APPBARDATA ();
            abd.cbSize = Marshal.SizeOf (abd);
            abd.hWnd = this.Handle;
            if(!fBarRegistered)
            {
                uCallBack = RegisterWindowMessage ("AppBarMessage");
                abd.uCallbackMessage = uCallBack;

                uint ret = SHAppBarMessage ((int)ABMsg.ABM_NEW, ref abd);
                fBarRegistered = true;

                ABSetPos ();
            }
            else
            {
                SHAppBarMessage ((int)ABMsg.ABM_REMOVE, ref abd);
                fBarRegistered = false;
            }
        }

        private void ABSetPos ()
        {
            APPBARDATA abd = new APPBARDATA ();
            abd.cbSize = Marshal.SizeOf (abd);
            abd.hWnd = this.Handle;
            abd.uEdge = (int)ABEdge.ABE_TOP;

            if(abd.uEdge == (int)ABEdge.ABE_LEFT || abd.uEdge == (int)ABEdge.ABE_RIGHT)
            {
                abd.rc.Top = 0;
                abd.rc.Bottom = SystemInformation.PrimaryMonitorSize.Height;
                if(abd.uEdge == (int)ABEdge.ABE_LEFT)
                {
                    abd.rc.Left = 0;
                    abd.rc.Right = Size.Width;
                }
                else
                {
                    abd.rc.Right = SystemInformation.PrimaryMonitorSize.Width;
                    abd.rc.Left = abd.rc.Right - Size.Width;
                }

            }
            else
            {
                abd.rc.Left = 0;
                abd.rc.Right = SystemInformation.PrimaryMonitorSize.Width;
                if(abd.uEdge == (int)ABEdge.ABE_TOP)
                {
                    abd.rc.Top = 0;
                    abd.rc.Bottom = Size.Height;
                }
                else
                {
                    abd.rc.Bottom = SystemInformation.PrimaryMonitorSize.Height;
                    abd.rc.Top = abd.rc.Bottom - Size.Height;
                }
            }

            // Query the system for an approved size and position. 
            SHAppBarMessage ((int)ABMsg.ABM_QUERYPOS, ref abd);

            // Adjust the rectangle, depending on the edge to which the 
            // appbar is anchored. 
            switch(abd.uEdge)
            {
                case (int)ABEdge.ABE_LEFT:
                    abd.rc.Right = abd.rc.Left + Size.Width;
                    break;
                case (int)ABEdge.ABE_RIGHT:
                    abd.rc.Left = abd.rc.Right - Size.Width;
                    break;
                case (int)ABEdge.ABE_TOP:
                    abd.rc.Bottom = abd.rc.Top + Size.Height;
                    break;
                case (int)ABEdge.ABE_BOTTOM:
                    abd.rc.Top = abd.rc.Bottom - Size.Height;
                    break;
            }

            // Pass the final bounding rectangle to the system. 
            SHAppBarMessage ((int)ABMsg.ABM_SETPOS, ref abd);

            // Move and size the appbar so that it conforms to the 
            // bounding rectangle passed to the system. 
            MoveWindow (abd.hWnd, abd.rc.Left, abd.rc.Top,
                abd.rc.Right - abd.rc.Left, abd.rc.Bottom - abd.rc.Top, true);
        }

        protected override void WndProc (ref System.Windows.Forms.Message m)
        {
            if(m.Msg == uCallBack)
            {
                switch(m.WParam.ToInt32 ())
                {
                    case (int)ABNotify.ABN_POSCHANGED:
                        ABSetPos ();
                        break;
                }
            }

            base.WndProc (ref m);
        }

        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style &= (~0x00C00000); // WS_CAPTION
                cp.Style &= (~0x00800000); // WS_BORDER
                cp.ExStyle = 0x00000080 | 0x00000008; // WS_EX_TOOLWINDOW | WS_EX_TopMOST
                return cp;
            }
        }

        private void Form1_Load (object sender, EventArgs e)
        {
            RegisterBar ();
        }

        private void Form1_FormClosed (object sender, FormClosedEventArgs e)
        {
            RegisterBar ();
        }
    }
}
