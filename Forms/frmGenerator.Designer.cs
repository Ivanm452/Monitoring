namespace Monitoring
{
    partial class frmGenerator
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
            this.cmbKlijenti = new System.Windows.Forms.ComboBox();
            this.lstBoxMaticni = new System.Windows.Forms.ListBox();
            this.btnGenerisiIzabrani = new System.Windows.Forms.Button();
            this.cmbMonitoring = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtConsole = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cmbKlijenti
            // 
            this.cmbKlijenti.FormattingEnabled = true;
            this.cmbKlijenti.Location = new System.Drawing.Point(12, 43);
            this.cmbKlijenti.Name = "cmbKlijenti";
            this.cmbKlijenti.Size = new System.Drawing.Size(164, 21);
            this.cmbKlijenti.TabIndex = 0;
            this.cmbKlijenti.SelectedIndexChanged += new System.EventHandler(this.cmbKlijenti_SelectedIndexChanged);
            // 
            // lstBoxMaticni
            // 
            this.lstBoxMaticni.FormattingEnabled = true;
            this.lstBoxMaticni.Location = new System.Drawing.Point(262, 43);
            this.lstBoxMaticni.Name = "lstBoxMaticni";
            this.lstBoxMaticni.Size = new System.Drawing.Size(196, 290);
            this.lstBoxMaticni.TabIndex = 1;
            // 
            // btnGenerisiIzabrani
            // 
            this.btnGenerisiIzabrani.Location = new System.Drawing.Point(12, 120);
            this.btnGenerisiIzabrani.Name = "btnGenerisiIzabrani";
            this.btnGenerisiIzabrani.Size = new System.Drawing.Size(164, 23);
            this.btnGenerisiIzabrani.TabIndex = 2;
            this.btnGenerisiIzabrani.Text = "Generisi za izabrani";
            this.btnGenerisiIzabrani.UseVisualStyleBackColor = true;
            this.btnGenerisiIzabrani.Click += new System.EventHandler(this.btnGenerisiIzabrani_Click);
            // 
            // cmbMonitoring
            // 
            this.cmbMonitoring.FormattingEnabled = true;
            this.cmbMonitoring.Location = new System.Drawing.Point(12, 93);
            this.cmbMonitoring.Name = "cmbMonitoring";
            this.cmbMonitoring.Size = new System.Drawing.Size(164, 21);
            this.cmbMonitoring.TabIndex = 3;
            this.cmbMonitoring.SelectedIndexChanged += new System.EventHandler(this.cmbMonitoring_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Klijent:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Monitoring:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(259, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Maticni brojevi:";
            // 
            // txtConsole
            // 
            this.txtConsole.BackColor = System.Drawing.Color.Black;
            this.txtConsole.ForeColor = System.Drawing.Color.Lime;
            this.txtConsole.Location = new System.Drawing.Point(15, 339);
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.Size = new System.Drawing.Size(446, 20);
            this.txtConsole.TabIndex = 8;
            // 
            // frmGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 370);
            this.Controls.Add(this.txtConsole);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbMonitoring);
            this.Controls.Add(this.btnGenerisiIzabrani);
            this.Controls.Add(this.lstBoxMaticni);
            this.Controls.Add(this.cmbKlijenti);
            this.Name = "frmGenerator";
            this.Text = "frmGenerator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbKlijenti;
        private System.Windows.Forms.ListBox lstBoxMaticni;
        private System.Windows.Forms.Button btnGenerisiIzabrani;
        private System.Windows.Forms.ComboBox cmbMonitoring;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtConsole;
    }
}