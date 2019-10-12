using System;
using System.Diagnostics;
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

        public readonly IntPtr Handle;

        private ActiveWindow(IntPtr handle)
        {
            this.Handle=handle;
        }

        public static ActiveWindow Get()
        {
            var activeWindowHandle=GetForegroundWindow();
            return new ActiveWindow(activeWindowHandle);
        }

        public string Title
        {
            get
            {
                var titleLength=GetWindowTextLength(Handle);
                var titleStringBuilder=new StringBuilder(titleLength+1);
                GetWindowText(Handle, titleStringBuilder, titleStringBuilder.Capacity);
                var title=titleStringBuilder.ToString();
                if (title.Length>=50)
                    return title.Substring(0, 50)+"...";

                if (title.Length==0)
                {
                    //get process executable name
                    ulong processId=0;
                    GetWindowThreadProcessId(Handle, ref processId);
                    var process=Process.GetProcessById((int)processId);
                    return process.ProcessName;
                }
                
                return title;
            }
        }
    }
}
