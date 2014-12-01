using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.Security;

namespace Monitoring
{
    public partial class frmEditParameters : Form
    {
        public frmEditParameters()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private void frmEditParameters_Load(object sender, EventArgs e)
        {
            //Monitoring server parameters
            txtHostname.Text = Properties.Settings.Default.HOST_NAME;
            txtUsername.Text = Properties.Settings.Default.USERNAME;
            txtPassword.Text = Properties.Settings.Default.PASSWORD;
            txtKey.Text = Properties.Settings.Default.SSH_HOST_KEY_FINGERPRINT;
            txtFileFormat.Text = Properties.Settings.Default.FILE_NAME_FORMAT;
            txtFilePath.Text = Properties.Settings.Default.FILE_PATH;
            txtFileExtension.Text = Properties.Settings.Default.FILE_EXTENSION;

            //Database parameters
            txtDatabaseUsername.Text = Properties.Settings.Default.DB_USERNAME;
            txtDatabasePassword.Text = Properties.Settings.Default.DB_PASSWORD;
            txtDatabaseCatalog.Text = Properties.Settings.Default.DB_CATALOG;
            txtDatabaseServer.Text = Properties.Settings.Default.DB_SERVER;

            //Mail parameters
            txtMailAddress.Text = Properties.Settings.Default.MAIL_ADDRESS;
            txtMailPassword.Text = Properties.Settings.Default.MAIL_PASSWORD;

            //Local files
            txtTemplatePath.Text = Properties.Settings.Default.FILE_TEMPLATE_PATH;
            txtTemplate2Path.Text = Properties.Settings.Default.FILE_TEMPLATE2_PATH;
            txtTemplate3Path.Text = Properties.Settings.Default.FILE_TEMPLATE3_PATH;
            txtResultDirectory.Text = Properties.Settings.Default.LOCAL_RESULT_DIRECTORY;
            txtLocalCsvDirectory.Text = Properties.Settings.Default.LOCAL_CSV_DIRECTORY;

            //interval times
            txtInterval.Text = Properties.Settings.Default.INTERVAL_INTERVAL;
            txtDownloadTime.Text = Properties.Settings.Default.INTERVAL_DOWNLOAD_TIME;            

        }

        private void btnSaveMonitoringParameters_Click(object sender, EventArgs e)
        {
            if (txtHostname.Text.Trim() == "" || txtUsername.Text.Trim() == "" || txtPassword.Text.Trim() == "" ||
                txtKey.Text.Trim() == "" || txtFileFormat.Text.Trim() == "" || txtFilePath.Text.Trim() == "" ||
                txtFileExtension.Text.Trim() == "")
            {
                MessageBox.Show("SVI PARAMETRI U OKVIRU GRUPE SU OBAVEZNI. FFS!!!", "WTFOMG");
                return;
            }

            Properties.Settings.Default.HOST_NAME = txtHostname.Text.Trim();
            Properties.Settings.Default.USERNAME = txtUsername.Text.Trim();
            Properties.Settings.Default.PASSWORD = txtPassword.Text.Trim();
            Properties.Settings.Default.SSH_HOST_KEY_FINGERPRINT = txtKey.Text.Trim();
            Properties.Settings.Default.FILE_NAME_FORMAT = txtFileFormat.Text.Trim();
            Properties.Settings.Default.FILE_PATH = txtFilePath.Text.Trim();
            Properties.Settings.Default.FILE_EXTENSION = txtFileExtension.Text.Trim();
            Properties.Settings.Default.Save();
        }

