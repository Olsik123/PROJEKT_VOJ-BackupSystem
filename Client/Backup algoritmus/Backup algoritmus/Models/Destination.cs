using System;
using System.Collections.Generic;
using System.Text;

namespace Backup_algoritmus
{
    public class Destination
    {
        public string Path { get; set; }
        public int SubfoldersCount { get; set; }
        public string LastWriteTime { get; set; }
        public string Place { get; set; }
        public string Host { get; set; }

        public Destination(string path)
        {
            this.Path = path;
            Count();

        }
        public Destination(string path, string place, string host)
        {
            this.Path = path;
            this.Place = place;
            this.Host = host;
            Count();

        }
        public Destination(string path, int subf)
        {
            this.Path = path;
            this.SubfoldersCount = subf;
        }
        public Destination (string path, int subf, string LWT)
        {
            this.Path = path;
            this.SubfoldersCount = subf;
            this.LastWriteTime = LWT;
        }
        public void Count()
        {
            this.SubfoldersCount = Path.Split(@"\").Length;
        }
    }
}
