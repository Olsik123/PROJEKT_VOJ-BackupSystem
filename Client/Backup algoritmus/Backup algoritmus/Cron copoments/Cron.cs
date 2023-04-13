using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Backup_algoritmus
{
    public class Cron
    {
        public IScheduler Scheduler { get; set; }

        public Cron(IScheduler scheduler)
        {
            this.Scheduler = scheduler;
        }

        public async Task Test(string cronstring,string service)
        {
            cronstring = "45 "+ cronstring;
            if (cronstring.Split(" ")[4][0] == '*')
            {
                string[] split = cronstring.Split(" ");
                split[3] = "?";
                cronstring = "";
                int i = 0;
                foreach (string item in split)
                {
                    
                    cronstring += item;
                    if (i != 5)
                    {
                        cronstring += " ";
                    }
                    i++;
                }

            }
            else if (cronstring.Split(" ")[6][0] == '*')
            {
                string[] split = cronstring.Split(" ");
                split[5] = "?";
                int i = 0;
                foreach (string item in split)
                {
                    cronstring += item;
                    if (i != 5)
                    {
                        cronstring += " ";
                    }
                    i++;
                }

            }
            else
            {
                throw new Exception("Not valid cron format!");
            }

            IJobDetail jobDetail = JobBuilder.Create<SetBackupSystem>()
                .WithIdentity(service)
                .UsingJobData("line", service)
                .Build();
            ITrigger trigger = TriggerBuilder.Create()
                .ForJob(jobDetail)
                .WithCronSchedule(cronstring) //sekundy, minuty, hodiny, dnyVMesici, Mesic, dnyvTydnu;  dny v tyndu nebo dny v mesici musi byt oteznik ; otaznik je neco jako "*"
                .StartNow()
                .Build();
            await Scheduler.ScheduleJob(jobDetail, trigger);


           
        }
    }

 
}