        private void btnSaveDatabaseParameters_Click(object sender, EventArgs e)
        {
            if(txtDatabaseUsername.Text.Trim() == "" || txtDatabasePassword.Text.Trim() == "" ||
                txtDatabaseCatalog.Text.Trim() == "" || txtDatabaseServer.Text.Trim() == "")
            {
                MessageBox.Show("SVI PARAMETRI U OKVIRU GRUPE SU OBAVEZNI. FFS!!!", "WTFOMG"); 
                return;
            }

            Properties.Settings.Default.DB_USERNAME = txtDatabaseUsername.Text.Trim();
            Properties.Settings.Default.DB_PASSWORD = txtDatabasePassword.Text.Trim();
            Properties.Settings.Default.DB_CATALOG = txtDatabaseCatalog.Text.Trim();
            Properties.Settings.Default.DB_SERVER = txtDatabaseServer.Text.Trim();
            Properties.Settings.Default.Save();
        }

        private void btnSaveMailParameters_Click(object sender, EventArgs e)
        {
            if(txtMailAddress.Text.Trim() == "" || txtMailPassword.Text.Trim() == "")
            {
                MessageBox.Show("SVI PARAMETRI U OKVIRU GRUPE SU OBAVEZNI. FFS!!!", "WTFOMG"); 
                return;
            }

            Properties.Settings.Default.MAIL_ADDRESS = txtMailAddress.Text.Trim();
            Properties.Settings.Default.MAIL_PASSWORD = txtMailPassword.Text.Trim();
            Properties.Settings.Default.Save();
        }

        private void btnSaveLocalFileParameters_Click(object sender, EventArgs e)
        {
            if (txtTemplatePath.Text.Trim() == "" ||
                txtResultDirectory.Text.Trim() == "" || txtLocalCsvDirectory.Text.Trim() == ""
                || txtTemplate2Path.Text.Trim() == "" || txtTemplate3Path.Text.Trim() == "")
            {
                MessageBox.Show("SVI PARAMETRI U OKVIRU GRUPE SU OBAVEZNI. FFS!!!", "WTFOMG");
                return;
            }
            Properties.Settings.Default.FILE_TEMPLATE_PATH = txtTemplatePath.Text.Trim();
            Properties.Settings.Default.FILE_TEMPLATE2_PATH = txtTemplate2Path.Text.Trim();
            Properties.Settings.Default.FILE_TEMPLATE3_PATH = txtTemplate3Path.Text.Trim();
            Properties.Settings.Default.LOCAL_RESULT_DIRECTORY = txtResultDirectory.Text.Trim();
            Properties.Settings.Default.LOCAL_CSV_DIRECTORY = txtLocalCsvDirectory.Text.Trim();           
            Properties.Settings.Default.Save();
        }

        private void btnSaveIntervalParameters_Click(object sender, EventArgs e)
        {
            if (txtInterval.Text.Trim() == "" || txtDownloadTime.Text.Trim() == "")
            {
                MessageBox.Show("SVI PARAMETRI U OKVIRU GRUPE SU OBAVEZNI. FFS!!!", "WTFOMG");
                return; 
            }
            Properties.Settings.Default.INTERVAL_INTERVAL = txtInterval.Text.Trim();
            Properties.Settings.Default.INTERVAL_DOWNLOAD_TIME = txtDownloadTime.Text.Trim();
            Properties.Settings.Default.Save();

        }

        private void btnSendTestMail_Click(object sender, EventArgs e)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(Monitoring.Classes.Rest.Parameters.smtpServer);
                mail.From = new MailAddress(txtMailAddress.Text.Trim());
                mail.To.Add("i.mentov@cube.rs");
                mail.Subject = "Test Mail " + DateTime.Now.ToString("yyyyMMdd");
                mail.Body = "Test Mail " + DateTime.Now.ToString("yyyyMMdd");

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(Properties.Settings.Default.MAIL_ADDRESS, Properties.Settings.Default.MAIL_PASSWORD);
                
                SmtpServer.EnableSsl = true;
                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                SmtpServer.Send(mail);
            }
            catch (Exception) { MessageBox.Show("Neuspesno slanje maila"); return; }
            MessageBox.Show("Poslat test mail");
        }
    }
}
