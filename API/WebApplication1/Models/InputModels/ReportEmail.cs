using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models.InputModels
{
    public class ReportEmail
    {
            public string Station { get; set; }
            public string Config { get; set; }
            public DateTime Time { get; set; }
            public bool Status { get; set; }
            public string Message { get; set; }
    }
}
