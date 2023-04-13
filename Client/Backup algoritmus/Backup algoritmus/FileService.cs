using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Backup_algoritmus
{
    public class FileService
    {
        public string Path { get; set; }

        public FileService()
        {

        }
        public FileService(string path)
        {
            this.Path = path;

        }
        public void CheckReportFile()
        {
            if (Directory.Exists(@"C:\Users\Public\Documents\Reports") == false)
            {
                Directory.CreateDirectory(@"C:\Users\Public\Documents\Reports");
            }
        }
        public void CheckFile()
        {
            if (Directory.Exists(Path) == false)
            {
                Directory.CreateDirectory(Path);
            }
        }
        public void CheckSnapshotLog()
        {
            if (!File.Exists(Path))
            {
                try
                {
                    File.Create(Path).Dispose();
                }
                catch (Exception e)
                {
                    string date = DateTime.UtcNow.ToString("MM-dd-yyyy-H-mm");
                    Console.WriteLine(e + " " + Path + " " + date);
                }
            }
        }
        public List<string> GetLogData()
        {
            List<string> log = new List<string>();
            using StreamReader sr = new StreamReader(Path);
            {
                while (!sr.EndOfStream)
                {
                    log.Add(sr.ReadLine());
                }
            }
            sr.Close();
            return log;
        }
    }
}
