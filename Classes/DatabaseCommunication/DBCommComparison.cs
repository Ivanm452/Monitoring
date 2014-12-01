using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Monitoring.Classes;

namespace Monitoring.DatabaseCommunication
{
    class DBCommComparison
    {
        public static string connectionString = "Persist Security Info=False" +
            ";User ID=" + Properties.Settings.Default.DB_USERNAME +
            ";Password=" + Properties.Settings.Default.DB_PASSWORD +
            ";Initial Catalog=" + Properties.Settings.Default.DB_CATALOG +
            ";Server=" + Properties.Settings.Default.DB_SERVER; 
        public static SqlConnection connection;
        static SqlDataReader reader;
        static string query;
        static SqlCommand command;

        public static void clearRezultatBlokade() 
        {
            query = "DELETE FROM RezultatBlokade";
            command = new SqlCommand(query, DBCommComparison.connection);
            command.ExecuteNonQuery();
        }

        public static List<String> getSveMaticne()
        {
            List<String> maticniBrojevi = new List<string>();
            query = "SELECT MaticniBroj FROM NadgledanaFirma";
            command = new SqlCommand(query, DBCommComparison.connection);
            reader = command.ExecuteReader();
            while (reader.Read())
                maticniBrojevi.Add(reader[0].ToString().Trim());
            reader.Close();
            if (maticniBrojevi.Count == 0)
                return null;
            return maticniBrojevi;
        }

        public static List<String> getNadgledaneMaticne()
        {
            List<String> maticniBrojevi = new List<string>();
            query = "SELECT DISTINCT nf.MaticniBroj FROM NadgledanaFirma nf INNER JOIN MonitoringFirma mf on mf.IDNadgledanaFirma = nf.IDNadgledanaFirma";
            command = new SqlCommand(query, DBCommComparison.connection);
            reader = command.ExecuteReader();
            while (reader.Read())
                maticniBrojevi.Add(reader[0].ToString().Trim());
            reader.Close();
            if (maticniBrojevi.Count == 0)
                return null;
            return maticniBrojevi;
        }

        public static BlokadaIzBaze getBlokadaIzBaze(String mb, String dan)
        {
            query = "SELECT bl.idNadgledanaFirma, nf.maticnibroj, oi.adresa, oi.pib, oi.grad, bl.datumOd, bl.datumDo, bl.iznos, bl.brojDana, bl.status, bl.zabranaPrenosa," +
                "bl.datumAzuriranja, bl.ukupanBrojDana, bl.DanBlokade, oi.naziv, oi.CarlCustomID " +
                "FROM Blokade bl " +
                "INNER JOIN NadgledanaFirma nf on bl.idNadgledanaFirma = nf.idNadgledanaFirma " +
                "LEFT JOIN OsnovneInformacije oi on oi.idNadgledanaFirma = nf.idNadgledanaFirma " +
                "WHERE nf.maticnibroj = @maticniBroj AND bl.DanBlokade = @danBlokade";

            command = new SqlCommand(query, DBCommComparison.connection);
            command.Parameters.AddWithValue("@maticniBroj", mb);
            command.Parameters.AddWithValue("@danBlokade", dan);
            reader = command.ExecuteReader();
            
            BlokadaIzBaze bib = new BlokadaIzBaze();
            if (reader.Read())
            {
                bib.idNadgledanaFirma = reader[0].ToString().Trim();
                bib.maticniBroj = reader[1].ToString().Trim();
                bib.adresa = reader[2].ToString().Trim();
                bib.pib = reader[3].ToString().Trim();
                bib.grad = reader[4].ToString().Trim();
                bib.datumOd = reader[5].ToString().Trim();
                bib.datumDo = reader[6].ToString().Trim();
                bib.iznos = reader[7].ToString().Trim();
                bib.brojDana = reader[8].ToString().Trim();
                bib.status = reader[9].ToString().Trim();
                bib.zabranaPrenosa = reader[10].ToString().Trim();
                bib.datumAzuriranja = reader[11].ToString().Trim();
                bib.ukupanBrojDana = reader[12].ToString().Trim();
                bib.danBlokade = reader[13].ToString().Trim();
                bib.naziv = reader[14].ToString().Trim();
                bib.carlCustomID = reader[15].ToString().Trim();

                reader.Close();
                return bib;
            }
            reader.Close();
            return null;
        }

