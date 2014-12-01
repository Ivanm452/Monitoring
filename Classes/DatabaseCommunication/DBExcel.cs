using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Monitoring.Classes;

namespace Monitoring.DatabaseCommunication
{
    class DBExcel
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

        public static BlokadaRazlika getBlokadaRazlika (String maticniBroj)
        {
            BlokadaRazlika br = new BlokadaRazlika();

            query = "SELECT * FROM RezultatBlokade where MaticniBroj = @maticniBroj";
            command = new SqlCommand(query, DBExcel.connection);
            command.Parameters.AddWithValue("@maticniBroj", maticniBroj);
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                br.maticniBroj = reader[2].ToString();
                br.naziv = reader[3].ToString();
                br.adresa = reader[4].ToString();
                br.pib = reader[5].ToString();
                br.grad = reader[6].ToString();
                br.datumOd = reader[7].ToString();
                br.datumDo = reader[8].ToString();
                br.iznos = reader[9].ToString();
                br.brojDana = reader[10].ToString();
                br.status = reader[11].ToString();
                br.zabranaPrenosa = reader[12].ToString();
                br.datumAzuriranja = reader[13].ToString();
                br.ukupanBrojDana = reader[14].ToString();
                br.promenjenStatus = reader[15].ToString();
                br.povecanIznos = reader[16].ToString();
                br.smanjenIznos = reader[17].ToString();
                br.iznosPromene = reader[18].ToString();
                br.statusKompanije = reader[19].ToString();
                br.promenjenStatusKompanije = reader[20].ToString();
                br.carlCustomID = reader[21].ToString();
            }
            reader.Close();
            return br;
        }

        public static bool getPotencijalnoNeaktivna(String maticniBroj)
        {
            bool potencijalnoNeaktivna = false;

            query = "SELECT * FROM Greska where MaticniBroj = @maticniBroj and Datum=@datum and Opis=@opis";
            command = new SqlCommand(query, DBExcel.connection);
            command.Parameters.AddWithValue("@maticniBroj", maticniBroj);
            command.Parameters.AddWithValue("@datum", DateTime.Now.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@opis", "Potencijalno neaktivan");
            reader = command.ExecuteReader();
            if (reader.Read())
                potencijalnoNeaktivna = true;
            reader.Close();
            return potencijalnoNeaktivna;
        }
    }
}
