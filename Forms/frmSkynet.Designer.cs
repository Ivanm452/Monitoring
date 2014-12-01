namespace Monitoring.Forms
{
    partial class frmSkynet
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
            this.txtConsole = new System.Windows.Forms.TextBox();
            this.txtConsole2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtConsole
            // 
            this.txtConsole.BackColor = System.Drawing.Color.Black;
            this.txtConsole.ForeColor = System.Drawing.Color.Lime;
            this.txtConsole.Location = new System.Drawing.Point(12, 12);
            this.txtConsole.Multiline = true;
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtConsole.Size = new System.Drawing.Size(496, 224);
            this.txtConsole.TabIndex = 1;
            // 
            // txtConsole2
            // 
            this.txtConsole2.BackColor = System.Drawing.Color.Black;
            this.txtConsole2.ForeColor = System.Drawing.Color.Lime;
            this.txtConsole2.Location = new System.Drawing.Point(12, 242);
            this.txtConsole2.Name = "txtConsole2";
            this.txtConsole2.Size = new System.Drawing.Size(496, 20);
            this.txtConsole2.TabIndex = 2;
            // 
            // frmSkynet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 262);
            this.Controls.Add(this.txtConsole2);
            this.Controls.Add(this.txtConsole);
            this.Name = "frmSkynet";
            this.Text = "frmSkynet";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSkynet_FormClosing);
            this.Load += new System.EventHandler(this.frmSkynet_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtConsole;
        private System.Windows.Forms.TextBox txtConsole2;
    }
}