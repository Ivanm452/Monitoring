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
using System.IO;
using System.Threading;
using Monitoring.Classes.OsnovnePostavke;

namespace Monitoring
{
    public partial class frmAddOsnovne : Form
    {
        private bool edit = false;
        private List<VrstaMonitoringa> vrstaMonitoringas;
        private List<TipFajla> tipFajlas;
        private Mail mailList;
        private Klijent klijent;
        private tblMonitoring monitoring;
        private NadgledanaFirmaPoMonitoringu nfpm;
        private frmAddMonitoring parent;

        public frmAddOsnovne(Object klijent, bool edit, frmAddMonitoring parent)
        {
            InitializeComponent();

            this.edit = edit;
            this.klijent = (Klijent)klijent;
            lblKlijent.Text = this.klijent.naziv;

            ucitajVrstuMonitoringa();
            ucitajTipFajla();
            btnDodajOsnovne.Text = "Dodaj osnovne";

            this.parent = parent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            chkSaljeSeMail.Checked = true;
            chkSaljeSeMail.Enabled = false;            
        }

        public frmAddOsnovne(Object klijent, bool edit, Object monitoring, frmAddMonitoring parent)
        {
            InitializeComponent();
            this.monitoring = (tblMonitoring)monitoring;

            this.edit = edit;
            this.klijent = (Klijent)klijent;
            lblKlijent.Text = this.klijent.naziv;

            ucitajVrstuMonitoringa();
            ucitajTipFajla();

            txtNaziv.Text = this.monitoring.naziv;

            ucitajMaticneBrojeve();
            ucitajMailove();

            btnDodajOsnovne.Text = "Snimi izmene";

            if (edit == true)
                grpMail.Enabled = grpNadgledaneFirme.Enabled = true;
            else
                grpMail.Enabled = grpNadgledaneFirme.Enabled = false;
            

            this.parent = parent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            if (this.monitoring.saljeSeMail == "1")
                chkSaljeSeMail.Checked = true;
            else
                chkSaljeSeMail.Checked = false;
 
        }

        private void ucitajMailove()
        {
            lstBoxMail.Items.Clear();
            DBCommOsnovnePostavke.connection = new SqlConnection(DBBlokadeData.connectionString);
            DBCommOsnovnePostavke.connection.Open();
            mailList = new Mail(DBCommOsnovnePostavke.getMailove(monitoring.idMonitoring));
            DBCommOsnovnePostavke.connection.Close();

            foreach(String s in mailList.mail)
                lstBoxMail.Items.Add(s);

            Console.WriteLine("UcitaniMAILOVI"); 
        }

        private void ucitajVrstuMonitoringa()
        {
            DBCommOsnovnePostavke.connection = new SqlConnection(DBBlokadeData.connectionString);
            DBCommOsnovnePostavke.connection.Open();
            vrstaMonitoringas = DBCommOsnovnePostavke.getVrstaMonitoringa();
            DBCommOsnovnePostavke.connection.Close();

            int i = 0;
            if (vrstaMonitoringas != null || vrstaMonitoringas.Count >= 1)
            {

                foreach (VrstaMonitoringa vm in vrstaMonitoringas)
                {
                    cmbVrstaMonitoringa.Items.Add(vm);
                    if (edit)
                        if (!vm.idVrstaMonitoringa.Equals(monitoring.idVrstaMonitoringa))
                            i++;
                }
                if (!edit)
                    cmbVrstaMonitoringa.SelectedIndex = 0;
                else
                    cmbVrstaMonitoringa.SelectedIndex = i;
            }
        }

        private void ucitajMaticneBrojeve()
        {
            lstBoxMaticni.Items.Clear();
            DBCommOsnovnePostavke.connection = new SqlConnection(DBBlokadeData.connectionString);
            DBCommOsnovnePostavke.connection.Open();
            nfpm = DBCommOsnovnePostavke.getNadgledaneFirme(monitoring.idMonitoring);
            DBCommOsnovnePostavke.connection.Close();

            if (nfpm != null || nfpm.maticniBrojevi.Count >= 1)
            {
                foreach (String s in nfpm.maticniBrojevi)
                    lstBoxMaticni.Items.Add(s);
            }
        }       

        private void ucitajTipFajla()
        {
            DBCommOsnovnePostavke.connection = new SqlConnection(DBBlokadeData.connectionString);
            DBCommOsnovnePostavke.connection.Open();
            tipFajlas = DBCommOsnovnePostavke.getTipFajla();
            DBCommOsnovnePostavke.connection.Close();

            int i = 0;
            if (tipFajlas != null || tipFajlas.Count >= 1)
            {
                
                foreach (TipFajla tf in tipFajlas)
                {
                    cmbTipFajla.Items.Add(tf);
                    if (edit)
                        if (!tf.idTipFajla.Equals(monitoring.idTipFajla))
                            i++;
                }
                if (!edit)
                    cmbTipFajla.SelectedIndex = 0;
                else
                {
                    cmbTipFajla.SelectedIndex = i;
 
                }
            } 
        }

