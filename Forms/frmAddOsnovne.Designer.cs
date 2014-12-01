namespace Monitoring
{
    partial class frmAddOsnovne
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
            this.lblKlijent = new System.Windows.Forms.Label();
            this.cmbVrstaMonitoringa = new System.Windows.Forms.ComboBox();
            this.cmbTipFajla = new System.Windows.Forms.ComboBox();
            this.txtNaziv = new System.Windows.Forms.TextBox();
            this.txtMail = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lstBoxMaticni = new System.Windows.Forms.ListBox();
            this.txtMaticniBroj = new System.Windows.Forms.TextBox();
            this.btnAddMaticni = new System.Windows.Forms.Button();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnAddMaticniFile = new System.Windows.Forms.Button();
            this.btnObrisiMaticni = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDodajOsnovne = new System.Windows.Forms.Button();
            this.grpNadgledaneFirme = new System.Windows.Forms.GroupBox();
            this.txtConsole = new System.Windows.Forms.TextBox();
            this.btnNadjiFajl = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lstBoxMail = new System.Windows.Forms.ListBox();
            this.grpMail = new System.Windows.Forms.GroupBox();
            this.btnDeleteMail = new System.Windows.Forms.Button();
            this.btnAddMail = new System.Windows.Forms.Button();
            this.chkSaljeSeMail = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.grpNadgledaneFirme.SuspendLayout();
            this.grpMail.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblKlijent
            // 
            this.lblKlijent.AutoSize = true;
            this.lblKlijent.Font = new System.Drawing.Font("Terminator Two", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKlijent.Location = new System.Drawing.Point(250, 9);
            this.lblKlijent.Name = "lblKlijent";
            this.lblKlijent.Size = new System.Drawing.Size(150, 21);
            this.lblKlijent.TabIndex = 0;
            this.lblKlijent.Text = "lblKlijent";
            // 
            // cmbVrstaMonitoringa
            // 
            this.cmbVrstaMonitoringa.FormattingEnabled = true;
            this.cmbVrstaMonitoringa.Location = new System.Drawing.Point(6, 39);
            this.cmbVrstaMonitoringa.Name = "cmbVrstaMonitoringa";
            this.cmbVrstaMonitoringa.Size = new System.Drawing.Size(138, 21);
            this.cmbVrstaMonitoringa.TabIndex = 1;
            // 
            // cmbTipFajla
            // 
            this.cmbTipFajla.FormattingEnabled = true;
            this.cmbTipFajla.Location = new System.Drawing.Point(6, 83);
            this.cmbTipFajla.Name = "cmbTipFajla";
            this.cmbTipFajla.Size = new System.Drawing.Size(138, 21);
            this.cmbTipFajla.TabIndex = 2;
            // 
            // txtNaziv
            // 
            this.txtNaziv.Location = new System.Drawing.Point(6, 123);
            this.txtNaziv.Name = "txtNaziv";
            this.txtNaziv.Size = new System.Drawing.Size(138, 20);
            this.txtNaziv.TabIndex = 3;
            // 
            // txtMail
            // 
            this.txtMail.Location = new System.Drawing.Point(79, 13);
            this.txtMail.Name = "txtMail";
            this.txtMail.Size = new System.Drawing.Size(138, 20);
            this.txtMail.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Vrsta monitoringa:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Tip fajla:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Naziv monitoringa:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Mail za slanje:";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // lstBoxMaticni
            // 
            this.lstBoxMaticni.FormattingEnabled = true;
            this.lstBoxMaticni.Location = new System.Drawing.Point(6, 39);
            this.lstBoxMaticni.Name = "lstBoxMaticni";
            this.lstBoxMaticni.Size = new System.Drawing.Size(138, 277);
            this.lstBoxMaticni.TabIndex = 9;
            // 
            // txtMaticniBroj
            // 
            this.txtMaticniBroj.Location = new System.Drawing.Point(150, 55);
            this.txtMaticniBroj.Name = "txtMaticniBroj";
            this.txtMaticniBroj.Size = new System.Drawing.Size(138, 20);
            this.txtMaticniBroj.TabIndex = 10;
            this.txtMaticniBroj.Text = "maticni broj";
            // 
            // btnAddMaticni
            // 
            this.btnAddMaticni.Location = new System.Drawing.Point(150, 81);
            this.btnAddMaticni.Name = "btnAddMaticni";
            this.btnAddMaticni.Size = new System.Drawing.Size(219, 23);
            this.btnAddMaticni.TabIndex = 11;
            this.btnAddMaticni.Text = "Dodaj";
            this.btnAddMaticni.UseVisualStyleBackColor = true;
            this.btnAddMaticni.Click += new System.EventHandler(this.btnAddMaticni_Click);
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(150, 267);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(134, 20);
            this.txtFilePath.TabIndex = 12;
            this.txtFilePath.Text = "putanja do fajla";
            // 
            // btnAddMaticniFile
            // 
            this.btnAddMaticniFile.Location = new System.Drawing.Point(153, 293);
            this.btnAddMaticniFile.Name = "btnAddMaticniFile";
            this.btnAddMaticniFile.Size = new System.Drawing.Size(216, 23);
            this.btnAddMaticniFile.TabIndex = 13;
            this.btnAddMaticniFile.Text = "Dodaj";
            this.btnAddMaticniFile.UseVisualStyleBackColor = true;
            this.btnAddMaticniFile.Click += new System.EventHandler(this.btnAddMaticniFile_Click);
            // 
            // btnObrisiMaticni
            // 
            this.btnObrisiMaticni.Location = new System.Drawing.Point(6, 320);
            this.btnObrisiMaticni.Name = "btnObrisiMaticni";
            this.btnObrisiMaticni.Size = new System.Drawing.Size(138, 23);
            this.btnObrisiMaticni.TabIndex = 14;
            this.btnObrisiMaticni.Text = "Obriši matični";
            this.btnObrisiMaticni.UseVisualStyleBackColor = true;
            this.btnObrisiMaticni.Click += new System.EventHandler(this.btnObrisiMaticni_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbVrstaMonitoringa);
            this.groupBox1.Controls.Add(this.btnDodajOsnovne);
            this.groupBox1.Controls.Add(this.cmbTipFajla);
            this.groupBox1.Controls.Add(this.txtNaziv);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(5, 35);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(223, 181);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Osnovne informacije";
            // 
            // btnDodajOsnovne
            // 
            this.btnDodajOsnovne.Location = new System.Drawing.Point(6, 149);
            this.btnDodajOsnovne.Name = "btnDodajOsnovne";
            this.btnDodajOsnovne.Size = new System.Drawing.Size(138, 23);
            this.btnDodajOsnovne.TabIndex = 9;
            this.btnDodajOsnovne.Text = "Dodaj osnovne";
            this.btnDodajOsnovne.UseVisualStyleBackColor = true;
            this.btnDodajOsnovne.Click += new System.EventHandler(this.btnDodajOsnovne_Click);
            // 
            // grpNadgledaneFirme
            // 
            this.grpNadgledaneFirme.Controls.Add(this.txtConsole);
            this.grpNadgledaneFirme.Controls.Add(this.btnNadjiFajl);
            this.grpNadgledaneFirme.Controls.Add(this.label7);
            this.grpNadgledaneFirme.Controls.Add(this.label6);
            this.grpNadgledaneFirme.Controls.Add(this.label5);
            this.grpNadgledaneFirme.Controls.Add(this.lstBoxMaticni);
            this.grpNadgledaneFirme.Controls.Add(this.txtMaticniBroj);
            this.grpNadgledaneFirme.Controls.Add(this.btnObrisiMaticni);
            this.grpNadgledaneFirme.Controls.Add(this.btnAddMaticni);
            this.grpNadgledaneFirme.Controls.Add(this.btnAddMaticniFile);
            this.grpNadgledaneFirme.Controls.Add(this.txtFilePath);
            this.grpNadgledaneFirme.Enabled = false;
            this.grpNadgledaneFirme.Location = new System.Drawing.Point(228, 35);
            this.grpNadgledaneFirme.Name = "grpNadgledaneFirme";
            this.grpNadgledaneFirme.Size = new System.Drawing.Size(383, 349);
            this.grpNadgledaneFirme.TabIndex = 16;
            this.grpNadgledaneFirme.TabStop = false;
            this.grpNadgledaneFirme.Text = "Nadgledane firme";
            // 
            // txtConsole
            // 
            this.txtConsole.BackColor = System.Drawing.Color.Black;
            this.txtConsole.ForeColor = System.Drawing.Color.Lime;
            this.txtConsole.Location = new System.Drawing.Point(153, 322);
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.Size = new System.Drawing.Size(216, 20);
            this.txtConsole.TabIndex = 18;
            // 
            // btnNadjiFajl
            // 
            this.btnNadjiFajl.Location = new System.Drawing.Point(290, 264);
            this.btnNadjiFajl.Name = "btnNadjiFajl";
            this.btnNadjiFajl.Size = new System.Drawing.Size(75, 23);
            this.btnNadjiFajl.TabIndex = 17;
            this.btnNadjiFajl.Text = "Nadji...";
            this.btnNadjiFajl.UseVisualStyleBackColor = true;
            this.btnNadjiFajl.Click += new System.EventHandler(this.btnNadjiFajl_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(147, 251);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(130, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Dodavanje maticnih broja:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(150, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(134, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Dodavanje maticnog broja:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Maticni brojevi:";
            // 
            // lstBoxMail
            // 
            this.lstBoxMail.FormattingEnabled = true;
            this.lstBoxMail.Location = new System.Drawing.Point(7, 72);
            this.lstBoxMail.Name = "lstBoxMail";
            this.lstBoxMail.Size = new System.Drawing.Size(210, 56);
            this.lstBoxMail.TabIndex = 10;
            // 
            // grpMail
            // 
            this.grpMail.Controls.Add(this.chkSaljeSeMail);
            this.grpMail.Controls.Add(this.btnDeleteMail);
            this.grpMail.Controls.Add(this.label4);
            this.grpMail.Controls.Add(this.btnAddMail);
            this.grpMail.Controls.Add(this.lstBoxMail);
            this.grpMail.Controls.Add(this.txtMail);
            this.grpMail.Enabled = false;
            this.grpMail.Location = new System.Drawing.Point(5, 222);
            this.grpMail.Name = "grpMail";
            this.grpMail.Size = new System.Drawing.Size(223, 162);
            this.grpMail.TabIndex = 17;
            this.grpMail.TabStop = false;
            this.grpMail.Text = "Mail";
            // 
            // btnDeleteMail
            // 
            this.btnDeleteMail.Location = new System.Drawing.Point(6, 39);
            this.btnDeleteMail.Name = "btnDeleteMail";
            this.btnDeleteMail.Size = new System.Drawing.Size(67, 23);
            this.btnDeleteMail.TabIndex = 11;
            this.btnDeleteMail.Text = "Obriši";
            this.btnDeleteMail.UseVisualStyleBackColor = true;
            this.btnDeleteMail.Click += new System.EventHandler(this.btnDeleteMail_Click);
            // 
            // btnAddMail
            // 
            this.btnAddMail.Location = new System.Drawing.Point(79, 39);
            this.btnAddMail.Name = "btnAddMail";
            this.btnAddMail.Size = new System.Drawing.Size(138, 23);
            this.btnAddMail.TabIndex = 12;
            this.btnAddMail.Text = "Dodaj";
            this.btnAddMail.UseVisualStyleBackColor = true;
            this.btnAddMail.Click += new System.EventHandler(this.btnAddMail_Click);
            // 
            // chkSaljeSeMail
            // 
            this.chkSaljeSeMail.AutoSize = true;
            this.chkSaljeSeMail.Location = new System.Drawing.Point(6, 133);
            this.chkSaljeSeMail.Name = "chkSaljeSeMail";
            this.chkSaljeSeMail.Size = new System.Drawing.Size(84, 17);
            this.chkSaljeSeMail.TabIndex = 13;
            this.chkSaljeSeMail.Text = "Šalje se mail";
            this.chkSaljeSeMail.UseVisualStyleBackColor = true;
            this.chkSaljeSeMail.CheckedChanged += new System.EventHandler(this.chkSaljeSeMail_CheckedChanged);
            // 
            // frmAddOsnovne
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 397);
            this.Controls.Add(this.grpMail);
            this.Controls.Add(this.grpNadgledaneFirme);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblKlijent);
            this.Name = "frmAddOsnovne";
            this.Text = "Form3";
            this.Load += new System.EventHandler(this.frmAddOsnovne_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpNadgledaneFirme.ResumeLayout(false);
            this.grpNadgledaneFirme.PerformLayout();
            this.grpMail.ResumeLayout(false);
            this.grpMail.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblKlijent;
        private System.Windows.Forms.ComboBox cmbVrstaMonitoringa;
        private System.Windows.Forms.ComboBox cmbTipFajla;
        private System.Windows.Forms.TextBox txtNaziv;
        private System.Windows.Forms.TextBox txtMail;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox lstBoxMaticni;
        private System.Windows.Forms.TextBox txtMaticniBroj;
        private System.Windows.Forms.Button btnAddMaticni;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnAddMaticniFile;
        private System.Windows.Forms.Button btnObrisiMaticni;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnDodajOsnovne;
        private System.Windows.Forms.GroupBox grpNadgledaneFirme;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnNadjiFajl;
        private System.Windows.Forms.TextBox txtConsole;
        private System.Windows.Forms.ListBox lstBoxMail;
        private System.Windows.Forms.GroupBox grpMail;
        private System.Windows.Forms.Button btnDeleteMail;
        private System.Windows.Forms.Button btnAddMail;
        private System.Windows.Forms.CheckBox chkSaljeSeMail;
    }
}