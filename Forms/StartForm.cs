using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Monitoring.Classes;
using Monitoring.WinScp;
using Monitoring.Forms;
using Monitoring.DatabaseCommunication;
using System.Data.SqlClient;
using Monitoring.Classes.SadistaCommunication;

namespace Monitoring
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private void btnUcitaj_Click(object sender, EventArgs e)
        {
            frmAddBlokadeData f1 = new frmAddBlokadeData();
            f1.ShowDialog();
        }

        private void btnAddMonitoring_Click(object sender, EventArgs e)
        {
            frmAddMonitoring fam = new frmAddMonitoring();
            fam.ShowDialog();
        }

        private void btnGenerator_Click(object sender, EventArgs e)
        {
            frmGenerator generator = new frmGenerator();
            generator.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmEditParameters fep = new frmEditParameters();
            fep.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            frmEditOsnovne frmOsnovne = new frmEditOsnovne();
            frmOsnovne.ShowDialog();
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            frmSkynet frm = frmSkynet.GetForm;
            frm.ShowDialog();
            frm.parent = this;
        }

        private void StartForm_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            SadistaImplementation.doYourThing();
        }        
    }
}
