using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Monitoring.DatabaseCommunication;
using Monitoring.Classes.DatabaseCommunication;
using System.Globalization;

namespace Monitoring.Classes
{
    class Comparison
    {
        public static BlokadaRazlika nadjiRazlike(BlokadaIzBaze b1, BlokadaIzBaze b2)
        {
            BlokadaRazlika br = null;

            if (b2 == null)
            {
                DBGreska.addGreska("", "nadjiRazlike", "B2==null");
                return null;
            }

            if (b1 == null && b2 != null)
                br = new BlokadaRazlika(b2, false, false, false, 0);
            else if(b2.maticniBroj!="" && b1.maticniBroj=="")
                br = new BlokadaRazlika(b2, false, false, false, 0);

            else if (b1.maticniBroj == "" && b2.maticniBroj == "")
            {
                DBGreska.addGreska("", "nadjiRazlike", "b1.maticniBroj == b2.maticniBroj == prazno");
                return null;
            }

            // Ako imamo samo N podatak za maticni broj
            else if (b1.maticniBroj == "")
                br = new BlokadaRazlika(b2, false, false, false, 0);

            // 00/00
            else if (b1.datumOd == "" && b1.datumDo == "" && b2.datumOd == "" && b2.datumDo == "")
                br = new BlokadaRazlika(b2, false, false, false, 0);            

            // 00/01
            else if (b1.datumOd == "" && b1.datumDo == "" && b2.datumOd == "" && b2.datumDo != "")
            {
                DBGreska.addGreska("", "Comparison", "Slucaj 00/01");
                return null;
            }

            // 00/10
            else if (b1.datumOd == "" && b1.datumDo == "" && b2.datumOd != "" && b2.datumDo == "")
            {
                br = new BlokadaRazlika(b2, true, false, false, 0);                
            }

            // 00/11
            else if (b1.datumOd == "" && b1.datumDo == "" && b2.datumOd != "" && b2.datumDo != "")
                br = new BlokadaRazlika(b2, false, false, false, 0);


            // 01/00  01/01  01/10  01/11
            else if (b1.datumOd == "" && b1.datumDo != "")
            {
                DBGreska.addGreska("", "Comparison", "Slucaj 01/XX");
                return null;
            }


            // 10/00
            else if (b1.datumOd != "" && b1.datumDo == "" && b2.datumOd == "" && b2.datumDo == "")
            {
                DBGreska.addGreska(b1.maticniBroj, "Comparison", "Potencijalno neaktivan");
                br = new BlokadaRazlika(b2, true, false, false, 0);
            }

            // 10/01
            else if (b1.datumOd != "" && b1.datumDo == "" && b2.datumOd == "" && b2.datumDo != "")
            {
                DBGreska.addGreska(b1.maticniBroj, "Comparison", "Slucaj 10/01");
                return null;
            }

            // 10/10
            else if (b1.datumOd != "" && b1.datumDo == "" && b2.datumOd != "" && b2.datumDo == "")
            {
                if (Double.Parse(b2.iznos) == Double.Parse(b1.iznos))
                    br = new BlokadaRazlika(b2, false, false, false, 0);
                else if (Double.Parse(b2.iznos) > Double.Parse(b1.iznos))
                    br = new BlokadaRazlika(b2, false, true, false, Math.Round(Double.Parse(b2.iznos) - Double.Parse(b1.iznos), 2));
                else if (Double.Parse(b2.iznos) < Double.Parse(b1.iznos))
                    br = new BlokadaRazlika(b2, false, false, true, Math.Round(Double.Parse(b2.iznos) - Double.Parse(b1.iznos), 2));
            }

            // 10/11
            else if (b1.datumOd != "" && b1.datumDo == "" && b2.datumOd != "" && b2.datumDo != "")
                br = new BlokadaRazlika(b2, true, false, false, 0);


            // 11/00
            else if (b1.datumOd != "" && b1.datumDo != "" && b2.datumOd == "" && b2.datumDo == "")
                br = new BlokadaRazlika(b2, false, false, false, 0);

            // 11/01
            else if (b1.datumOd != "" && b1.datumDo != "" && b2.datumOd == "" && b2.datumDo != "")
            {
                DBGreska.addGreska(b1.maticniBroj, "Comparison", "Slucaj 11/01");
                return null;
            }

            // 11/10
            else if (b1.datumOd != "" && b1.datumDo != "" && b2.datumOd != "" && b2.datumDo == "")
                br = new BlokadaRazlika(b2, true, false, false, 0);

            // 11/11
            else if (b1.datumOd != "" && b1.datumDo != "" && b2.datumOd != "" && b2.datumDo != "")
                br = new BlokadaRazlika(b2, false, false, false, 0);


            if (br != null)
            {
                br.naziv = b2.naziv;
                if(b2.maticniBroj!="")
                {
                    if (b2.zabranaPrenosa.Trim().Length > 2)
                    {
                        String s = b2.zabranaPrenosa.Substring(0, 11);
                        DateTime dt =  DateTime.ParseExact(s, "dd.MM.yyyy.", CultureInfo.InvariantCulture);

                        if (dt.Day == DateTime.Now.Day && dt.Month == DateTime.Now.Month && dt.Year == DateTime.Now.Year)
                            br.promenjenStatus = "True";
                        else
                        {
                            if(br.promenjenStatus!="True")
                                br.promenjenStatus = "False";
                        }

                        br.zabranaPrenosa = b2.zabranaPrenosa;
                        br.status = "Blokiran";

                    }
                }
               
            }
            else if (br == null)
            {
                DBGreska.addGreska("", "Comparison", "Neobradjen slucaj: br==null");
            }

            return br;
        }
    }
}
