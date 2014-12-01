using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Monitoring.Classes;
using System.Globalization;
using Monitoring.Classes.DatabaseCommunication;

namespace Monitoring.DatabaseCommunication
{
    class DBBlokadeData
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
        public static void shiftOld()
        {
            query = "DELETE Blokade WHERE DanBlokade = @danBlokade";
            command = new SqlCommand(query, DBBlokadeData.connection);
            command.Parameters.AddWithValue("@danBlokade", "N_1");
            command.ExecuteNonQuery();

            query = "UPDATE BLOKADE SET DanBlokade = @danBlokadeNovi WHERE DanBlokade = @DanBlokadeStari";
            command = new SqlCommand(query, DBBlokadeData.connection);
            command.Parameters.AddWithValue("@danBlokadeNovi", "N_1");
            command.Parameters.AddWithValue("@danBlokadeStari", "N");
            command.ExecuteNonQuery(); 
        }
        
        // Upisuje blokadu iz CSV-a
        public static void writeBlokadaIzCsv(BlokadaIzCsv blcsv)
        {
            string idNadgledanaFirma = null;

            // provera u nadgledana firma
            idNadgledanaFirma = proveraMBuNadgledanaFirma(blcsv.maticniBroj);

            // insert u nadgledana firma
            if(idNadgledanaFirma == null)
                idNadgledanaFirma = insertIntoNadgledanaFirma(blcsv.maticniBroj);

            // provera i insert u osnovne informacije
            if (!proveraIDNadgledanaUOsnovneInformacije(idNadgledanaFirma))
                insertIntoOsnovneInformacije(blcsv, idNadgledanaFirma);

            // insert into blokade
            insertIntoBlokade(blcsv, idNadgledanaFirma, "N");
        }

        // provera za dati maticni broj da li postoji u tabeli NadgledanaFirma
        // i ako postoji vraca idNadgledanaFirma
        public static string proveraMBuNadgledanaFirma(String maticniBrojZaProveru)
        {
            string idNadgledanaFirma = null;

            query = "SELECT IDNadgledanaFirma FROM [NadgledanaFirma] WHERE MaticniBroj = @maticniBroj";
            command = new SqlCommand(query, DBBlokadeData.connection);
            command.Parameters.AddWithValue("@maticniBroj", maticniBrojZaProveru);
            reader = command.ExecuteReader();
            if (reader.Read())
                idNadgledanaFirma = reader[0].ToString().Trim();
            reader.Close();
            return idNadgledanaFirma;
        }

        // insertuje maticni broj u tabelu NadgledanaFirma
        // moglo bi da se poboljsa sa executeScalar
        public static string insertIntoNadgledanaFirma(string maticnibroj)
        {
            string idNadgledanaFirma = null;

            query = "INSERT INTO NadgledanaFirma (MaticniBroj) OUTPUT INSERTED.IDNadgledanaFirma VALUES(@maticniBroj)";
            command = new SqlCommand(query, DBBlokadeData.connection);
            command.Parameters.AddWithValue("@maticniBroj", maticnibroj);
            idNadgledanaFirma = command.ExecuteScalar().ToString().Trim();
            return idNadgledanaFirma;            
        }

