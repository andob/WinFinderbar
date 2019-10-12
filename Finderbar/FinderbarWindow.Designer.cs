namespace WinFinderbar
{
    partial class FinderbarWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose ();
            }
            base.Dispose (disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
	        this.components=new System.ComponentModel.Container();
	        this.RefreshTimer=new System.Windows.Forms.Timer(this.components);
	        this.ActiveWindowMenuPictureBox=new System.Windows.Forms.PictureBox();
	        this.ActiveWindowTitleLabel=new System.Windows.Forms.Label();
	        this.CloseFinderbarButton=new System.Windows.Forms.Label();
	        ((System.ComponentModel.ISupportInitialize) (this.ActiveWindowMenuPictureBox)).BeginInit();
	        this.SuspendLayout();
	        // 
	        // RefreshTimer
	        // 
	        this.RefreshTimer.Interval=500;
	        this.RefreshTimer.Tick+=new System.EventHandler(this.Refresh);
	        // 
	        // ActiveWindowMenuPictureBox
	        // 
	        this.ActiveWindowMenuPictureBox.Dock=System.Windows.Forms.DockStyle.Fill;
	        this.ActiveWindowMenuPictureBox.Location=new System.Drawing.Point(40, 0);
	        this.ActiveWindowMenuPictureBox.Margin=new System.Windows.Forms.Padding(31, 3, 3, 3);
	        this.ActiveWindowMenuPictureBox.Name="ActiveWindowMenuPictureBox";
	        this.ActiveWindowMenuPictureBox.Size=new System.Drawing.Size(1560, 22);
	        this.ActiveWindowMenuPictureBox.TabIndex=0;
	        this.ActiveWindowMenuPictureBox.TabStop=false;
	        this.ActiveWindowMenuPictureBox.Click+=new System.EventHandler(this.OnActiveWindowMenuPictureBoxClicked);
	        // 
	        // ActiveWindowTitleLabel
	        // 
	        this.ActiveWindowTitleLabel.AutoSize=true;
	        this.ActiveWindowTitleLabel.BackColor=System.Drawing.Color.Transparent;
	        this.ActiveWindowTitleLabel.Dock=System.Windows.Forms.DockStyle.Left;
	        this.ActiveWindowTitleLabel.ForeColor=System.Drawing.Color.White;
	        this.ActiveWindowTitleLabel.Location=new System.Drawing.Point(0, 0);
	        this.ActiveWindowTitleLabel.Name="ActiveWindowTitleLabel";
	        this.ActiveWindowTitleLabel.Size=new System.Drawing.Size(40, 15);
	        this.ActiveWindowTitleLabel.TabIndex=1;
	        this.ActiveWindowTitleLabel.Text="Finder";
	        this.ActiveWindowTitleLabel.TextAlign=System.Drawing.ContentAlignment.MiddleCenter;
	        this.ActiveWindowTitleLabel.Click+=new System.EventHandler(this.OnWindowTitleLabelClicked);
	        // 
	        // CloseFinderbarButton
	        // 
	        this.CloseFinderbarButton.AutoSize=true;
	        this.CloseFinderbarButton.BackColor=System.Drawing.Color.Transparent;
	        this.CloseFinderbarButton.Dock=System.Windows.Forms.DockStyle.Right;
	        this.CloseFinderbarButton.ForeColor=System.Drawing.Color.White;
	        this.CloseFinderbarButton.Location=new System.Drawing.Point(1586, 0);
	        this.CloseFinderbarButton.Name="CloseFinderbarButton";
	        this.CloseFinderbarButton.Size=new System.Drawing.Size(14, 15);
	        this.CloseFinderbarButton.TabIndex=2;
	        this.CloseFinderbarButton.Text="X";
	        this.CloseFinderbarButton.TextAlign=System.Drawing.ContentAlignment.MiddleCenter;
	        this.CloseFinderbarButton.Click+=new System.EventHandler(this.OnCloseFinderbarWindowClicked);
	        // 
	        // FinderbarWindow
	        // 
	        this.AutoScaleDimensions=new System.Drawing.SizeF(7F, 15F);
	        this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
	        this.BackColor=System.Drawing.Color.Black;
	        this.ClientSize=new System.Drawing.Size(1600, 22);
	        this.Controls.Add(this.CloseFinderbarButton);
	        this.Controls.Add(this.ActiveWindowMenuPictureBox);
	        this.Controls.Add(this.ActiveWindowTitleLabel);
	        this.FormBorderStyle=System.Windows.Forms.FormBorderStyle.None;
	        this.Name="FinderbarWindow";
	        this.Text="Form1";
	        ((System.ComponentModel.ISupportInitialize) (this.ActiveWindowMenuPictureBox)).EndInit();
	        this.ResumeLayout(false);
	        this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Timer RefreshTimer;
        private System.Windows.Forms.PictureBox ActiveWindowMenuPictureBox;
        private System.Windows.Forms.Label ActiveWindowTitleLabel;
        private System.Windows.Forms.Label CloseFinderbarButton;
    }
}

