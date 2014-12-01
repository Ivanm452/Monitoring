namespace Monitoring
{
    partial class frmAddBlokadeData
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
            this.btnUploadSource = new System.Windows.Forms.Button();
            this.txtSourceFilePath = new System.Windows.Forms.TextBox();
            this.btn_Generisi = new System.Windows.Forms.Button();
            this.txtConsole = new System.Windows.Forms.TextBox();
            this.btnNadjiFajl = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnUploadSource
            // 
            this.btnUploadSource.Location = new System.Drawing.Point(13, 13);
            this.btnUploadSource.Name = "btnUploadSource";
            this.btnUploadSource.Size = new System.Drawing.Size(110, 23);
            this.btnUploadSource.TabIndex = 0;
            this.btnUploadSource.Text = "Upload Source";
            this.btnUploadSource.UseVisualStyleBackColor = true;
            this.btnUploadSource.Click += new System.EventHandler(this.btnUploadSource_Click);
            // 
            // txtSourceFilePath
            // 
            this.txtSourceFilePath.Location = new System.Drawing.Point(129, 16);
            this.txtSourceFilePath.Name = "txtSourceFilePath";
            this.txtSourceFilePath.Size = new System.Drawing.Size(160, 20);
            this.txtSourceFilePath.TabIndex = 1;
            // 
            // btn_Generisi
            // 
            this.btn_Generisi.Location = new System.Drawing.Point(13, 42);
            this.btn_Generisi.Name = "btn_Generisi";
            this.btn_Generisi.Size = new System.Drawing.Size(110, 23);
            this.btn_Generisi.TabIndex = 2;
            this.btn_Generisi.Text = "Generisi";
            this.btn_Generisi.UseVisualStyleBackColor = true;
            this.btn_Generisi.Click += new System.EventHandler(this.btn_Generisi_Click);
            // 
            // txtConsole
            // 
            this.txtConsole.BackColor = System.Drawing.Color.Black;
            this.txtConsole.ForeColor = System.Drawing.Color.Lime;
            this.txtConsole.Location = new System.Drawing.Point(12, 71);
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtConsole.Size = new System.Drawing.Size(393, 20);
            this.txtConsole.TabIndex = 4;
            // 
            // btnNadjiFajl
            // 
            this.btnNadjiFajl.Location = new System.Drawing.Point(295, 16);
            this.btnNadjiFajl.Name = "btnNadjiFajl";
            this.btnNadjiFajl.Size = new System.Drawing.Size(110, 23);
            this.btnNadjiFajl.TabIndex = 5;
            this.btnNadjiFajl.Text = "Nadji fajl...";
            this.btnNadjiFajl.UseVisualStyleBackColor = true;
            this.btnNadjiFajl.Click += new System.EventHandler(this.btnNadjiFajl_Click);
            // 
            // frmAddBlokadeData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 103);
            this.Controls.Add(this.btnNadjiFajl);
            this.Controls.Add(this.txtConsole);
            this.Controls.Add(this.btn_Generisi);
            this.Controls.Add(this.txtSourceFilePath);
            this.Controls.Add(this.btnUploadSource);
            this.Name = "frmAddBlokadeData";
            this.Text = "Manuelno učitavanje podataka";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUploadSource;
        private System.Windows.Forms.TextBox txtSourceFilePath;
        private System.Windows.Forms.Button btn_Generisi;
        private System.Windows.Forms.TextBox txtConsole;
        private System.Windows.Forms.Button btnNadjiFajl;
    }
}

