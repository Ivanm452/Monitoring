using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CsvHelper;
using Monitoring.DatabaseCommunication;
using System.Data.SqlClient;
using Monitoring.OsnovnePostavke;
using SpreadsheetLight;
using Monitoring.Classes.DatabaseCommunication;
using System.Globalization;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net.Mime;
using Monitoring.Classes.CSV;

namespace Monitoring.Classes
{
    class UcitavanjeGenerisanje
    {

        // ucitava source fajl u bazu, console1 i console2 su TextBox-ovi u kojima ispisuje
        public static void uploadSource(TextBox console1, TextBox console2, String filePath)
        {
            ActuallyPerformStep.performStepTxtBox(console1, "Pocinje upload novog source-a...", true);

            // deklaracija
            BlokadaIzCsv blcsv1, blcsv2;
            int i;
            DateTime pocetak;
            StreamReader sr;
            CsvReader csvread;
            int lineCount;

            // inicijalizacija
            blcsv1 = blcsv2 = null;
            i = 1;
            pocetak = DateTime.Now;
            sr = new StreamReader(filePath);
            csvread = new CsvReader(sr);
            lineCount = File.ReadLines(filePath).Count();


            DBBlokadeData.connection = new SqlConnection(DBBlokadeData.connectionString);
            DBBlokadeData.connection.Open();
            DBBlokadeData.shiftOld();

            csvread.Configuration.HasHeaderRecord = false;

            // U bazu se ubacuje samo poslednji red u grupi redova po maticnom broju. 
            // uporedjuju se blcsv2 i blcsv1 i ako je isti maticni menjaju se vrednosti
            // ako je maticni razlicit, prethodni se upisuje kao poslednji iz grupe
            while (csvread.Read())
            {
                blcsv2 = new BlokadaIzCsv(csvread);
                while (blcsv2.maticniBroj == null)//slucajno ako maticni broj ne postoji
                    blcsv2 = new BlokadaIzCsv(csvread);

                // ako je prvi prolaz u pitanju izjednacava ih
                if (i == 1)
                    blcsv1 = blcsv2;

                // kako ukupan broj dana postoji samo u prvom redu grupe 
                // ovo se prebacuje iz njega u sledeci
                // ako nisu isti maticni upisuje prethodni u bazu
                if (blcsv1.maticniBroj.Equals(blcsv2.maticniBroj))
                    blcsv2.ukupanBrojDana = blcsv1.ukupanBrojDana;
                else
                    DBBlokadeData.writeBlokadaIzCsv(blcsv1);
                blcsv1 = blcsv2; // zamenjuje im mesta

                ActuallyPerformStep.performStepTxtBox(console2, "Ucitan red: " + i++ + @"/" + lineCount, false);
            }
            DBBlokadeData.writeBlokadaIzCsv(blcsv2);
            DBBlokadeData.doneInserting();
            DBBlokadeData.connection.Close();
            sr.Close();
            ActuallyPerformStep.performStepTxtBox(console1, "Zavrseno ucitavanje. Vreme: " + (DateTime.Now - pocetak), true);
        }

        public static void uploadStatus(TextBox console1, TextBox console2, String filePath)
        {
            ActuallyPerformStep.performStepTxtBox(console1, "Pocinje upload novog statusa-a...", true);

            // deklaracija
            StatusIzCsv sic;
            StreamReader sr;
            CsvReader csvread;
            int i;

            // inicijalizacija
            sic = null;
            sr = new StreamReader(filePath);
            csvread = new CsvReader(sr);
            csvread.Configuration.HasHeaderRecord = false;
            i = 1;

            DBStatusData.connection = new SqlConnection(DBStatusData.connectionString);
            DBStatusData.connection.Open();
            DBStatusData.shiftOldStatuses();

            while (csvread.Read())
            {
                sic = new StatusIzCsv(csvread);
                if (!DBStatusData.writeStatusIzCsv(sic))
                    Console.WriteLine("Greska sa upisom statusa");

                ActuallyPerformStep.performStepTxtBox(console2, "Ucitan status: " + i++, false);

            }
            DBStatusData.connection.Close();
            sr.Close();
            ActuallyPerformStep.performStepTxtBox(console1, "Zavrseno ucitavanje statusa", true);
        }

        public static void generisiRezultat(TextBox console1, TextBox console2)
        {
            DateTime pocetak = DateTime.Now;

            ActuallyPerformStep.performStepTxtBox(console1, "Pocelo generisanje: " + pocetak, true);

            DBCommComparison.connection = new SqlConnection(DBCommComparison.connectionString);
            DBCommComparison.connection.Open();
            BlokadaIzBaze bib1, bib2;
            BlokadaRazlika br;
            int i = 1;
            DBCommComparison.clearRezultatBlokade();
            List<String> maticniBrojevi = DBCommComparison.getNadgledaneMaticne();

            foreach (string ss in maticniBrojevi)
            {
                bib1 = DBCommComparison.getBlokadaIzBaze(ss.Trim(), "N_1");
                bib2 = DBCommComparison.getBlokadaIzBaze(ss.Trim(), "N");

                if (bib1 != null && bib2 != null)
                {
                    br = Comparison.nadjiRazlike(bib1, bib2);
                    if (br != null)
                        DBCommComparison.writeRazlika(br);
                    else
                    {
                        Console.WriteLine("bib1 maticni: " + bib1);
                        Console.WriteLine("bib2 maticni: " + bib2);
                    }

                    ActuallyPerformStep.performStepTxtBox(console2, i++ + "/" + maticniBrojevi.Count + " Generisano za maticni: " + ss, false);
                }
                else if (bib1 == null && bib2 != null)
                {
                    DBCommComparison.writeRazlika(new BlokadaRazlika(bib2));
                    ActuallyPerformStep.performStepTxtBox(console2, i++ + "/" + maticniBrojevi.Count + " Generisano za maticni: " + ss, false);
                }
                else
                    DBGreska.addGreska("", "generisiRezultat", "Greska u generisanju, bib2 == null za maticni: " + ss.Trim());
            }
            DBCommComparison.connection.Close();
            ActuallyPerformStep.performStepTxtBox(console1, "Zavrseno generisanje. Vreme: " + (DateTime.Now - pocetak), true);
        }

        public static FileAndMails generisiIzvestaj(Klijent klijent, tblMonitoring m, TextBox txtConsole1, TextBox txtConsole2)
        {
            if (m.idVrstaMonitoringa == Rest.Parameters.ID_VRSTE_1.ToString())
                return generisiIzvestajTip1(klijent, m, txtConsole1, txtConsole2);
            else if (m.idVrstaMonitoringa == Rest.Parameters.ID_VRSTE_2.ToString())
                return generisiIzvestajTip2(klijent, m, txtConsole1, txtConsole2);
            else if (m.idVrstaMonitoringa == Rest.Parameters.ID_VRSTE_3.ToString())
                return generisiIzvestajTip3(klijent, m, txtConsole1, txtConsole2);
            else if (m.idVrstaMonitoringa == Rest.Parameters.ID_VRSTE_4.ToString())
                return generisiIzvestajTip4(klijent, m, txtConsole1, txtConsole2);
            else if (m.idVrstaMonitoringa == Rest.Parameters.ID_VRSTE_5.ToString())
                return generisiIzvestajTip5(klijent, m, txtConsole1, txtConsole2);
            else if (m.idVrstaMonitoringa == Rest.Parameters.ID_VRSTE_6.ToString())
                return generisiIzvestajTip6(klijent, m, txtConsole1, txtConsole2);
            else if (m.idVrstaMonitoringa == Rest.Parameters.ID_VRSTE_7.ToString())
                return generisiIzvestajTip7(klijent, m, txtConsole1, txtConsole2);
            else if (m.idVrstaMonitoringa == Rest.Parameters.ID_VRSTE_9.ToString())
                return generisiIzvestajTip9(klijent, m, txtConsole1, txtConsole2);
            else if (m.idVrstaMonitoringa == Rest.Parameters.ID_VRSTE_10.ToString())
                return generisiIzvestajTip10(klijent, m, txtConsole1, txtConsole2);
            else
            {
                DBGreska.addGreska(klijent.maticniBroj, "Generisanje izvestaja", "Nepostojeci tip monitoringa");
                return null;
            }
        }

        public static FileAndMails generisiIzvestajTip1(Klijent klijent, tblMonitoring m, TextBox txtConsole1, TextBox txtConsole2)
        {
            BlokadaRazlika br = null;
            int i = 0;
            SLDocument sl;
            DateTime pocetak = DateTime.Now;
            int aktivni = 0;
            int blokirani = 0;
            bool potencijalnoNeaktiva = false;
            bool maticniNePostoji = false;

            DBExcel.connection = new SqlConnection(DBExcel.connectionString);
            DBExcel.connection.Open();


            sl = new SLDocument(Properties.Settings.Default.FILE_TEMPLATE_PATH);

            foreach (String ss in m.lstMaticniBrojevi)
            {
                br = DBExcel.getBlokadaRazlika(ss.Trim());
                if (!potencijalnoNeaktiva)
                    potencijalnoNeaktiva = DBExcel.getPotencijalnoNeaktivna(ss.Trim());

                if (br.maticniBroj == null)
                {
                    DBGreska.addGreska(ss.Trim(), "Generisanje izvestaja", "Maticni broj ne postoji");
                    maticniNePostoji = true;
                    continue;
                }
                if (br.status == "Aktivan")
                    aktivni++;
                else
                    blokirani++;

                upisiUFajlTip1(br, i++, sl);
                ActuallyPerformStep.performStepTxtBox(txtConsole2, "Upisan: " + i + "/" + m.lstMaticniBrojevi.Count, false);
            }
            sl.RenameWorksheet(SLDocument.DefaultFirstSheetName, klijent.naziv);
            sl.SetCellValue("F5", klijent.naziv);
            sl.SetCellValue("L2", DateTime.Now.ToString("dd. MMMM yyyy", CultureInfo.GetCultureInfo("sr-Latn-CS")));
            sl.SetCellValue("L3", m.lstMaticniBrojevi.Count);
            sl.SetCellValue("L4", aktivni);
            sl.SetCellValue("L5", blokirani);

            DBExcel.connection.Close();
            String filepath = Properties.Settings.Default.LOCAL_RESULT_DIRECTORY + "\\" + "Qbing" + " - " + m.naziv + " - " + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            sl.SaveAs(filepath);

            ActuallyPerformStep.performStepTxtBox(txtConsole1, "Zavrseno generisanje za MB: " + klijent.naziv + " Vreme: " + (DateTime.Now - pocetak), true);

            if (m.saljeSeMail == "1")
                return new FileAndMails(filepath, m.lstMail, false, potencijalnoNeaktiva, maticniNePostoji);
            return null;
        }

