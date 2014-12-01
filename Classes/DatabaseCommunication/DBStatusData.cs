using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Monitoring.Classes;
using System.Globalization;
using Monitoring.Classes.DatabaseCommunication;
using Monitoring.Classes.CSV;

namespace Monitoring.DatabaseCommunication
{
    class DBStatusData
    {
        public static string connectionString = "Persist Security Info=False" +
            ";User ID=" + Properties.Settings.Default.DB_USERNAME + 
            ";Password=" + Properties.Settings.Default.DB_PASSWORD + 
            ";Initial Catalog=" +Properties.Settings.Default.DB_CATALOG +
            ";Server=" + Properties.Settings.Default.DB_SERVER;
        public static SqlConnection connection;
        static SqlDataReader reader;
        static string query;
        static SqlCommand command;
        public static String maxDatumAzuriranja = null;

        // Brise redove koji imaju DanBlokade=N_1 i azurira DanBlokade onima sa N na N_1
        public static void shiftOldStatuses()
        {
            query = "UPDATE OsnovneInformacije SET STATUS_N_1=STATUS_N";
            command = new SqlCommand(query, DBStatusData.connection);
            command.ExecuteNonQuery();

            query = "UPDATE OsnovneInformacije SET STATUS_N='-'";
            command = new SqlCommand(query, DBStatusData.connection);
            command.ExecuteNonQuery();
        }

        public static bool writeStatusIzCsv(StatusIzCsv cis)
        {
            string idNadgledanaFirma = null;

            
            // provera u nadgledana firma
            idNadgledanaFirma = proveraMBuNadgledanaFirma(cis.maticniBroj);
            if (idNadgledanaFirma == null)
                return false;

            updateStatusIntoOsnovneInformacije(cis,idNadgledanaFirma);
            return true; 
        }

        private static void updateStatusIntoOsnovneInformacije(StatusIzCsv cis,string idNadgledanaFirma)
        {
            
            query = "UPDATE OsnovneInformacije SET STATUS_N = @status_n WHERE IDNADGLEDANAFIRMA=@idNadgledanaFirma";
            command = new SqlCommand(query, DBStatusData.connection);
            command.Parameters.AddWithValue("@idNadgledanaFirma", idNadgledanaFirma);
            command.Parameters.AddWithValue("@status_n", cis.status);
            command.ExecuteNonQuery();
        }

        public static string proveraMBuNadgledanaFirma(String maticniBrojZaProveru)
        {
            string idNadgledanaFirma = null;

            query = "SELECT IDNadgledanaFirma FROM [NadgledanaFirma] WHERE MaticniBroj = @maticniBroj";
            command = new SqlCommand(query, DBStatusData.connection);
            command.Parameters.AddWithValue("@maticniBroj", maticniBrojZaProveru);
            reader = command.ExecuteReader();
            if (reader.Read())
                idNadgledanaFirma = reader[0].ToString().Trim();
            reader.Close();
            return idNadgledanaFirma;
        }
        
    }
}