using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Monitoring.DatabaseCommunication;

namespace Monitoring.Classes
{
    class BlokadaRazlika
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
        public String iznosPromene;//za koliko je promenjen
        public String statusKompanije;
        public String promenjenStatusKompanije = "false";
        public String carlCustomID;

        public String promenjenStatus = "false";
        public String povecanIznos = "false";
        public String smanjenIznos = "false";

           public override string ToString()
           {
               return idNadgledanaFirma + " maticnibroj:" + maticniBroj + " adresa:" + adresa + " pib:" + pib + " grad:" + grad + " datumOd:" + datumOd + " datumDo:" + datumDo + " iznos:" + iznos + " brojDana:" +
                   brojDana + " status:" + status + " zabranaPrenosa:" + zabranaPrenosa + " datumAzuriranja:" + datumAzuriranja + " ukupanBrojDana:" + ukupanBrojDana + " promenjenStatus:" +
                       promenjenStatus + " povecanIznos:" + povecanIznos + " smanjenIznos:" + smanjenIznos + " iznosPromene:" + iznosPromene;
           
           }

           public BlokadaRazlika() { }

           public BlokadaRazlika(BlokadaIzBaze bib, bool promenjenStatus, bool povecanIznos, bool smanjenIznos,Double promenaIznosa)
        {
            this.promenjenStatus = promenjenStatus.ToString();
            this.povecanIznos = povecanIznos.ToString();
            this.smanjenIznos = smanjenIznos.ToString();

            idNadgledanaFirma = bib.idNadgledanaFirma;
            maticniBroj = bib.maticniBroj;
            adresa = bib.adresa;
            pib = bib.pib;
            grad = bib.grad;
            datumOd = bib.datumOd;
            datumDo = bib.datumDo;
            iznos = bib.iznos;
            carlCustomID = bib.carlCustomID;

            if (!bib.datumOd.Equals(""))
            {
                DateTime d1, d2;
                d1 = DateTime.Parse(bib.datumOd.Substring(0, bib.datumOd.IndexOf(" ")));
                d2 = (bib.datumDo == "") ? DateTime.Parse(bib.datumAzuriranja.Substring(0, bib.datumAzuriranja.IndexOf(" "))) : DateTime.Parse(bib.datumDo.Substring(0, bib.datumDo.IndexOf(" ")));

                brojDana = (d2 - d1).TotalDays.ToString();
            }
            else
                brojDana = null;

            if (bib.datumDo.Equals("") && !bib.datumOd.Equals(""))
                status = "Blokiran";
            else
                status = "Aktivan";

            zabranaPrenosa = bib.zabranaPrenosa;
            datumAzuriranja = bib.datumAzuriranja;
            this.iznosPromene = promenaIznosa.ToString();
            if (status.Equals("Aktivan"))
                datumOd = datumDo = iznos = brojDana = zabranaPrenosa = this.iznosPromene = "";
            ukupanBrojDana = bib.ukupanBrojDana;
            this.naziv = bib.naziv;
        }
           public BlokadaRazlika(BlokadaIzBaze bib)
           {
               this.promenjenStatus = "False";
               this.povecanIznos = "False";
               this.smanjenIznos = "False";

               idNadgledanaFirma = bib.idNadgledanaFirma;
               maticniBroj = bib.maticniBroj;
               adresa = bib.adresa;
               pib = bib.pib;
               grad = bib.grad;
               datumOd = bib.datumOd;
               datumDo = bib.datumDo;
               iznos = bib.iznos;
               carlCustomID = bib.carlCustomID;

               if (!bib.datumOd.Equals(""))
               {
                   DateTime d1, d2;
                   d1 = DateTime.Parse(bib.datumOd.Substring(0, bib.datumOd.IndexOf(" ")));
                   d2 = (bib.datumDo == "") ? DateTime.Parse(bib.datumAzuriranja.Substring(0, bib.datumAzuriranja.IndexOf(" "))) : DateTime.Parse(bib.datumDo.Substring(0, bib.datumDo.IndexOf(" ")));

                   brojDana = (d2 - d1).TotalDays.ToString();
               }
               else
                   brojDana = null;

               if (bib.datumDo.Equals("") && !bib.datumOd.Equals(""))
                   status = "Blokiran";
               else
                   status = "Aktivan";

               zabranaPrenosa = bib.zabranaPrenosa;
               datumAzuriranja = bib.datumAzuriranja;
               this.iznosPromene = "";
               if (status.Equals("Aktivan"))
                   datumOd = datumDo = iznos = brojDana = zabranaPrenosa = this.iznosPromene = "";
               ukupanBrojDana = bib.ukupanBrojDana;
               this.naziv = bib.naziv;
           }
    }
}
