using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFinderbar
{
    public partial class FinderbarWindow : AppBarWindow
    {
        private ActiveWindow _activeWindow;

        public FinderbarWindow()
        {
            InitializeComponent();
            
            this.StartPosition=FormStartPosition.Manual;
            this.Size=new Size(Screen.PrimaryScreen.Bounds.Width,  this.Height);
            this.Location=new Point(0, 0);
            
            this.RefreshTimer.Start();
        }

        private void Refresh(object sender, EventArgs e)
        {
            ActiveWindow activeWindow=ActiveWindow.Get();
            if (activeWindow.Handle!=this.Handle)
                this.ActiveWindow=activeWindow;
        }
        
        public ActiveWindow ActiveWindow
        {
            get => _activeWindow;
            set
            {
                _activeWindow=value;
                
                ActiveWindowTitleLabel.Text=_activeWindow.Title;
                
                if (_activeWindow.ActiveWindowMenu!=null)
                    ActiveWindowMenuPictureBox.Image=_activeWindow.ActiveWindowMenu.Screenshot;
                else ActiveWindowMenuPictureBox.Image=null;
            }
        }

        private void OnCloseFinderbarWindowClicked(object sender, EventArgs e)
        {
            DialogResult dialogResult=MessageBox.Show(
                caption: "WinFinderbar",
                text: "Are you sure you want to close WinFinderbar?",
                buttons: MessageBoxButtons.YesNo);

            if (dialogResult==DialogResult.Yes)
                this.Close();
        }
        
        private void OnWindowTitleLabelClicked(object sender, EventArgs e)
        {
            //todo simulate window icon click
            if (this.ActiveWindow!=null)
            {
                Point windowLocation=this.ActiveWindow.Location;
                MouseClickSimulator.SimulateRightClick(
                    x: windowLocation.X+30,
                    y: windowLocation.Y+20);
            }
        }

        public void OnActiveWindowMenuPictureBoxClicked(object sender, EventArgs e)
        {
            if (this.ActiveWindow!=null&&this.ActiveWindow.ActiveWindowMenu!=null)
            {
                Point clickLocation=ActiveWindowMenuPictureBox.PointToClient(Cursor.Position);
                if (clickLocation.X<this.ActiveWindow.ActiveWindowMenu.Size.Width)
                {
                    Point menuLocation=this.ActiveWindow.ActiveWindowMenu.Location;
                    MouseClickSimulator.SimulateClick(
                        x: clickLocation.X+menuLocation.X,
                        y: menuLocation.Y);                   
                }
            }
        }
        
        public void OnFinderbarWindowFocused(object sender, EventArgs e)
        {
            if (this.ActiveWindow!=null)
                this.ActiveWindow.Focus();
        }
    }
}
