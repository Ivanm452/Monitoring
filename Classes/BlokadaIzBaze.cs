using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monitoring.Classes
{
    class BlokadaIzBaze
    {
        public String idNadgledanaFirma;
        public String maticniBroj;
        public String naziv;
        public String adresa;
        public String pib;
        public String grad;
        public String datumOd;
        public String datumDo;
        public String iznos;
        public String brojDana;
        public String status;
        public String zabranaPrenosa;
        public String datumAzuriranja;
        public String ukupanBrojDana;
        public String danBlokade;
        public String carlCustomID;

        public override string ToString()
        {
            return "idNadgledanaFirma: " + idNadgledanaFirma +
                " maticniBroj: " + maticniBroj +
                " naziv: " + naziv +
                " adresa: " + adresa +
                " pib: " + pib +
                " grad: " + grad +
                " datumOd: " + datumOd +
                " datumDo: " + datumDo +
                " iznos: " + iznos +
                " brojDana: " + brojDana +
                " status: " + status +
                " zabranaPrenosa: " + zabranaPrenosa +
                " datumAzuriranja: " + datumAzuriranja +
                " ukupanBrojDana: " + ukupanBrojDana +
                " danBlokade: " + danBlokade +
                " carlCustomID: " + carlCustomID;
        }
    }
}
