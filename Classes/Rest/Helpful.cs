using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Monitoring.Classes
{
    class Helpful
    {

        public static string openFileDialog()
        {
            OpenFileDialog fDialog = new OpenFileDialog();

            if (fDialog.ShowDialog() == DialogResult.OK)
                return fDialog.FileName.ToString().Trim();
            return "";
        }
    }
}
