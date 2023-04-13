using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backup_algoritmus
{
    public class SetBackupSystem : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            string line = dataMap.GetString("line");
            string[] Details = line.Split(";");
            Console.WriteLine("Backup začal : "+ Details[1]);
            try
            {
                BackupSystem service = new BackupSystem(Details[0], Details[1], Details[3], Details[7].Split("?"), Details[8].Split("?"), Convert.ToInt32(Details[5]), Convert.ToInt32(Details[6]), Details[10].Split("?"), Details[11].Split("?"), Details[2]);
                Console.WriteLine("Uspěšně : " + Details[1]);
                ReportService rp = new ReportService();
                rp.Create(line);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Neuspěšně : " + Details[1]);
                ReportService rp = new ReportService();
                rp.Create(line,ex.Message);
            }
             return Task.CompletedTask;
        }

    }
}
