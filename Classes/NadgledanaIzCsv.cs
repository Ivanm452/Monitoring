using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monitoring.Classes
{
    class NadgledanaIzCsv
    {
        public string maticniBroj;
        public string naziv;
        public string adresa;
        public string mesto;

        public NadgledanaIzCsv()
        {
            maticniBroj = naziv = adresa = mesto = null;
        }

        public NadgledanaIzCsv(string maticniBroj, string naziv, string adresa, string mesto)
        {
            this.maticniBroj = maticniBroj;
            this.naziv = naziv;
            this.adresa = adresa;
            this.mesto = mesto;
        }

        public override string ToString()
        {
            return maticniBroj + "   " + naziv + "   " + adresa + "   " + mesto;
        }
    }
}
