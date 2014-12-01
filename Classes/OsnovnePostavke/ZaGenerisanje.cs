using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monitoring.OsnovnePostavke;

namespace Monitoring.Classes.OsnovnePostavke
{
    class ZaGenerisanje
    {
        public Klijent klijent;
        public List<tblMonitoring> monitoring;

        public ZaGenerisanje()
        {
            monitoring = new List<tblMonitoring>(); 
        }
    }
}
