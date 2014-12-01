using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace Monitoring.Classes.SadistaCommunication
{
    class SadistaFunctions : IDisposable
    {
        private string ipAddress;
        private string username;
        private string password;
        private string ssh_host_key_fingerprint;

        private WinSCP.Session session;
        private WinSCP.SessionOptions sessionOptions;

        // constructor
        public SadistaFunctions(string ipAddress, string username,
            string password, string ssh_host_key_fingerprint)
        {
            this.ipAddress = ipAddress;
            this.username = username;
            this.password = password;
            this.ssh_host_key_fingerprint = ssh_host_key_fingerprint;

            this.session = null;
            this.sessionOptions = null;
        }

        // check server status by trying to connect to it
        public bool checkServerStatus()
        {
            return connect();
        }

        // check if file exsists
        public bool checkIfFileExsists(string remoteFilePath)
        {
            connectIfNotConnected();
            return this.session.FileExists(remoteFilePath);
        }

        // get a remoteFile and put it localDirectory
        public bool getFile(string remoteFilePath, string localDirectory)
        {
            try
            {
                connectIfNotConnected();
                WinSCP.TransferOptions transferOptions = new WinSCP.TransferOptions();
                transferOptions.TransferMode = WinSCP.TransferMode.Binary;

                WinSCP.TransferOperationResult transferResult;
                transferResult = session.GetFiles(remoteFilePath, localDirectory, false, transferOptions);

                // Throw on any error
                transferResult.Check();

            }
            catch (Exception) { return false; }
            return true;
        }

        public WinSCP.RemoteFileInfo getRemoteFileInfo(string remoteFilePath)
        {
            connectIfNotConnected();
            return this.session.GetFileInfo(remoteFilePath);
        }

        // check file size (in KiB)
        public bool compareSize(string remoteFilePath, float min, float max)
        {
            long sizeInKiB = getFileSize(remoteFilePath);
            return sizeInKiB >= min && sizeInKiB <= max;
        }

        // gets remote file size (in KiB)
        public long getFileSize(string remoteFilePath)
        {
            WinSCP.RemoteFileInfo rfi = this.getRemoteFileInfo(remoteFilePath);
            long sizeInKiB = rfi.Length / 1024;
            return sizeInKiB;
        }

        // check if two files have the same number of lines
        public static bool compareNumberOfLines(string csvFilePath, string txtFilePath)
        {
            string[] localFile1 = File.ReadAllLines(csvFilePath);
            string[] localFile2 = File.ReadAllLines(txtFilePath);

            int distinctLinesCount1 = countDistinctInCsv(localFile1);
            int distinctLinesCount2 = localFile2.Distinct().Count();

            return distinctLinesCount1 == distinctLinesCount2;
        }

        // runs multiple commands on the server
        public bool runCommands(string[] commands)
        {
            Renci.SshNet.SshCommand sshCommand;
            Renci.SshNet.SshClient sshClient = new Renci.SshNet.SshClient(this.ipAddress, 8888, this.username, this.password);
            sshClient.Connect();
            
            foreach (string s in commands)
            {
                sshCommand = sshClient.RunCommand(s);
                Thread.Sleep(100);
            }
            sshClient.Disconnect();
            return true;
        }

        // deletes a local file
        public void deleteLocalFile(string localFilePath)
        {
            File.Delete(localFilePath);
        }

        // aux function to count the unique MBs in the CSV file
        private static int countDistinctInCsv(string[] array)
        {
            List<string> distinctArray = new List<string>();
            foreach (string s in array)
                if (s.Trim() != "")
                    if (!distinctArray.Contains(s.Substring(1, 9)))
                        distinctArray.Add(s.Substring(1, 9));

            return distinctArray.Count;
        }

        private void connectIfNotConnected()
        {
            // connect if its not already connected
            if (this.session == null || !this.session.Opened)
                if (!connect())
                    throw new Exception("ERROR ON CONNECT");
        }

        private bool connect()
        {
            try
            {
                this.sessionOptions = new WinSCP.SessionOptions
                {
                    Protocol = WinSCP.Protocol.Sftp,
                    PortNumber = 8888,
                    HostName = this.ipAddress,
                    UserName = this.username,
                    Password = this.password,
                    SshHostKeyFingerprint = this.ssh_host_key_fingerprint
                };

                this.session = new WinSCP.Session();
                this.session.Open(this.sessionOptions);
                return true;
            }
            catch (Exception) { return false; }
        }

        // in progress
        public void Dispose()
        {
            this.session.Dispose();
        }
    }
}
