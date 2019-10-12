using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace WinFinderbar
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            bool shouldContinueRunning=true;
            using (Mutex mutex=new Mutex(
                    initiallyOwned: true, 
                    name: "WinFinderbar", 
                    createdNew: out shouldContinueRunning))
            {
                if (shouldContinueRunning)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new FinderbarWindow());
                }
                else
                {
                    MessageBox.Show("Another instance of WinFinderbar is already running!");
                }
            }
        }
    }
}
