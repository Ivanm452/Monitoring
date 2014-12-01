namespace Monitoring
{
    partial class frmAddMonitoring
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
            this.lstMonitorings = new System.Windows.Forms.ListBox();
            this.txtMaticniBroj = new System.Windows.Forms.TextBox();
            this.txtNaziv = new System.Windows.Forms.TextBox();
            this.btnDodajKlijenta = new System.Windows.Forms.Button();
            this.btnEditMonitoring = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAddMonitoring = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnObrisiMonitoring = new System.Windows.Forms.Button();
            this.btnObrisiKlijenta = new System.Windows.Forms.Button();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnAddPath = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnFindFile = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbKlijenti
            // 
            this.cmbKlijenti.FormattingEnabled = true;
            this.cmbKlijenti.Location = new System.Drawing.Point(12, 31);
            this.cmbKlijenti.Name = "cmbKlijenti";
            this.cmbKlijenti.Size = new System.Drawing.Size(190, 21);
            this.cmbKlijenti.TabIndex = 0;
            this.cmbKlijenti.SelectedIndexChanged += new System.EventHandler(this.cmbKlijenti_SelectedIndexChanged);
            // 
            // lstMonitorings
            // 
            this.lstMonitorings.FormattingEnabled = true;
            this.lstMonitorings.Location = new System.Drawing.Point(276, 15);
            this.lstMonitorings.Name = "lstMonitorings";
            this.lstMonitorings.Size = new System.Drawing.Size(178, 225);
            this.lstMonitorings.TabIndex = 1;
            // 
            // txtMaticniBroj
            // 
            this.txtMaticniBroj.Location = new System.Drawing.Point(6, 38);
            this.txtMaticniBroj.Name = "txtMaticniBroj";
            this.txtMaticniBroj.Size = new System.Drawing.Size(184, 20);
            this.txtMaticniBroj.TabIndex = 2;
            // 
            // txtNaziv
            // 
            this.txtNaziv.Location = new System.Drawing.Point(6, 81);
            this.txtNaziv.Name = "txtNaziv";
            this.txtNaziv.Size = new System.Drawing.Size(184, 20);
            this.txtNaziv.TabIndex = 3;
            // 
            // btnDodajKlijenta
            // 
            this.btnDodajKlijenta.Location = new System.Drawing.Point(6, 107);
            this.btnDodajKlijenta.Name = "btnDodajKlijenta";
            this.btnDodajKlijenta.Size = new System.Drawing.Size(184, 23);
            this.btnDodajKlijenta.TabIndex = 4;
            this.btnDodajKlijenta.Text = "Dodaj klijenta";
            this.btnDodajKlijenta.UseVisualStyleBackColor = true;
            this.btnDodajKlijenta.Click += new System.EventHandler(this.btnDodajKlijenta_Click);
            // 
            // btnEditMonitoring
            // 
            this.btnEditMonitoring.Location = new System.Drawing.Point(276, 277);
            this.btnEditMonitoring.Name = "btnEditMonitoring";
            this.btnEditMonitoring.Size = new System.Drawing.Size(178, 23);
            this.btnEditMonitoring.TabIndex = 5;
            this.btnEditMonitoring.Text = "Izmeni monitoring";
            this.btnEditMonitoring.UseVisualStyleBackColor = true;
            this.btnEditMonitoring.Click += new System.EventHandler(this.btnEditMonitoring_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtMaticniBroj);
            this.groupBox1.Controls.Add(this.txtNaziv);
            this.groupBox1.Controls.Add(this.btnDodajKlijenta);
            this.groupBox1.Location = new System.Drawing.Point(12, 87);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 139);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Dodavanje klijenta";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Naziv:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Matični broj:";
            // 
            // btnAddMonitoring
            // 
            this.btnAddMonitoring.Location = new System.Drawing.Point(276, 248);
            this.btnAddMonitoring.Name = "btnAddMonitoring";
            this.btnAddMonitoring.Size = new System.Drawing.Size(178, 23);
            this.btnAddMonitoring.TabIndex = 7;
            this.btnAddMonitoring.Text = "Dodaj monitoring";
            this.btnAddMonitoring.UseVisualStyleBackColor = true;
            this.btnAddMonitoring.Click += new System.EventHandler(this.btnAddMonitoring_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Klijent:";
            // 
            // btnObrisiMonitoring
            // 
            this.btnObrisiMonitoring.Location = new System.Drawing.Point(276, 306);
            this.btnObrisiMonitoring.Name = "btnObrisiMonitoring";
            this.btnObrisiMonitoring.Size = new System.Drawing.Size(178, 23);
            this.btnObrisiMonitoring.TabIndex = 9;
            this.btnObrisiMonitoring.Text = "Obriši monitoring";
            this.btnObrisiMonitoring.UseVisualStyleBackColor = true;
            this.btnObrisiMonitoring.Click += new System.EventHandler(this.btnObrisiMonitoring_Click);
            // 
            // btnObrisiKlijenta
            // 
            this.btnObrisiKlijenta.Location = new System.Drawing.Point(12, 58);
            this.btnObrisiKlijenta.Name = "btnObrisiKlijenta";
            this.btnObrisiKlijenta.Size = new System.Drawing.Size(190, 23);
            this.btnObrisiKlijenta.TabIndex = 10;
            this.btnObrisiKlijenta.Text = "Obriši klijenta";
            this.btnObrisiKlijenta.UseVisualStyleBackColor = true;
            this.btnObrisiKlijenta.Click += new System.EventHandler(this.btnObrisiKlijenta_Click);
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(6, 37);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(83, 20);
            this.txtFilePath.TabIndex = 11;
            // 
            // btnAddPath
            // 
            this.btnAddPath.Location = new System.Drawing.Point(3, 63);
            this.btnAddPath.Name = "btnAddPath";
            this.btnAddPath.Size = new System.Drawing.Size(184, 23);
            this.btnAddPath.TabIndex = 11;
            this.btnAddPath.Text = "Dodaj";
            this.btnAddPath.UseVisualStyleBackColor = true;
            this.btnAddPath.Click += new System.EventHandler(this.btnAddPath_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnFindFile);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtFilePath);
            this.groupBox2.Controls.Add(this.btnAddPath);
            this.groupBox2.Location = new System.Drawing.Point(15, 232);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(197, 97);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Dodavanje kroz fajl";
            // 
            // btnFindFile
            // 
            this.btnFindFile.Location = new System.Drawing.Point(95, 34);
            this.btnFindFile.Name = "btnFindFile";
            this.btnFindFile.Size = new System.Drawing.Size(92, 23);
            this.btnFindFile.TabIndex = 13;
            this.btnFindFile.Text = "Nađi...";
            this.btnFindFile.UseVisualStyleBackColor = true;
            this.btnFindFile.Click += new System.EventHandler(this.btnFindFile_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Putanja do fajla:";
            // 
            // frmAddMonitoring
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 340);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnObrisiKlijenta);
            this.Controls.Add(this.btnObrisiMonitoring);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnAddMonitoring);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnEditMonitoring);
            this.Controls.Add(this.lstMonitorings);
            this.Controls.Add(this.cmbKlijenti);
            this.Name = "frmAddMonitoring";
            this.Text = "Dodaj podatke o monitoringu";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstMonitorings;
        private System.Windows.Forms.TextBox txtMaticniBroj;
        private System.Windows.Forms.TextBox txtNaziv;
        private System.Windows.Forms.Button btnDodajKlijenta;
        private System.Windows.Forms.Button btnEditMonitoring;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAddMonitoring;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnObrisiMonitoring;
        private System.Windows.Forms.Button btnObrisiKlijenta;
        public System.Windows.Forms.ComboBox cmbKlijenti;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnAddPath;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnFindFile;
    }
}