using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models.InputModels
{
    public class StationsInput
    {
        public int Id { get; set; }
        public int[] configIds { get; set; }
        public string[] configs { get; set; }
        public string Mac { get; set; }
        public string Ip { get; set; }
        public string Alias { get; set; }
        public bool Verified { get; set; }
        public bool Ban { get; set; }

    }
}
