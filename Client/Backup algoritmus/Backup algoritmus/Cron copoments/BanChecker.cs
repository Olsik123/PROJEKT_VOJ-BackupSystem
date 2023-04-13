using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Backup_algoritmus.Cron_copoments
{
    class BanChecker : IJob
    {
        private HttpClient client;

        public BanChecker()
        {
            this.client = new HttpClient();
            this.client.BaseAddress = new Uri("http://localhost:18788");
        }

        public async Task Execute(IJobExecutionContext context)
        {
            string result;
            try
            {
                result = await this.client.GetStringAsync("/Stations/CheckBlock/" + Program.IdOfThisStation);
            }
            catch
            {
                result = "10";
                Console.WriteLine("Nedalo se zjistit zda-li stanice je bloknutá.");
            }

            if (result == "-1")
            {
                IDChecker check = new IDChecker();
                Console.WriteLine("ID stanice neexistuje.");
                check.CheckId();

            }
            else if (result == "0")
            {
                Console.WriteLine("Stanice není bloknutá.");
                Program.BlockOfThisStation = false;
                DirectoryInfo d = new DirectoryInfo(@"C:\Users\Public\Documents\Configs");
                foreach (var file in d.GetFiles())
                {
                    string line;
                    using (StreamReader sr = new StreamReader(file.FullName))
                    {
                        line = sr.ReadLine();
                    }
                    JobKey jk = new JobKey(line);
                    try
                    {
                        await Program.CronForConfigs.Scheduler.ResumeJob(jk);

                    }
                    catch 
                    {

                    }

                }
            }
            else if (result == "1")
            {
                Console.WriteLine("Stanice je bloknutá.");

                Program.BlockOfThisStation = true;

                DirectoryInfo d = new DirectoryInfo(@"C:\Users\Public\Documents\Configs");

                foreach (var file in d.GetFiles())
                {
                    string line;
                    using (StreamReader sr = new StreamReader(file.FullName))
                    {
                        line = sr.ReadLine();
                    }
                    JobKey jk = new JobKey(line);
                    try
                    {
                        await Program.CronForConfigs.Scheduler.PauseJob(jk);
                    }
                    catch 
                    {


                    }
                }
            }


        }
        public async void CheckBan()
        {
            string result;
            try
            {
                result = await this.client.GetStringAsync("/Stations/CheckBlock/" + Program.IdOfThisStation);
            }
            catch
            {
                result = "10";
                Console.WriteLine("Nedalo se zjistit zda-li stanice je bloknutá.");
            }

            if (result == "-1")
            {
                IDChecker check = new IDChecker();
                Console.WriteLine("ID stanice neexistuje.");
                check.CheckId();

            }
            else if (result == "0")
            {
                Console.WriteLine("Stanice není bloknutá.");
                Program.BlockOfThisStation = false;
                DirectoryInfo d = new DirectoryInfo(@"C:\Users\Public\Documents\Configs");
                foreach (var file in d.GetFiles())
                {
                    string line;
                    using (StreamReader sr = new StreamReader(file.FullName))
                    {
                        line = sr.ReadLine();
                    }
                    JobKey jk = new JobKey(line);
                    try
                    {
                        await Program.CronForConfigs.Scheduler.ResumeJob(jk);
                    }
                    catch 
                    {

               
                    }


                }
            }
            else if (result == "1")
            {
                Console.WriteLine("Stanice je bloknutá.");

                Program.BlockOfThisStation = true;

                DirectoryInfo d = new DirectoryInfo(@"C:\Users\Public\Documents\Configs");

                foreach (var file in d.GetFiles())
                {
                    string line;
                    using (StreamReader sr = new StreamReader(file.FullName))
                    {
                        line = sr.ReadLine();
                    }
                    JobKey jk = new JobKey(line);
                    try
                    {
                        await Program.CronForConfigs.Scheduler.PauseJob(jk);
                    }
                    catch 
                    {

                    }

                }
            }
        }
    }
}

