using Backup_algoritmus.Cron_copoments;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Backup_algoritmus
{
    public class  Program
    {
        public static int IdOfThisStation { get; set; } = 0;
        public static bool BlockOfThisStation { get; set; } = false;
        public static List<Configuration> ListOfConfigs { get; set; } = new List<Configuration>();
        public static ISchedulerFactory  SchedulerFactory = new StdSchedulerFactory();
        public static Cron CronForConfigs;


        static async Task Main(string[] args)
        {
            if (Directory.Exists(@"C:\Users\Public\Documents\Configs") == false)
            {
                Directory.CreateDirectory(@"C:\Users\Public\Documents\Configs");
            }

            if (Directory.Exists(@"C:\Users\Public\Documents\Reports") == false)
            {
                Directory.CreateDirectory(@"C:\Users\Public\Documents\Reports");
            }



            IDChecker check = new IDChecker();
            check.CheckId();
            Thread.Sleep(10000);
            while (IdOfThisStation <= 0)
            {
                check.CheckId();
                Thread.Sleep(10000);
            }
            CronForConfigs = new Cron(await SchedulerFactory.GetScheduler());
            await CronForConfigs.Scheduler.Start();

            BanChecker bancheck = new BanChecker();
            bancheck.CheckBan();

            ISchedulerFactory schedulerFactory2 = new StdSchedulerFactory();
            ISchedulerFactory schedulerFactory3 = new StdSchedulerFactory();
            ISchedulerFactory schedulerFactory4 = new StdSchedulerFactory();


            CronForBan cronForBan = new CronForBan(await schedulerFactory3.GetScheduler());
            CronForReports cronReports = new CronForReports(await schedulerFactory2.GetScheduler());
            CronForConfigs cronConfigs = new CronForConfigs(await schedulerFactory4.GetScheduler());

            await cronForBan.Test();
            await cronReports.Test();
            await cronConfigs.Test();

      
           


            DirectoryInfo d = new DirectoryInfo(@"C:\Users\Public\Documents\Configs");

            foreach (var file in d.GetFiles())
            {
                string line;
                using (StreamReader sr = new StreamReader(file.FullName))
                {
                    line = sr.ReadLine();
                }
                string[] Details = line.Split(";");
                await Program.CronForConfigs.Test(Details[4], line);

            }

            while (true)
            {
                Thread.Sleep(1000);
            }


        }
    }
}
