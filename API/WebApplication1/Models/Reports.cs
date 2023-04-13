using System;
using System.Collections.Generic;
using System.Text;

namespace API_Serivce.Models
{
    public class Reports
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
    }
}