        public static void upisiUFajlTip1(BlokadaRazlika br, int i, SLDocument sl)
        {
            SLComment comm;
            SLStyle globalFontStyle = sl.CreateStyle();
            globalFontStyle.Font.FontName = "Century Gothic";
            globalFontStyle.Font.FontSize = 8;

            // redni broj
            sl.SetCellValue(8 + i, 1, i + 1);
            sl.SetCellStyle(8 + i, 1, globalFontStyle);

            // maticni broj
            sl.SetCellValue(8 + i, 2, br.maticniBroj);
            sl.SetCellStyle(8 + i, 2, globalFontStyle);

            // promena statusa
            SLStyle style1 = sl.CreateStyle();
            style1.Font.FontName = "Century Gothic";
            style1.Font.FontSize = 8;
            style1.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.FromArgb(0, 176, 240), SLThemeColorIndexValues.Light2Color, 22);
            if (br.promenjenStatus.Equals("True"))
            {
                sl.SetCellValue(8 + i, 3, br.status);
                sl.SetCellStyle(8 + i, 3, style1);
            }
            else
            {
                sl.SetCellValue(8 + i, 3, br.status);
                sl.SetCellStyle(8 + i, 3, globalFontStyle);
            }

            if (br.zabranaPrenosa.Length > 2)
            {
                //sl.SetCellStyle(8 + i, 3, style2);
                comm = sl.CreateComment();
                comm.AutoSize = true;
                comm.SetText(br.zabranaPrenosa);
                sl.InsertComment(8 + i, 3, comm);
            }

            // Naziv
            if (br.naziv != null)
                sl.SetCellValue(8 + i, 4, br.naziv);
            sl.SetCellStyle(8 + i, 4, globalFontStyle);

            // Adresa
            sl.SetCellValue(8 + i, 5, br.adresa);
            sl.SetCellStyle(8 + i, 5, globalFontStyle);

            // Grad
            sl.SetCellValue(8 + i, 6, br.grad);
            sl.SetCellStyle(8 + i, 6, globalFontStyle);

            // DatumOd
            globalFontStyle.Alignment.Horizontal = HorizontalAlignmentValues.Center;
            sl.SetCellStyle(8 + i, 7, globalFontStyle);
            if (br.datumOd != null && br.datumOd != "")
                sl.SetCellValue(8 + i, 7, DateTime.ParseExact(br.datumOd, "m/d/yyyy", CultureInfo.InvariantCulture).ToString("dd.mm.yyyy"));

            // Iznos
            //PROVERA ZASTO JE BR VRATIO KAO NULL
            SLStyle style2 = sl.CreateStyle();
            style2.Font.FontName = "Century Gothic";
            style2.Font.FontSize = 8;
            style2.FormatCode = "#,###";
            style2.Alignment.Horizontal = HorizontalAlignmentValues.Center;
            if (br.iznos != null && br.iznos != "")
            {
                sl.SetCellStyle(8 + i, 8, style2);
                String broj = br.iznos.Substring(0, br.iznos.Length - 3);
                broj = broj.Replace(",", string.Empty);
                long iznos = long.Parse(broj);
                sl.SetCellValue(8 + i, 8, iznos);
            }
            else
                sl.SetCellValue(8 + i, 8, br.iznos);

            // iznos promene
            if (br.iznosPromene.Contains("."))
                br.iznosPromene = br.iznosPromene.Substring(0, br.iznosPromene.IndexOf('.'));
            sl.SetCellStyle(8 + i, 9, style2);
            if (br.iznosPromene == "0" || br.iznosPromene == "")
                sl.SetCellValue(8 + i, 9, "");
            else
            {
                long iznos = long.Parse(br.iznosPromene);
                sl.SetCellValue(8 + i, 9, iznos);
            }

            if (br.povecanIznos.Equals("True"))
            {
                sl.SetCellValue(8 + i, 10, "Povećan");
                sl.SetCellStyle(8 + i, 10, style1);
                sl.SetCellStyle(8 + i, 9, style1);
                sl.SetCellStyle(8 + i, 8, style1);

            }
            else if (br.smanjenIznos.Equals("True"))
            {
                sl.SetCellValue(8 + i, 10, "Smanjen");
                sl.SetCellStyle(8 + i, 10, style1);
                sl.SetCellStyle(8 + i, 9, style1);
                sl.SetCellStyle(8 + i, 8, style1);
            }
            else if (br.povecanIznos.Equals("False") && br.smanjenIznos.Equals("False") && br.status == "Blokiran")
            {
                if (br.promenjenStatus.Equals("True"))
                    sl.SetCellValue(8 + i, 10, "");
                else if (br.iznos != "")
                    sl.SetCellValue(8 + i, 10, "Nepromenjen");
            }
            else
                sl.SetCellValue(8 + i, 10, "");

            sl.SetCellStyle(8 + i, 11, globalFontStyle);
            sl.SetCellStyle(8 + i, 12, globalFontStyle);

            // broj dana
            if (br.brojDana != null && br.brojDana != "")
                sl.SetCellValue(8 + i, 11, int.Parse(br.brojDana));

            // ukupan broj dana
            if (br.ukupanBrojDana != null && br.ukupanBrojDana != "" && br.ukupanBrojDana != "0")
                sl.SetCellValue(8 + i, 12, int.Parse(br.ukupanBrojDana));
        }

        public static FileAndMails generisiIzvestajTip2(Klijent klijent, tblMonitoring m, TextBox txtConsole1, TextBox txtConsole2)
        {
            BlokadaRazlika br = null;
            int i = 0;
            SLDocument sl;
            DateTime pocetak = DateTime.Now;
            int aktivni = 0;
            int blokirani = 0;
            bool potencijalnoNeaktiva = false;
            bool maticniNePostoji = false;

            DBExcel.connection = new SqlConnection(DBExcel.connectionString);
            DBExcel.connection.Open();


            sl = new SLDocument(Properties.Settings.Default.FILE_TEMPLATE2_PATH);

            foreach (String ss in m.lstMaticniBrojevi)
            {
                br = DBExcel.getBlokadaRazlika(ss.Trim());
                if (!potencijalnoNeaktiva)
                    potencijalnoNeaktiva = DBExcel.getPotencijalnoNeaktivna(ss.Trim());

                if (br.maticniBroj == null)
                {
                    DBGreska.addGreska(ss.Trim(), "Generisanje izvestaja", "Maticni broj ne postoji");
                    maticniNePostoji = true;
                    continue;
                }
                if (br.status == "Aktivan")
                    aktivni++;
                else
                    blokirani++;

                upisiUFajlTip2(br, i++, sl);
                ActuallyPerformStep.performStepTxtBox(txtConsole2, "Upisan: " + i + "/" + m.lstMaticniBrojevi.Count, false);
            }
            sl.RenameWorksheet(SLDocument.DefaultFirstSheetName, klijent.naziv);
            sl.SetCellValue("H5", klijent.naziv);
            sl.SetCellValue("N2", DateTime.Now.ToString("dd. MMMM yyyy", CultureInfo.GetCultureInfo("sr-Latn-CS")));
            sl.SetCellValue("N3", m.lstMaticniBrojevi.Count);
            sl.SetCellValue("N4", aktivni);
            sl.SetCellValue("N5", blokirani);

            DBExcel.connection.Close();
            String filepath = Properties.Settings.Default.LOCAL_RESULT_DIRECTORY + "\\" + "Qbing" + " - " + m.naziv + " - " + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            sl.SaveAs(filepath);

            ActuallyPerformStep.performStepTxtBox(txtConsole1, "Zavrseno generisanje za MB: " + klijent.naziv + " Vreme: " + (DateTime.Now - pocetak), true);

            if (m.saljeSeMail == "1")
                return new FileAndMails(filepath, m.lstMail, false, potencijalnoNeaktiva, maticniNePostoji);
            return null;
        }

        public static void upisiUFajlTip2(BlokadaRazlika br, int i, SLDocument sl)
        {
            SLComment comm;
            SLStyle globalFontStyle = sl.CreateStyle();
            globalFontStyle.Font.FontName = "Century Gothic";
            globalFontStyle.Font.FontSize = 8;

            // redni broj
            sl.SetCellValue(8 + i, 1, i + 1);
            sl.SetCellStyle(8 + i, 1, globalFontStyle);

            // maticni broj
            sl.SetCellValue(8 + i, 2, br.maticniBroj);
            sl.SetCellStyle(8 + i, 2, globalFontStyle);

            // promena statusa
            // SLStyle style1 = sl.CreateStyle();
            // style1.Font.FontName = "Century Gothic";
            // style1.Font.FontSize = 8;
            // style1.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.FromArgb(0, 176, 240), SLThemeColorIndexValues.Light2Color, 22);
            if (br.promenjenStatus.Equals("True"))
            {
                sl.SetCellStyle(8 + i, 3, globalFontStyle);
                sl.SetCellStyle(8 + i, 4, globalFontStyle);
                sl.SetCellValue(8 + i, 3, br.status);
                sl.SetCellValue(8 + i, 4, "x");
            }
            else
            {
                sl.SetCellValue(8 + i, 3, br.status);
                sl.SetCellStyle(8 + i, 3, globalFontStyle);
            }

            if (br.zabranaPrenosa.Length > 2)
            {
                comm = sl.CreateComment();
                comm.AutoSize = true;
                comm.SetText(br.zabranaPrenosa);
                sl.InsertComment(8 + i, 3, comm);
            }

            // Naziv
            if (br.naziv != null)
                sl.SetCellValue(8 + i, 5, br.naziv);
            sl.SetCellStyle(8 + i, 5, globalFontStyle);

            // Adresa
            sl.SetCellValue(8 + i, 6, br.adresa);
            sl.SetCellStyle(8 + i, 6, globalFontStyle);

            // Grad
            sl.SetCellValue(8 + i, 7, br.grad);
            sl.SetCellStyle(8 + i, 7, globalFontStyle);

            // DatumOd
            globalFontStyle.Alignment.Horizontal = HorizontalAlignmentValues.Center;
            sl.SetCellStyle(8 + i, 8, globalFontStyle);
            if (br.datumOd != null && br.datumOd != "")
                sl.SetCellValue(8 + i, 8, DateTime.ParseExact(br.datumOd, "m/d/yyyy", CultureInfo.InvariantCulture).ToString("dd.mm.yyyy"));

            // Iznos
            //PROVERA ZASTO JE BR VRATIO KAO NULL
            SLStyle style2 = sl.CreateStyle();
            style2.Font.FontName = "Century Gothic";
            style2.Font.FontSize = 8;
            style2.FormatCode = "#,###";
            style2.Alignment.Horizontal = HorizontalAlignmentValues.Center;
            if (br.iznos != null && br.iznos != "")
            {
                sl.SetCellStyle(8 + i, 9, style2);

                String broj = br.iznos.Substring(0, br.iznos.Length - 3);
                broj = broj.Replace(",", string.Empty);
                long iznos = long.Parse(broj);
                sl.SetCellValue(8 + i, 9, iznos);
            }
            else
                sl.SetCellValue(8 + i, 9, br.iznos);

            // iznos promene
            if (br.iznosPromene.Contains("."))
                br.iznosPromene = br.iznosPromene.Substring(0, br.iznosPromene.IndexOf('.'));
            sl.SetCellStyle(8 + i, 10, style2);
            if (br.iznosPromene == "0" || br.iznosPromene == "")
                sl.SetCellValue(8 + i, 10, "");
            else
            {
                long iznos = long.Parse(br.iznosPromene);
                sl.SetCellValue(8 + i, 10, iznos);
            }

            if (br.povecanIznos.Equals("True"))
            {
                sl.SetCellValue(8 + i, 11, "Povećan");

            }
            else if (br.smanjenIznos.Equals("True"))
            {
                sl.SetCellValue(8 + i, 11, "Smanjen");
            }
            else if (br.povecanIznos.Equals("False") && br.smanjenIznos.Equals("False") && br.status == "Blokiran")
            {
                if (br.promenjenStatus.Equals("True"))
                    sl.SetCellValue(8 + i, 11, "");
                else if (br.iznos != "")
                    sl.SetCellValue(8 + i, 11, "Nepromenjen");
            }
            else
                sl.SetCellValue(8 + i, 11, "");

            sl.SetCellStyle(8 + i, 12, globalFontStyle);
            sl.SetCellStyle(8 + i, 13, globalFontStyle);

            // broj dana
            if (br.brojDana != null && br.brojDana != "")
                sl.SetCellValue(8 + i, 12, int.Parse(br.brojDana));

            // ukupan broj dana
            if (br.ukupanBrojDana != null && br.ukupanBrojDana != "" && br.ukupanBrojDana != "0")
                sl.SetCellValue(8 + i, 13, int.Parse(br.ukupanBrojDana));

            globalFontStyle.Alignment.Horizontal = HorizontalAlignmentValues.Left;
            if (br.promenjenStatusKompanije == "true")
            {
                sl.SetCellValue(8 + i, 14, br.statusKompanije);
                sl.SetCellValue(8 + i, 15, "x");
                sl.SetCellStyle(8 + i, 14, globalFontStyle);
            }
            else
            {
                if (br.statusKompanije == "")
                    sl.SetCellValue(8 + i, 14, "-");
                else
                    sl.SetCellValue(8 + i, 14, br.statusKompanije);
                sl.SetCellStyle(8 + i, 14, globalFontStyle);

            }
        }

