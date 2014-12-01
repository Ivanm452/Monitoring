using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monitoring.Classes.OsnovnePostavke
{
    class OsnovneInformacije
    {
        public string idNadgledanaFirma;
        public string naziv;
        public string adresa;
        public string pib;
        public string grad;

        public OsnovneInformacije(string idNadgledanaFirma, string naziv, string adresa,
            string pib, string grad)
        {
            this.idNadgledanaFirma = idNadgledanaFirma;
            this.naziv = naziv;
            this.adresa = adresa;
            this.pib = pib;
            this.grad = grad;
        }

        public OsnovneInformacije()
        {
            this.idNadgledanaFirma = this.naziv = this.adresa = this.pib = this.grad = null;
        }
    }
}
