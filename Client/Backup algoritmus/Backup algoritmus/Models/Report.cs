using System;
using System.Collections.Generic;
using System.Text;

namespace Backup_algoritmus
{
    public class Report
    {
        public int StationID { get; set; }
        public int ConfigID { get; set; }
        public bool Status { get; set; }
        public string Date { get; set; }
        public string Message { get; set; }
    }
}
