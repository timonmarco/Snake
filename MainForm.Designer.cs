namespace Snake
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.gameDrawingControl1 = new Snake.GameDrawingControl();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 95;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // gameDrawingControl1
            // 
            this.gameDrawingControl1.BackColor = System.Drawing.Color.White;
            this.gameDrawingControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gameDrawingControl1.Location = new System.Drawing.Point(0, 0);
            this.gameDrawingControl1.Name = "gameDrawingControl1";
            this.gameDrawingControl1.Size = new System.Drawing.Size(884, 881);
            this.gameDrawingControl1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 881);
            this.Controls.Add(this.gameDrawingControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Text = "Snake";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private GameDrawingControl gameDrawingControl1;
        private System.Windows.Forms.Timer timer;
    }
}

