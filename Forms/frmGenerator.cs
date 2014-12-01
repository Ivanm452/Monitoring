using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Monitoring.OsnovnePostavke;
using Monitoring.DatabaseCommunication;
using System.Data.SqlClient;
using Monitoring.Classes;
using SpreadsheetLight;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Threading;
using System.Globalization;

namespace Monitoring
{
    public partial class frmGenerator : Form
    {
        private List<Klijent> klijenti;
        private List<tblMonitoring> tblMonitorings;
        private NadgledanaFirmaPoMonitoringu nfpm;
        private Thread generisiIzvestajThread;
        private Klijent selectedKlijent;
        private tblMonitoring selectedMonitoring;

        public frmGenerator()
        {
            InitializeComponent();
            ucitajKlijenteUcmb();
            ucitajMonitoringUcmb();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private void ucitajKlijenteUcmb()
        {
            ucitajKlijenteIzBaze();
            cmbKlijenti.Items.Clear();            

            if (klijenti != null && klijenti.Count >= 1)
            {
                foreach (Klijent k in klijenti)
                    cmbKlijenti.Items.Add(k);
                cmbKlijenti.SelectedIndex = 0;
            }
        }

        private void ucitajKlijenteIzBaze()
        {
            DBCommOsnovnePostavke.connection = new SqlConnection(DBBlokadeData.connectionString);
            DBCommOsnovnePostavke.connection.Open();
            klijenti = DBCommOsnovnePostavke.getKlijenti();
            DBCommOsnovnePostavke.connection.Close(); 
        }

        private void ucitajMonitoringUcmb()
        {
            cmbMonitoring.Items.Clear();

            if (klijenti != null && klijenti.Count >= 1)
            {
                ucitajMonitoringIzBaze(((Klijent)cmbKlijenti.SelectedItem).idKlijent);

                foreach (tblMonitoring tblm in tblMonitorings)
                    cmbMonitoring.Items.Add(tblm);
                if (tblMonitorings.Count >= 1)
                {
                    cmbMonitoring.SelectedIndex = 0;
                    ucitajMaticneBrojeveUlstbox();
                }
            }
        }

        private void ucitajMonitoringIzBaze(String idKlijent)
        {
            DBCommOsnovnePostavke.connection = new SqlConnection(DBBlokadeData.connectionString);
            DBCommOsnovnePostavke.connection.Open();
            tblMonitorings = DBCommOsnovnePostavke.getMonitoring(idKlijent);
            DBCommOsnovnePostavke.connection.Close(); 
        }

        private void ucitajMaticneBrojeveUlstbox()
        {
            lstBoxMaticni.Items.Clear();

            if (tblMonitorings != null && tblMonitorings.Count >= 1)
            {
                ucitajMaticneBrojeve(((tblMonitoring)cmbMonitoring.SelectedItem).idMonitoring);

                if (nfpm != null || nfpm.maticniBrojevi.Count >= 1)
                    foreach (String s in nfpm.maticniBrojevi)
                        lstBoxMaticni.Items.Add(s);
            }
        }

        private void ucitajMaticneBrojeve(String idMonitoring)
        {
            DBCommOsnovnePostavke.connection = new SqlConnection(DBBlokadeData.connectionString);
            DBCommOsnovnePostavke.connection.Open();
            nfpm = DBCommOsnovnePostavke.getNadgledaneFirme(idMonitoring);
            DBCommOsnovnePostavke.connection.Close(); 
        }
        
        private void cmbKlijenti_SelectedIndexChanged(object sender, EventArgs e)
        {
            ucitajMonitoringUcmb();
        }

        private void cmbMonitoring_SelectedIndexChanged(object sender, EventArgs e)
        {
            ucitajMaticneBrojeveUlstbox();
        }

             
        private void btnGenerisiIzabrani_Click(object sender, EventArgs e)
        {
            selectedKlijent = (Klijent)cmbKlijenti.SelectedItem;
            selectedMonitoring = (tblMonitoring)cmbMonitoring.SelectedItem;
            selectedMonitoring.lstMaticniBrojevi = nfpm.maticniBrojevi;
            generisiIzvestajThread = new Thread(new ThreadStart(generisi));
            generisiIzvestajThread.Start();
        }

        private void generisi()
        {
            UcitavanjeGenerisanje.generisiIzvestaj(selectedKlijent, selectedMonitoring, txtConsole, txtConsole);
            selectedMonitoring = null;
            selectedKlijent = null;
        }
    }
}
