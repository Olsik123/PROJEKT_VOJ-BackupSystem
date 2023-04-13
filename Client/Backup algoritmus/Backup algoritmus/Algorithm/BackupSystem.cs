using Backup_algoritmus.Algorithm;
using FluentFTP;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backup_algoritmus
{
    public class BackupSystem 

    {
        public string Id { get; set; }
        public string ConfigName { get; set; }
        public string SnapshotLogPath { get; set; }
        public List<Source> Sources { get; set; } = new List<Source>();
        public List<Destination> Destinations { get; set; } = new List<Destination>();
        public string Type { get; set; }
        public string Format { get; set; }
        public int Packages { get; set; }
        public int PackagesCount { get; set; }
        public int Retention { get; set; }
        public int RetentionCount { get; set; }
        public int TotalRetentionCount { get; set; }
        public string PathEnd { get; set; }
        public static object IO { get; internal set; }
        

        public BackupSystem(string id, string configname, string type, string[] sources, string[] destinations, int retention, int packages, string[] places, string[] hosts,string format)
        {
            this.Id = id;
            this.Type = type;
            this.Format = format;
            this.ConfigName = configname;
            this.Retention = retention;
            this.Packages = packages;
            this.PathEnd = @"\" + ConfigName + "_" + Type + "_" + Id;
            this.SnapshotLogPath = @"C:\Users\Public\Documents" + PathEnd;
            int destnumber = 0;
            foreach (string item in destinations)
            {
                Destination source = new Destination(item,places[destnumber],hosts[destnumber]);
                Destinations.Add(source);
                destnumber++;

            }
            foreach (string item in sources)
            {
                Source source = new Source(item);
                Sources.Add(source);

            }
            StartBackup();
        }
        public void StartBackup()
        {
            foreach (Destination item in Destinations)
            {
                if (item.Place == "local")
                {
                    CheckFiles(item);
                    CheckSnapshotLogs(item);
                    SetCounter(item);
                    StartBackup(SetSnapshot(item), item);
                    if (Format == "archive")
                    {
                        ZipTheFile(item); 
                    }
                }
                else if (item.Place == "ftp")
                {
                    CheckFilesFTP(item);
                    CheckSnapshotLogs(item);
                    SetCounter(item);
                    if (Format == "plain")
                    {
                        StartBackupFTP(SetSnapshot(item), item, item.Host);
                    }
                    if (Format == "archive")
                    {
                        Destination d = new Destination(@"C:\Users\Public\Documents\FTP-Backups");
                        StartBackup(SetSnapshot(item), d);
                        ZipTheFile(d);
                        SendBackuptoFTP(item);
                    }
                }
            }
            UpdateLog();
            foreach (Destination item in Destinations)
            {
                DeleteByRetention(item);
            }
        }
        public void CheckFiles(Destination destination)
        {
            string path = SnapshotLogPath;
            FileService service = new FileService(path);
            service.CheckFile();
            string path2 = destination.Path + PathEnd;
            FileService service2 = new FileService(path2);
            service2.CheckFile();
        }
        public void CheckFilesFTP(Destination destination)
        {
            string path = SnapshotLogPath;
            FileService service = new FileService(path);
            service.CheckFile();
            string path2 = destination.Path + PathEnd.Replace(@"\",@"/");
            string host = destination.Host;
            string user = host.Split(@"//")[1];
            user = user.Split(":")[0];
            string password = host.Split(":")[2];
            password = password.Split("@")[0];
            string link = host.Split("@")[1];
            link = link.Split(":")[0];
            string port = host.Split(":")[3];
            port = port.Split(@"/")[0];
            FtpClient ftp = new FtpClient() { Host = link, Port = Convert.ToInt32(port), Credentials = { UserName = user, Password = password } };
            try
            {
                ftp.Connect();
                if (!ftp.DirectoryExists(path2))
                    ftp.CreateDirectory(path2);
                ftp.Disconnect();
            }
            catch
            { 

            }

        }
        public void CheckSnapshotLogs(Destination destination)
        {
            string path = SnapshotLogPath + @"\SnapshotLog.txt";
            FileService service = new FileService(path);
            service.CheckSnapshotLog();

        }
        public Snapshot SetSnapshot(Destination destination)
        {
            if (Type == "full")
            {
                string path = SnapshotLogPath + @"\SnapshotLog.txt";
                Snapshot snap = new Snapshot(Sources, path, RetentionCount);
                return snap;
            }
            else if (Type == "incr")
            {
                string path = SnapshotLogPath + @"\" + TotalRetentionCount + "_" + PackagesCount + @"_SnapshotBackupData.txt";
                List<Destination> destinations = new List<Destination>();
                for (int i = PackagesCount - 1; i >= 1; i--)
                {
                    Destination SearchedFolder = new Destination(SnapshotLogPath + @"\" + TotalRetentionCount + "_" + i + @"_SnapshotBackupData.txt");
                    destinations.Add(SearchedFolder);
                }
                Snapshot snap = new Snapshot(Sources, path, RetentionCount, PackagesCount, Type, destinations);
                return snap;
            }
            else if (Type == "diff")
            {
                string path = SnapshotLogPath + @"\" + TotalRetentionCount + "_" + PackagesCount + @"_SnapshotBackupData.txt";
                Destination SearchedFolder = new Destination(SnapshotLogPath + @"\" + TotalRetentionCount + "_1_SnapshotBackupData.txt");
                Snapshot snap = new Snapshot(Sources, path, RetentionCount, PackagesCount, Type, SearchedFolder);
                return snap;
            }
            else
            {
                throw new InvalidOperationException("Not valid backup type.");
            }
        }
        public void StartBackup(Snapshot snap, Destination destination)
        {
            string logDestination = SnapshotLogPath + @"\SnapshotLog.txt";
            if (Type == "full")
            {
                Destination newDest = new Destination(destination.Path + PathEnd + @"\" + TotalRetentionCount +"___"+ DateTime.Now.ToString("dd.MM.yyyy_HH-mm-ss") + @"\");
                Backup backup = new Backup(snap, newDest, logDestination);
            }
            else if (Type == "incr")
            {
                Destination newDest = new Destination(destination.Path + PathEnd + @"\" + TotalRetentionCount + @"\" + PackagesCount + "___" + DateTime.Now.ToString("dd.MM.yyyy_HH-mm-ss") + @"\");
                Backup backup = new Backup(snap, newDest, logDestination);
            }
            else if (Type == "diff")
            {
                Destination newDest = new Destination(destination.Path + PathEnd + @"\" + TotalRetentionCount + @"\" + PackagesCount + "___" + DateTime.Now.ToString("dd.MM.yyyy_HH-mm-ss") + @"\");
                Backup backup = new Backup(snap, newDest, logDestination);
            }
            else
            {
                throw new InvalidOperationException("Not valid backup type.");
            }

        }
        public void StartBackupFTP(Snapshot snap, Destination destination,string host)
        {
            string logDestination = SnapshotLogPath + @"\SnapshotLog.txt";
            if (Type == "full")
            {
                Destination newDest = new Destination(destination.Path + PathEnd.Replace(@"\", @"/") + @"/" + TotalRetentionCount + "___" + DateTime.Now.ToString("dd.MM.yyyy_HH-mm-ss") + @"/");
                BackupFTP backup = new BackupFTP(snap, newDest, logDestination,host);
            }
            else if (Type == "incr")
            {
                Destination newDest = new Destination(destination.Path + PathEnd.Replace(@"\", @"/") + @"/" + TotalRetentionCount + @"/" + PackagesCount + "___" + DateTime.Now.ToString("dd.MM.yyyy_HH-mm-ss") + @"/");
                BackupFTP backup = new BackupFTP(snap, newDest, logDestination,host);
            }
            else if (Type == "diff")
            {
                Destination newDest = new Destination(destination.Path + PathEnd.Replace(@"\", @"/") + @"/" + TotalRetentionCount + @"/" + PackagesCount + "___" + DateTime.Now.ToString("dd.MM.yyyy_HH-mm-ss") + @"/");
                BackupFTP backup = new BackupFTP(snap, newDest, logDestination, host);
            }
            else
            {
                throw new InvalidOperationException("Not valid backup type.");
            }

        }
        public void SetCounter(Destination destination)
        {
            string logDestination = SnapshotLogPath + @"\SnapshotLog.txt";
            List<string> log = new List<string>();
            using StreamReader sr = new StreamReader(logDestination);
            {
                while (!sr.EndOfStream)
                {
                    log.Add(sr.ReadLine());
                }
            }
            sr.Close();
            if (log.Count != 0)
            {
                if (Type == "full")
                {
                    RetentionCount = Convert.ToInt32(log[0].Split(";")[0]);
                    TotalRetentionCount = Convert.ToInt32(log[0].Split(";")[1]);
                }
                else
                {
                    RetentionCount = Convert.ToInt32(log[0].Split(";")[0]);
                    TotalRetentionCount = Convert.ToInt32(log[0].Split(";")[1]);
                    PackagesCount = Convert.ToInt32(log[0].Split(";")[2]);
                }
            }
            else
            {
                if (Type == "full")
                {
                    RetentionCount = 1;
                    TotalRetentionCount = 1;

                }
                else
                {
                    RetentionCount = 1;
                    TotalRetentionCount = 1;
                    PackagesCount = 1;
                }
            }        
        }
        public void UpdateLog()
        {
            string logDestination = SnapshotLogPath + @"\SnapshotLog.txt";
            List<string> log = new List<string>();
            using StreamReader sr = new StreamReader(logDestination);
            {
                while (!sr.EndOfStream)
                {
                    log.Add(sr.ReadLine());
                }
            }
            sr.Close();
            using (StreamWriter sw = new StreamWriter(logDestination))
            {
                if (log.Count != 0)
                {
                    if (Type == "full")
                    {
                        RetentionCount = Convert.ToInt32(log[0].Split(";")[0]);
                        TotalRetentionCount = Convert.ToInt32(log[0].Split(";")[1]);
                        TotalRetentionCount++;
                        if (RetentionCount > Retention)
                        {
                            RetentionCount = 1;
                        }
                        else
                        {
                            RetentionCount++;
                        }                     
                        sw.WriteLine(RetentionCount.ToString() + ";" + TotalRetentionCount);

                    }
                    else if (Type == "diff")
                    {
                        RetentionCount = Convert.ToInt32(log[0].Split(";")[0]);
                        TotalRetentionCount = Convert.ToInt32(log[0].Split(";")[1]);
                        PackagesCount = Convert.ToInt32(log[0].Split(";")[2]);
                        if (PackagesCount >= Packages)
                        {
                            if (RetentionCount > Retention)
                            {
                                RetentionCount = 1;
                                TotalRetentionCount++;
                            }
                            else
                            {
                                RetentionCount++;
                                TotalRetentionCount++;
                            }
                            PackagesCount = 1;
                        }
                        else
                        {
                            PackagesCount++;
                        }                      
                        sw.WriteLine(RetentionCount.ToString() + ";" + TotalRetentionCount + ";" + PackagesCount);
                    }
                    else if (Type == "incr")
                    {
                        RetentionCount = Convert.ToInt32(log[0].Split(";")[0]);
                        TotalRetentionCount = Convert.ToInt32(log[0].Split(";")[1]);
                        PackagesCount = Convert.ToInt32(log[0].Split(";")[2]);
                        if (PackagesCount >= Packages)
                        {
                            if (RetentionCount > Retention)
                            {
                                RetentionCount = 1;
                                TotalRetentionCount++;
                            }
                            else
                            {
                                RetentionCount++;
                                TotalRetentionCount++;
                            }
                            PackagesCount = 1;
                        }
                        else
                        {
                            PackagesCount++;
                        }                     
                        sw.WriteLine(RetentionCount.ToString() + ";" + TotalRetentionCount + ";" + PackagesCount);
                    }
                }
                else
                {
                    if (Type == "full")
                    {
                        sw.WriteLine("2;2");
                    }
                    else
                    {
                        sw.WriteLine("1;1;2");
                    }
                }
                sw.Close();
            }


        }
        public void DeleteByRetention(Destination destination)
        {
            string logDestination = SnapshotLogPath + @"\SnapshotLog.txt";
            List<string> log = new List<string>();
            using StreamReader sr = new StreamReader(logDestination);
            {
                while (!sr.EndOfStream)
                {
                    log.Add(sr.ReadLine());
                }
            }
            sr.Close();

                if (log.Count != 0)
                {
                    if (Type == "full")
                    {                        
                        var dir = new DirectoryInfo(destination.Path + PathEnd);
                        if (destination.Place == "local")
                        {
                            if (dir.GetDirectories().Length > Retention)
                            {
                                DeleteOldestBackup(dir);
                            }
                        }
                        else if (destination.Place == "ftp")
                        {

                            string path2 = destination.Path + PathEnd.Replace(@"\", @"/");
                            string host = destination.Host;
                            string user = host.Split(@"//")[1];
                            user = user.Split(":")[0];
                            string password = host.Split(":")[2];
                            password = password.Split("@")[0];
                            string link = host.Split("@")[1];
                            link = link.Split(":")[0];
                            string port = host.Split(":")[3];
                            port = port.Split(@"/")[0];
                            FtpClient ftp = new FtpClient() { Host = link, Port = Convert.ToInt32(port), Credentials = { UserName = user, Password = password } };
                            try
                            {
                                ftp.Connect();
                                if (ftp.GetListing(path2).Length > Retention)
                                {
                                    FtpListItem it = ftp.GetListing(path2).OrderBy(fi => fi.Modified).First();
                                    ftp.DeleteDirectory(it.FullName);
                                }
                                ftp.Disconnect();
                            }
                            catch
                            {

                            }
                        }
                    }
                    else if (Type == "diff")
                    {
                      
                        var dir = new DirectoryInfo(destination.Path + PathEnd);
                        if (destination.Place == "local")
                        {
                            if (dir.GetDirectories().Length > Retention)
                            {
                                DeleteOldestBackup(dir);
                            }
                        }
                        else if (destination.Place == "ftp")
                        {

                            string path2 = destination.Path + PathEnd.Replace(@"\", @"/");
                            string host = destination.Host;
                            string user = host.Split(@"//")[1];
                            user = user.Split(":")[0];
                            string password = host.Split(":")[2];
                            password = password.Split("@")[0];
                            string link = host.Split("@")[1];
                            link = link.Split(":")[0];
                            string port = host.Split(":")[3];
                            port = port.Split(@"/")[0];
                            FtpClient ftp = new FtpClient() { Host = link, Port = Convert.ToInt32(port), Credentials = { UserName = user, Password = password } };
                            try
                            {
                                ftp.Connect();
                                if (ftp.GetListing(path2).Length > Retention)
                                {
                                    FtpListItem it = ftp.GetListing(path2).OrderBy(fi => fi.Modified).First();
                                    ftp.DeleteDirectory(it.FullName);
                                }
                                ftp.Disconnect();
                            }
                            catch
                            {

                            }
                        }
                    }
                    else if (Type == "incr")
                    {
                      
                        var dir = new DirectoryInfo(destination.Path + PathEnd);
                        if (destination.Place == "local")
                        {
                            if (dir.GetDirectories().Length > Retention)
                            {
                                DeleteOldestBackup(dir);
                            }
                        }
                        else if (destination.Place == "ftp")
                        {

                            string path2 = destination.Path + PathEnd.Replace(@"\", @"/");
                            string host = destination.Host;
                            string user = host.Split(@"//")[1];
                            user = user.Split(":")[0];
                            string password = host.Split(":")[2];
                            password = password.Split("@")[0];
                            string link = host.Split("@")[1];
                            link = link.Split(":")[0];
                            string port = host.Split(":")[3];
                            port = port.Split(@"/")[0];
                            FtpClient ftp = new FtpClient() { Host = link, Port = Convert.ToInt32(port), Credentials = { UserName = user, Password = password } };
                            try
                            {
                                ftp.Connect();
                                if (ftp.GetListing(path2).Length > Retention)
                                {
                                    FtpListItem it = ftp.GetListing(path2).OrderBy(fi => fi.Modified).First();
                                    ftp.DeleteDirectory(it.FullName);
                                }
                                ftp.Disconnect();
                            }
                            catch
                            {

                            }
                        }                      
                    }
                }
            


        }
        public void ZipTheFile(Destination destination)           
        {
            if (Type == "full")
            {
                string zipPath = destination.Path + PathEnd;
                DirectoryInfo dir = new DirectoryInfo(zipPath);
                DirectoryInfo fileInfo = dir.GetDirectories().OrderBy(fi => fi.CreationTime).Last();
                zipPath = fileInfo.FullName + ".zip";
                string startPath = fileInfo.FullName;
                ZipFile.CreateFromDirectory(startPath, zipPath);
                Directory.Delete(startPath, true);
            }
            else if (Type == "incr" || Type == "diff")
            {
                string zipPath = destination.Path + PathEnd + @"\" + RetentionCount;
                DirectoryInfo dir = new DirectoryInfo(zipPath);
                DirectoryInfo fileInfo = dir.GetDirectories().OrderBy(fi => fi.CreationTime).Last();
                zipPath = fileInfo.FullName + ".zip";
                string startPath = fileInfo.FullName;
                ZipFile.CreateFromDirectory(startPath, zipPath);
                Directory.Delete(startPath, true);
            }

        }
        public void SendBackuptoFTP(Destination destination)
        {
            if (Type == "full")
            {
                string zipPath = @"C:\Users\Public\Documents\FTP-Backups";
                DirectoryInfo dir = new DirectoryInfo(zipPath);
                DirectoryInfo directory = dir.GetDirectories().OrderBy(fi => fi.CreationTime).Last();
                string path2 = destination.Path + PathEnd.Replace(@"\", @"/");
                string host = destination.Host;
                string user = host.Split(@"//")[1];
                user = user.Split(":")[0];
                string password = host.Split(":")[2];
                password = password.Split("@")[0];
                string link = host.Split("@")[1];
                link = link.Split(":")[0];
                string port = host.Split(":")[3];
                port = port.Split(@"/")[0];
                FtpClient ftp = new FtpClient() { Host = link, Port = Convert.ToInt32(port), Credentials = { UserName = user, Password = password } };
                try
                {
                    ftp.Connect();
                    if (!ftp.DirectoryExists(path2))
                        ftp.CreateDirectory(path2);
                    ftp.UploadDirectory(directory.FullName, path2);
                    ftp.Disconnect();
                }
                catch 
                {

                }
                directory.Delete(true);
            }
            else if (Type == "incr" || Type == "diff")
            {
                string zipPath = @"C:\Users\Public\Documents\FTP-Backups" + PathEnd;
                DirectoryInfo dir = new DirectoryInfo(zipPath);
                DirectoryInfo directory = dir.GetDirectories().OrderBy(fi => fi.CreationTime).Last();
                string path2 = destination.Path + PathEnd.Replace(@"\", @"/") + @"/" + TotalRetentionCount;
                string host = destination.Host;
                string user = host.Split(@"//")[1];
                user = user.Split(":")[0];
                string password = host.Split(":")[2];
                password = password.Split("@")[0];
                string link = host.Split("@")[1];
                link = link.Split(":")[0];
                string port = host.Split(":")[3];
                port = port.Split(@"/")[0];
                FtpClient ftp = new FtpClient() { Host = link, Port = Convert.ToInt32(port), Credentials = { UserName = user, Password = password } };
                try
                {
                    ftp.Connect();
                    if (!ftp.DirectoryExists(path2))
                        ftp.CreateDirectory(path2);
                    ftp.UploadDirectory(directory.FullName, path2);
                    ftp.Disconnect();
                }
                catch
                {

                }
                directory.Delete(true);
            }
        }
        public void DeleteOldestBackup(DirectoryInfo dir)
        {
            DirectoryInfo fileInfo = dir.GetDirectories().OrderBy(fi => fi.CreationTime).First();
            Directory.Delete(fileInfo.FullName, true);
            if (dir.GetDirectories().Length > Retention)
            {
                DeleteOldestBackup(dir);
            }
        }

    
    }
}
