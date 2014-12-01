using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Monitoring.Classes.OsnovnePostavke;
using Monitoring.DatabaseCommunication;
using System.Data.SqlClient;

namespace Monitoring.Forms
{
    public partial class frmEditOsnovne : Form
    {
        OsnovneInformacije osnovneInformacije;
        public frmEditOsnovne()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private void btnTrazi_Click(object sender, EventArgs e)
        {
            if (txtMaticniBroj.Text.Trim().Length < 8)
            {
                MessageBox.Show("Neispravan matični broj!", "Greška");
                this.osnovneInformacije = null;
            }
            else
            {
                DBCommOsnovnePostavke.connection = new SqlConnection(DBBlokadeData.connectionString);
                DBCommOsnovnePostavke.connection.Open();
                this.osnovneInformacije = DBCommOsnovnePostavke.getOsnovneInformacije(txtMaticniBroj.Text.Trim());
                DBCommOsnovnePostavke.connection.Close();
                if (this.osnovneInformacije == null)
                {
                    MessageBox.Show("Maticni broj nije nadjen", "Greška");
                    this.osnovneInformacije = null;
                }
                else
                {
                    txtNaziv.Text = this.osnovneInformacije.naziv;
                    txtAdresa.Text = this.osnovneInformacije.adresa;
                    txtPib.Text = this.osnovneInformacije.pib;
                    txtGrad.Text = this.osnovneInformacije.grad;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.osnovneInformacije == null)
                MessageBox.Show("Maticni nije izabran", "Greska");
            else
            {
                DBCommOsnovnePostavke.connection = new SqlConnection(DBBlokadeData.connectionString);
                DBCommOsnovnePostavke.connection.Open();
                this.osnovneInformacije.naziv = txtNaziv.Text.Trim();
                this.osnovneInformacije.adresa = txtAdresa.Text.Trim();
                this.osnovneInformacije.pib = txtPib.Text.Trim();
                this.osnovneInformacije.grad = txtGrad.Text.Trim();
                DBCommOsnovnePostavke.updateOsnovneInformacije(this.osnovneInformacije);
                DBCommOsnovnePostavke.connection.Close();
                MessageBox.Show("Uspesno sacuvano");
                txtNaziv.Clear();
                txtAdresa.Clear();
                txtPib.Clear();
                txtGrad.Clear();
                this.osnovneInformacije = null;
            }
        }
    }
}
