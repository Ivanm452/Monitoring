using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinSCP;

namespace Monitoring.WinScp
{
    public class GetFile
    {
        public static bool getFile()
        {
            try
            {
                // Setup session options
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Sftp,
                    HostName = Properties.Settings.Default.HOST_NAME,
                    UserName = Properties.Settings.Default.USERNAME,
                    Password = Properties.Settings.Default.PASSWORD,
                    PortNumber = 8888,
                    SshHostKeyFingerprint = Properties.Settings.Default.SSH_HOST_KEY_FINGERPRINT
                };

                using (Session session = new Session())
                {
                    // Connect
                    session.Open(sessionOptions);

                    // Upload files
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;

                    TransferOperationResult transferResult;
                    transferResult = session.GetFiles(Properties.Settings.Default.FILE_PATH + "/" 
                        + DateTime.Now.ToString(Properties.Settings.Default.FILE_NAME_FORMAT) + 
                        Properties.Settings.Default.FILE_EXTENSION,
                        Properties.Settings.Default.LOCAL_CSV_DIRECTORY+"\\", false, transferOptions);
                    
                    // Throw on any error
                    transferResult.Check();

                    // Print results
                    foreach (TransferEventArgs transfer in transferResult.Transfers)
                        Console.WriteLine("Download of {0} succeeded", transfer.FileName);
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
                return false;
            }
        }

        public static bool checkServerStatus()
        {
            try
            {
                // Setup session options
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Sftp,
                    HostName = Properties.Settings.Default.HOST_NAME,
                    UserName = Properties.Settings.Default.USERNAME,
                    Password = Properties.Settings.Default.PASSWORD,
                    PortNumber = 8888,
                    SshHostKeyFingerprint = Properties.Settings.Default.SSH_HOST_KEY_FINGERPRINT
                };

                using (Session session = new Session())
                {
                    // Connect
                    session.Open(sessionOptions);
                    return true;
                }
            }
            catch (Exception e) { Console.Out.WriteLine(e.ToString()); return false; }
        }
    }
}