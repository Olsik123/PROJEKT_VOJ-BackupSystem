using System;
using System.Collections.Generic;
using System.Text;

namespace Backup_algoritmus
{
    public class Source
    {
        public string Path { get; set; }
        public int SubfoldersCount { get; set; }
        public string LastWriteTime { get; set; }

        public Source(string path)
        {
            this.Path = path;
            Count();

        }
        public Source(string path, int subf)
        {
            this.Path = path;
            this.SubfoldersCount = subf;
        }
        public Source(string path, int subf,string LWT)
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
