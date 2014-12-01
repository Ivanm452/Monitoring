using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Monitoring.Classes
{
    class ActuallyPerformStep
    {
        delegate void CallPerformStepTxtBox(TextBox txtBox, String text, bool time);
        public static void performStepTxtBox(TextBox txtBox, String text, bool time)
        {
            if (txtBox.InvokeRequired)
            {
                CallPerformStepTxtBox del = performStepTxtBox;
                txtBox.Invoke(del, new object[] { txtBox, text, time });
                return;
            }

            if(time)
                txtBox.Text += DateTime.Now.ToString("dd.MM HH:mm:ss - ") + text + Environment.NewLine;
            else
                txtBox.Text = text + Environment.NewLine;
            txtBox.SelectionStart = txtBox.Text.Length;
            txtBox.ScrollToCaret();

        }
    }
}
