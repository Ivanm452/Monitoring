using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using CsvHelper;

namespace Monitoring.Classes
{
    class BlokadaIzCsv
    {
        public String maticniBroj;
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

        

        public BlokadaIzCsv()
        {
            maticniBroj = adresa = pib = grad = datumOd = datumDo = iznos = brojDana = status = zabranaPrenosa = datumAzuriranja = ukupanBrojDana = null;
        }

        public BlokadaIzCsv(CsvReader csv)
        {
            try
            {
                if (csv.CurrentRecord.Length == 1)
                {
                    if (csv[0] != null)
                        maticniBroj = csv[0].Trim();
                }
                else if (csv.CurrentRecord.Length == 11)
                {
                    if (csv[0] != null)
                        maticniBroj = (csv[0].Trim().Equals("")) ? null : csv[0].Trim();

                    if (csv[1] != null)
                        adresa = (csv[1].Trim().Equals("")) ? null : csv[1].Trim();

                    if (csv[2] != null)
                        pib = (csv[2].Trim().Equals("")) ? null : csv[2].Trim();

                    if (csv[3] != null)
                        grad = (csv[3].Trim().Equals("")) ? null : csv[3].Trim();

                    if (csv[4] != null)
                        datumOd = (csv[4].Trim().Equals("")) ? null : csv[4].Trim();

                    if (csv[5] != null)
                        datumDo = (csv[5].Trim().Equals("")) ? null : csv[5].Trim();

                    if (csv[6] != null)
                        iznos = (csv[6].Trim().Equals("")) ? null : csv[6].Trim();

                    if (csv[7] != null)
                        brojDana = (csv[7].Trim().Equals("")) ? null : csv[7].Trim();

                    if (csv[8] != null)
                        status = (csv[8].Trim().Equals("")) ? null : csv[8].Trim();

                    if (csv[9] != null)
                        zabranaPrenosa = (csv[9].Trim().Equals("")) ? null : csv[9].Trim();

                    if (csv[10] != null)
                        datumAzuriranja = (csv[10].Trim().Equals("")) ? null : csv[10].Trim();

                    ukupanBrojDana = null;
                }
                else if (csv.CurrentRecord.Length == 12)
                {
                    if (csv[0] != null)
                        maticniBroj = (csv[0].Trim().Equals("")) ? null : csv[0].Trim();

                    if (csv[1] != null)
                        adresa = (csv[1].Trim().Equals("")) ? null : csv[1].Trim();

                    if (csv[2] != null)
                        pib = (csv[2].Trim().Equals("")) ? null : csv[2].Trim();

                    if (csv[3] != null)
                        grad = (csv[3].Trim().Equals("")) ? null : csv[3].Trim();

                    if (csv[4] != null)
                        datumOd = (csv[4].Trim().Equals("")) ? null : csv[4].Trim();

                    if (csv[5] != null)
                        datumDo = (csv[5].Trim().Equals("")) ? null : csv[5].Trim();

                    if (csv[6] != null)
                        iznos = (csv[6].Trim().Equals("")) ? null : csv[6].Trim();

                    if (csv[7] != null)
                        brojDana = (csv[7].Trim().Equals("")) ? null : csv[7].Trim();

                    if (csv[8] != null)
                        status = (csv[8].Trim().Equals("")) ? null : csv[8].Trim();

                    if (csv[9] != null)
                        zabranaPrenosa = (csv[9].Trim().Equals("")) ? null : csv[9].Trim();

                    if (csv[10] != null)
                        datumAzuriranja = (csv[10].Trim().Equals("")) ? null : csv[10].Trim();

                    if (csv[11] != null)
                        ukupanBrojDana = (csv[11].Trim().Equals("")) ? null : csv[11].Trim();
                }
            }
            catch (Exception e) { Console.WriteLine("Greska u BlokadaIzCsv! " + e.ToString()); }
        }
    }
}
