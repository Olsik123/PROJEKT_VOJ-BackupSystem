using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models.InputModels
{
    public class AdminInput
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string Frequency { get; set; }
        public string Email { get; set; }
    }
}
