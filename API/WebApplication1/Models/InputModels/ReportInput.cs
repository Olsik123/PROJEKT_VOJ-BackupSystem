using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models.InputModels
{
    public class ReportInput
    {
        public int StationID { get; set; }
        public int ConfigID { get; set; }
        public bool Status { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; } 
    }
}
