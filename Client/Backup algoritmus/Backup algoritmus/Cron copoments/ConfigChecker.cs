using Backup_algoritmus.Models;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backup_algoritmus.Algorithm
{
    public class ConfigChecker : IJob
    {

        public async Task Execute(IJobExecutionContext context)
        {
            ConfigService configService = new ConfigService();
            List<Configuration> list;

            if (Directory.Exists(@"C:\Users\Public\Documents\Configs") == false)
            {
                Directory.CreateDirectory(@"C:\Users\Public\Documents\Configs");    
            }
            try
            {
                list = await configService.GetAllConfigs(Program.IdOfThisStation);
            }
            catch (Exception)
            {
                Console.WriteLine("Z API nepřišli data o Konfiguracích!");
                return;                
            }

            DirectoryInfo d = new DirectoryInfo(@"C:\Users\Public\Documents\Configs");





            //CHECKNE VŠECHNY CONFIGY SMAŽE TY KTERE JSOU NAVIC/NEBO JINÉ A STOPNE JEJICH CRON
            foreach (var file in d.GetFiles())
            {
                bool different = true;
                string configSetting;
                using (StreamReader sr = new StreamReader(file.FullName))
                {
                    configSetting = sr.ReadLine();
                    string[] splitSettings = { };
                    string[] splitSourc = { };
                    string[] splitPath = { };
                    string[] splitPlace = { };
                    string[] splitHosts = { };
                    try
                    {
                        splitSettings = configSetting.Split(';');
                        splitSourc = splitSettings[7].Split('?');
                        splitPath = splitSettings[8].Split('?');
                        splitPlace = splitSettings[10].Split('?');
                        splitHosts = splitSettings[11].Split('?');
                    }
                    catch
                    {
                        Console.WriteLine("CHYBNĚ ZAPSANÝ CONFIG!!!  " + file.FullName);
                        return;
                    }
                    int i = 0;
                    List<Destinations> destList = new List<Destinations>();
                    foreach (var de in splitPath)
                    {
                        destList.Add(new Destinations { path = splitPath[i], host = splitHosts[i], place = splitPlace[i] });
                        i++;
                    }

                    Configuration cr = new Configuration { id = Convert.ToInt32(splitSettings[0]), alias = splitSettings[1], format = splitSettings[2], type = splitSettings[3], frequency = splitSettings[4], retention = Convert.ToInt32(splitSettings[5]), packages = Convert.ToInt32(splitSettings[6]), sources = splitSourc, destinations = destList.ToArray() };
                    foreach (var conf in list)
                    {
                        
                        if (conf.id == cr.id && conf.alias == cr.alias && conf.format == cr.format && conf.type == cr.type && conf.frequency == cr.frequency && conf.retention == cr.retention && conf.packages == cr.packages && Enumerable.SequenceEqual(conf.sources,cr.sources) && conf.destinations.Length == cr.destinations.Length)
                        {
                            bool differentdest = false;

                            for (int o = 0; o < conf.destinations.Length; o++)
                            {
                                if ( !Enumerable.SequenceEqual(conf.destinations[o].host, cr.destinations[o].host) || !Enumerable.SequenceEqual(conf.destinations[o].path, cr.destinations[o].path) ||  !Enumerable.SequenceEqual(conf.destinations[o].place, cr.destinations[o].place))
                                {
                                     differentdest = true;
                                }
                            }
                            if (differentdest == false)
                            {
                                different = false;
                                break;
                            }
                        }
                    }
                   
                }
                if (different == true)
                {
                    JobKey jk = new JobKey(configSetting);
                    await Program.CronForConfigs.Scheduler.DeleteJob(jk);
                    try
                    {
                        File.Delete(file.FullName);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + "Nepodařilo se deletenou soubor" + file.FullName);
                    }
                    Console.WriteLine("Config " + file + " nebyl nalezen / byl upraven a byl odstraněn!");
                }

            }

            foreach (var item in list)
            {
                bool exists = false;
                foreach (var file in d.GetFiles())
                {
                    string line;
                    using (StreamReader sr = new StreamReader(file.FullName))
                    {
                        line = sr.ReadLine();
                    }

                    string[] Details = line.Split(";");
                    if (Convert.ToInt32(Details[0]) == item.id && Details[1] == item.alias)
                    {
                        exists = true;
                        break;
                    }
                }
                if (exists == false)
                {
                    using (StreamWriter sw = new StreamWriter(@"C:\Users\Public\Documents\Configs\" + item.id + "_" + item.alias + ".txt"))
                    {
                        sw.Write(item.id + ";" + item.alias + ";" + item.format + ";" + item.type + ";" + item.frequency + ";" + item.retention + ";" + item.packages + ";");
                        int count = item.sources.Length;
                        int i = 1;
                        foreach (string item2 in item.sources)
                        {
                            sw.Write(item2);
                            if (i != count)
                            {
                                i++;
                                sw.Write("?");
                            }
                        }
                        sw.Write(";");
                        int count2 = item.destinations.Length;
                        int i2 = 1;
                        foreach (Destinations item2 in item.destinations)
                        {
                            sw.Write(item2.path);
                            if (i2 != count2)
                            {
                                i2++;
                                sw.Write("?");
                            }
                        }
                        sw.Write(";");

                        sw.Write(Program.IdOfThisStation);
                        sw.Write(";");

                        i2 = 1;
                        foreach (Destinations item2 in item.destinations)
                        {
                            sw.Write(item2.place);
                            if (i2 != count2)
                            {
                                i2++;
                                sw.Write("?");
                            }
                        }
                        sw.Write(";");

                        i2 = 1;
                        foreach (Destinations item2 in item.destinations)
                        {
                            sw.Write(item2.host);
                            if (i2 != count2)
                            {
                                i2++;
                                sw.Write("?");
                            }
                        }
                    }
                    string line;
                    using (StreamReader sr = new StreamReader(@"C:\Users\Public\Documents\Configs\" + item.id + "_" + item.alias + ".txt"))
                    {
                        line = sr.ReadLine();
                    }
                    string[] Details = line.Split(";");
                    await Program.CronForConfigs.Test(Details[4], line);
                    if(Program.BlockOfThisStation == true)
                    {
                        JobKey jk = new JobKey(line);
                        await Program.CronForConfigs.Scheduler.PauseJob(jk);
                    }
                    Console.WriteLine("Vytvořil jsem nový config! " + item.id +" " + item.alias);
                }
               
            }



        }
    }
}
