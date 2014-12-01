using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Monitoring.DatabaseCommunication;
using System.Data.SqlClient;
using WinSCP;
using Monitoring.Classes;
using System.IO;
using CsvHelper;
using Monitoring.OsnovnePostavke;
using SpreadsheetLight;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Monitoring.Classes.DatabaseCommunication;
using Monitoring.Classes.OsnovnePostavke;
using Monitoring.Classes.SadistaCommunication;

namespace Monitoring.Forms
{
    // testGITs
    //TESTGIT2
    //TestGit3
    public partial class frmSkynet : Form
    {        
        private static frmSkynet inst;
        public static frmSkynet GetForm
        {
            get
            {
                if (inst == null || inst.IsDisposed)
                    inst = new frmSkynet();
                return inst; 
            }
        }

        public StartForm parent;

        public Thread skynetThread;
        public bool generatedToday = false;

        public frmSkynet()
        {
            InitializeComponent();
        }

        public void skynetWork()
        {
            ActuallyPerformStep.performStepTxtBox(txtConsole, "Skynet started", true);
              
            while (true)
            {
                if (!checkDatabaseStatus())
                    return;
                if (!checkServerStatus())
                    return;
                
                if (DateTime.Now.ToString("HH").Equals("00"))
                    generatedToday = false;
                else if (DateTime.Now.ToString("HH").Equals(Properties.Settings.Default.INTERVAL_DOWNLOAD_TIME) && generatedToday == false)
                {
                    doYourShit();
                    generatedToday = true;
                }

                ActuallyPerformStep.performStepTxtBox(txtConsole, "Skynet spava: " + Properties.Settings.Default.INTERVAL_INTERVAL + " minuta. Do: " + Properties.Settings.Default.INTERVAL_DOWNLOAD_TIME + "h.", true);
                Thread.Sleep(new TimeSpan(0, int.Parse(Properties.Settings.Default.INTERVAL_INTERVAL), 0));
            }
        }

        private void frmSkynet_Load(object sender, EventArgs e)
        {
            if (skynetThread == null || skynetThread.IsAlive == false)
            {
                skynetThread = new Thread(new ThreadStart(skynetWork));
                skynetThread.Name = "Skynet thread";
                skynetThread.Start();
            }
        }

        private void doYourShit()
        {            
            // provera i preuzimanje fajla
            SadistaImplementation.doYourThing();
            SadistaImplementation.doYourThingStatus();
               
            // Upload u bazu
            uploadSource();
            uploadStatus();
            
            // Generisanje u bazi
            generisi();  
            
            // Generisanje excela
            List<FileAndMails> listFam = generisanjeExcela();
          
            // slanje maila
           slanjeMaila(listFam); 
            
            // slanje internog mail-a
            slanjeTamariMaila();
        }

        private bool preuzimanjeFajlaSaServera()
        {
            ActuallyPerformStep.performStepTxtBox(txtConsole, "Pocinje preuzimanje fajla...", true);
            bool uspehFajl = WinScp.GetFile.getFile();
            if (uspehFajl)
            {
                ActuallyPerformStep.performStepTxtBox(txtConsole, "Uspesno preuzet fajl.", true);
                return true;
            }
            else
            {
                ActuallyPerformStep.performStepTxtBox(txtConsole, "GRESKA: Neuspesno preuzet fajl!", true);
                return false;
            }
        }

        private void uploadSource()
        {
            String filePath = Properties.Settings.Default.LOCAL_CSV_DIRECTORY + "\\" +
                 DateTime.Now.ToString(Properties.Settings.Default.FILE_NAME_FORMAT) +
                 Properties.Settings.Default.FILE_EXTENSION;
            UcitavanjeGenerisanje.uploadSource(txtConsole, txtConsole2, filePath);
        }

        private void uploadStatus()
        {
            String filePath = Properties.Settings.Default.LOCAL_CSV_DIRECTORY + "\\" +
                 "status_"+DateTime.Now.ToString(Properties.Settings.Default.FILE_NAME_FORMAT) +
                 Properties.Settings.Default.FILE_EXTENSION;
            UcitavanjeGenerisanje.uploadStatus(txtConsole, txtConsole2, filePath);

 
        }

        public void generisi()
        {
            UcitavanjeGenerisanje.generisiRezultat(txtConsole, txtConsole2);
        }

