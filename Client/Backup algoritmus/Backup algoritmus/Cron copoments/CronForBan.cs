using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backup_algoritmus.Cron_copoments
{
    public class CronForBan
    {
        public IScheduler Scheduler { get; set; }

        public CronForBan(IScheduler scheduler)
        {
            this.Scheduler = scheduler;
        }

        public async Task Test()
        {


            IJobDetail jobDetail = JobBuilder.Create<BanChecker>()
                .Build();
            ITrigger trigger = TriggerBuilder.Create()
                .ForJob(jobDetail)
                .WithCronSchedule("30 * * * * ?") //sekundy, minuty, hodiny, dnyVMesici, Mesic, dnyvTydnu;  dny v tyndu nebo dny v mesici musi byt oteznik ; otaznik je neco jako "*"
                .StartNow()
                .Build();
            await Scheduler.ScheduleJob(jobDetail, trigger);



        }
    }
}
