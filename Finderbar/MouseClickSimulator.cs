using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WinFinderbar
{
    public static class MouseClickSimulator
    {
        private const string WINAPI_DLL_NAME = "user32.dll";
        
        [DllImport(WINAPI_DLL_NAME)]
        public static extern long SetCursorPos(int x, int y);

        [DllImport(WINAPI_DLL_NAME, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        
        //Mouse actions
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        private const int MOUSEEVENTF_MOVE = 0x8000;
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const int MOUSEEVENTF_LEFTUP = 0x0004;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const int MOUSEEVENTF_RIGHTUP = 0x0010;

        public static void SimulateClick(int x, int y)
        {
            SimulateLeftClick(x, y);
        }

        public static void SimulateLeftClick(int x, int y)
        {
            SetCursorPos(x, y);
            
            mouse_event(MOUSEEVENTF_MOVE|MOUSEEVENTF_LEFTDOWN|MOUSEEVENTF_LEFTUP, 
                (uint)x, (uint)y, 0, 0);
        }

        public static void SimulateRightClick(int x, int y)
        {
            SetCursorPos(x, y);
            
            mouse_event(MOUSEEVENTF_MOVE|MOUSEEVENTF_RIGHTDOWN|MOUSEEVENTF_RIGHTUP, 
                (uint)x, (uint)y, 0, 0);
        }
    }
}