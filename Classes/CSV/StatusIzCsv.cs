using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsvHelper;

namespace Monitoring.Classes.CSV
{
    class StatusIzCsv
    {
        public string maticniBroj;
        public string status;

        public StatusIzCsv()
        {
            maticniBroj = status = null;
        }

        public StatusIzCsv(CsvReader csv)
        {
            maticniBroj = csv[0].Trim();
            status = csv[1].Trim();
        }
    }
}
