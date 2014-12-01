using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monitoring.OsnovnePostavke
{
    class VrstaMonitoringa
    {
        public string idVrstaMonitoringa, naziv;

        public VrstaMonitoringa(string idVrstaMonitoringa, string naziv)
        {
            this.idVrstaMonitoringa = idVrstaMonitoringa;
            this.naziv = naziv;
        }

        public override string ToString()
        {
            return this.naziv;
        }
    }
}
