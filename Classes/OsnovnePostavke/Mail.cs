using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monitoring.Classes.OsnovnePostavke
{
    class Mail
    {
        public List<string> mail;

        public Mail()
        {
            mail = new List<string>();
        }

        public Mail(List<string> mail)
        {
            this.mail = mail;
        }       
    }
}
