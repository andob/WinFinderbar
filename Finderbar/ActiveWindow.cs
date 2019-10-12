using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace WinFinderbar
{
    public class ActiveWindow
    {
        private const string WINAPI_DLL_NAME = "user32.dll";
        
        [DllImport(WINAPI_DLL_NAME, CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();
        
        [DllImport(WINAPI_DLL_NAME, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, [Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpString, int nMaxCount);

        [DllImport(WINAPI_DLL_NAME, SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowTextLength(IntPtr hWnd);
        
        [DllImport(WINAPI_DLL_NAME)]
        static extern long GetWindowThreadProcessId(IntPtr hWnd, ref ulong lpdwProcessId);
        
        [DllImport(WINAPI_DLL_NAME)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        
        [DllImport(WINAPI_DLL_NAME)]
        static extern bool GetWindowRect(IntPtr hwnd, ref RECT rectangle);
        
        [StructLayout(LayoutKind.Sequential)]
        struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        public readonly IntPtr Handle;

        private ActiveWindow(IntPtr handle)
        {
            this.Handle=handle;
        }

        public static ActiveWindow Get()
        {
            IntPtr activeWindowHandle=GetForegroundWindow();
            return new ActiveWindow(activeWindowHandle);
        }

        public string Title
        {
            get
            {
                int titleLength=GetWindowTextLength(Handle);
                StringBuilder titleStringBuilder=new StringBuilder(titleLength+1);
                GetWindowText(Handle, titleStringBuilder, titleStringBuilder.Capacity);
                string title=titleStringBuilder.ToString();
                if (title.Length>=50)
                    return title.Substring(0, 50)+"...";

                if (title.Length==0)
                {
                    //get process executable name
                    ulong processId=0;
                    GetWindowThreadProcessId(Handle, ref processId);
                    Process process=Process.GetProcessById((int)processId);
                    return process.ProcessName;
                }
                
                return title;
            }
        }

        public Point Location
        {
            get
            {
                RECT rectangle=new RECT();
                GetWindowRect(this.Handle, ref rectangle);
                return new Point(rectangle.left, rectangle.top);
            }
        }

        private ActiveWindowMenu _activeWindowMenu;
        public ActiveWindowMenu ActiveWindowMenu
        {
            get
            {
                if (_activeWindowMenu==null)
                    _activeWindowMenu=ActiveWindowMenu.Get(this);
                return _activeWindowMenu;
            }
        }

        public void Focus()
        {
            SetForegroundWindow(this.Handle);
        }
    }
}
