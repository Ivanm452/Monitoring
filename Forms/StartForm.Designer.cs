namespace Monitoring
{
    partial class StartForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartForm));
            this.btnUcitaj = new System.Windows.Forms.Button();
            this.btnAddMonitoring = new System.Windows.Forms.Button();
            this.btnGenerator = new System.Windows.Forms.Button();
            this.btnPodesavanja = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnUcitaj
            // 
            this.btnUcitaj.Location = new System.Drawing.Point(12, 45);
            this.btnUcitaj.Name = "btnUcitaj";
            this.btnUcitaj.Size = new System.Drawing.Size(160, 40);
            this.btnUcitaj.TabIndex = 0;
            this.btnUcitaj.Text = "Manuelno učitavanje podataka";
            this.btnUcitaj.UseVisualStyleBackColor = true;
            this.btnUcitaj.Click += new System.EventHandler(this.btnUcitaj_Click);
            // 
            // btnAddMonitoring
            // 
            this.btnAddMonitoring.Location = new System.Drawing.Point(12, 103);
            this.btnAddMonitoring.Name = "btnAddMonitoring";
            this.btnAddMonitoring.Size = new System.Drawing.Size(160, 40);
            this.btnAddMonitoring.TabIndex = 3;
            this.btnAddMonitoring.Text = "Dodaj podatke o monitoringu";
            this.btnAddMonitoring.UseVisualStyleBackColor = true;
            this.btnAddMonitoring.Click += new System.EventHandler(this.btnAddMonitoring_Click);
            // 
            // btnGenerator
            // 
            this.btnGenerator.Font = new System.Drawing.Font("Terminator Real NFI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerator.Location = new System.Drawing.Point(139, 258);
            this.btnGenerator.Name = "btnGenerator";
            this.btnGenerator.Size = new System.Drawing.Size(259, 40);
            this.btnGenerator.TabIndex = 4;
            this.btnGenerator.Text = "Generator";
            this.btnGenerator.UseVisualStyleBackColor = true;
            this.btnGenerator.Click += new System.EventHandler(this.btnGenerator_Click);
            // 
            // btnPodesavanja
            // 
            this.btnPodesavanja.Location = new System.Drawing.Point(362, 103);
            this.btnPodesavanja.Name = "btnPodesavanja";
            this.btnPodesavanja.Size = new System.Drawing.Size(160, 40);
            this.btnPodesavanja.TabIndex = 5;
            this.btnPodesavanja.Text = "Podešavanja";
            this.btnPodesavanja.UseVisualStyleBackColor = true;
            this.btnPodesavanja.Click += new System.EventHandler(this.button1_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(362, 45);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(160, 40);
            this.button1.TabIndex = 6;
            this.button1.Text = "Edit osnovne informacije";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Terminator Real NFI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(104, 193);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(334, 37);
            this.button2.TabIndex = 7;
            this.button2.Text = "SKYNET";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // StartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(534, 310);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnUcitaj);
            this.Controls.Add(this.btnAddMonitoring);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnPodesavanja);
            this.Controls.Add(this.btnGenerator);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "StartForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Skynet";
            this.Load += new System.EventHandler(this.StartForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnUcitaj;
        private System.Windows.Forms.Button btnAddMonitoring;
        private System.Windows.Forms.Button btnGenerator;
        private System.Windows.Forms.Button btnPodesavanja;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}