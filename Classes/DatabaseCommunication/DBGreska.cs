using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Monitoring.Classes.DatabaseCommunication
{
    class DBGreska
    {
        public static void addGreska(string maticniBroj, string grupa, string opis)
        {
            string connectionString = "Persist Security Info=False" +
           ";User ID=" + Properties.Settings.Default.DB_USERNAME +
           ";Password=" + Properties.Settings.Default.DB_PASSWORD +
           ";Initial Catalog=" + Properties.Settings.Default.DB_CATALOG +
           ";Server=" + Properties.Settings.Default.DB_SERVER;

            SqlConnection connection;
            string query;
            SqlCommand command;

            DateTime datum = DateTime.Now;

            connection = new SqlConnection(connectionString);
            connection.Open();
            query = "INSERT INTO Greska (Datum, MaticniBroj, Grupa, Opis) VALUES (@datum, @maticniBroj, @grupa, @opis)";
            command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@datum", datum);
            command.Parameters.AddWithValue("@maticniBroj", maticniBroj);
            command.Parameters.AddWithValue("@grupa", grupa);
            command.Parameters.AddWithValue("@opis", opis);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
