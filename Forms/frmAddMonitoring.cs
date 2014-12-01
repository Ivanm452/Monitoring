using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Monitoring.DatabaseCommunication;
using Monitoring.OsnovnePostavke;
using System.IO;
using System.Threading;
using Monitoring.Classes;

namespace Monitoring
{
    public partial class frmAddMonitoring : Form
    {
        private List<Klijent> klijenti;
        private List<tblMonitoring> tblMonitorings;
        public frmAddMonitoring()
        {
            InitializeComponent();
            ucitajKlijente(0);
            ucitajMonitoring();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        public void ucitajKlijente(int index)
        {
            cmbKlijenti.Items.Clear();
            DBCommOsnovnePostavke.connection = new SqlConnection(DBBlokadeData.connectionString);
            DBCommOsnovnePostavke.connection.Open();
            klijenti = DBCommOsnovnePostavke.getKlijenti();
            DBCommOsnovnePostavke.connection.Close();

            if (klijenti != null && klijenti.Count >= 1)
            {
                foreach (Klijent k in klijenti)
                    cmbKlijenti.Items.Add(k);
                if (index == 0)
                    cmbKlijenti.SelectedIndex = 0;
                else
                    cmbKlijenti.SelectedIndex = klijenti.Count - 1;
            }
        }

        public void ucitajMonitoring()
        {
            lstMonitorings.Items.Clear();

            if (klijenti != null && klijenti.Count >= 1)
            {
                DBCommOsnovnePostavke.connection = new SqlConnection(DBBlokadeData.connectionString);
                DBCommOsnovnePostavke.connection.Open();
                tblMonitorings = DBCommOsnovnePostavke.getMonitoring(((Klijent)cmbKlijenti.SelectedItem).idKlijent);
                DBCommOsnovnePostavke.connection.Close();

                foreach (tblMonitoring tblm in tblMonitorings)
                    lstMonitorings.Items.Add(tblm);
            }
        }       

        private void btnDodajKlijenta_Click(object sender, EventArgs e)
        {
            DBCommOsnovnePostavke.connection = new SqlConnection(DBBlokadeData.connectionString);
            DBCommOsnovnePostavke.connection.Open();
            DBCommOsnovnePostavke.dodajKlijenta(txtMaticniBroj.Text, txtNaziv.Text);
            DBCommOsnovnePostavke.connection.Close();
            txtMaticniBroj.Clear();
            txtNaziv.Clear();
            ucitajKlijente(1);
        }

        private void btnAddMonitoring_Click(object sender, EventArgs e)
        {
            frmAddOsnovne f3 = new frmAddOsnovne(cmbKlijenti.SelectedItem, false, this);
            f3.ShowDialog();
        }

        private void cmbKlijenti_SelectedIndexChanged(object sender, EventArgs e)
        {
            ucitajMonitoring();
        }

        private void btnEditMonitoring_Click(object sender, EventArgs e)
        {
            if (lstMonitorings.SelectedIndex >= 0)
            {
                frmAddOsnovne f3 = new frmAddOsnovne(cmbKlijenti.SelectedItem, true, lstMonitorings.SelectedItem,this);
                f3.ShowDialog();
            }
        }

        private void btnObrisiMonitoring_Click(object sender, EventArgs e)
        {
            tblMonitoring monitoring = (tblMonitoring)lstMonitorings.SelectedItem;
            DBCommOsnovnePostavke.connection = new SqlConnection(DBBlokadeData.connectionString);
            DBCommOsnovnePostavke.connection.Open();
            DBCommOsnovnePostavke.obrisiMonitoring(monitoring);            
            DBCommOsnovnePostavke.connection.Close();
            ucitajMonitoring();
        }

        private void btnObrisiKlijenta_Click(object sender, EventArgs e)
        {
            Klijent klijent = (Klijent)cmbKlijenti.SelectedItem;
            if (klijent == null)
                return;

            DBCommOsnovnePostavke.connection = new SqlConnection(DBBlokadeData.connectionString);
            DBCommOsnovnePostavke.connection.Open();
            DBCommOsnovnePostavke.obrisiKlijenta(klijent);            
            DBCommOsnovnePostavke.connection.Close();
            ucitajKlijente(0);
        }

        private void btnAddPath_Click(object sender, EventArgs e)
        {
            StreamReader sr;
            CsvHelper.CsvReader csvread;

            sr = new StreamReader(txtFilePath.Text);
            csvread = new CsvHelper.CsvReader(sr);

            KlijentIzCsv kicsv = new KlijentIzCsv();
            csvread.Configuration.HasHeaderRecord = false;
            int i = 1;
            while (csvread.Read())
            {
                if (i == 1)
                {
                    kicsv.klijent = csvread[4].Trim();
                    kicsv.nazivMonitoringa = csvread[5].Trim();
                    
                }
                Console.WriteLine(csvread[0]);
                if(csvread[0].Trim()!="")
                    kicsv.nicsvList.Add(new NadgledanaIzCsv(csvread[0].Trim(), csvread[1].Trim(), csvread[2].Trim(), csvread[3].Trim()));
                i++;
            }
            DBCommOsnovnePostavke.connection = new SqlConnection(DBBlokadeData.connectionString);
            DBCommOsnovnePostavke.connection.Open();
            DBCommOsnovnePostavke.addKlijentIzCsv(kicsv);
            DBCommOsnovnePostavke.connection.Close();
            txtMaticniBroj.Clear();
            txtNaziv.Clear();
            txtFilePath.Clear();
            ucitajKlijente(1);
        }

        private void btnFindFile_Click(object sender, EventArgs e)
        {
            txtFilePath.Text = "";
            OpenFileDialog fDialog = new OpenFileDialog();

            if (fDialog.ShowDialog() == DialogResult.OK)
                txtFilePath.Text = fDialog.FileName.ToString();
        }
    }
}
