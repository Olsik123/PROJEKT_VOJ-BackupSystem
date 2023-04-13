using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Backup_algoritmus
{
    public class ReportSaver
    {
        Report Report1 { get; set; }

        public ReportSaver(Report report)
        {
            this.Report1 = report;
            SaveReportToFiled();
        }

        public void SaveReportToFiled()
        {
            FileService fs = new FileService();
            fs.CheckReportFile();

            string changedDate =  this.Report1.Date.Replace(":", "T");
            using (StreamWriter sw = new StreamWriter(@"C:\Users\Public\Documents\Reports\"+this.Report1.ConfigID+"__"+ changedDate+".txt", true))
            {
                string reportInLine = this.Report1.StationID + "?:_:?:_:?" + this.Report1.ConfigID + "?:_:?:_:?" + this.Report1.Status + "?:_:?:_:?" + this.Report1.Date + "?:_:?:_:?" + this.Report1.Message;
                sw.WriteLine(reportInLine);
                Console.WriteLine("Uložil jsem report : " + reportInLine);
            }
        }

    }
}