        private List<FileAndMails> generisanjeExcela()
        {
            DBZaGenerisanje.connection = new SqlConnection(DBZaGenerisanje.connectionString);
            DBZaGenerisanje.connection.Open();
            List<ZaGenerisanje> lstGen = DBZaGenerisanje.getListZaGenerisanje();
            DBZaGenerisanje.connection.Close();
            List<FileAndMails> listFam = new List<FileAndMails>();
            FileAndMails fam;
            foreach (ZaGenerisanje zg in lstGen)
            {
                
                foreach (tblMonitoring m in zg.monitoring)
                {
                   
                    fam = UcitavanjeGenerisanje.generisiIzvestaj(zg.klijent, m, txtConsole, txtConsole2);

                    if (fam != null)
                    {
                        fam.klijentNaziv = zg.klijent.naziv;
                        listFam.Add(fam);
                    }
                    else
                        DBGreska.addGreska(zg.klijent.maticniBroj, "List<FileAndMails> generisanjeExcela()", "fam == null");
                }                
            }
            return listFam;
        }

        private void slanjeMaila(List<FileAndMails> listFam)
        {
            bool nekiNijePoslat = false;
            int pauza = 60000;
            int i = 1;
            ActuallyPerformStep.performStepTxtBox(txtConsole, "Pocinje slanje mailova...", true);

            int j = 1;
            while (i <= 3)
            {
                foreach (FileAndMails fam in listFam)
                {                    
                    if (fam.poslato == false)
                    {
                        fam.poslato = UcitavanjeGenerisanje.email_send(fam.klijentNaziv, fam.filePath, fam.mails, fam.potencijalnoNeaktivna, fam.maticniNePostoji);
                        if (fam.poslato == false)
                            nekiNijePoslat = true;
                        else
                            ActuallyPerformStep.performStepTxtBox(txtConsole2, "Poslat mail: " + j++ + "/"+listFam.Count, false);
                    }
                }
                j = 0;
                if (nekiNijePoslat)
                {
                    ActuallyPerformStep.performStepTxtBox(txtConsole, "Neki mail nije poslat, spava: " + (i * pauza), true);
                    Thread.Sleep(i * pauza);
                    i++;
                    nekiNijePoslat = false;
                }
                else
                    i = 5;
            }

            if (nekiNijePoslat)
                ActuallyPerformStep.performStepTxtBox(txtConsole, "Neki mail nije poslat", true);

            ActuallyPerformStep.performStepTxtBox(txtConsole, "Zavrseno slanje mailova.", true);    
        }

        private void slanjeTamariMaila()
        {
            ActuallyPerformStep.performStepTxtBox(txtConsole, "Pocinje slanje potencijalno neaktivnih...", true);
            
            // slanje tamari informaciju o potencijalno neaktivnim firmama
            string text = DBZaGenerisanje.getPotencijalnoNeaktivne();

            if (!UcitavanjeGenerisanje.email_sendTamara(text))
            {
                Thread.Sleep(2 * 60 * 1000);
                if (!UcitavanjeGenerisanje.email_sendTamara(text))
                    ActuallyPerformStep.performStepTxtBox(txtConsole, "Tamari nije poslat mail", true);
            }
            ActuallyPerformStep.performStepTxtBox(txtConsole, "Zavrseno slanje potencijalno neaktivnih.", true);
        }

        private bool checkDatabaseStatus()
        {
            ActuallyPerformStep.performStepTxtBox(txtConsole, "Provera statusa baze...", true);
            DBCommOsnovnePostavke.connection = new SqlConnection(DBBlokadeData.connectionString);
            try
            {
                DBCommOsnovnePostavke.connection.Open();
                DBCommOsnovnePostavke.connection.Close();
            }
            catch (SqlException ex)
            {
                ActuallyPerformStep.performStepTxtBox(txtConsole, "Database error! " + ex.ToString(), true);
                Thread.Sleep(2 * 60 * 1000);
                return checkDatabaseStatus();
            }
            ActuallyPerformStep.performStepTxtBox(txtConsole, "Database OK!", true);
            return true;
        }

        private bool checkServerStatus()
        {
            ActuallyPerformStep.performStepTxtBox(txtConsole, "Provera statusa servera...", true);
            bool serverStatus = WinScp.GetFile.checkServerStatus();
            if (serverStatus)
            {
                ActuallyPerformStep.performStepTxtBox(txtConsole, "Server status: OK.", true);
                return true;
            }
            else
            {
                ActuallyPerformStep.performStepTxtBox(txtConsole, "Server status: ERROR", true);
                Thread.Sleep(2 * 60 * 1000);
                return checkDatabaseStatus();
            }
        }
        
        private void frmSkynet_FormClosing(object sender, FormClosingEventArgs e)
        {
            ActuallyPerformStep.performStepTxtBox(txtConsole, "Stopiranje servisa", true);
            skynetThread.Abort();
        }
    }
}