    /*    public static List<BlokadaIzBaze> getBlokadaIzBaze2(String mb)
        {
            List<BlokadaIzBaze> bibList = new List<BlokadaIzBaze>();
            query = "SELECT bl.idNadgledanaFirma, nf.maticnibroj, oi.adresa, oi.pib, oi.grad, bl.datumOd, bl.datumDo, bl.iznos, bl.brojDana, bl.status, bl.zabranaPrenosa," +
                "bl.datumAzuriranja, bl.ukupanBrojDana, bl.DanBlokade, oi.naziv " +
                "FROM Blokade bl " +
                "INNER JOIN NadgledanaFirma nf on bl.idNadgledanaFirma = nf.idNadgledanaFirma " +
                "INNER JOIN OsnovneInformacije oi on oi.idNadgledanaFirma = nf.idNadgledanaFirma " +
                "WHERE nf.maticnibroj = @maticniBroj ORDER BY DanBlokade";

            command = new SqlCommand(query, DBCommComparison.connection);
            command.Parameters.AddWithValue("@maticniBroj", mb);
            reader = command.ExecuteReader();
            int i = 0;
            if (reader.Read())
            {
                bibList.Add(new BlokadaIzBaze());
                bibList[i].idNadgledanaFirma = reader[0].ToString().Trim();
                bibList[i].maticniBroj = reader[1].ToString().Trim();
                bibList[i].adresa = reader[2].ToString().Trim();
                bibList[i].pib = reader[3].ToString().Trim();
                bibList[i].grad = reader[4].ToString().Trim();
                bibList[i].datumOd = reader[5].ToString().Trim();
                bibList[i].datumDo = reader[6].ToString().Trim();
                bibList[i].iznos = reader[7].ToString().Trim();
                bibList[i].brojDana = reader[8].ToString().Trim();
                bibList[i].status = reader[9].ToString().Trim();
                bibList[i].zabranaPrenosa = reader[10].ToString().Trim();
                bibList[i].datumAzuriranja = reader[11].ToString().Trim();
                bibList[i].ukupanBrojDana = reader[12].ToString().Trim();
                bibList[i].danBlokade = reader[13].ToString().Trim();
                bibList[i].naziv = reader[14].ToString().Trim();
                i++;

                reader.Close();
            }
            reader.Close();
            return bibList;
        }       */     
        