        public static FileAndMails generisiIzvestajTip3(Klijent klijent, tblMonitoring m, TextBox txtConsole1, TextBox txtConsole2)
        {
            BlokadaRazlika br = null;
            int i = 0;
            SLDocument sl;
            DateTime pocetak = DateTime.Now;
            int aktivni = 0;
            int blokirani = 0;
            bool potencijalnoNeaktiva = false;
            bool maticniNePostoji = false;

            DBExcel.connection = new SqlConnection(DBExcel.connectionString);
            DBExcel.connection.Open();


            sl = new SLDocument(Properties.Settings.Default.FILE_TEMPLATE3_PATH);

            foreach (String ss in m.lstMaticniBrojevi)
            {
                br = DBExcel.getBlokadaRazlika(ss.Trim());
                if (!potencijalnoNeaktiva)
                    potencijalnoNeaktiva = DBExcel.getPotencijalnoNeaktivna(ss.Trim());

                if (br.maticniBroj == null)
                {
                    DBGreska.addGreska(ss.Trim(), "Generisanje izvestaja", "Maticni broj ne postoji");
                    maticniNePostoji = true;
                    continue;
                }
                if (br.status == "Aktivan")
                    aktivni++;
                else
                    blokirani++;

                upisiUFajlTip3(br, i++, sl);
                ActuallyPerformStep.performStepTxtBox(txtConsole2, "Upisan: " + i + "/" + m.lstMaticniBrojevi.Count, false);
            }
            sl.RenameWorksheet(SLDocument.DefaultFirstSheetName, klijent.naziv);
            sl.SetCellValue("F5", klijent.naziv);
            sl.SetCellValue("L2", DateTime.Now.ToString("dd. MMMM yyyy", CultureInfo.GetCultureInfo("sr-Latn-CS")));
            sl.SetCellValue("L3", m.lstMaticniBrojevi.Count);
            sl.SetCellValue("L4", aktivni);
            sl.SetCellValue("L5", blokirani);

            DBExcel.connection.Close();
            String filepath = Properties.Settings.Default.LOCAL_RESULT_DIRECTORY + "\\" + "Qbing" + " - " + m.naziv + " - " + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            sl.SaveAs(filepath);

            ActuallyPerformStep.performStepTxtBox(txtConsole1, "Zavrseno generisanje za MB: " + klijent.naziv + " Vreme: " + (DateTime.Now - pocetak), true);

            if (m.saljeSeMail == "1")
                return new FileAndMails(filepath, m.lstMail, false, potencijalnoNeaktiva, maticniNePostoji);
            return null;
        }

        public static void upisiUFajlTip3(BlokadaRazlika br, int i, SLDocument sl)
        {
            SLComment comm;
            SLStyle globalFontStyle = sl.CreateStyle();
            globalFontStyle.Font.FontName = "Century Gothic";
            globalFontStyle.Font.FontSize = 8;

            // redni broj
            sl.SetCellValue(8 + i, 1, i + 1);
            sl.SetCellStyle(8 + i, 1, globalFontStyle);

            // maticni broj
            sl.SetCellValue(8 + i, 2, br.maticniBroj);
            sl.SetCellStyle(8 + i, 2, globalFontStyle);

            // promena statusa
            SLStyle style1 = sl.CreateStyle();
            style1.Font.FontName = "Century Gothic";
            style1.Font.FontSize = 8;
            style1.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.FromArgb(0, 176, 240), SLThemeColorIndexValues.Light2Color, 22);
            if (br.promenjenStatus.Equals("True"))
            {
                sl.SetCellValue(8 + i, 3, br.status);
                sl.SetCellStyle(8 + i, 3, style1);
            }
            else
            {
                sl.SetCellValue(8 + i, 3, br.status);
                sl.SetCellStyle(8 + i, 3, globalFontStyle);
            }

            if (br.zabranaPrenosa.Length > 2)
            {
                comm = sl.CreateComment();
                comm.AutoSize = true;
                comm.SetText(br.zabranaPrenosa);
                sl.InsertComment(8 + i, 3, comm);
            }

            // Naziv
            if (br.naziv != null)
                sl.SetCellValue(8 + i, 4, br.naziv);
            sl.SetCellStyle(8 + i, 4, globalFontStyle);

            // Adresa
            sl.SetCellValue(8 + i, 5, br.adresa);
            sl.SetCellStyle(8 + i, 5, globalFontStyle);

            // Grad
            sl.SetCellValue(8 + i, 6, br.grad);
            sl.SetCellStyle(8 + i, 6, globalFontStyle);

            // DatumOd
            globalFontStyle.Alignment.Horizontal = HorizontalAlignmentValues.Center;
            sl.SetCellStyle(8 + i, 7, globalFontStyle);
            if (br.datumOd != null && br.datumOd != "")
                sl.SetCellValue(8 + i, 7, DateTime.ParseExact(br.datumOd, "m/d/yyyy", CultureInfo.InvariantCulture).ToString("dd.mm.yyyy"));

            // Iznos
            //PROVERA ZASTO JE BR VRATIO KAO NULL
            SLStyle style2 = sl.CreateStyle();
            style2.Font.FontName = "Century Gothic";
            style2.Font.FontSize = 8;
            style2.FormatCode = "#,###";
            style2.Alignment.Horizontal = HorizontalAlignmentValues.Center;
            if (br.iznos != null && br.iznos != "")
            {
                sl.SetCellStyle(8 + i, 8, style2);

                String broj = br.iznos.Substring(0, br.iznos.Length - 3);
                broj = broj.Replace(",", string.Empty);
                long iznos = long.Parse(broj);
                sl.SetCellValue(8 + i, 8, iznos);
            }
            else
                sl.SetCellValue(8 + i, 8, br.iznos);

            // iznos promene
            if (br.iznosPromene.Contains("."))
                br.iznosPromene = br.iznosPromene.Substring(0, br.iznosPromene.IndexOf('.'));
            sl.SetCellStyle(8 + i, 9, style2);
            if (br.iznosPromene == "0" || br.iznosPromene == "")
                sl.SetCellValue(8 + i, 9, "");
            else
            {
                long iznos = long.Parse(br.iznosPromene);
                sl.SetCellValue(8 + i, 9, iznos);
            }

            if (br.povecanIznos.Equals("True"))
            {
                sl.SetCellValue(8 + i, 10, "Povećan");
                sl.SetCellStyle(8 + i, 10, style1);
                sl.SetCellStyle(8 + i, 9, style1);
                sl.SetCellStyle(8 + i, 8, style1);

            }
            else if (br.smanjenIznos.Equals("True"))
            {
                sl.SetCellValue(8 + i, 10, "Smanjen");
                sl.SetCellStyle(8 + i, 10, style1);
                sl.SetCellStyle(8 + i, 9, style1);
                sl.SetCellStyle(8 + i, 8, style1);
            }
            else if (br.povecanIznos.Equals("False") && br.smanjenIznos.Equals("False") && br.status == "Blokiran")
            {
                if (br.promenjenStatus.Equals("True"))
                    sl.SetCellValue(8 + i, 10, "");
                else if (br.iznos != "")
                    sl.SetCellValue(8 + i, 10, "Nepromenjen");
            }
            else
                sl.SetCellValue(8 + i, 10, "");

            sl.SetCellStyle(8 + i, 11, globalFontStyle);
            sl.SetCellStyle(8 + i, 12, globalFontStyle);

            // broj dana
            if (br.brojDana != null && br.brojDana != "")
                sl.SetCellValue(8 + i, 11, int.Parse(br.brojDana));

            // ukupan broj dana
            if (br.ukupanBrojDana != null && br.ukupanBrojDana != "" && br.ukupanBrojDana != "0")
                sl.SetCellValue(8 + i, 12, int.Parse(br.ukupanBrojDana));

