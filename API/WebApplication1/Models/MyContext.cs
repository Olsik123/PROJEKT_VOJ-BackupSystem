using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace API_Serivce.Models
{
   
    public class MyContext : DbContext
    {
        public DbSet<Admins> Admins { get; set; }
        public DbSet<Assignments> Assignments { get; set; }
        public DbSet<Configurations> Configurations { get; set; }
        public DbSet<Destinations> Destinations { get; set; }
        public DbSet<Reports> Reports { get; set; }
        public DbSet<Stations> Stations { get; set; }
        public DbSet<Sources> Sources { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder.UseMySQL("server=mysqlstudenti.litv.sssvt.cz;database=3b2_bucinajonas_db1;user=bucinajonas;password=clashroyale1;SslMode=none");
        }
    }
}