        public static bool proveraIDNadgledanaUOsnovneInformacije(string idNadgledanaFirma)
        {
            query = "SELECT IDNadgledanaFirma FROM [OsnovneInformacije] WHERE IDNadgledanaFirma = @idNadgledanaFirma";
            command = new SqlCommand(query, DBBlokadeData.connection);
            command.Parameters.AddWithValue("@idNadgledanaFirma", idNadgledanaFirma);
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                reader.Close();
                return true;
            }
            reader.Close();
            return false; 
        }

        public static void insertIntoOsnovneInformacije(BlokadaIzCsv blcsv, string idNadgledanaFirma)
        {
            query = "INSERT INTO [OsnovneInformacije](IDNadgledanaFirma, Adresa, PIB, Grad)" +
                "VALUES(@idNadgledanaFirma,@adresa,@pib,@grad)";
            command = new SqlCommand(query, DBBlokadeData.connection);
            command.Parameters.AddWithValue("@idNadgledanaFirma", idNadgledanaFirma.Trim());
            command.Parameters.AddWithValue("@adresa", (blcsv.adresa == null) ? DBNull.Value : (object)blcsv.adresa);
            command.Parameters.AddWithValue("@pib", (blcsv.pib == null) ? DBNull.Value : (object)blcsv.pib);
            command.Parameters.AddWithValue("@grad", (blcsv.grad == null) ? DBNull.Value : (object)blcsv.grad);
            command.ExecuteNonQuery();
        }

        // vraca ID nadgledane firme
        // moglo bi da se poboljsa sa executeScalar
        public static String getIDNadgledanaFirma(String maticniBroj)
        {
            String idNadgledanaFirma;

            query = "SELECT IDNadgledanaFirma FROM [NadgledanaFirma] WHERE MaticniBroj = @maticniBroj";
            command = new SqlCommand(query, DBBlokadeData.connection);
            command.Parameters.AddWithValue("@maticniBroj", maticniBroj);
            reader = command.ExecuteReader();
            reader.Read();
            idNadgledanaFirma = reader[0].ToString();
            reader.Close();
            return idNadgledanaFirma;
        }

        // insertuje u blokade
        public static void insertIntoBlokade(BlokadaIzCsv blcsv, String idNadgledanaFirma, String danBlokade)
        {
            DateTime datumOdDT, datumDoDT, datumAzuriranjaDT;
            datumDoDT = datumOdDT = datumAzuriranjaDT = DateTime.Now;

            try
            {
                if (blcsv.datumOd != null)
                {
                    datumOdDT = DateTime.ParseExact(blcsv.datumOd, "dd.MM.yyyy.", CultureInfo.InvariantCulture);
                }

                if (blcsv.datumDo != null)
                {
                    if (blcsv.datumDo.Equals("-"))
                        blcsv.datumDo = null;
                    else
                        datumDoDT = DateTime.ParseExact(blcsv.datumDo, "dd.MM.yyyy.", CultureInfo.InvariantCulture);
                }
                if (blcsv.datumAzuriranja == null || blcsv.datumAzuriranja.Equals(""))
                {
                    if (DBBlokadeData.maxDatumAzuriranja == null)
                        datumAzuriranjaDT = DateTime.ParseExact("06.06.6666", "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    else
                        datumAzuriranjaDT = DateTime.ParseExact(DBBlokadeData.maxDatumAzuriranja, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    datumAzuriranjaDT = DateTime.ParseExact(blcsv.datumAzuriranja, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    DBBlokadeData.maxDatumAzuriranja = blcsv.datumAzuriranja;
                    updateDatumAzuriranja();
                }
            }
            catch (Exception e) { Console.WriteLine(blcsv.maticniBroj + " NEUSPENO UBACIVANJE"); DBGreska.addGreska(blcsv.maticniBroj,"insertIntoBlokade",e.ToString());return;}

            query = "INSERT INTO [Blokade](MaticniBroj,IDNadgledanaFirma,DatumOd,DatumDo,Iznos,BrojDana,Status,ZabranaPrenosa,DatumAzuriranja,UkupanBrojDana,DanBlokade)" +
                "VALUES(@maticniBroj,@idNadgledanaFirma,@datumOd,@datumDo,@iznos,@brojDana,@status,@zabranaPrenosa,@datumAzuriranja,@ukupanBrojDana,@danBlokade)";

            command = new SqlCommand(query, DBBlokadeData.connection);

            command.Parameters.AddWithValue("@maticniBroj", blcsv.maticniBroj);
            command.Parameters.AddWithValue("@idNadgledanaFirma", idNadgledanaFirma);
            command.Parameters.AddWithValue("@datumOd", (blcsv.datumOd == null) ? DBNull.Value : (object)datumOdDT);
            command.Parameters.AddWithValue("@datumDo", (blcsv.datumDo == null) ? DBNull.Value : (object)datumDoDT);
            command.Parameters.AddWithValue("@iznos", (blcsv.iznos == null) ? DBNull.Value : (object)blcsv.iznos);
            command.Parameters.AddWithValue("@brojDana", (blcsv.brojDana == null) ? DBNull.Value : (object)blcsv.brojDana);
            command.Parameters.AddWithValue("@status", (blcsv.status == null) ? DBNull.Value : (object)blcsv.status.Trim());
            command.Parameters.AddWithValue("@zabranaPrenosa", (blcsv.zabranaPrenosa == null) ? DBNull.Value : (object)blcsv.zabranaPrenosa);
            command.Parameters.AddWithValue("@datumAzuriranja", (blcsv.datumAzuriranja == null) ? DBNull.Value : (object)datumAzuriranjaDT);
            command.Parameters.AddWithValue("@ukupanBrojDana", (blcsv.ukupanBrojDana == null) ? DBNull.Value : (object)blcsv.ukupanBrojDana);
            command.Parameters.AddWithValue("@danBlokade", danBlokade.Trim());
            command.ExecuteNonQuery();            
        }

        //Postavlja datum azuriranja za sva predhodna polja na prvi koji nadje
        public static void updateDatumAzuriranja()
        {
            query = "UPDATE Blokade SET DatumAzuriranja = @maxDatumAzuriranja WHERE DatumAzuriranja = @tempDatumAzuriranja";
            command = new SqlCommand(query, DBBlokadeData.connection);
            command.Parameters.AddWithValue("@maxDatumAzuriranja", DateTime.ParseExact(DBBlokadeData.maxDatumAzuriranja, "dd.MM.yyyy", CultureInfo.InvariantCulture));
            command.Parameters.AddWithValue("@tempDatumAzuriranja", DateTime.ParseExact("06.06.6666", "dd.MM.yyyy", CultureInfo.InvariantCulture));
            command.ExecuteNonQuery();
        }

        public static void doneInserting()
        {
            DBBlokadeData.maxDatumAzuriranja = null;
        }
    }
}