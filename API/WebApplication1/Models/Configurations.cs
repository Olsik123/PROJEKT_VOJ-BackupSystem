using System;
using System.Collections.Generic;
using System.Text;

namespace API_Serivce.Models
{
    public class Configurations
    {
        public int Id { get; set; }
        public int AdminId { get; set; }
        public string Format { get; set; }
        public string Type { get; set; }
        public int Retention { get; set; }
        public string Frequency { get; set; }
        public int Packages { get; set; }
        public string Alias { get; set; }
    }
}