        private void btnDodajOsnovne_Click(object sender, EventArgs e)
        {
            if (txtNaziv.Text.Trim() == "")
                return;
            if (!edit)
            {
                DBCommOsnovnePostavke.connection = new SqlConnection(DBBlokadeData.connectionString);
                DBCommOsnovnePostavke.connection.Open();
                DBCommOsnovnePostavke.insertOsnovne(klijent.idKlijent,
                    ((VrstaMonitoringa)cmbVrstaMonitoringa.SelectedItem).idVrstaMonitoringa,
                    ((TipFajla)cmbTipFajla.SelectedItem).idTipFajla, txtNaziv.Text);
                 DBCommOsnovnePostavke.connection.Close();
            }
            else
            {
                DBCommOsnovnePostavke.connection = new SqlConnection(DBBlokadeData.connectionString);
                DBCommOsnovnePostavke.connection.Open();
                DBCommOsnovnePostavke.updateOsnovne(monitoring.idMonitoring,
                    ((VrstaMonitoringa)cmbVrstaMonitoringa.SelectedItem).idVrstaMonitoringa,
                    ((TipFajla)cmbTipFajla.SelectedItem).idTipFajla, txtNaziv.Text, txtMail.Text);
                DBCommOsnovnePostavke.connection.Close();                
            }

            DBCommOsnovnePostavke.connection = new SqlConnection(DBBlokadeData.connectionString);
            DBCommOsnovnePostavke.connection.Open();
            monitoring = DBCommOsnovnePostavke.getSingleMonitoring(klijent.idKlijent, txtNaziv.Text);
            DBCommOsnovnePostavke.connection.Close();

            grpMail.Enabled = grpNadgledaneFirme.Enabled = true;

            parent.ucitajKlijente(parent.cmbKlijenti.SelectedIndex);
            parent.ucitajMonitoring();
 
        }

        private void btnAddMaticni_Click(object sender, EventArgs e)
        {
            if (txtMaticniBroj.Text.Trim() == "" || txtMaticniBroj.Text.Trim().Length != 8)
            {
                MessageBox.Show("...");
                return;
            }
            DBCommOsnovnePostavke.connection = new SqlConnection(DBBlokadeData.connectionString);
            DBCommOsnovnePostavke.connection.Open();

            DBCommOsnovnePostavke.addMaticniBrojUNadgledane(monitoring.idMonitoring, txtMaticniBroj.Text.Trim());
            DBCommOsnovnePostavke.connection.Close();
            ucitajMaticneBrojeve();
        }

        private void btnObrisiMaticni_Click(object sender, EventArgs e)
        {
            DBCommOsnovnePostavke.connection = new SqlConnection(DBBlokadeData.connectionString);
            DBCommOsnovnePostavke.connection.Open();
            DBCommOsnovnePostavke.obrisiMaticniIzMonitoringa(monitoring.idMonitoring,lstBoxMaticni.SelectedItem.ToString());
            DBCommOsnovnePostavke.connection.Close();
            ucitajMaticneBrojeve();
        }



        private void btnAddMaticniFile_Click(object sender, EventArgs e)
        {
            int i = 1;
            DBCommOsnovnePostavke.connection = new SqlConnection(DBBlokadeData.connectionString);
            DBCommOsnovnePostavke.connection.Open();
            string[] ss = File.ReadAllText(txtFilePath.Text.Trim()).Split('\n');
            foreach (string s in ss)
            {
                if (s.Trim() != "" && s.Trim().Length == 8)
                {
                    DBCommOsnovnePostavke.addMaticniBrojUNadgledane(monitoring.idMonitoring, s.Trim());
                    txtConsole.Text = "Ubacen maticni: " + i++ + "/" + ss.Length;
                }
            }
            DBCommOsnovnePostavke.connection.Close();   
        }

        private void frmAddOsnovne_Load(object sender, EventArgs e)
        {

        }

        private void btnNadjiFajl_Click(object sender, EventArgs e)
        {
            txtFilePath.Text = "";
            OpenFileDialog fDialog = new OpenFileDialog();

            if (fDialog.ShowDialog() == DialogResult.OK)
                txtFilePath.Text = fDialog.FileName.ToString();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnAddMail_Click(object sender, EventArgs e)
        {
            if (txtMail.Text.Trim() == "")
            {
                MessageBox.Show("NISI UNEO MAIL OMG!!!");
                return;
            }
            DBCommOsnovnePostavke.connection = new SqlConnection(DBBlokadeData.connectionString);
            DBCommOsnovnePostavke.connection.Open();
            DBCommOsnovnePostavke.dodajMail(monitoring.idMonitoring, txtMail.Text.Trim());
            DBCommOsnovnePostavke.connection.Close();
            ucitajMailove();
        }

        private void btnDeleteMail_Click(object sender, EventArgs e)
        {
            DBCommOsnovnePostavke.connection = new SqlConnection(DBBlokadeData.connectionString);
            DBCommOsnovnePostavke.connection.Open();
            DBCommOsnovnePostavke.obrisiMail(monitoring.idMonitoring, lstBoxMail.SelectedItem.ToString());
            DBCommOsnovnePostavke.connection.Close();
            ucitajMailove();
        }

        private void chkSaljeSeMail_CheckedChanged(object sender, EventArgs e)
        {
            if (this.edit)
            {
                if (chkSaljeSeMail.Checked)
                {
                    DBCommOsnovnePostavke.connection = new SqlConnection(DBBlokadeData.connectionString);
                    DBCommOsnovnePostavke.connection.Open();
                    DBCommOsnovnePostavke.saljeSeMail(monitoring.idMonitoring, 1);
                    DBCommOsnovnePostavke.connection.Close();
                }
                else
                {
                    DBCommOsnovnePostavke.connection = new SqlConnection(DBBlokadeData.connectionString);
                    DBCommOsnovnePostavke.connection.Open();
                    DBCommOsnovnePostavke.saljeSeMail(monitoring.idMonitoring, 0);
                    DBCommOsnovnePostavke.connection.Close();
                }
            }
        }        
    }
}
