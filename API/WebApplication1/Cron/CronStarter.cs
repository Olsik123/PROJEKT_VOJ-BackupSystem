using API_Serivce.Models;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models.InputModels;

namespace WebApplication1.Cron
{
    public class CronStarter
    {
        private MyContext context = new MyContext();
        public static ISchedulerFactory SchedulerFactory = new StdSchedulerFactory();
        public static CronEmail CronForEmail;

        public async void Initalize()
        {
           
            bool emailSetup = false;
            List<Admins> ad = new List<Admins>();
            while (!emailSetup)
            {
                try
                {
                    ad = this.context.Admins.ToList();
                    emailSetup = true;
                }
                catch
                {

                }
            }

            foreach (Admins item in ad)
            {
                ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
                CronForEmail = new CronEmail(await schedulerFactory.GetScheduler());
                await CronForEmail.Test(item);        
            }
            await CronForEmail.Scheduler.Start();
        }
    }
}
