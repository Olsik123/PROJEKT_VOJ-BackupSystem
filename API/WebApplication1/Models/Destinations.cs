using System;
using System.Collections.Generic;
using System.Text;

namespace API_Serivce.Models
{
    public class Destinations
    {
        public int Id { get; set; }
        public int ConfigurationId { get; set; }
        public string Place { get; set; }
        public string Path { get; set; }
        public string FTPurl { get; set; }
    }
}
