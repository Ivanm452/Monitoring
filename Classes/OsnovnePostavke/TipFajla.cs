using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monitoring.OsnovnePostavke
{
    class TipFajla
    {
        public string idTipFajla, opis;

        public TipFajla(string idTipFajla, string opis)
        {
            this.idTipFajla = idTipFajla;
            this.opis = opis;
        }

        public override string ToString()
        {
            return opis;
        }
    }
}
