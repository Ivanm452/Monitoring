using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Monitoring.Classes.Rest;

namespace Monitoring.Classes.SadistaCommunication
{
    class SadistaImplementation
    {
        public static bool doYourThing()
        {
            using (SadistaFunctions sf = new SadistaFunctions(Properties.Settings.Default.HOST_NAME, Properties.Settings.Default.USERNAME, Properties.Settings.Default.PASSWORD, Properties.Settings.Default.SSH_HOST_KEY_FINGERPRINT))
            {
                int i;

                // checks server status 3 times 
                // each time waiting i*TIME_TO_WAIT_FOR_SERVER_STATUS minutes
                // if server status at the end is false then it exits the function
                i = 1;
                bool serverStatus = sf.checkServerStatus();
                while (!serverStatus && i<=3)
                {
                    Thread.Sleep(Parameters.TIME_TO_WAIT_FOR_SERVER_STATUS * 60 * 1000 * i);
                    i++;
                    serverStatus = sf.checkServerStatus();
                }
                if (!serverStatus) 
                    throw new Exception("SADISTA IS NOT AVAILABLE");
                Console.WriteLine("SADISTA IS AVAILABLE");

                // check input file
                if (!sf.checkIfFileExsists(Parameters.INPUT_FILE_PATH))
                    throw new Exception("INPUT FILE DOES NOT EXSIST");
                Console.WriteLine("INPUT FILE EXSISTS");

                // check result file
                if (!sf.checkIfFileExsists(Properties.Settings.Default.FILE_PATH + "/"
                        + DateTime.Now.ToString(Properties.Settings.Default.FILE_NAME_FORMAT) +
                        Properties.Settings.Default.FILE_EXTENSION))
                {
                    Console.WriteLine("RESULT FILE MISSING - RUNNING COMMANDS. Waiting for: " + (1000 * 60 * Rest.Parameters.TIME_TO_WAIT_FOR_COMMANDS) / (1000 * 60) + " minutes.");
                    sf.runCommands(Parameters.COMMANDS_TO_RESTART_PARSER);
                    Thread.Sleep(1000 * 60 * Rest.Parameters.TIME_TO_WAIT_FOR_COMMANDS); // sleeps for an hour
                    if (!sf.checkIfFileExsists(Properties.Settings.Default.FILE_PATH + "/"
                       + DateTime.Now.ToString(Properties.Settings.Default.FILE_NAME_FORMAT) +
                       Properties.Settings.Default.FILE_EXTENSION)) 
                    throw new Exception("RESULT FILE DOES NOT EXSIST"); 
                }
                Console.WriteLine("RESULT FILE EXSISTS");
                    

                // download input and result file
                sf.getFile(Parameters.INPUT_FILE_PATH, Properties.Settings.Default.LOCAL_CSV_DIRECTORY + "\\");
                sf.getFile(Properties.Settings.Default.FILE_PATH + "/"
                        + DateTime.Now.ToString(Properties.Settings.Default.FILE_NAME_FORMAT) +
                        Properties.Settings.Default.FILE_EXTENSION, Properties.Settings.Default.LOCAL_CSV_DIRECTORY + "\\");
                Console.WriteLine("BOTH FILES DOWNLOADED");

                // compare lengths
                // if length is not the same runs the program and then tries it again
                i = 1;
                bool haveSameNumberOfLines = SadistaFunctions.compareNumberOfLines(Properties.Settings.Default.LOCAL_CSV_DIRECTORY + "\\" +
                    DateTime.Now.ToString(Properties.Settings.Default.FILE_NAME_FORMAT) +
                    Properties.Settings.Default.FILE_EXTENSION,
                    Properties.Settings.Default.LOCAL_CSV_DIRECTORY + "\\input.txt");
                if(!haveSameNumberOfLines)
                {
                    Console.WriteLine("DIFFERENT LINE NUMBERS - RUNNING COMMANDS. Waiting for: " + (1000 * 60 * Rest.Parameters.TIME_TO_WAIT_FOR_COMMANDS) / (1000 * 60) + " minutes.");
                    sf.runCommands(Parameters.COMMANDS_TO_RESTART_PARSER);
                    Thread.Sleep(1000 * 60 * Rest.Parameters.TIME_TO_WAIT_FOR_COMMANDS); // sleeps for an hour
                    sf.getFile(Parameters.INPUT_FILE_PATH, Properties.Settings.Default.LOCAL_CSV_DIRECTORY + "\\");
                    sf.getFile(Properties.Settings.Default.FILE_PATH + "/"
                        + DateTime.Now.ToString(Properties.Settings.Default.FILE_NAME_FORMAT) +
                        Properties.Settings.Default.FILE_EXTENSION, Properties.Settings.Default.LOCAL_CSV_DIRECTORY + "\\");
                    haveSameNumberOfLines = SadistaFunctions.compareNumberOfLines(Properties.Settings.Default.LOCAL_CSV_DIRECTORY + "\\" +
                    DateTime.Now.ToString(Properties.Settings.Default.FILE_NAME_FORMAT) +
                    Properties.Settings.Default.FILE_EXTENSION,
                    Properties.Settings.Default.LOCAL_CSV_DIRECTORY + "\\input.txt");
                    if(!haveSameNumberOfLines)
                        throw new Exception("FILES DONT HAVE THE SAME NUMBER OF LINES");
                  
                }
                Console.WriteLine("BOTH FILES HAVE THE SAME NUBMER OF LINES");
            }
            return true;
        }

        public static bool doYourThingStatus()
        {
            try
            {
               /* using (SadistaFunctions sf = new SadistaFunctions(Properties.Settings.Default.HOST_NAME, Properties.Settings.Default.USERNAME, Properties.Settings.Default.PASSWORD, Properties.Settings.Default.SSH_HOST_KEY_FINGERPRINT))
                {
                    sf.getFile(Rest.Parameters.STATUS_FILE_PATH+DateTime.Now.ToString("yyyyMMdd")+".csv", Properties.Settings.Default.LOCAL_CSV_DIRECTORY + "\\status_" + DateTime.Now.ToString("yyyyMMdd") + ".csv");
                }*/

                using (SadistaFunctions sf = new SadistaFunctions("77.81.241.236", "root", "oki.delamol", "ssh-rsa 2048 81:6e:ec:c2:3e:de:6d:da:c8:f2:27:52:e6:2f:70:47"))
                {
                    sf.getFile("/home/APR/status/output/" + DateTime.Now.ToString("yyyyMMdd") + ".csv", Properties.Settings.Default.LOCAL_CSV_DIRECTORY + "\\status_" + DateTime.Now.ToString("yyyyMMdd") + ".csv");
                   // sf.getFile("/home/APR/samo_status/input.txt", Properties.Settings.Default.LOCAL_CSV_DIRECTORY + "\\status_" + DateTime.Now.ToString("yyyyMMdd") + ".csv");
                
                }
                Console.WriteLine("Uspesno skinut status");
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); return false; }
            return true;
        }    
    }
}
