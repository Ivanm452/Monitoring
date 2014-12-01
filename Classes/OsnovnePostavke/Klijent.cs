using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monitoring.OsnovnePostavke
{
    class Klijent
    {
        public string idKlijent;
        public string maticniBroj;
        public string naziv;

        public Klijent(string idKlijent, string maticniBroj, string naziv)
        {
            this.idKlijent = idKlijent;
            this.maticniBroj = maticniBroj;
            this.naziv = naziv;
        }

        public override string ToString()
        {
            return naziv;
        }
    }
}
