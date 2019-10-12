using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFinderbar
{
    public partial class FinderbarWindow : Form
    {
        public FinderbarWindow()
        {
            InitializeComponent();
            this.StartPosition=FormStartPosition.Manual;
            this.Size=new Size(Screen.PrimaryScreen.Bounds.Width,  this.Height);
            this.Location=new Point(0, 0);
            this.ShowInTaskbar=false;
            this.TopMost=true;
            this.RefreshTimer.Start();
        }

        private void Refresh(object sender, EventArgs e)
        {
            var activeWindow=ActiveWindow.Get();
            if (activeWindow.Handle!=this.Handle)
                ActiveWindowTitleLabel.Text=activeWindow.Title;                
        }

        private void OnWindowTitleLabelClicked(object sender, EventArgs e)
        {
        }
        
        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                var createParams = base.CreateParams;
                createParams.Style &=(~0x00C00000); // remove window caption
                createParams.Style &=(~0x00800000); // remove window border
                createParams.ExStyle = 0x00000080 | 0x00000008; // remove from Alt+TAB menu, make always on top
                return createParams;
            }
        }
        
        private void OnFinderbarWindowLoaded(object sender, EventArgs e)
        {
//            RegisterBar();
        }

        private void OnFinderbarWindowClosed(object sender, FormClosedEventArgs e)
        {
//            RegisterBar();
        }
    }
}
