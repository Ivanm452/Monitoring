using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Monitoring.Classes.OsnovnePostavke;
using Monitoring.OsnovnePostavke;

namespace Monitoring.Classes.DatabaseCommunication
{
    class DBZaGenerisanje
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

        public static List<ZaGenerisanje> getListZaGenerisanje()
        {
            SqlDataReader innerReader;
            string innerQuery;
            SqlCommand innerCommand;
            SqlConnection innerConnection;

            List<ZaGenerisanje> listZaGenerisanje = new List<ZaGenerisanje>();

            List<Klijent> lstKlijent = new List<Klijent>();

            query = "SELECT IDKlijent, MaticniBroj, Naziv FROM Klijent";
            command = new SqlCommand(query, DBZaGenerisanje.connection);
            reader = command.ExecuteReader();
            while (reader.Read())
                lstKlijent.Add(new Klijent(reader[0].ToString().Trim(),reader[1].ToString().Trim(),reader[2].ToString().Trim()));
            reader.Close();

            int i = 0;
            foreach (Klijent k in lstKlijent)
            {
                listZaGenerisanje.Add(new ZaGenerisanje());
                listZaGenerisanje[i].klijent = k;

                query = "SELECT IDMonitoring, IDKlijent, IDVrstaMonitoringa, IDTipFajla, Naziv, SaljeSeMail " +
                    "FROM Monitoring WHERE IDKlijent = @idKlijent";
                command = new SqlCommand(query, DBZaGenerisanje.connection);
                command.Parameters.AddWithValue("@idKlijent", k.idKlijent);
                reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    tblMonitoring m = new tblMonitoring();
                    m.idMonitoring = reader[0].ToString().Trim();
                    m.idKlijent = reader[1].ToString().Trim();
                    m.idVrstaMonitoringa = reader[2].ToString().Trim();
                    m.idTipFajla = reader[3].ToString().Trim();
                    m.naziv = reader[4].ToString().Trim();
                    m.saljeSeMail = reader[5].ToString().Trim();

                    innerConnection = new SqlConnection(DBZaGenerisanje.connectionString);
                    innerConnection.Open();
                    innerQuery = "SELECT Mail FROM Mail WHERE IDMonitoring = @idMonitoring";
                    innerCommand = new SqlCommand(innerQuery, innerConnection);
                    innerCommand.Parameters.AddWithValue("@idMonitoring",m.idMonitoring);
                    innerReader = innerCommand.ExecuteReader();
                    while (innerReader.Read())
                        m.lstMail.Add(innerReader[0].ToString().Trim());
                    innerReader.Close();


                    innerQuery = "SELECT MaticniBroj FROM NadgledanaFirma INNER JOIN MonitoringFirma on NadgledanaFirma.IDNadgledanaFirma = MonitoringFirma.IDNadgledanaFirma WHERE IDMonitoring = @idMonitoring";
                    innerCommand = new SqlCommand(innerQuery, innerConnection);
                    innerCommand.Parameters.AddWithValue("@idMonitoring", m.idMonitoring);
                    innerReader = innerCommand.ExecuteReader();
                    while (innerReader.Read())
                    {
                        m.lstMaticniBrojevi.Add(innerReader[0].ToString().Trim());
                    }
                    innerReader.Close();
                    innerConnection.Close();

                    listZaGenerisanje[i].monitoring.Add(m);
 
                }
                reader.Close();
                i++;
            }
            return listZaGenerisanje; 
        }


        public static string getPotencijalnoNeaktivne()
        {
            string text = null;

            DBZaGenerisanje.connection = new SqlConnection(DBZaGenerisanje.connectionString);
            DBZaGenerisanje.connection.Open();

            query = "SELECT distinct mon.naziv, nf.maticnibroj FROM Monitoring mon " +
                "INNER JOIN MonitoringFirma mf ON mf.idmonitoring = mon.idmonitoring " +
                "INNER JOIN NadgledanaFirma nf ON nf.idnadgledanafirma = mf.idnadgledanafirma " +
                "WHERE nf.maticnibroj in (SELECT DISTINCT MaticniBroj from Greska WHERE Opis=@opis AND Datum = @datum)";

            command = new SqlCommand(query, DBZaGenerisanje.connection);
            command.Parameters.AddWithValue("@opis", "Potencijalno neaktivan");
            command.Parameters.AddWithValue("@datum", DateTime.Now.ToString("yyyy-MM-dd"));
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (text == null)
                    text = reader[0].ToString().Trim() + " - " + reader[1].ToString().Trim();
                else
                    text += '\n' + reader[0].ToString().Trim() + " - " + reader[1].ToString().Trim();
            }
            reader.Close();
            DBZaGenerisanje.connection.Close();
            return text;
        }
    }
}
