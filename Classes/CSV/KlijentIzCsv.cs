using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsvHelper;

namespace Monitoring.Classes
{
    class KlijentIzCsv
    {
        public string klijent;
        public string nazivMonitoringa;

        public List<NadgledanaIzCsv> nicsvList;

        public KlijentIzCsv() { klijent = nazivMonitoringa = null; nicsvList = new List<NadgledanaIzCsv>(); }
               
    }
}
