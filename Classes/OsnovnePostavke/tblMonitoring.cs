using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monitoring.OsnovnePostavke
{
    class tblMonitoring
    {
        public string idMonitoring, idKlijent, idVrstaMonitoringa, idTipFajla, naziv;
        public List<string> lstMail;
        public List<string> lstMaticniBrojevi;
        public string saljeSeMail;

        public tblMonitoring(string idMonitoring, string idKlijent, string idVrstaMonitoringa,
            string idTipFajla, string naziv, string saljeSeMail)
        {
            this.idMonitoring = idMonitoring;
            this.idKlijent = idKlijent;
            this.idVrstaMonitoringa = idVrstaMonitoringa;
            this.idTipFajla = idTipFajla;
            this.naziv = naziv;
            this.saljeSeMail = saljeSeMail;
        }

        public tblMonitoring() { idMonitoring = idKlijent = idVrstaMonitoringa = idTipFajla = naziv = null; lstMail = new List<string>(); lstMaticniBrojevi = new List<string>(); }

        public override string ToString()
        {
            return naziv;
        }
    }
}
