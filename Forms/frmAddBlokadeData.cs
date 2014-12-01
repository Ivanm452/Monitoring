using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Monitoring.Classes;
using SpreadsheetLight;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using Monitoring.DatabaseCommunication;
using System.Data.SqlClient;
using CsvHelper;
using System.Threading;
using System.Reflection;

namespace Monitoring
{
    public partial class frmAddBlokadeData : Form
    {
        Thread uploadSourceThread, generisiThread;

        public frmAddBlokadeData()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        // dugme za upload source podataka. Vraca se ako putanja do fajla nije uneta.
        private void btnUploadSource_Click(object sender, EventArgs e)
        {
            if (txtSourceFilePath.Text.Trim() == "")
                return;
            uploadSourceThread = new Thread(new ThreadStart(uploadSource));
            uploadSourceThread.Start();
        }

        public void uploadSource()
        {
            UcitavanjeGenerisanje.uploadSource(txtConsole, txtConsole, txtSourceFilePath.Text);               
        }

        private void btn_Generisi_Click(object sender, EventArgs e)
        {
            generisiThread = new Thread(new ThreadStart(generisi));
            generisiThread.Start();
        }           

        public void generisi()
        {
            UcitavanjeGenerisanje.generisiRezultat(txtConsole, txtConsole);            
        }            

        private void btnNadjiFajl_Click(object sender, EventArgs e)
        {
            txtSourceFilePath.Text = Helpful.openFileDialog();
        }           
    }
}