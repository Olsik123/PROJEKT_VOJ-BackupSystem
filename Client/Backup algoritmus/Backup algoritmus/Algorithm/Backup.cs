using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Backup_algoritmus
{
    public class Backup
    {

        public Snapshot Snap { get; set; }
        public Destination Destination { get; set; }
        public string LogDestination { get; set; }
        public int Retention { get; set; }
        public int Packages { get; set; }


        public Backup(Snapshot snap, Destination destination, string logdestination)
        {
            this.Snap = snap;
            this.Destination = destination;
            this.LogDestination = logdestination;
            StartBackup();
        }
        public void StartBackup()
        {
            if (Snap.NewDirectories.Count == 0 && Snap.NewFiles.Count == 0)
            {
                Directory.CreateDirectory(Destination.Path);
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
                Combined += splitted[i] + @"\";
            }
            Directory.CreateDirectory(Combined);
        }
        public void CopyFile(Source sourcefile, Destination destinationfile)
        {
            string Combined = destinationfile.Path;
            string[] splitted = sourcefile.Path.Split(@"\");
            for (int i = sourcefile.SubfoldersCount - 1; i < splitted.Length; i++)
            {
                Combined += splitted[i] + @"\";
            }
            Combined = Combined.Remove(Combined.Length - 1);
            FileInfo File2 = new FileInfo(Combined);
            if(!File2.Directory.Exists)
            Directory.CreateDirectory(File2.DirectoryName);
            File.Copy(sourcefile.Path,Combined,true);
        }
    }
}
