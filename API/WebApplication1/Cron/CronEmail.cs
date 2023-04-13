using API_Serivce.Models;
using Quartz;
using System;
using System.Threading.Tasks;

namespace WebApplication1.Cron
{
    public class CronEmail
    {
        public IScheduler Scheduler { get; set; }
        public CronEmail(IScheduler scheduler)
        {
            this.Scheduler = scheduler;
        }

        public async Task Test(Admins admin)
        {
            IJobDetail jobDetail = JobBuilder.Create<EmailSender>()
                .WithIdentity(admin.Id.ToString())
                .UsingJobData("id",admin.Id.ToString())
                .Build();
            ITrigger trigger = TriggerBuilder.Create()
                .ForJob(jobDetail)
                .WithCronSchedule(admin.Frequency)
                .StartNow()
                .Build();
            await Scheduler.ScheduleJob(jobDetail, trigger);
        }
    }
}
