using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models.InputModels
{
    public class ConfigurationInput2
    {
        public int Id { get; set; }
        public int adminId { get; set; }
        public string alias { get; set; }
        public int[] clientIds { get; set; }
        public string format { get; set; }
        public string type { get; set; }
        public string frequency { get; set; }
        public int retention { get; set; }
        public int packages { get; set; }
        public string[] sources { get; set; }
        public Destination[] destinations { get; set; }
    }

}
