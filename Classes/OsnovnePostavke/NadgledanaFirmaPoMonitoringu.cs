using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monitoring.OsnovnePostavke
{
    class NadgledanaFirmaPoMonitoringu
    {
        public string idMonitoring;
        public List<string> maticniBrojevi;

        public NadgledanaFirmaPoMonitoringu(string idMonitoring, List<string> maticniBrojevi)
        {
            this.idMonitoring = idMonitoring;
            this.maticniBrojevi = maticniBrojevi;
        }

        public NadgledanaFirmaPoMonitoringu(string idMonitoring)
        {
            this.idMonitoring = idMonitoring;
            maticniBrojevi = new List<string>(); 
        }
    }
}