            globalFontStyle.Alignment.Horizontal = HorizontalAlignmentValues.Left;
            if (br.promenjenStatusKompanije == "true")
            {
                sl.SetCellValue(8 + i, 13, br.statusKompanije);
                sl.SetCellStyle(8 + i, 13, style1);
            }
            else
            {
                if (br.statusKompanije == "")
                    sl.SetCellValue(8 + i, 13, "-");
                else
                    sl.SetCellValue(8 + i, 13, br.statusKompanije);
                sl.SetCellStyle(8 + i, 13, globalFontStyle);

            }
        }


        public static FileAndMails generisiIzvestajTip4(Klijent klijent, tblMonitoring m, TextBox txtConsole1, TextBox txtConsole2)
        {
            BlokadaRazlika br = null;
            int i = 0;
            SLDocument sl;
            DateTime pocetak = DateTime.Now;
            int aktivni = 0;
            int blokirani = 0;
            bool potencijalnoNeaktiva = false;
            bool maticniNePostoji = false;

            int brojPromenaBlokada = 0;
            int brojPromenaIznosa = 0;
            int brojPromenaStatusa = 0;

            DBExcel.connection = new SqlConnection(DBExcel.connectionString);
            DBExcel.connection.Open();


            sl = new SLDocument(Properties.Settings.Default.FILE_TEMPLATE4_PATH);

            foreach (String ss in m.lstMaticniBrojevi)
            {
                br = DBExcel.getBlokadaRazlika(ss.Trim());
                if (!potencijalnoNeaktiva)
                    potencijalnoNeaktiva = DBExcel.getPotencijalnoNeaktivna(ss.Trim());

                if (br.maticniBroj == null)
                {
                    DBGreska.addGreska(ss.Trim(), "Generisanje izvestaja", "Maticni broj ne postoji");
                    maticniNePostoji = true;
                    continue;
                }
                if (br.status == "Aktivan")
                    aktivni++;
                else
                    blokirani++;

                upisiUFajlTip4(br, i++, sl, ref brojPromenaBlokada, ref brojPromenaIznosa, ref brojPromenaStatusa);
                ActuallyPerformStep.performStepTxtBox(txtConsole2, "Upisan: " + i + "/" + m.lstMaticniBrojevi.Count, false);
            }

            sl.RenameWorksheet(SLDocument.DefaultFirstSheetName, klijent.naziv);
            sl.SetCellValue("F2", klijent.naziv);
            sl.SetCellValue("F3", DateTime.Now.ToString("dd. MMMM yyyy", CultureInfo.GetCultureInfo("sr-Latn-CS")));
            sl.SetCellValue("E5", m.lstMaticniBrojevi.Count);
            sl.SetCellValue("E6", aktivni);
            sl.SetCellValue("E7", blokirani);

            sl.SetCellValue("H5", brojPromenaBlokada);
            sl.SetCellValue("K5", brojPromenaIznosa);
            sl.SetCellValue("N5", brojPromenaStatusa);

            DBExcel.connection.Close();
            String filepath = Properties.Settings.Default.LOCAL_RESULT_DIRECTORY + "\\" + "Qbing" + " - " + m.naziv + " - " + DateTime.Now.ToString("yyyyMMdd") + ".xlsm";
            sl.SaveAs(filepath);

            ActuallyPerformStep.performStepTxtBox(txtConsole1, "Zavrseno generisanje za MB: " + klijent.naziv + " Vreme: " + (DateTime.Now - pocetak), true);

            if (m.saljeSeMail == "1")
                return new FileAndMails(filepath, m.lstMail, false, potencijalnoNeaktiva, maticniNePostoji);
            return null;
        }

        public static void upisiUFajlTip4(BlokadaRazlika br, int i, SLDocument sl, ref int brojPromenaBlokada, ref  int brojPromenaIznosa, ref  int brojPromenaStatusa)
        {
            SLComment comm;
            SLStyle globalFontStyle = sl.CreateStyle();
            globalFontStyle.Font.FontName = "Century Gothic";
            globalFontStyle.Font.FontSize = 8;

            // redni broj
            sl.SetCellValue(9 + i, 1, i + 1);
            sl.SetCellStyle(9 + i, 1, globalFontStyle);

            // mesto za pib
            sl.SetCellValue(9 + i, 2, br.pib);
            sl.SetCellStyle(9 + i, 2, globalFontStyle);


            // maticni broj
            sl.SetCellValue(9 + i, 3, br.maticniBroj);
            sl.SetCellStyle(9 + i, 3, globalFontStyle);

            // promena statusa
            SLStyle style1 = sl.CreateStyle();
            style1.Font.FontName = "Century Gothic";
            style1.Font.FontSize = 8;
            style1.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.FromArgb(0, 176, 240), SLThemeColorIndexValues.Light2Color, 22);
            if (br.promenjenStatus.Equals("True"))
            {
                sl.SetCellValue(9 + i, 7, br.status);
                sl.SetCellStyle(9 + i, 7, style1);
                brojPromenaBlokada++;
            }
            else
            {
                sl.SetCellValue(9 + i, 7, br.status);
                sl.SetCellStyle(9 + i, 7, globalFontStyle);
            }

            if (br.zabranaPrenosa.Length > 2)
            {
                comm = sl.CreateComment();
                comm.AutoSize = true;
                comm.SetText(br.zabranaPrenosa);
                sl.InsertComment(9 + i, 7, comm);
            }

            // Naziv
            if (br.naziv != null)
                sl.SetCellValue(9 + i, 4, br.naziv);
            sl.SetCellStyle(9 + i, 4, globalFontStyle);

            // Adresa
            sl.SetCellValue(9 + i, 5, br.adresa);
            sl.SetCellStyle(9 + i, 5, globalFontStyle);

            // Grad
            sl.SetCellValue(9 + i, 6, br.grad);
            sl.SetCellStyle(9 + i, 6, globalFontStyle);

            // DatumOd
            globalFontStyle.Alignment.Horizontal = HorizontalAlignmentValues.Center;
            sl.SetCellStyle(9 + i, 7, globalFontStyle);
            if (br.datumOd != null && br.datumOd != "")
                sl.SetCellValue(9 + i, 8, DateTime.ParseExact(br.datumOd, "m/d/yyyy", CultureInfo.InvariantCulture).ToString("dd.mm.yyyy"));

            // Iznos
            //PROVERA ZASTO JE BR VRATIO KAO NULL
            SLStyle style2 = sl.CreateStyle();
            style2.Font.FontName = "Century Gothic";
            style2.Font.FontSize = 8;
            style2.FormatCode = "#,###";
            style2.Alignment.Horizontal = HorizontalAlignmentValues.Center;
            if (br.iznos != null && br.iznos != "")
            {
                sl.SetCellStyle(9 + i, 9, style2);

                String broj = br.iznos.Substring(0, br.iznos.Length - 3);
                broj = broj.Replace(",", string.Empty);
                long iznos = long.Parse(broj);
                sl.SetCellValue(9 + i, 9, iznos);
            }
            else
                sl.SetCellValue(9 + i, 9, br.iznos);

            // iznos promene
            if (br.iznosPromene.Contains("."))
                br.iznosPromene = br.iznosPromene.Substring(0, br.iznosPromene.IndexOf('.'));
            sl.SetCellStyle(9 + i, 10, style2);
            if (br.iznosPromene == "0" || br.iznosPromene == "")
                sl.SetCellValue(9 + i, 10, "");
            else
            {
                long iznos = long.Parse(br.iznosPromene);
                sl.SetCellValue(9 + i, 10, iznos);
            }

            if (br.povecanIznos.Equals("True"))
            {
                sl.SetCellValue(9 + i, 11, "Povećan");
                sl.SetCellStyle(9 + i, 11, style1);
                sl.SetCellStyle(9 + i, 10, style1);
                sl.SetCellStyle(9 + i, 9, style1);
                brojPromenaIznosa++;

            }
            else if (br.smanjenIznos.Equals("True"))
            {
                sl.SetCellValue(9 + i, 11, "Smanjen");
                sl.SetCellStyle(9 + i, 11, style1);
                sl.SetCellStyle(9 + i, 10, style1);
                sl.SetCellStyle(9 + i, 9, style1);
                brojPromenaIznosa++;
            }
            else if (br.povecanIznos.Equals("False") && br.smanjenIznos.Equals("False") && br.status == "Blokiran")
            {
                if (br.promenjenStatus.Equals("True"))
                    sl.SetCellValue(9 + i, 11, "");
                else if (br.iznos != "")
                    sl.SetCellValue(9 + i, 11, "Nepromenjen");
            }
            else
                sl.SetCellValue(9 + i, 11, "");

            sl.SetCellStyle(9 + i, 12, globalFontStyle);
            sl.SetCellStyle(9 + i, 13, globalFontStyle);

            // broj dana
            if (br.brojDana != null && br.brojDana != "")
                sl.SetCellValue(9 + i, 12, int.Parse(br.brojDana));

            // ukupan broj dana
            if (br.ukupanBrojDana != null && br.ukupanBrojDana != "" && br.ukupanBrojDana != "0")
                sl.SetCellValue(9 + i, 13, int.Parse(br.ukupanBrojDana));

            globalFontStyle.Alignment.Horizontal = HorizontalAlignmentValues.Left;
            if (br.promenjenStatusKompanije == "true")
            {
                sl.SetCellValue(9 + i, 14, br.statusKompanije);
                sl.SetCellStyle(9 + i, 14, style1);
                brojPromenaStatusa++;
            }
            else
            {
                if (br.statusKompanije == "")
                    sl.SetCellValue(9 + i, 14, "-");
                else
                    sl.SetCellValue(9 + i, 14, br.statusKompanije);
                sl.SetCellStyle(9 + i, 14, globalFontStyle);

            }
        }

        public static FileAndMails generisiIzvestajTip5(Klijent klijent, tblMonitoring m, TextBox txtConsole1, TextBox txtConsole2)
        {
            BlokadaRazlika br = null;
            int i = 0;
            SLDocument sl;
            DateTime pocetak = DateTime.Now;
            int aktivni = 0;
            int blokirani = 0;
            bool potencijalnoNeaktiva = false;
            bool maticniNePostoji = false;

            int brojPromenaBlokada = 0;
            int brojPromenaIznosa = 0;
            int brojPromenaStatusa = 0;

            DBExcel.connection = new SqlConnection(DBExcel.connectionString);
            DBExcel.connection.Open();


            sl = new SLDocument(Properties.Settings.Default.FILE_TEMPLATE5_PATH);

            foreach (String ss in m.lstMaticniBrojevi)
            {
                br = DBExcel.getBlokadaRazlika(ss.Trim());
                if (!potencijalnoNeaktiva)
                    potencijalnoNeaktiva = DBExcel.getPotencijalnoNeaktivna(ss.Trim());

                if (br.maticniBroj == null)
                {
                    DBGreska.addGreska(ss.Trim(), "Generisanje izvestaja", "Maticni broj ne postoji");
                    maticniNePostoji = true;
                    continue;
                }
                if (br.status == "Aktivan")
                    aktivni++;
                else
                    blokirani++;

                upisiUFajlTip5(br, i++, sl, ref brojPromenaBlokada, ref brojPromenaIznosa, ref brojPromenaStatusa);
                ActuallyPerformStep.performStepTxtBox(txtConsole2, "Upisan: " + i + "/" + m.lstMaticniBrojevi.Count, false);
            }

            sl.RenameWorksheet(SLDocument.DefaultFirstSheetName, klijent.naziv);
            sl.SetCellValue("F2", klijent.naziv);
            sl.SetCellValue("F3", DateTime.Now.ToString("dd. MMMM yyyy", CultureInfo.GetCultureInfo("sr-Latn-CS")));
            sl.SetCellValue("E5", m.lstMaticniBrojevi.Count);
            sl.SetCellValue("E6", aktivni);
            sl.SetCellValue("E7", blokirani);

            sl.SetCellValue("H5", brojPromenaBlokada);
            sl.SetCellValue("K5", brojPromenaIznosa);
            //sl.SetCellValue("N5", brojPromenaStatusa);

            DBExcel.connection.Close();
            String filepath = Properties.Settings.Default.LOCAL_RESULT_DIRECTORY + "\\" + "Qbing" + " - " + m.naziv + " - " + DateTime.Now.ToString("yyyyMMdd") + ".xlsm";
            sl.SaveAs(filepath);

            ActuallyPerformStep.performStepTxtBox(txtConsole1, "Zavrseno generisanje za MB: " + klijent.naziv + " Vreme: " + (DateTime.Now - pocetak), true);

            if (m.saljeSeMail == "1")
                return new FileAndMails(filepath, m.lstMail, false, potencijalnoNeaktiva, maticniNePostoji);
            return null;
        }

        public static void upisiUFajlTip5(BlokadaRazlika br, int i, SLDocument sl, ref int brojPromenaBlokada, ref  int brojPromenaIznosa, ref  int brojPromenaStatusa)
        {
            SLComment comm;
            SLStyle globalFontStyle = sl.CreateStyle();
            globalFontStyle.Font.FontName = "Century Gothic";
            globalFontStyle.Font.FontSize = 8;

            // redni broj
            sl.SetCellValue(9 + i, 1, i + 1);
            sl.SetCellStyle(9 + i, 1, globalFontStyle);

            // mesto za pib
            sl.SetCellValue(9 + i, 2, br.pib);
            sl.SetCellStyle(9 + i, 2, globalFontStyle);


            // maticni broj
            sl.SetCellValue(9 + i, 3, br.maticniBroj);
            sl.SetCellStyle(9 + i, 3, globalFontStyle);

            
            // promena statusa
            SLStyle style1 = sl.CreateStyle();
            style1.Font.FontName = "Century Gothic";
            style1.Font.FontSize = 8;
            style1.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.FromArgb(0, 176, 240), SLThemeColorIndexValues.Light2Color, 22);
            if (br.promenjenStatus.Equals("True"))
            {
                sl.SetCellValue(9 + i, 7, br.status);
                sl.SetCellStyle(9 + i, 7, style1);
                brojPromenaBlokada++;
            }
            else
            {
                sl.SetCellValue(9 + i, 7, br.status);
                sl.SetCellStyle(9 + i, 7, globalFontStyle);
            }

            if (br.zabranaPrenosa.Length > 2)
            {
                comm = sl.CreateComment();
                comm.AutoSize = true;
                comm.SetText(br.zabranaPrenosa);
                sl.InsertComment(9 + i, 7, comm);
            }

            // Naziv
            if (br.naziv != null)
                sl.SetCellValue(9 + i, 4, br.naziv);
            sl.SetCellStyle(9 + i, 4, globalFontStyle);

            // Adresa
            sl.SetCellValue(9 + i, 5, br.adresa);
            sl.SetCellStyle(9 + i, 5, globalFontStyle);

            // Grad
            sl.SetCellValue(9 + i, 6, br.grad);
            sl.SetCellStyle(9 + i, 6, globalFontStyle);

            // DatumOd
            globalFontStyle.Alignment.Horizontal = HorizontalAlignmentValues.Center;
            sl.SetCellStyle(9 + i, 7, globalFontStyle);
            if (br.datumOd != null && br.datumOd != "")
                sl.SetCellValue(9 + i, 8, DateTime.ParseExact(br.datumOd, "m/d/yyyy", CultureInfo.InvariantCulture).ToString("dd.mm.yyyy"));

            // Iznos
            //PROVERA ZASTO JE BR VRATIO KAO NULL
            SLStyle style2 = sl.CreateStyle();
            style2.Font.FontName = "Century Gothic";
            style2.Font.FontSize = 8;
            style2.FormatCode = "#,###";
            style2.Alignment.Horizontal = HorizontalAlignmentValues.Center;
            if (br.iznos != null && br.iznos != "")
            {
                sl.SetCellStyle(9 + i, 9, style2);

                String broj = br.iznos.Substring(0, br.iznos.Length - 3);
                broj = broj.Replace(",", string.Empty);
                long iznos = long.Parse(broj);
                sl.SetCellValue(9 + i, 9, iznos);
            }
            else
                sl.SetCellValue(9 + i, 9, br.iznos);

            // iznos promene
            if (br.iznosPromene.Contains("."))
                br.iznosPromene = br.iznosPromene.Substring(0, br.iznosPromene.IndexOf('.'));
            sl.SetCellStyle(9 + i, 10, style2);
            if (br.iznosPromene == "0" || br.iznosPromene == "")
                sl.SetCellValue(9 + i, 10, "");
            else
            {
                long iznos = long.Parse(br.iznosPromene);
                sl.SetCellValue(9 + i, 10, iznos);
            }

            if (br.povecanIznos.Equals("True"))
            {
                sl.SetCellValue(9 + i, 11, "Povećan");
                sl.SetCellStyle(9 + i, 11, style1);
                sl.SetCellStyle(9 + i, 10, style1);
                sl.SetCellStyle(9 + i, 9, style1);
                brojPromenaIznosa++;

            }
            else if (br.smanjenIznos.Equals("True"))
            {
                sl.SetCellValue(9 + i, 11, "Smanjen");
                sl.SetCellStyle(9 + i, 11, style1);
                sl.SetCellStyle(9 + i, 10, style1);
                sl.SetCellStyle(9 + i, 9, style1);
                brojPromenaIznosa++;
            }
            else if (br.povecanIznos.Equals("False") && br.smanjenIznos.Equals("False") && br.status == "Blokiran")
            {
                if (br.promenjenStatus.Equals("True"))
                    sl.SetCellValue(9 + i, 11, "");
                else if (br.iznos != "")
                    sl.SetCellValue(9 + i, 11, "Nepromenjen");
            }
            else
                sl.SetCellValue(9 + i, 11, "");

            sl.SetCellStyle(9 + i, 12, globalFontStyle);
            sl.SetCellStyle(9 + i, 13, globalFontStyle);

            // broj dana
            if (br.brojDana != null && br.brojDana != "")
                sl.SetCellValue(9 + i, 12, int.Parse(br.brojDana));

            // ukupan broj dana
            if (br.ukupanBrojDana != null && br.ukupanBrojDana != "" && br.ukupanBrojDana != "0")
                sl.SetCellValue(9 + i, 13, int.Parse(br.ukupanBrojDana));

            globalFontStyle.Alignment.Horizontal = HorizontalAlignmentValues.Left;
           /* if (br.promenjenStatusKompanije == "true")
            {
                sl.SetCellValue(9 + i, 14, br.statusKompanije);
                sl.SetCellStyle(9 + i, 14, style1);
                brojPromenaStatusa++;
            }
            else
            {
                if (br.statusKompanije == "")
                    sl.SetCellValue(9 + i, 14, "-");
                else
                    sl.SetCellValue(9 + i, 14, br.statusKompanije);
                sl.SetCellStyle(9 + i, 14, globalFontStyle);

            }*/
        }


        public static FileAndMails generisiIzvestajTip6(Klijent klijent, tblMonitoring m, TextBox txtConsole1, TextBox txtConsole2)
        {
            BlokadaRazlika br = null;
            int i = 0;
            SLDocument sl;
            DateTime pocetak = DateTime.Now;
            int aktivni = 0;
            int blokirani = 0;
            bool potencijalnoNeaktiva = false;
            bool maticniNePostoji = false;

            int brojPromenaBlokada = 0;
            int brojPromenaIznosa = 0;
            int brojPromenaStatusa = 0;

            DBExcel.connection = new SqlConnection(DBExcel.connectionString);
            DBExcel.connection.Open();


            sl = new SLDocument(Properties.Settings.Default.FILE_TEMPLATE6_PATH);

            foreach (String ss in m.lstMaticniBrojevi)
            {
                br = DBExcel.getBlokadaRazlika(ss.Trim());
                if (!potencijalnoNeaktiva)
                    potencijalnoNeaktiva = DBExcel.getPotencijalnoNeaktivna(ss.Trim());

                if (br.maticniBroj == null)
                {
                    DBGreska.addGreska(ss.Trim(), "Generisanje izvestaja", "Maticni broj ne postoji");
                    maticniNePostoji = true;
                    continue;
                }
                if (br.status == "Aktivan")
                    aktivni++;
                else
                    blokirani++;

                upisiUFajlTip6(br, i++, sl, ref brojPromenaBlokada, ref brojPromenaIznosa, ref brojPromenaStatusa);
                ActuallyPerformStep.performStepTxtBox(txtConsole2, "Upisan: " + i + "/" + m.lstMaticniBrojevi.Count, false);
            }

            sl.RenameWorksheet(SLDocument.DefaultFirstSheetName, klijent.naziv);
            //sl.SetCellValue("F2", klijent.naziv);
            //sl.SetCellValue("F3", DateTime.Now.ToString("dd. MMMM yyyy", CultureInfo.GetCultureInfo("sr-Latn-CS")));
            sl.SetCellValue("E5", m.lstMaticniBrojevi.Count);
            sl.SetCellValue("E6", aktivni);
            sl.SetCellValue("E7", blokirani);

            sl.SetCellValue("H5", brojPromenaBlokada);
            sl.SetCellValue("K5", brojPromenaIznosa);
            sl.SetCellValue("N5", brojPromenaStatusa);

            DBExcel.connection.Close();
            String filepath = Properties.Settings.Default.LOCAL_RESULT_DIRECTORY + "\\" + "Qbing" + " - " + m.naziv + " - " + DateTime.Now.ToString("yyyyMMdd") + ".xlsm";
            sl.SaveAs(filepath);

            ActuallyPerformStep.performStepTxtBox(txtConsole1, "Zavrseno generisanje za MB: " + klijent.naziv + " Vreme: " + (DateTime.Now - pocetak), true);

            if (m.saljeSeMail == "1")
                return new FileAndMails(filepath, m.lstMail, false, potencijalnoNeaktiva, maticniNePostoji);
            return null;
        }

        public static void upisiUFajlTip6(BlokadaRazlika br, int i, SLDocument sl, ref int brojPromenaBlokada, ref  int brojPromenaIznosa, ref  int brojPromenaStatusa)
        {
            SLComment comm;
            SLStyle globalFontStyle = sl.CreateStyle();
            globalFontStyle.Font.FontName = "Century Gothic";
            globalFontStyle.Font.FontSize = 8;

            // redni broj
            sl.SetCellValue(9 + i, 1, i + 1);
            sl.SetCellStyle(9 + i, 1, globalFontStyle);

            // mesto za pib
            sl.SetCellValue(9 + i, 2, br.pib);
            sl.SetCellStyle(9 + i, 2, globalFontStyle);


            // maticni broj
            sl.SetCellValue(9 + i, 3, br.maticniBroj);
            sl.SetCellStyle(9 + i, 3, globalFontStyle);

            // promena statusa
            SLStyle style1 = sl.CreateStyle();
            style1.Font.FontName = "Century Gothic";
            style1.Font.FontSize = 8;
            style1.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.FromArgb(255, 0, 0), SLThemeColorIndexValues.Light2Color, 22);
            if (br.promenjenStatus.Equals("True"))
            {
                sl.SetCellValue(9 + i, 7, br.status);
                sl.SetCellStyle(9 + i, 7, style1);
                brojPromenaBlokada++;
            }
            else
            {
                sl.SetCellValue(9 + i, 7, br.status);
                sl.SetCellStyle(9 + i, 7, globalFontStyle);
            }

            if (br.zabranaPrenosa.Length > 2)
            {
                comm = sl.CreateComment();
                comm.AutoSize = true;
                comm.SetText(br.zabranaPrenosa);
                sl.InsertComment(9 + i, 7, comm);
            }


            // Naziv
            if (br.naziv != null)
                sl.SetCellValue(9 + i, 4, br.naziv);
            sl.SetCellStyle(9 + i, 4, globalFontStyle);

            // Adresa
            sl.SetCellValue(9 + i, 5, br.adresa);
            sl.SetCellStyle(9 + i, 5, globalFontStyle);

            // Grad
            sl.SetCellValue(9 + i, 6, br.grad);
            sl.SetCellStyle(9 + i, 6, globalFontStyle);

            // DatumOd
            globalFontStyle.Alignment.Horizontal = HorizontalAlignmentValues.Center;
            sl.SetCellStyle(9 + i, 7, globalFontStyle);
            if (br.datumOd != null && br.datumOd != "")
                sl.SetCellValue(9 + i, 8, DateTime.ParseExact(br.datumOd, "m/d/yyyy", CultureInfo.InvariantCulture).ToString("dd.mm.yyyy"));

            // Iznos
            //PROVERA ZASTO JE BR VRATIO KAO NULL
            SLStyle style2 = sl.CreateStyle();
            style2.Font.FontName = "Century Gothic";
            style2.Font.FontSize = 8;
            style2.FormatCode = "#,###";
            style2.Alignment.Horizontal = HorizontalAlignmentValues.Center;
            if (br.iznos != null && br.iznos != "")
            {
                sl.SetCellStyle(9 + i, 9, style2);

                String broj = br.iznos.Substring(0, br.iznos.Length - 3);
                broj = broj.Replace(",", string.Empty);
                long iznos = long.Parse(broj);
                sl.SetCellValue(9 + i, 9, iznos);
            }
            else
                sl.SetCellValue(9 + i, 9, br.iznos);

            // iznos promene
            if (br.iznosPromene.Contains("."))
                br.iznosPromene = br.iznosPromene.Substring(0, br.iznosPromene.IndexOf('.'));
            sl.SetCellStyle(9 + i, 10, style2);
            if (br.iznosPromene == "0" || br.iznosPromene == "")
                sl.SetCellValue(9 + i, 10, "");
            else
            {
                long iznos = long.Parse(br.iznosPromene);
                sl.SetCellValue(9 + i, 10, iznos);
            }

            if (br.povecanIznos.Equals("True"))
            {
                sl.SetCellValue(9 + i, 11, "Povećan");
                sl.SetCellStyle(9 + i, 11, style1);
                sl.SetCellStyle(9 + i, 10, style1);
                sl.SetCellStyle(9 + i, 9, style1);
                brojPromenaIznosa++;

            }
            else if (br.smanjenIznos.Equals("True"))
            {
                sl.SetCellValue(9 + i, 11, "Smanjen");
                sl.SetCellStyle(9 + i, 11, style1);
                sl.SetCellStyle(9 + i, 10, style1);
                sl.SetCellStyle(9 + i, 9, style1);
                brojPromenaIznosa++;
            }
            else if (br.povecanIznos.Equals("False") && br.smanjenIznos.Equals("False") && br.status == "Blokiran")
            {
                if (br.promenjenStatus.Equals("True"))
                    sl.SetCellValue(9 + i, 11, "");
                else if (br.iznos != "")
                    sl.SetCellValue(9 + i, 11, "Nepromenjen");
            }
            else
                sl.SetCellValue(9 + i, 11, "");

            sl.SetCellStyle(9 + i, 12, globalFontStyle);
            sl.SetCellStyle(9 + i, 13, globalFontStyle);

            // broj dana
            if (br.brojDana != null && br.brojDana != "")
                sl.SetCellValue(9 + i, 12, int.Parse(br.brojDana));

            // ukupan broj dana
            if (br.ukupanBrojDana != null && br.ukupanBrojDana != "" && br.ukupanBrojDana != "0")
                sl.SetCellValue(9 + i, 13, int.Parse(br.ukupanBrojDana));

            globalFontStyle.Alignment.Horizontal = HorizontalAlignmentValues.Left;
            if (br.promenjenStatusKompanije == "true")
            {
                sl.SetCellValue(9 + i, 14, br.statusKompanije);
                sl.SetCellStyle(9 + i, 14, style1);
                brojPromenaStatusa++;
            }
            else
            {
                if (br.statusKompanije == "")
                    sl.SetCellValue(9 + i, 14, "-");
                else
                    sl.SetCellValue(9 + i, 14, br.statusKompanije);
                sl.SetCellStyle(9 + i, 14, globalFontStyle);

            }
        }

        public static FileAndMails generisiIzvestajTip7(Klijent klijent, tblMonitoring m, TextBox txtConsole1, TextBox txtConsole2)
        {
            BlokadaRazlika br = null;
            int i = 0;
            SLDocument sl;
            DateTime pocetak = DateTime.Now;
            int aktivni = 0;
            int blokirani = 0;
            bool potencijalnoNeaktiva = false;
            bool maticniNePostoji = false;

            int brojPromenaBlokada = 0;
            int brojPromenaIznosa = 0;
            int brojPromenaStatusa = 0;

            DBExcel.connection = new SqlConnection(DBExcel.connectionString);
            DBExcel.connection.Open();


            sl = new SLDocument(Properties.Settings.Default.FILE_TEMPLATE7_PATH);

            foreach (String ss in m.lstMaticniBrojevi)
            {
                br = DBExcel.getBlokadaRazlika(ss.Trim());
                if (!potencijalnoNeaktiva)
                    potencijalnoNeaktiva = DBExcel.getPotencijalnoNeaktivna(ss.Trim());

                if (br.maticniBroj == null)
                {
                    DBGreska.addGreska(ss.Trim(), "Generisanje izvestaja", "Maticni broj ne postoji");
                    maticniNePostoji = true;
                    continue;
                }
                if (br.status == "Aktivan")
                    aktivni++;
                else
                    blokirani++;

                upisiUFajlTip7(br, i++, sl, ref brojPromenaBlokada, ref brojPromenaIznosa, ref brojPromenaStatusa);
                ActuallyPerformStep.performStepTxtBox(txtConsole2, "Upisan: " + i + "/" + m.lstMaticniBrojevi.Count, false);
            }

            sl.RenameWorksheet(SLDocument.DefaultFirstSheetName, klijent.naziv);
            sl.SetCellValue("F2", klijent.naziv);
            sl.SetCellValue("F3", DateTime.Now.ToString("dd. MMMM yyyy", CultureInfo.GetCultureInfo("sr-Latn-CS")));
            sl.SetCellValue("E5", m.lstMaticniBrojevi.Count);
            sl.SetCellValue("E6", aktivni);
            sl.SetCellValue("E7", blokirani);

            sl.SetCellValue("H5", brojPromenaBlokada);
            sl.SetCellValue("K5", brojPromenaIznosa);
            sl.SetCellValue("N5", brojPromenaStatusa);

            DBExcel.connection.Close();
            String filepath = Properties.Settings.Default.LOCAL_RESULT_DIRECTORY + "\\" + "Qbing" + " - " + m.naziv + " - " + DateTime.Now.ToString("yyyyMMdd") + ".xlsm";
            sl.SaveAs(filepath);

            ActuallyPerformStep.performStepTxtBox(txtConsole1, "Zavrseno generisanje za MB: " + klijent.naziv + " Vreme: " + (DateTime.Now - pocetak), true);

            if (m.saljeSeMail == "1")
                return new FileAndMails(filepath, m.lstMail, false, potencijalnoNeaktiva, maticniNePostoji);
            return null;
        }

        public static void upisiUFajlTip7(BlokadaRazlika br, int i, SLDocument sl, ref int brojPromenaBlokada, ref  int brojPromenaIznosa, ref  int brojPromenaStatusa)
        {
            SLComment comm;
            SLStyle globalFontStyle = sl.CreateStyle();
            globalFontStyle.Font.FontName = "Century Gothic";
            globalFontStyle.Font.FontSize = 8;

            // redni broj
            sl.SetCellValue(9 + i, 1, br.carlCustomID);
            sl.SetCellStyle(9 + i, 1, globalFontStyle);

            // mesto za pib
            sl.SetCellValue(9 + i, 2, br.pib);
            sl.SetCellStyle(9 + i, 2, globalFontStyle);


            // maticni broj
            sl.SetCellValue(9 + i, 3, br.maticniBroj);
            sl.SetCellStyle(9 + i, 3, globalFontStyle);

            // promena statusa
            SLStyle style1 = sl.CreateStyle();
            style1.Font.FontName = "Century Gothic";
            style1.Font.FontSize = 8;
            style1.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.FromArgb(0, 176, 240), SLThemeColorIndexValues.Light2Color, 22);
            if (br.promenjenStatus.Equals("True"))
            {
                sl.SetCellValue(9 + i, 7, br.status);
                sl.SetCellStyle(9 + i, 7, style1);
                brojPromenaBlokada++;
            }
            else
            {
                sl.SetCellValue(9 + i, 7, br.status);
                sl.SetCellStyle(9 + i, 7, globalFontStyle);
            }

            if (br.zabranaPrenosa.Length > 2)
            {
                comm = sl.CreateComment();
                comm.AutoSize = true;
                comm.SetText(br.zabranaPrenosa);
                sl.InsertComment(9 + i, 7, comm);
            }

            // Naziv
            if (br.naziv != null)
                sl.SetCellValue(9 + i, 4, br.naziv);
            sl.SetCellStyle(9 + i, 4, globalFontStyle);

            // Adresa
            sl.SetCellValue(9 + i, 5, br.adresa);
            sl.SetCellStyle(9 + i, 5, globalFontStyle);

            // Grad
            sl.SetCellValue(9 + i, 6, br.grad);
            sl.SetCellStyle(9 + i, 6, globalFontStyle);

            // DatumOd
            globalFontStyle.Alignment.Horizontal = HorizontalAlignmentValues.Center;
            sl.SetCellStyle(9 + i, 7, globalFontStyle);
            if (br.datumOd != null && br.datumOd != "")
                sl.SetCellValue(9 + i, 8, DateTime.ParseExact(br.datumOd, "m/d/yyyy", CultureInfo.InvariantCulture).ToString("dd.mm.yyyy"));

            // Iznos
            //PROVERA ZASTO JE BR VRATIO KAO NULL
            SLStyle style2 = sl.CreateStyle();
            style2.Font.FontName = "Century Gothic";
            style2.Font.FontSize = 8;
            style2.FormatCode = "#,###";
            style2.Alignment.Horizontal = HorizontalAlignmentValues.Center;
            if (br.iznos != null && br.iznos != "")
            {
                sl.SetCellStyle(9 + i, 9, style2);

                String broj = br.iznos.Substring(0, br.iznos.Length - 3);
                broj = broj.Replace(",", string.Empty);
                long iznos = long.Parse(broj);
                sl.SetCellValue(9 + i, 9, iznos);
            }
            else
                sl.SetCellValue(9 + i, 9, br.iznos);

            // iznos promene
            if (br.iznosPromene.Contains("."))
                br.iznosPromene = br.iznosPromene.Substring(0, br.iznosPromene.IndexOf('.'));
            sl.SetCellStyle(9 + i, 10, style2);
            if (br.iznosPromene == "0" || br.iznosPromene == "")
                sl.SetCellValue(9 + i, 10, "");
            else
            {
                long iznos = long.Parse(br.iznosPromene);
                sl.SetCellValue(9 + i, 10, iznos);
            }

            if (br.povecanIznos.Equals("True"))
            {
                sl.SetCellValue(9 + i, 11, "Povećan");
                sl.SetCellStyle(9 + i, 11, style1);
                sl.SetCellStyle(9 + i, 10, style1);
                sl.SetCellStyle(9 + i, 9, style1);
                brojPromenaIznosa++;

            }
            else if (br.smanjenIznos.Equals("True"))
            {
                sl.SetCellValue(9 + i, 11, "Smanjen");
                sl.SetCellStyle(9 + i, 11, style1);
                sl.SetCellStyle(9 + i, 10, style1);
                sl.SetCellStyle(9 + i, 9, style1);
                brojPromenaIznosa++;
            }
            else if (br.povecanIznos.Equals("False") && br.smanjenIznos.Equals("False") && br.status == "Blokiran")
            {
                if (br.promenjenStatus.Equals("True"))
                    sl.SetCellValue(9 + i, 11, "");
                else if (br.iznos != "")
                    sl.SetCellValue(9 + i, 11, "Nepromenjen");
            }
            else
                sl.SetCellValue(9 + i, 11, "");

            sl.SetCellStyle(9 + i, 12, globalFontStyle);
            sl.SetCellStyle(9 + i, 13, globalFontStyle);

            // broj dana
            if (br.brojDana != null && br.brojDana != "")
                sl.SetCellValue(9 + i, 12, int.Parse(br.brojDana));

            // ukupan broj dana
            if (br.ukupanBrojDana != null && br.ukupanBrojDana != "" && br.ukupanBrojDana != "0")
                sl.SetCellValue(9 + i, 13, int.Parse(br.ukupanBrojDana));

            globalFontStyle.Alignment.Horizontal = HorizontalAlignmentValues.Left;
            if (br.promenjenStatusKompanije == "true")
            {
                sl.SetCellValue(9 + i, 14, br.statusKompanije);
                sl.SetCellStyle(9 + i, 14, style1);
                brojPromenaStatusa++;
            }
            else
            {
                if (br.statusKompanije == "")
                    sl.SetCellValue(9 + i, 14, "-");
                else
                    sl.SetCellValue(9 + i, 14, br.statusKompanije);
                sl.SetCellStyle(9 + i, 14, globalFontStyle);

            }
        }

        public static FileAndMails generisiIzvestajTip9(Klijent klijent, tblMonitoring m, TextBox txtConsole1, TextBox txtConsole2)
        {
            BlokadaRazlika br = null;
            int i = 0;
            SLDocument sl;
            DateTime pocetak = DateTime.Now;
            int aktivni = 0;
            int blokirani = 0;
            bool potencijalnoNeaktiva = false;
            bool maticniNePostoji = false;

            int brojPromenaBlokada = 0;
            int brojPromenaIznosa = 0;
            int brojPromenaStatusa = 0;

            DBExcel.connection = new SqlConnection(DBExcel.connectionString);
            DBExcel.connection.Open();


            sl = new SLDocument(Properties.Settings.Default.FILE_TEMPLATE9_PATH);

            foreach (String ss in m.lstMaticniBrojevi)
            {
                br = DBExcel.getBlokadaRazlika(ss.Trim());
                if (!potencijalnoNeaktiva)
                    potencijalnoNeaktiva = DBExcel.getPotencijalnoNeaktivna(ss.Trim());

                if (br.maticniBroj == null)
                {
                    DBGreska.addGreska(ss.Trim(), "Generisanje izvestaja", "Maticni broj ne postoji");
                    maticniNePostoji = true;
                    continue;
                }
                if (br.status == "Aktivan")
                    aktivni++;
                else
                    blokirani++;

                if (br.promenjenStatus == "True" || br.promenjenStatusKompanije == "true")
                {
                    if (!(br.status == "Aktivan" && br.statusKompanije.Contains("risan") && br.promenjenStatus == "True"))
                        upisiUFajlTip9(br, i++, sl, ref brojPromenaBlokada, ref brojPromenaIznosa, ref brojPromenaStatusa);

                }
                ActuallyPerformStep.performStepTxtBox(txtConsole2, "Upisan: " + i + "/" + m.lstMaticniBrojevi.Count, false);
            }

            sl.RenameWorksheet(SLDocument.DefaultFirstSheetName, klijent.naziv);
            sl.SetCellValue("H3", klijent.naziv);
            sl.SetCellValue("H2", DateTime.Now.ToString("dd. MMMM yyyy", CultureInfo.GetCultureInfo("sr-Latn-CS")));
            /* sl.SetCellValue("E5", m.lstMaticniBrojevi.Count);
             sl.SetCellValue("E6", aktivni);
             sl.SetCellValue("E7", blokirani);

             sl.SetCellValue("H5", brojPromenaBlokada);
             sl.SetCellValue("K5", brojPromenaIznosa);
             sl.SetCellValue("N5", brojPromenaStatusa);*/

            DBExcel.connection.Close();
            String filepath = Properties.Settings.Default.LOCAL_RESULT_DIRECTORY + "\\" + "Qbing promene" + " - " + m.naziv + " - " + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            sl.SaveAs(filepath);

            ActuallyPerformStep.performStepTxtBox(txtConsole1, "Zavrseno generisanje za MB: " + klijent.naziv + " Vreme: " + (DateTime.Now - pocetak), true);

            if (m.saljeSeMail == "1")
                return new FileAndMails(filepath, m.lstMail, false, potencijalnoNeaktiva, maticniNePostoji);
            return null;
        }


        public static void upisiUFajlTip9(BlokadaRazlika br, int i, SLDocument sl, ref int brojPromenaBlokada, ref  int brojPromenaIznosa, ref  int brojPromenaStatusa)
        {
            SLStyle globalFontStyle = sl.CreateStyle();
            globalFontStyle.Font.FontName = "Century Gothic";
            globalFontStyle.Font.FontSize = 8;

            // carlCustomId
            sl.SetCellValue(6 + i, 1, br.carlCustomID);
            sl.SetCellStyle(6 + i, 1, globalFontStyle);

            // Naziv
            if (br.naziv != null)
                sl.SetCellValue(6 + i, 2, br.naziv);
            sl.SetCellStyle(6 + i, 2, globalFontStyle);

            // maticni broj
            sl.SetCellValue(6 + i, 3, br.maticniBroj);
            sl.SetCellStyle(6 + i, 3, globalFontStyle);

            // mesto za pib
            sl.SetCellValue(6 + i, 4, br.pib);
            sl.SetCellStyle(6 + i, 4, globalFontStyle);


            // promena statusa pravnog lica
            sl.SetCellValue(6 + i, 5, br.statusKompanije);
            if (br.promenjenStatusKompanije == "true")
            {
                sl.SetCellValue(6 + i, 8, "PROMENA STATUSA");
                sl.SetCellStyle(6 + i, 8, globalFontStyle);
            }

            // promena statusa racuna
            sl.SetCellValue(6 + i, 6, br.status);
            if (br.promenjenStatus == "True")
            {
                if (br.status.Equals("Aktivan"))
                {
                    sl.SetCellValue(6 + i, 8, "IZLAZAK IZ BLOKADE");
                    sl.SetCellStyle(6 + i, 8, globalFontStyle);
                }
                else if (br.status.Equals("Blokiran"))
                {
                    sl.SetCellValue(6 + i, 8, "ULAZAK U BLOKADU");
                    sl.SetCellStyle(6 + i, 8, globalFontStyle);
                }
            }

            // Iznos
            //PROVERA ZASTO JE BR VRATIO KAO NULL
            SLStyle style2 = sl.CreateStyle();
            style2.Font.FontName = "Century Gothic";
            style2.Font.FontSize = 8;
            style2.FormatCode = "#,###";
            style2.Alignment.Horizontal = HorizontalAlignmentValues.Center;
            if (br.iznos != null && br.iznos != "")
            {
                sl.SetCellStyle(6 + i, 7, style2);

                String broj = br.iznos.Substring(0, br.iznos.Length - 3);
                broj = broj.Replace(",", string.Empty);
                long iznos = long.Parse(broj);
                sl.SetCellValue(6 + i, 7, iznos);
            }
            else
                sl.SetCellValue(6 + i, 7, br.iznos);


        }




        public static FileAndMails generisiIzvestajTip10(Klijent klijent, tblMonitoring m, TextBox txtConsole1, TextBox txtConsole2)
        {
            BlokadaRazlika br = null;
            int i = 0;
            SLDocument sl;
            DateTime pocetak = DateTime.Now;
            int aktivni = 0;
            int blokirani = 0;
            bool potencijalnoNeaktiva = false;
            bool maticniNePostoji = false;

            int brojPromenaBlokada = 0;
            int brojPromenaIznosa = 0;
            int brojPromenaStatusa = 0;

            DBExcel.connection = new SqlConnection(DBExcel.connectionString);
            DBExcel.connection.Open();


            sl = new SLDocument(Properties.Settings.Default.FILE_TEMPLATE10_PATH);

            foreach (String ss in m.lstMaticniBrojevi)
            {
                br = DBExcel.getBlokadaRazlika(ss.Trim());
                if (!potencijalnoNeaktiva)
                    potencijalnoNeaktiva = DBExcel.getPotencijalnoNeaktivna(ss.Trim());

                if (br.maticniBroj == null)
                {
                    DBGreska.addGreska(ss.Trim(), "Generisanje izvestaja", "Maticni broj ne postoji");
                    maticniNePostoji = true;
                    continue;
                }
                if (br.status == "Aktivan")
                    aktivni++;
                else
                    blokirani++;

                if (br.povecanIznos == "True" || br.smanjenIznos == "True" || br.promenjenStatus == "True" || br.promenjenStatusKompanije == "true")
                {
                    if (!(br.status == "Aktivan" && br.statusKompanije.Contains("risan") && br.promenjenStatus == "True"))
                        upisiUFajlTip10(br, i++, sl, ref brojPromenaBlokada, ref brojPromenaIznosa, ref brojPromenaStatusa);
                }
                ActuallyPerformStep.performStepTxtBox(txtConsole2, "Upisan: " + i + "/" + m.lstMaticniBrojevi.Count, false);
            }

            sl.RenameWorksheet(SLDocument.DefaultFirstSheetName, klijent.naziv);
            sl.SetCellValue("L3", klijent.naziv);
            sl.SetCellValue("L2", DateTime.Now.ToString("dd. MMMM yyyy", CultureInfo.GetCultureInfo("sr-Latn-CS")));

            DBExcel.connection.Close();
            String filepath = Properties.Settings.Default.LOCAL_RESULT_DIRECTORY + "\\" + "Qbing promene " + " - " + m.naziv + " svi subjekti - " + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            sl.SaveAs(filepath);

            ActuallyPerformStep.performStepTxtBox(txtConsole1, "Zavrseno generisanje za MB: " + klijent.naziv + " Vreme: " + (DateTime.Now - pocetak), true);

            if (m.saljeSeMail == "1")
                return new FileAndMails(filepath, m.lstMail, false, potencijalnoNeaktiva, maticniNePostoji);
            return null;
        }

        public static void upisiUFajlTip10(BlokadaRazlika br, int i, SLDocument sl, ref int brojPromenaBlokada, ref  int brojPromenaIznosa, ref  int brojPromenaStatusa)
        {
            SLStyle globalFontStyle = sl.CreateStyle();
            globalFontStyle.Font.FontName = "Century Gothic";
            globalFontStyle.Font.FontSize = 8;

            // redni broj
            sl.SetCellValue(6 + i, 1, br.carlCustomID);
            sl.SetCellStyle(6 + i, 1, globalFontStyle);

            // Naziv
            if (br.naziv != null)
                sl.SetCellValue(6 + i, 2, br.naziv);
            sl.SetCellStyle(6 + i, 2, globalFontStyle);

            // maticni broj
            sl.SetCellValue(6 + i, 3, br.maticniBroj);
            sl.SetCellStyle(6 + i, 3, globalFontStyle);

            // mesto za pib
            sl.SetCellValue(6 + i, 4, br.pib);
            sl.SetCellStyle(6 + i, 4, globalFontStyle);

            // promena statusa pravnog lica
            sl.SetCellValue(6 + i, 5, br.statusKompanije);
            if (br.promenjenStatusKompanije == "true")
            {
                sl.SetCellValue(6 + i, 6, "Promena statusa");
                sl.SetCellStyle(6 + i, 6, globalFontStyle);
            }

            // promena statusa racuna
            sl.SetCellValue(6 + i, 7, br.status);
            if (br.promenjenStatus == "True")
            {
                if (br.status.Equals("Aktivan"))
                {
                    sl.SetCellValue(6 + i, 6, "Izlazak iz blokade");
                    sl.SetCellStyle(6 + i, 6, globalFontStyle);
                }
                else if (br.status.Equals("Blokiran"))
                {
                    sl.SetCellValue(6 + i, 6, "Ulazak u blokadu");
                    sl.SetCellStyle(6 + i, 6, globalFontStyle);
                }
            }

            //iznos
            SLStyle style2 = sl.CreateStyle();
            style2.Font.FontName = "Century Gothic";
            style2.Font.FontSize = 8;
            style2.FormatCode = "#,###";
            style2.Alignment.Horizontal = HorizontalAlignmentValues.Center;
            if (br.iznos != null && br.iznos != "")
            {
                sl.SetCellStyle(6 + i, 8, style2);

                String broj = br.iznos.Substring(0, br.iznos.Length - 3);
                broj = broj.Replace(",", string.Empty);
                long iznos = long.Parse(broj);
                sl.SetCellValue(6 + i, 8, iznos);
            }
            else
                sl.SetCellValue(6 + i, 8, br.iznos);
            if (br.povecanIznos == "True" || br.smanjenIznos == "True")
            {
                sl.SetCellValue(6 + i, 6, "Ulazak u blokadu");
                sl.SetCellStyle(6 + i, 6, globalFontStyle);
            }


            if (br.povecanIznos.Equals("True"))
            {
                sl.SetCellValue(6 + i, 6, "Promena iznosa blokade");
                sl.SetCellValue(6 + i, 9, "Povećan");
            }
            else if (br.smanjenIznos.Equals("True"))
            {
                sl.SetCellValue(6 + i, 6, "Promena iznosa blokade");
                sl.SetCellValue(6 + i, 9, "Smanjen");
            }

            //iznos promene
            if (br.iznosPromene.Contains("."))
                br.iznosPromene = br.iznosPromene.Substring(0, br.iznosPromene.IndexOf('.'));
            if (br.iznosPromene == "0" || br.iznosPromene == "")
                sl.SetCellValue(6 + i, 10, "");
            else
            {
                long iznos = long.Parse(br.iznosPromene);
                sl.SetCellValue(6 + i, 10, iznos);
            }

            // broj dana
            if (br.brojDana != null && br.brojDana != "")
                sl.SetCellValue(6 + i, 11, int.Parse(br.brojDana));

            // ukupan broj dana
            if (br.ukupanBrojDana != null && br.ukupanBrojDana != "" && br.ukupanBrojDana != "0")
                sl.SetCellValue(6 + i, 12, int.Parse(br.ukupanBrojDana));
        }





        public static bool email_send(string klijent, string attachmentPath, List<string> lstMail, bool potencijalnoNeaktivna, bool maticniNePostoji)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(Rest.Parameters.smtpServer);
                mail.From = new MailAddress(Properties.Settings.Default.MAIL_ADDRESS);

                foreach (string s in Rest.Parameters.adreseZaSlanjePotencijalnoNeaktivnih)
                    mail.Bcc.Add(s);

                if (!potencijalnoNeaktivna && !maticniNePostoji)
                {
                    foreach (string s in lstMail)
                        mail.To.Add(s.Trim());
                }

                mail.Subject = "Qbing " + klijent + " " + DateTime.Now.ToString("dd.MM.yyy");

                if (potencijalnoNeaktivna)
                {
                    mail.Subject = "Monitoring sa potencijalno neaktivnom firmom!!!!";
                    mail.To.Add(Rest.Parameters.mailZaPotencijalnoNeaktivanMonitoring1);
                    mail.To.Add(Rest.Parameters.mailZaPotencijalnoNeaktivanMonitoring2);
                }
                if (maticniNePostoji)
                {
                    mail.Subject = "Monitoring sa firmom sa nepostojecim maticnim!!!";
                    mail.To.Add(Rest.Parameters.mailZaPotencijalnoNeaktivanMonitoring1);
                    mail.To.Add(Rest.Parameters.mailZaPotencijalnoNeaktivanMonitoring2);
                }

                // Set delivery notifications for success and failed messages
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess | DeliveryNotificationOptions.OnFailure;
                mail.Headers.Add("Read-Receipt-To", Rest.Parameters.mailReadRecepient);
                mail.Headers.Add("Disposition-Notification-To", Rest.Parameters.mailReadRecepient);

                LinkedResource theEmailImage = new LinkedResource("../../Resources/logo.png");
                theEmailImage.ContentId = "myImageID";

                string body =
                @"<html>
                <body>
                <table width=""100%"">
                <tr>
                <td style=""font-family:Century Gothic"">
                Poštovani, <br><br>
                U prilogu Vam šaljemo Monitoring izveštaj<br><br>
                Molimo Vas da sve eventualne sugestije i pohvale šaljete na claims@cube.rs<br><br>
                Srdačno Vas pozdravljamo.<br><br>
                Vaš CUBE.<br><br>
                ____________________________________________________________________________<br>
                <img src=cid:myImageID><br></td></tr>
                <tr>
                <td style=""font-family:Century Gothic; font-size: 10pt"">
                CUBE Risk Managment Solutions d.o.o.<br>
                Jurija Gagarina 28<br>
                11070 Novi Beograd<br>
                Serbia<br><br>
                T:           +381 11 414 2823<br><br>
                info@cube.rs<br>
                www.cube.rs<br>
                ____________________________________________________________________________<br>
                Please do not reply to this message.<br>
                Molimo Vas da ne odgovarate na ovaj mail.<br><br></td><tr>
                <tr><td style=""font-family:Century Gothic; font-size: 9pt"">
                Disclaimer:<br>
                This document should only be read by those persons to whom it is addressed and is not intended to be relied upon by any person without subsequent written confirmation of its contents. Accordingly, ”CUBE Risk Management Solutions d.o.o.”, disclaims all responsibility and accept no liability (including in negligence) for the consequences for any person acting, or refraining from acting, on such information prior to the receipt by those persons of subsequent written confirmation. If you have received this E-mail message in error, destroy and delete the message from your computer. Any form of reproduction, dissemination, copying, disclosure, modification, distribution and/or publication of this E-mail message is strictly prohibited. The contents of this e-mail do not necessarily represent the views of the “CUBE Risk Management Solutions d.o.o.”<br><br>
                Odricanje od odgovornosti:<br>
                Ovaj dokument namenjen je samo licima kojima je upucen i za pozivanje na isti od strane bilo kog lica, neophodna je naknadna pismena potvrda njegovog sadzaja. Shodno tome,” CUBE Risk Management Solutions d.o.o.” odrice svaku odgovornost i ne prihvata bilo kakvu obavezu (ukljucujuci slucaj nepaznje) za posledice koje moze pretrpeti bilo koje lice zbog cinjenja ili necinjenja na bazi takve informacije pre nego sto takva lica prime dodatnu pismenu potrvdu. Ukoliko ste greskom primili ovu elektronsku poruku, unistite i izbrisite istu sa vaseg racunara. Svako umnozavanje, sirenje, kopiranje, obelodanjivanje, izmena, distribucija i/ili objavljivanje ove elektronske poruke je strogo zabranjeno.Sadrzaj ove elektronske poruke ne predstavlja nuzno stavove “CUBE Risk Management Solutions d.o.o.”-a.
                </td>
                </tr>
                </table>
                </body>
                </html>";

                mail.IsBodyHtml = true;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                htmlView.LinkedResources.Add(theEmailImage);

                mail.AlternateViews.Add(htmlView);

                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(attachmentPath);
                mail.Attachments.Add(attachment);

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(Properties.Settings.Default.MAIL_ADDRESS, Properties.Settings.Default.MAIL_PASSWORD);
                SmtpServer.EnableSsl = true;
                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                SmtpServer.Send(mail);
            }
            catch (Exception) { return false; }
            return true;
        }

        public static bool email_sendTamara(string text)
        {
            if (text == null || text.Trim() == "")
                return true;
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(Rest.Parameters.smtpServer);
                mail.From = new MailAddress(Properties.Settings.Default.MAIL_ADDRESS);

                foreach (string s in Rest.Parameters.adreseZaSlanjePotencijalnoNeaktivnih)
                    mail.To.Add(s);

                mail.Subject = Rest.Parameters.potencijalnoNeaktivneMailSubject;
                mail.Body = text;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(Properties.Settings.Default.MAIL_ADDRESS, Properties.Settings.Default.MAIL_PASSWORD);
                SmtpServer.EnableSsl = true;
                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                SmtpServer.Send(mail);
            }
            catch (Exception) { return false; }
            return true;
        }

    }
}