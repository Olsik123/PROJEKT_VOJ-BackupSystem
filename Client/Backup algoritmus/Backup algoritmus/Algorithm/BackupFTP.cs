using FluentFTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Backup_algoritmus.Algorithm
{
    public class BackupFTP
    {
        public Snapshot Snap { get; set; }
        public Destination Destination { get; set; }
        public string LogDestination { get; set; }
        public int Retention { get; set; }
        public int Packages { get; set; }
        public string Host { get; set; }
        public FtpClient ftp { get; set; }


        public BackupFTP(Snapshot snap, Destination destination, string logdestination, string host)
        {
            this.Snap = snap;
            this.Host = host;
            this.Destination = destination;
            this.LogDestination = logdestination;
            string user = host.Split(@"//")[1];
            user = user.Split(":")[0];
            string password = host.Split(":")[2];
            password = password.Split("@")[0];
            string link = host.Split("@")[1];
            link = link.Split(":")[0];
            string port = host.Split(":")[3];
            port = port.Split(@"/")[0];
            ftp = new FtpClient() { Host = link, Port = Convert.ToInt32(port), Credentials = { UserName = user, Password = password } };
            try
            {
                ftp.Connect();
            }
            catch (Exception)
            {

                throw;
            }
            StartBackup();
            try
            {
                ftp.Disconnect();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void StartBackup()
        {
            if (Snap.NewDirectories.Count == 0 && Snap.NewFiles.Count == 0)
            {
                ftp.CreateDirectory(Destination.Path);
            }
            else
            {
                foreach (Source item in Snap.NewDirectories)
                {
                    try
                    {
                        CopyDirectory(item, Destination);
                    }
                    catch (Exception exc)
                    {
                        string date = DateTime.UtcNow.ToString("MM-dd-yyyy-H-mm");
                        Console.WriteLine(exc + " Source: " + item + " Dest: " + Destination + " " + date);
                    }
                }
                foreach (Source item in Snap.NewFiles)
                {
                    try
                    {
                        CopyFile(item, Destination);

                    }
                    catch (Exception exc)
                    {
                        string date = DateTime.UtcNow.ToString("MM-dd-yyyy-H-mm");
                        Console.WriteLine(exc + " Source: " + item + " Dest: " + Destination + " " + date);
                    }
                }
            }
        }
        public void CopyDirectory(Source sourcefile, Destination destinationfile)
        {
            string Combined = destinationfile.Path;
            string[] splitted = sourcefile.Path.Split(@"\");
            for (int i = sourcefile.SubfoldersCount - 1; i < splitted.Length; i++)
            {
                Combined += splitted[i] + @"/";
            }
            ftp.CreateDirectory(Combined);
        }
        public void CopyFile(Source sourcefile, Destination destinationfile)
        {
            string Combined = destinationfile.Path;
            string[] splitted = sourcefile.Path.Split(@"\");
            for (int i = sourcefile.SubfoldersCount - 1; i < splitted.Length; i++)
            {
                Combined += splitted[i] + @"/";
            }
            Combined = Combined.Remove(Combined.Length - 1);
            FileInfo File2 = new FileInfo(Combined);
            string[] array = Combined.Split(@"/");
            string parent = @"/";
            for (int i = 1; i < array.Length-1; i++)
            {
                parent += array[i] + @"/"; 
            }
            CheckDir(Combined,parent);
            if (!ftp.FileExists(Combined))
                ftp.CreateDirectory(File2.DirectoryName);
            ftp.UploadFile(sourcefile.Path, Combined);
        }
        public void CheckDir(string path1, string parentPath)
        {
            try
            {
                if (!ftp.DirectoryExists(parentPath))
                {
                    string[] array = parentPath.Split(@"/");
                    string parent = @"/";
                    for (int i = 1; i < array.Length - 2; i++)
                    {
                        parent += array[i] + @"/";
                    }
                    CheckDir(parentPath, parent);
                    ftp.CreateDirectory(parentPath);
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
