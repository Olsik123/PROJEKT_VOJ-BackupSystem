using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Backup_algoritmus
{
    public class ReportService : IJob
    {
        private HttpClient client;

        public ReportService()
        {
            this.client = new HttpClient();
            this.client.BaseAddress = new Uri("http://localhost:18788");

        }

        public async void Create(string blah)
        {
            //SUCCES
            string[] Details = blah.Split(";");
            Report report = new Report() {StationID = Convert.ToInt32(Details[9]),ConfigID = Convert.ToInt32(Details[0]),Status = true,Date= DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),Message = "-"};
            try
            {
                await this.client.PostAsJsonAsync("/Reports/CreateOne", report);
                Console.WriteLine("Poslal jsem report!");
            }
            catch
            {
                Console.WriteLine("Nepodařilo se poslat report!");
                ReportSaver RepS = new ReportSaver(report);
            }
        }
        public async void Create(string blah,string message)
        {
            //FAILED
            string[] Details = blah.Split(";");
            Report report = new Report() { StationID = Convert.ToInt32(Details[9]), ConfigID = Convert.ToInt32(Details[0]), Status = false, Date = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),Message = message };
            try
            {
                await this.client.PostAsJsonAsync("/Reports/CreateOne", report);
                Console.WriteLine("Poslal jsem report!");
            }
            catch
            {
                Console.WriteLine("Nepodařilo se poslat report!");
                ReportSaver RepS = new ReportSaver(report);

            }
        }

        public async Task Execute(IJobExecutionContext context)
        {
            FileService fs = new FileService();
            fs.CheckReportFile();
            string[] dirs = Directory.GetFiles(@"C:\Users\Public\Documents\Reports", "*");

            foreach (string item in dirs)
            {
                bool passed = true;
                string line;
                using (StreamReader sr = new StreamReader(item))
                {
                    line = sr.ReadLine();
                }
                string[] data = line.Split("?:_:?:_:?");
                Report report = new Report() { StationID = Convert.ToInt32(data[0]), ConfigID = Convert.ToInt32(data[1]), Status = Convert.ToBoolean(data[2]), Date = data[3], Message = data[4] };
                try
                {
                    await this.client.PostAsJsonAsync("/Reports/CreateOne", report);
                }
                catch
                {
                    passed = false;
                    Console.WriteLine("Uložený report se nepodařil poslat! :(  " + item);
                }
                if (passed == true)
                {
                    File.Delete(item);
                    Console.WriteLine("Uložený report se podařil poslat! :)  " + item);
                }
            }

        }
        
    }

}
