using API_Serivce.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using WebApplication1.Models.InputModels;

namespace WebApplication1.Cron
{
    public class EmailSender : IJob
    {
        private MyContext context = new MyContext();

        public Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            string line = dataMap.GetString("id");
            Admins ad = new Admins();
            try
            {
                ad = this.context.Admins.FirstOrDefault(x => x.Id == Convert.ToInt32(line));
            }
            catch
            {
                throw new Exception("Invalid station");
            }

            SmtpClient Client = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential()
                {
                    UserName = "projektvoj.sssvt@gmail.com",
                    Password = "nkesplqbodcchnxi",



                }
            };
            string mes = "";
            DateTime tm = DateTime.Now.AddDays(-1);
            if (ad.Frequency == "0 0 0 * * ?")
            {
               tm = DateTime.Now.AddDays(-1);

            }
            if (ad.Frequency == "0 0 0 1 * ?")
            {
                tm = DateTime.Now.AddDays(-7);
            }
            if (ad.Frequency == "0 0 0 ? * 1")
            {
                tm = DateTime.Now.AddDays(-31);
            }
            else
            {
                tm = DateTime.Now.AddDays(-1);
            }

            var reports = (from r in this.context.Reports
                           where r.Date > tm
                           join a in this.context.Assignments on r.AssignmentId equals a.Id
                           join s in this.context.Stations on a.StationId equals s.Id
                           join c in this.context.Configurations on a.ConfigurationId equals c.Id
                           select new ReportEmail { Station = s.Alias, Config = c.Alias, Time = r.Date, Status = r.Status, Message = r.Message }
            );
            List<ReportEmail> re = reports.ToList();

            foreach (ReportEmail item in re)
            {
                string suc = item.Status ? "Succes" : "Failure";
                mes += $"Config : {item.Config}, Station : {item.Station}, Status : {suc}, Date : {item.Time}\n";
            }



            MailAddress FromEmail = new MailAddress("projektvoj.sssvt@gmail.com", "Projekt VOJ");
            MailAddress ToEmail = new MailAddress(ad.Email, "Someone");
            MailMessage Message = new MailMessage()
            {
                From = FromEmail,
                Subject = "Report " + DateTime.Now.ToString("dd.MM.yyyy H:mm"),
                Body = mes,
               
            };
            try
            {
                Message.To.Add(ToEmail);
                Client.SendMailAsync(Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Task.CompletedTask;
        }
    }
}
