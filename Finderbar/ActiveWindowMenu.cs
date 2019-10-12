using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace WinFinderbar
{
    public class ActiveWindowMenu
    {
        private const string WINAPI_DLL_NAME = "user32.dll";
        
        [DllImport(WINAPI_DLL_NAME, CharSet = CharSet.Auto)]
        public static extern IntPtr GetMenu(HandleRef hWnd);
        
        [DllImport (WINAPI_DLL_NAME)]
        static extern bool GetMenuItemRect(IntPtr hWnd, IntPtr hMenu, uint uItem, ref RECT lprcItem);

        [DllImport (WINAPI_DLL_NAME)]
        static extern int GetMenuItemCount(IntPtr hMenu);
        
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public Point Location = new Point();
        public Size Size = new Size();
        public Bitmap Screenshot;

        private ActiveWindowMenu(ActiveWindow activeWindow, IntPtr menuHandle)
        {
            RECT menuRect=new RECT ();
            int menuItemCount=GetMenuItemCount(menuHandle);
            for (int i=0; i<menuItemCount; i++)
            {
                GetMenuItemRect(activeWindow.Handle, menuHandle, 0, ref menuRect);
                if (i==0)
                {
                    this.Location.X=menuRect.Left;
                    this.Location.Y=menuRect.Top;
                    this.Size.Width=menuRect.Right-menuRect.Left;
                    this.Size.Height=menuRect.Bottom-menuRect.Top;
                }
                else this.Size.Width+=menuRect.Right-menuRect.Left+12;
            }
                
            this.Screenshot=new Bitmap(this.Size.Width, this.Size.Height);
            Graphics graphics=Graphics.FromImage(this.Screenshot);
            graphics.CopyFromScreen(
                sourceX: this.Location.X,
                sourceY: this.Location.Y,
                destinationX: 0,
                destinationY: 0,
                blockRegionSize: this.Size,
                copyPixelOperation: CopyPixelOperation.SourceCopy);
            
            new DarkThemeBitmapTransformer(this.Screenshot)
                .TransformIfNeeded();
        }

        public static ActiveWindowMenu Get(ActiveWindow activeWindow)
        {
            IntPtr menuHandle=GetMenu(new HandleRef(new Object(), activeWindow.Handle));
            if (menuHandle!=IntPtr.Zero)
                return new ActiveWindowMenu(activeWindow, menuHandle);
            return null;
        }
    }
}