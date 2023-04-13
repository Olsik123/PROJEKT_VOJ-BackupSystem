using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Backup_algoritmus
{
    public class Configuration
    {
        public int id { get; set; }
        public string alias { get; set; }
        public string format { get; set; }
        public string type { get; set; }
        public string frequency { get; set; }
        public int retention { get; set; }
        public int packages { get; set; }
        public string[] sources { get; set; }
        public Destinations[] destinations { get; set; }
    }
    public class Destinations 
    {
        public string place { get; set; }
        public string path { get; set; }
        public string host { get; set; }

    }

}
