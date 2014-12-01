using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Monitoring.OsnovnePostavke;
using Monitoring.Classes.OsnovnePostavke;
using Monitoring.Classes;

namespace Monitoring.DatabaseCommunication
{
    class DBCommOsnovnePostavke
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

        public static void dodajKlijenta(String maticnibroj, String naziv)
        {
            query = "INSERT INTO Klijent (MaticniBroj, Naziv) VALUES (@maticniBroj, @naziv)";
            command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
            command.Parameters.AddWithValue("@maticniBroj",maticnibroj);
            command.Parameters.AddWithValue("@naziv",naziv);
            command.ExecuteNonQuery();
        }

        public static List<Klijent> getKlijenti() 
        {
            List<Klijent> klijenti = new List<Klijent>();
            query = "SELECT IDKlijent, MaticniBroj, Naziv FROM Klijent";
            command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
            reader = command.ExecuteReader();
            while (reader.Read())
                klijenti.Add(new Klijent(reader[0].ToString().Trim(), reader[1].ToString().Trim(), reader[2].ToString().Trim()));
            reader.Close();
            return klijenti;
        }

        public static Klijent getKlijent(String maticniBroj)
        {
            Klijent k = null;
            query = "SELECT IDKlijent, MaticniBroj, Naziv FROM Klijent where MaticniBroj = @maticniBroj";
            command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
            command.Parameters.AddWithValue("@maticniBroj",maticniBroj);
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                k=new Klijent(reader[0].ToString().Trim(), reader[1].ToString().Trim(), reader[2].ToString().Trim());
            }
            reader.Close();
            return k;
 
        }

        public static List<tblMonitoring> getMonitoring(String idKlijent)
        {
            List<tblMonitoring> tblMonitorings = new List<tblMonitoring>();

            query = "SELECT IDMonitoring, IDKlijent, IDVrstaMonitoringa, IDTipFajla, Naziv, SaljeSeMail " + 
                "FROM Monitoring WHERE " + 
                "IDKlijent = @idKlijent";
            command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
            command.Parameters.AddWithValue("@idKlijent",idKlijent);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                tblMonitorings.Add(new tblMonitoring(reader[0].ToString().Trim(), reader[1].ToString().Trim(),
                    reader[2].ToString().Trim(), reader[3].ToString().Trim(), reader[4].ToString().Trim(), reader[5].ToString().Trim()));
            }

            reader.Close();

            return tblMonitorings;
        }

        public static List<VrstaMonitoringa> getVrstaMonitoringa()
        {
            List<VrstaMonitoringa> vrstaMonitoringas = new List<VrstaMonitoringa>();
            query = "SELECT IDVrstaMonitoringa, Naziv FROM VrstaMonitoringa";
            command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
            reader = command.ExecuteReader();
            while (reader.Read())
                vrstaMonitoringas.Add(new VrstaMonitoringa(reader[0].ToString().Trim(), reader[1].ToString().Trim()));
            reader.Close();
            return vrstaMonitoringas;
        }

        public static List<TipFajla> getTipFajla()
        {
            List<TipFajla> tipFajlas = new List<TipFajla>();

            query = "SELECT IDTipFajla, Opis FROM TipFajla";
            command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
            reader = command.ExecuteReader();
            while (reader.Read())
                tipFajlas.Add(new TipFajla(reader[0].ToString().Trim(), reader[1].ToString().Trim()));
            reader.Close();

            return tipFajlas;
        }

        public static void insertOsnovne(string idKlijent, string idVrstaMonitoringa, string idTipFajla,
            string naziv)
        {
            query = "INSERT INTO Monitoring (IDKlijent, IDVrstaMonitoringa, IDTipFajla, Naziv) " + 
                "VALUES (@idKlijent, @idVrstaMonitoringa, @idTIpFajla, @naziv)";
            command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
            command.Parameters.AddWithValue("@idKlijent", idKlijent);
            command.Parameters.AddWithValue("@idVrstaMonitoringa", idVrstaMonitoringa);
            command.Parameters.AddWithValue("@idTIpFajla", idTipFajla);
            command.Parameters.AddWithValue("@naziv", naziv);
            command.ExecuteNonQuery();
        }

        public static void updateOsnovne(string idMonitoring, string idVrstaMonitoringa, string idTipFajla,
            string naziv, string mailZaSlanje)
        {
            query = "UPDATE Monitoring SET IDVrstaMonitoringa = @idVrstaMonitoringa, " + 
                "IDTipFajla = @idTipFajla, Naziv = @naziv " + 
                "where IDMonitoring = @idMonitoring";
            command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
            command.Parameters.AddWithValue("@IDMonitoring", idMonitoring);
            command.Parameters.AddWithValue("@idVrstaMonitoringa", idVrstaMonitoringa);
            command.Parameters.AddWithValue("@idTIpFajla", idTipFajla);
            command.Parameters.AddWithValue("@naziv", naziv);
            command.ExecuteNonQuery();
        }

        public static void addMaticniBrojUNadgledane(string idMonitoring, string maticniBroj)
        {
            string idNadgledanaFirma ="";
            query = "SELECT IDNadgledanaFirma FROM NadgledanaFirma where MaticniBroj=@maticniBroj";
            command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
            command.Parameters.AddWithValue("@maticniBroj", maticniBroj);
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                idNadgledanaFirma = reader[0].ToString();
            }
            else
            {
                reader.Close();
                query = "INSERT INTO NadgledanaFirma (MaticniBroj) VALUES (@maticniBroj)";
                command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
                command.Parameters.AddWithValue("@maticniBroj", maticniBroj);
                command.ExecuteNonQuery(); 
            }
            reader.Close();

            query = "SELECT IDNadgledanaFirma FROM NadgledanaFirma where MaticniBroj=@maticniBroj";
            command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
            command.Parameters.AddWithValue("@maticniBroj", maticniBroj);
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                idNadgledanaFirma = reader[0].ToString();
            }
            reader.Close();

            query = "INSERT INTO MonitoringFirma (IDMonitoring, IDNadgledanaFirma) " + 
                "VALUES(@idMonitoring, @idNadgledanaFirma)";
            command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
            command.Parameters.AddWithValue("@idMonitoring", idMonitoring);
            command.Parameters.AddWithValue("@idNadgledanaFirma", idNadgledanaFirma);
            command.ExecuteNonQuery();
            

        }

        public static NadgledanaFirmaPoMonitoringu getNadgledaneFirme(string idMonitoring)
        {
            NadgledanaFirmaPoMonitoringu nfpm = new NadgledanaFirmaPoMonitoringu(idMonitoring);

            query = "SELECT nf.MaticniBroj FROM Monitoring mo " + 
                "INNER JOIN MonitoringFirma mf ON mf.IDMonitoring = mo.IDMonitoring " + 
                "INNER JOIN NadgledanaFirma nf ON nf.IDNadgledanaFIrma = mf.IDNadgledanaFirma " + 
                "WHERE mo.IDMonitoring = @idMonitoring";
            command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
            command.Parameters.AddWithValue("@idMonitoring", idMonitoring);
            reader = command.ExecuteReader();
            while (reader.Read())
                nfpm.maticniBrojevi.Add(reader[0].ToString().Trim());
            reader.Close();
            return nfpm;
        }

        public static tblMonitoring getSingleMonitoring(string idKlijent, string naziv)
        {
            tblMonitoring monitoring = new tblMonitoring();

            query = "SELECT IDMonitoring, IDKlijent, IDVrstaMonitoringa, IDTipFajla, Naziv, SaljeSeMail " +
                "FROM Monitoring WHERE " +
                "IDKlijent = @idKlijent AND naziv = @naziv";
            command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
            command.Parameters.AddWithValue("@idKlijent", idKlijent);
            command.Parameters.AddWithValue("@naziv", naziv);
            reader = command.ExecuteReader();
            Console.WriteLine(idKlijent + " " + naziv);
            if (reader.Read())
            {
                monitoring.idMonitoring = reader[0].ToString().Trim();
                monitoring.idKlijent = reader[1].ToString().Trim();
                monitoring.idVrstaMonitoringa = reader[2].ToString().Trim();
                monitoring.idTipFajla = reader[3].ToString().Trim();
                monitoring.naziv = reader[4].ToString().Trim();
                monitoring.saljeSeMail = reader[5].ToString().Trim();
            }
            reader.Close();
            
            return monitoring;
        }

        public static void obrisiMonitoring(tblMonitoring monitoring)
        {
            query = "DELETE Monitoring WHERE IDMonitoring = @idMonitoring";
            command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
            command.Parameters.AddWithValue("@idMonitoring", monitoring.idMonitoring);
            command.ExecuteNonQuery(); 
        }

        public static void obrisiKlijenta(Klijent klijent)
        {
            query = "DELETE Klijent WHERE IDKlijent=@idKlijent";
            command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
            command.Parameters.AddWithValue("@idKlijent", klijent.idKlijent);
            command.ExecuteNonQuery();
        }

        public static void obrisiMaticniIzMonitoringa(string idMonitoring, string maticniBroj)
        {
            query = "DELETE MonitoringFirma WHERE IDMonitoring = @idMonitoring " + 
                "AND IDNadgledanaFirma = " +
                "(SELECT IDNadgledanaFirma FROM NadgledanaFirma where MaticniBroj = @maticniBroj)";
            command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
            command.Parameters.AddWithValue("@idMonitoring", idMonitoring);
            command.Parameters.AddWithValue("@maticniBroj", maticniBroj);
            command.ExecuteNonQuery();            
        }

        public static OsnovneInformacije getOsnovneInformacije(string maticniBroj)
        {
            OsnovneInformacije osnovneInformacije = null;

            query = "SELECT IDNadgledanaFirma, Naziv, Adresa, PIB, Grad FROM OsnovneInformacije " +
                "WHERE IDNadgledanaFirma in (SELECT IDNadgledanaFirma from NadgledanaFirma " +
                "WHERE MaticniBroj = @maticniBroj)";
            command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
            command.Parameters.AddWithValue("@maticniBroj", maticniBroj);
            reader = command.ExecuteReader();
            if (reader.Read())
                osnovneInformacije = new OsnovneInformacije(reader[0].ToString().Trim(), reader[1].ToString().Trim(), reader[2].ToString().Trim(), reader[3].ToString().Trim(), reader[4].ToString().Trim());
            reader.Close();
            return osnovneInformacije; 
        }

        public static void updateOsnovneInformacije(OsnovneInformacije osnovneInformacije)
        {
            query = "UPDATE OsnovneInformacije SET " + 
                "Naziv=@naziv, Adresa=@adresa, PIB=@pib, Grad=@grad " + 
                "WHERE IDNadgledanaFirma = @idNadgledanaFirma";
            command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
            command.Parameters.AddWithValue("@naziv", osnovneInformacije.naziv);
            command.Parameters.AddWithValue("@adresa", osnovneInformacije.adresa);
            command.Parameters.AddWithValue("@pib", osnovneInformacije.pib);
            command.Parameters.AddWithValue("@grad", osnovneInformacije.grad);
            command.Parameters.AddWithValue("@idNadgledanaFirma", osnovneInformacije.idNadgledanaFirma);
            command.ExecuteNonQuery();
        }

        public static List<string> getMailove(String idMonitoring)
        {
            List<string> mailList = new List<string>();

            query = "SELECT Mail from Mail where IDMonitoring = @idMonitoring";
            command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
            command.Parameters.AddWithValue("@idMonitoring", idMonitoring);
            reader = command.ExecuteReader();
            while (reader.Read())
                mailList.Add(reader[0].ToString().Trim());
            reader.Close();

            return mailList;
        }

        public static void dodajMail(string idMonitoring, string mail)
        {
            query = "INSERT INTO Mail (IDMonitoring, Mail) VALUES (@idMonitoring,@mail)";
            command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
            command.Parameters.AddWithValue("@idMonitoring", idMonitoring);
            command.Parameters.AddWithValue("@mail", mail);
            command.ExecuteNonQuery();

        }

        public static void obrisiMail(string idMonitoring, string mail)
        {
            query = "DELETE Mail WHERE IDMonitoring = @idMonitoring AND Mail = @mail";
            command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
            command.Parameters.AddWithValue("@idMonitoring", idMonitoring);
            command.Parameters.AddWithValue("@mail", mail);
            command.ExecuteNonQuery();
        }

        public static void saljeSeMail(string idMonitoring, int i)
        {
            query = "UPDATE Monitoring SET SaljeSeMail = @saljeSeMail WHERE IDMonitoring = @idMonitoring";
            command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
            command.Parameters.AddWithValue("@idMonitoring", idMonitoring);
            command.Parameters.AddWithValue("@saljeSeMail", i);
            command.ExecuteNonQuery();
        }

        public static void addKlijentIzCsv(KlijentIzCsv kicsv)
        {
            string idKlijent = null;
            string idMonitoring = null;
            string idNadgledanaFirma = null;

            query = "SELECT IDKlijent FROM Klijent WHERE Naziv = @naziv";
            command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
            command.Parameters.AddWithValue("@naziv", kicsv.klijent);
            reader = command.ExecuteReader();
            if (reader.Read())
                idKlijent = reader[0].ToString().Trim();
            reader.Close();

            if (idKlijent == null)
            {
                query = "INSERT INTO Klijent (Naziv) OUTPUT INSERTED.IDKlijent VALUES(@naziv)";
                command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
                command.Parameters.AddWithValue("@naziv", kicsv.klijent);
                idKlijent = command.ExecuteScalar().ToString().Trim();
                
            }

            // Za insertovanje monitoringa
            query = "SELECT IDMonitoring FROM Monitoring WHERE Naziv = @naziv";
            command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
            command.Parameters.AddWithValue("@naziv", kicsv.nazivMonitoringa);
            reader = command.ExecuteReader();
            if (reader.Read())
                idMonitoring = reader[0].ToString().Trim();
            reader.Close();

            if (idMonitoring == null)
            {
                query = "INSERT INTO Monitoring (IDKlijent,IDVrstaMonitoringa,IDTipFajla,Naziv) OUTPUT INSERTED.IDMonitoring " +
                    "VALUES(@idKlijent,@idVrstaMonitoringa,@idTipFajla,@naziv) ";
                command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
                command.Parameters.AddWithValue("@idKlijent", idKlijent);
                command.Parameters.AddWithValue("@idVrstaMonitoringa", 1);
                command.Parameters.AddWithValue("@idTipFajla", 1);
                command.Parameters.AddWithValue("@naziv", kicsv.nazivMonitoringa);
                idMonitoring = command.ExecuteScalar().ToString().Trim();
            }
            List<string> idNadgledanaFirmaList = new List<string>();
            
            foreach (NadgledanaIzCsv nicsv in kicsv.nicsvList)
            {
                idNadgledanaFirma = null;

                query = "SELECT IDNadgledanaFirma FROM NadgledanaFirma WHERE MaticniBroj = @maticniBroj";
                command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
                command.Parameters.AddWithValue("@maticniBroj", nicsv.maticniBroj);
                reader = command.ExecuteReader();
                if (reader.Read())
                    idNadgledanaFirma = reader[0].ToString().Trim();
                reader.Close();

                if (idNadgledanaFirma == null)
                {
                    query = "INSERT INTO NadgledanaFirma(MaticniBroj) OUTPUT INSERTED.IdNadgledanaFirma VALUES(@maticniBroj) ";
                    command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
                    command.Parameters.AddWithValue("@maticniBroj", nicsv.maticniBroj);
                    idNadgledanaFirma = command.ExecuteScalar().ToString().Trim();
                }

                query = "SELECT * FROM MonitoringFirma WHERE IDNadgledanaFirma = @idNadgledanaFirma AND IDMonitoring=@idMonitoring";
                command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
                command.Parameters.AddWithValue("@idNadgledanaFirma", idNadgledanaFirma);
                command.Parameters.AddWithValue("@idMonitoring", idMonitoring);
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    reader.Close();
                }
                else
                {
                    reader.Close();
                    query = "INSERT INTO MonitoringFirma (IDNadgledanaFirma, IDMonitoring) VALUES (@idNadgledanaFirma,@idMonitoring)";
                    command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
                    command.Parameters.AddWithValue("@idNadgledanaFirma", idNadgledanaFirma);
                    command.Parameters.AddWithValue("@idMonitoring", idMonitoring);
                    command.ExecuteNonQuery();
                }

                query = "SELECT IDOsnovneInformacije FROM OsnovneInformacije WHERE IDNadgledanaFirma = @idNadgledanaFirma";
                command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
                command.Parameters.AddWithValue("@idNadgledanaFirma", idNadgledanaFirma);
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    reader.Close();
                    query = "UPDATE OsnovneInformacije SET Naziv = @naziv, Adresa = @adresa, Grad = @grad WHERE IDNadgledanaFirma = @idNadgledanaFirma";
                    command = new SqlCommand(query, DBCommOsnovnePostavke.connection);

                    command.Parameters.AddWithValue("@naziv", (nicsv.naziv == null) ? DBNull.Value : (object)nicsv.naziv);
                    command.Parameters.AddWithValue("@adresa", (nicsv.adresa == null) ? DBNull.Value : (object)nicsv.adresa);
                    command.Parameters.AddWithValue("@grad", (nicsv.mesto == null) ? DBNull.Value : (object)nicsv.mesto);
                    command.Parameters.AddWithValue("@idNadgledanaFirma", idNadgledanaFirma);
                    command.ExecuteNonQuery();
                }
                else
                {
                    reader.Close();
                    query = "INSERT INTO OsnovneInformacije (IDNadgledanaFirma, Naziv, Adresa, Grad) VALUES (@idNadgledanaFirma, @naziv, @adresa, @grad)";
                    command = new SqlCommand(query, DBCommOsnovnePostavke.connection);
                    command.Parameters.AddWithValue("@idNadgledanaFirma", idNadgledanaFirma);
                    command.Parameters.AddWithValue("@naziv", (nicsv.naziv == null) ? DBNull.Value : (object)nicsv.naziv);
                    command.Parameters.AddWithValue("@adresa", (nicsv.adresa == null) ? DBNull.Value : (object)nicsv.adresa);
                    command.Parameters.AddWithValue("@grad", (nicsv.mesto == null) ? DBNull.Value : (object)nicsv.mesto);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}