        public static void writeRazlika(BlokadaRazlika br)
        {
            query = "SELECT STATUS_N, STATUS_N_1 FROM OsnovneInformacije where IDNadgledanaFirma = @idNadgledanaFirma";
            command = new SqlCommand(query, DBCommComparison.connection);
            command.Parameters.AddWithValue("@idNadgledanaFirma", br.idNadgledanaFirma);
            reader = command.ExecuteReader();

            string status_n, status_n_1;
            if (reader.Read())
            {
                status_n = reader[0].ToString().Trim();
                status_n_1 = reader[1].ToString().Trim();

                // #  N                 -  N_1

                // 1. ima status        -  ima status
                if (status_n != "" && status_n != "-" && status_n_1 != "" && status_n_1 != "-")
                {
                    br.statusKompanije = status_n;
                    if (status_n == status_n_1)
                        br.promenjenStatusKompanije = "false";
                    else
                        br.promenjenStatusKompanije = "true";
                }

                // 2. ima status        -  status je prazan
                else if (status_n != "" && status_n != "-" && status_n_1 == "")
                    br.statusKompanije = "Nepoznat";

                // 3. ima status        -  nema status
                else if (status_n != "" && status_n != "-" && status_n_1 == "-")
                    br.statusKompanije = "Nepoznat";

                // 4. nema status       -  ima status
                else if (status_n == "-" && status_n_1 != "" && status_n_1 != "-")
                    br.statusKompanije = "Nepoznat";

                // 5. nema status       -  status je prazan
                else if (status_n == "-" && status_n_1 == "")
                    br.statusKompanije = "Nepoznat";

                // 6. nema status       -  nema status
                else if (status_n == "-" && status_n_1 == "-")
                    br.statusKompanije = "-";

                // 7. status je prazan  -  ima status
                else if (status_n == "" && status_n_1 != "" && status_n_1 != "-")
                    br.statusKompanije = "Nepoznat";                

                // 8. status je prazan  -  status je prazan
                else if (status_n == "" && status_n_1 == "")
                    br.statusKompanije = "Nepoznat";  

                // 9. status je prazan  -  nema status
                else if (status_n == "" && status_n_1 == "-")
                    br.statusKompanije = "Nepoznat";                  

            }
            reader.Close();

            query = "INSERT INTO [RezultatBlokade](IDNadgledanaFirma,MaticniBroj,Adresa,Pib,"+
            "Grad,DatumOd,DatumDo,Iznos,BrojDana,Status,ZabranaPrenosa, DatumAzuriranja," +
            "UkupanBrojDana,PromenjenStatus,PovecanIznos,SmanjenIznos,IznosPromene,Naziv,StatusKompanije, PromenaStatusaKompanije, CarlCustomID) "+
                "VALUES(@idNadgledanaFirma,@maticniBroj,@adresa,@pib,@grad,@datumOd,@datumDo,"+
            "@iznos,@brojDana,@status,@zabranaPrenosa,@datumAzuriranja,@ukupanBrojDana,@promenjenStatus,"+
            "@povecanIznos,@smanjenIznos,@iznosPromene,@naziv,@statusKompanije, @promenaStatusaKompanije, @carlCustomID)";
            command = new SqlCommand(query, DBCommComparison.connection);
            command.Parameters.AddWithValue("@idNadgledanaFirma", br.idNadgledanaFirma);
            command.Parameters.AddWithValue("@maticniBroj", br.maticniBroj);
            command.Parameters.AddWithValue("@adresa", (br.adresa == "") ? DBNull.Value : (object)br.adresa);
            command.Parameters.AddWithValue("@pib", (br.pib == "") ? DBNull.Value : (object)br.pib);
            command.Parameters.AddWithValue("@grad", (br.grad == "") ? DBNull.Value : (object)br.grad);
            command.Parameters.AddWithValue("@datumOd", (br.datumOd == "") ? DBNull.Value : (object)br.datumOd.Substring(0,br.datumOd.IndexOf(' ')));
            command.Parameters.AddWithValue("@datumDo", (br.datumDo == "") ? DBNull.Value : (object)br.datumDo.Substring(0, br.datumDo.IndexOf(' ')));
            command.Parameters.AddWithValue("@iznos", (br.iznos == "") ? DBNull.Value : (object)br.iznos);
            command.Parameters.AddWithValue("@brojDana", (br.brojDana == null) ? DBNull.Value : (object)br.brojDana);
            command.Parameters.AddWithValue("@status", (br.status == "") ? DBNull.Value : (object)br.status);
            command.Parameters.AddWithValue("@zabranaPrenosa", (br.zabranaPrenosa == "") ? DBNull.Value : (object)br.zabranaPrenosa);
            command.Parameters.AddWithValue("@datumAzuriranja", (br.datumAzuriranja == "") ? DBNull.Value : (object)br.datumAzuriranja.Substring(0, br.datumAzuriranja.IndexOf(' ')));
            command.Parameters.AddWithValue("@ukupanBrojDana", (br.ukupanBrojDana == "") ? DBNull.Value : (object)br.ukupanBrojDana);
            command.Parameters.AddWithValue("@promenjenStatus", br.promenjenStatus.ToString());
            command.Parameters.AddWithValue("@povecanIznos", br.povecanIznos.ToString());
            command.Parameters.AddWithValue("@smanjenIznos", br.smanjenIznos.ToString());
            command.Parameters.AddWithValue("@iznosPromene", br.iznosPromene.ToString());
            command.Parameters.AddWithValue("@naziv", (br.naziv == "") ? DBNull.Value : (object)br.naziv);
            command.Parameters.AddWithValue("@statusKompanije", (br.statusKompanije == "") ? DBNull.Value : (object)br.statusKompanije);
            command.Parameters.AddWithValue("@promenaStatusaKompanije", (br.promenjenStatusKompanije == "") ? DBNull.Value : (object)br.promenjenStatusKompanije);
            command.Parameters.AddWithValue("@carlCustomID", (br.carlCustomID == "") ? DBNull.Value : (object)br.carlCustomID);
            
            command.ExecuteNonQuery(); 
        }        
    }
}
