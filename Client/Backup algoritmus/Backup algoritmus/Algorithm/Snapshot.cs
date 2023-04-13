using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Backup_algoritmus
{
    public class Snapshot
    {
        public List<Source> Sources { get; set; }
        public List<Source> NewFiles { get; set; } = new List<Source>();
        public List<Source> NewDirectories { get; set; } = new List<Source>();
        public string LogDataDest { get; set; }
        public int PackagesCount { get; set; }
        public int RetentionCount { get; set; }
        public string Type { get; set; }
        public Destination SearchedFolder { get; set; }
        public List<Destination> SearchedFolders { get; set; }
        public Snapshot(List<Source> sources, string path, int retentioncount)
        {

            //FULL
            this.Sources = sources;
            this.LogDataDest = path;
            this.RetentionCount = retentioncount;
            foreach (Source source in Sources)
            {
                string[] dirsWithoutRootS = Directory.GetDirectories(source.Path, "*.*", SearchOption.AllDirectories);
                string[] filesWithoutRootS = Directory.GetFiles(source.Path, "*.*", SearchOption.AllDirectories);
                foreach (string item in dirsWithoutRootS)
                {
                    Source sourceRoot = new Source(item, source.SubfoldersCount);
                    NewDirectories.Add(sourceRoot);
                }
                foreach (string item in filesWithoutRootS)
                {
                    Source sourceRoot = new Source(item, source.SubfoldersCount);
                    NewFiles.Add(sourceRoot);
                }
            }
        }
        public Snapshot(List<Source> sources, string path, int retentioncount, int packagescount, string type, Destination searchedfolder)
        {
            //DIFF
            this.Sources = sources;
            this.LogDataDest = path;
            this.RetentionCount = retentioncount;
            this.PackagesCount = packagescount;
            this.Type = type;
            this.SearchedFolder = searchedfolder;
            ReduceSourcesDiff();
        }
        public Snapshot(List<Source> sources, string path, int retentioncount, int packagescount, string type, List<Destination> searchedfolder)
        {
            //INCR
            this.Sources = sources;
            this.LogDataDest = path;
            this.RetentionCount = retentioncount;
            this.PackagesCount = packagescount;
            this.Type = type;
            this.SearchedFolders = searchedfolder;
            ReduceSourcesIncr();
        }

        public void ReduceSourcesDiff()
        {
            if (PackagesCount != 1)
            {

                List<Source> dirWithRootS = new List<Source>();
                List<Source> fileWithRootS = new List<Source>();

                foreach (Source source in Sources)
                {
                    string[] dirsWithoutRootS = Directory.GetDirectories(source.Path, "*.*", SearchOption.AllDirectories);
                    string[] filesWithoutRootS = Directory.GetFiles(source.Path, "*.*", SearchOption.AllDirectories);
                    foreach (string item in dirsWithoutRootS)
                    {
                        Source sourceRoot = new Source(item, source.SubfoldersCount);
                        dirWithRootS.Add(sourceRoot);
                    }
                    foreach (string item in filesWithoutRootS)
                    {
                        Source sourceRoot = new Source(item, source.SubfoldersCount);
                        fileWithRootS.Add(sourceRoot);
                    }
                }

                List<string> data = new List<string>();
                using StreamReader sr = new StreamReader(SearchedFolder.Path);
                {
                    while (!sr.EndOfStream)
                    {
                        data.Add(sr.ReadLine());
                    }
                }
                int index = data.FindIndex(x => x.StartsWith(@"\/:?*"));

                List<string> DirData = data.GetRange(0, index);
                data.RemoveRange(0, index + 1);
                List<string> FileData = data;

                List<Destination> fileWithRootD = new List<Destination>();
                List<Destination> dirWithRootD = new List<Destination>();

                foreach (string item in FileData)
                {
                    string[] splitted = item.Split(";");
                    Destination sourceRoot = new Destination(splitted[0], Convert.ToInt32(splitted[1]), splitted[2]);
                    fileWithRootD.Add(sourceRoot);
                }
                foreach (string item in DirData)
                {
                    string[] splitted = item.Split(";");
                    Destination destRoot = new Destination(splitted[0], Convert.ToInt32(splitted[1]));
                    dirWithRootD.Add(destRoot);
                }
                CompareDirs(dirWithRootS, dirWithRootD);
                CompareFiles(fileWithRootS, fileWithRootD);

            }
            else
            {
                foreach (Source source in Sources)
                {
                    string[] dirsWithoutRootS = Directory.GetDirectories(source.Path, "*.*", SearchOption.AllDirectories);
                    string[] filesWithoutRootS = Directory.GetFiles(source.Path, "*.*", SearchOption.AllDirectories);

                    foreach (string item in dirsWithoutRootS)
                    {
                        Source sourceRoot = new Source(item, source.SubfoldersCount);
                        NewDirectories.Add(sourceRoot);
                    }
                    foreach (string item in filesWithoutRootS)
                    {
                        Source sourceRoot = new Source(item, source.SubfoldersCount);
                        NewFiles.Add(sourceRoot);
                    }
                }
                using (StreamWriter sw = new StreamWriter(LogDataDest))
                {
                    foreach (Source item in NewDirectories)
                    {
                        DirectoryInfo dir = new DirectoryInfo(item.Path);
                        sw.WriteLine(dir.FullName + ";" + item.SubfoldersCount );
                    }
                    sw.WriteLine(@"\/:?*");
                    foreach (Source item in NewFiles)
                    {
                        FileInfo dir = new FileInfo(item.Path);
                        sw.WriteLine(dir.FullName + ";" + item.SubfoldersCount + ";" + dir.LastWriteTime);
                    }
                }
            }
        }
        public void ReduceSourcesIncr()
        {
            if (PackagesCount != 1)
            {

                List<Source> dirWithRootS = new List<Source>();
                List<Source> fileWithRootS = new List<Source>();
                List<Destination> fileWithRootD = new List<Destination>();
                List<Destination> dirWithRootD = new List<Destination>();
                List<string> DirDataFull = new List<string>();
                List<string> FileDataFull = new List<string>();

                foreach (Source source in Sources)
                {
                    string[] dirsWithoutRootS = Directory.GetDirectories(source.Path, "*.*", SearchOption.AllDirectories);
                    string[] filesWithoutRootS = Directory.GetFiles(source.Path, "*.*", SearchOption.AllDirectories);
                    foreach (string item in dirsWithoutRootS)
                    {
                        Source sourceRoot = new Source(item, source.SubfoldersCount);
                        dirWithRootS.Add(sourceRoot);
                    }
                    foreach (string item in filesWithoutRootS)
                    {
                        Source sourceRoot = new Source(item, source.SubfoldersCount);
                        fileWithRootS.Add(sourceRoot);
                    }
                }

                foreach (Destination destination in SearchedFolders)
                {
                    List<string> data = new List<string>();
                    using StreamReader sr = new StreamReader(destination.Path);
                    {
                        while (!sr.EndOfStream)
                        {
                            data.Add(sr.ReadLine());
                        }
                    }
                    int index = data.FindIndex(x => x.StartsWith(@"\/:?*"));

                    List<string> DirData = data.GetRange(0, index);
                    data.RemoveRange(0, index + 1);
                    List<string> FileData = data;
                    DirDataFull.AddRange(DirData);
                    FileDataFull.AddRange(FileData);
                }
                foreach (string item in FileDataFull)
                {
                    string[] splitted = item.Split(";");
                    Destination sourceRoot = new Destination(splitted[0], Convert.ToInt32(splitted[1]), splitted[2]);
                    fileWithRootD.Add(sourceRoot);
                }
                foreach (string item in DirDataFull)
                {
                    string[] splitted = item.Split(";");
                    Destination destRoot = new Destination(splitted[0], Convert.ToInt32(splitted[1]));
                    dirWithRootD.Add(destRoot);
                }
                CompareDirs(dirWithRootS, dirWithRootD);
                CompareFiles(fileWithRootS, fileWithRootD);
                ImplementSnapshot();


            }
            else
            {
                foreach (Source source in Sources)
                {
                    string[] dirsWithoutRootS = Directory.GetDirectories(source.Path, "*.*", SearchOption.AllDirectories);
                    string[] filesWithoutRootS = Directory.GetFiles(source.Path, "*.*", SearchOption.AllDirectories);

                    foreach (string item in dirsWithoutRootS)
                    {
                        Source sourceRoot = new Source(item, source.SubfoldersCount);
                        NewDirectories.Add(sourceRoot);
                    }
                    foreach (string item in filesWithoutRootS)
                    {
                        Source sourceRoot = new Source(item, source.SubfoldersCount);
                        NewFiles.Add(sourceRoot);
                    }
                }
                ImplementSnapshot();
            }
        }
        public void CompareDirs(List<Source> source, List<Destination> destination)
        {
            foreach (Source item in source)
            {
                string[] splitted = item.Path.Split(@"\");
                string splittedSource = "";
                for (int i = item.SubfoldersCount-1; i < splitted.Length; i++)
                {
                    splittedSource += splitted[i] + @"\";
                }
                bool verified = false;
                foreach (Destination item2 in destination)
                {
                    string[] splitted2 = item2.Path.Split(@"\");
                    string splittedDest = "";
                    for (int i = item2.SubfoldersCount-1; i < splitted2.Length; i++)
                    {
                        splittedDest += splitted2[i] + @"\";
                    }
                    if (splittedSource == splittedDest)
                    {
                        destination.Remove(item2);
                        verified = true;
                        break;
                    }   
                }
                if (!verified)
                {
                    NewDirectories.Add(item);
                }
                     
            }
        }
        public void CompareFiles(List<Source> source, List<Destination> destination)
        {
            foreach (Source item in source)
            {
                string[] splitted = item.Path.Split(@"\");
                string splittedSource = "";
                for (int i = item.SubfoldersCount - 1; i < splitted.Length; i++)
                {
                    splittedSource += splitted[i] + @"\";
                }
                bool verified = false;
                foreach (Destination item2 in destination)
                {
                    string[] splitted2 = item2.Path.Split(@"\");
                    string splittedDest = "";
                    for (int i = item2.SubfoldersCount - 1; i < splitted2.Length; i++)
                    {
                        splittedDest += splitted2[i] + @"\";
                    }
                    if (splittedSource == splittedDest)
                    {
                        DirectoryInfo dir1 = new DirectoryInfo(item.Path);
                        DateTime myDate = DateTime.ParseExact(item2.LastWriteTime, "dd.MM.yyyy H:mm:ss",
                                                System.Globalization.CultureInfo.InvariantCulture);
                        DateTime myDate2 = DateTime.ParseExact(dir1.LastWriteTime.ToString(), "dd.MM.yyyy HH:mm:ss",
                                               System.Globalization.CultureInfo.InvariantCulture);

                        if (myDate >= myDate2)
                        {
                            destination.Remove(item2);
                            verified = true;                         
                        }
                        break;
                    }
                }
                if (!verified)
                {
                    NewFiles.Add(item);
                }

            }
        }
        public void ImplementSnapshot()
        {
            using (StreamWriter sw = new StreamWriter(LogDataDest))
            {
                foreach (Source item in NewDirectories)
                {
                    DirectoryInfo dir = new DirectoryInfo(item.Path);
                    sw.WriteLine(dir.FullName + ";" + item.SubfoldersCount);
                }
                sw.WriteLine(@"\/:?*");
                foreach (Source item in NewFiles)
                {
                    FileInfo dir = new FileInfo(item.Path);
                    sw.WriteLine(dir.FullName + ";" + item.SubfoldersCount + ";" + dir.LastWriteTime);
                }
            }
        }

    }
}
