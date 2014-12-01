using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monitoring.Classes.Rest
{
    class Parameters
    {
        public static List<string> adreseZaSlanjePotencijalnoNeaktivnih =
            new List<string>(new string[] { "i.mentov@cube.rs","no-reply@cube.rs" });
           //new List<string>(new string[] { "i.mentov@cube.rs" });
        public static string mailZaPotencijalnoNeaktivanMonitoring1 = "no-reply@cube.rs";
        public static string mailZaPotencijalnoNeaktivanMonitoring2 = "i.mentov@cube.rs";
        public static string mailReadRecepient = "no-reply@cube.rs";
        
        public static string potencijalnoNeaktivneMailSubject = "Potencijalno neaktivne firme";

        public static string smtpServer = "mail.cube.rs";

        //public static string AKS_TEMPLATE_PATH = @"C:\Users\IvanHP\Desktop\Monitoring/template_aks.xlsx";
       // public static string TIP_3_TEMPLATE_PATH = @"C:\Users\IvanHP\Desktop\Monitoring/template_sa_statusom.xlsx";

        // SadistaFunctions and SadistaImplementation
        public static int TIME_TO_WAIT_FOR_SERVER_STATUS = 5; // in minutes
        public static int TIME_TO_WAIT_FOR_COMMANDS = 30; // in minutes
        public static string INPUT_FILE_PATH = "/home/MONITORING/blokade/input.txt";
       // public static string[] COMMANDS_TO_RESTART_PARSER = { ". /home/MONITORING/blokade/blokada_script.sh" };
        public static string[] COMMANDS_TO_RESTART_PARSER = { "cd.." }; //promenjeno jer valjda vise nije potrebno da se restartuje parser ovako

        // vrste monitoringa
        public static int ID_VRSTE_1 = 1;
        public static int ID_VRSTE_2 = 2;
        public static int ID_VRSTE_3 = 3;
        public static int ID_VRSTE_4 = 4;
        public static int ID_VRSTE_5 = 5;
        public static int ID_VRSTE_6 = 6;
        public static int ID_VRSTE_7 = 7;

        public static string STATUS_FILE_PATH = "/home/APR/status/output/";

    }
}
