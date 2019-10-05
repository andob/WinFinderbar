namespace Finderbar
{
    partial class Form1
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
        private void InitializeComponent ()
        {
        	this.components = new System.ComponentModel.Container();
        	this.timer1 = new System.Windows.Forms.Timer(this.components);
        	this.pictureBox1 = new System.Windows.Forms.PictureBox();
        	this.label1 = new System.Windows.Forms.Label();
        	((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
        	this.SuspendLayout();
        	// 
        	// timer1
        	// 
        	this.timer1.Interval = 500;
        	this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
        	// 
        	// pictureBox1
        	// 
        	this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.pictureBox1.Location = new System.Drawing.Point(36, 0);
        	this.pictureBox1.Name = "pictureBox1";
        	this.pictureBox1.Size = new System.Drawing.Size(610, 19);
        	this.pictureBox1.TabIndex = 0;
        	this.pictureBox1.TabStop = false;
        	// 
        	// label1
        	// 
        	this.label1.AutoSize = true;
        	this.label1.BackColor = System.Drawing.Color.Transparent;
        	this.label1.Dock = System.Windows.Forms.DockStyle.Left;
        	this.label1.ForeColor = System.Drawing.Color.White;
        	this.label1.Location = new System.Drawing.Point(0, 0);
        	this.label1.Name = "label1";
        	this.label1.Size = new System.Drawing.Size(36, 13);
        	this.label1.TabIndex = 1;
        	this.label1.Text = "Finder";
        	this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        	this.label1.Click += new System.EventHandler(this.label1_Click);
        	// 
        	// Form1
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.BackColor = System.Drawing.Color.Black;
        	this.ClientSize = new System.Drawing.Size(646, 19);
        	this.Controls.Add(this.pictureBox1);
        	this.Controls.Add(this.label1);
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        	this.Name = "Form1";
        	this.Text = "Form1";
        	this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
        	this.Load += new System.EventHandler(this.Form1_Load);
        	((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
        	this.ResumeLayout(false);
        	this.PerformLayout();

        }this.PerformLayout();

        }this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
    }
}

