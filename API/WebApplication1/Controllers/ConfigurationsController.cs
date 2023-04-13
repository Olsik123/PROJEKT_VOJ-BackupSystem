using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using API_Serivce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication1.Controllers;
using WebApplication1.Models;
using WebApplication1.Models.InputModels;

namespace API_Serivce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigurationsController
    {
        private MyContext context = new MyContext();

        [HttpGet("GetAll")]
        public dynamic GetAll()
        {
            return GetOne(-100);
        }
        private dynamic CheckAssi(int Ids1, int Ids2)
        {
            bool AssFound = Ids1 == Ids2;

            if (!AssFound)
                return false;

            return true;


        }

        [HttpGet("GetByStationId/{id}")]
        public dynamic GetByStationId(int id)
        {
            IEnumerable<Assignments> assi = new List<Assignments>();

            try
            {
                assi = this.context.Assignments.ToList().Where(x => CheckAssi(id, x.StationId));
            }
            catch
            {
                throw new Exception("Invalid station");
            }


            if (assi == null)
                throw new Exception("Invalid station");

            List<dynamic> listik = new List<dynamic>();

            foreach (var item in assi)
            {
                listik.Add(this.GetOne(item.ConfigurationId));
            }

            return listik;
        }

        [HttpGet("GetOne/{id}")]
        public dynamic GetOne(int id)
        {
            int ConfigId = id;
            var config =

                 from configInfo in
                 (
                 from srcs in (
                      from c in context.Configurations.ToList()
                      join s in context.Sources.ToList() on c.Id equals s.ConfigId
                      group s.Path by c.Id into grp
                      select new { id = grp.Key, grp }
                 )
                 join dests in (from c in context.Configurations.ToList()
                                join d in context.Destinations.ToList() on c.Id equals d.ConfigurationId
                                group new { place = d.Place, path = d.Path, host = d.FTPurl } by c.Id into grp
                                select new { id = grp.Key, grp }) on srcs.id equals dests.id

                 join clientsArray in
                 (
                     from cnfigs in context.Configurations.ToList()
                     join assignms in context.Assignments.ToList() on cnfigs.Id equals assignms.ConfigurationId
                     join clnts in context.Stations.ToList() on assignms.StationId equals clnts.Id
                     group clnts.Alias by cnfigs.Id into grp
                     select new { id = grp.Key, grp }
                 ) on srcs.id equals clientsArray.id
                 join clientsArray2 in
                 (
                     from cnfigs in context.Configurations.ToList()
                     join assignms in context.Assignments.ToList() on cnfigs.Id equals assignms.ConfigurationId
                     join clnts in context.Stations.ToList() on assignms.StationId equals clnts.Id
                     group clnts.Id by cnfigs.Id into grp2
                     select new { id = grp2.Key, grp2 }
                 ) on srcs.id equals clientsArray2.id
                 select new { srcs, dests, clientsArray, clientsArray2 }
                 )
                 join outerConfigs in context.Configurations on configInfo.srcs.id equals outerConfigs.Id
                 where outerConfigs.Id == ConfigId || ConfigId == -100
                 select new
                 {
                     id = outerConfigs.Id,
                     adminId = outerConfigs.AdminId,
                     alias = outerConfigs.Alias,
                     clientIds = configInfo.clientsArray2.grp2,
                     clients = configInfo.clientsArray.grp,
                     format = outerConfigs.Format,
                     type = outerConfigs.Type,
                     frequency = outerConfigs.Frequency,
                     retention = outerConfigs.Retention,
                     packages = outerConfigs.Packages,
                     sources = configInfo.srcs.grp,
                     destinations = configInfo.dests.grp
                 }
                 ;

            if (ConfigId != -100)
            {
                if (config.ToList().Count == 0)
                {
                    return new JsonResult("ID not found") { StatusCode = StatusCodes.Status400BadRequest };
                }
                return config.First();
            }


            return config;
        }

        [HttpPost("CreatoneOne")]
        public JsonResult CreateOne(ConfigurationInput configuration)
        {
            try
            {
                Configurations co = new Configurations();
                co.AdminId = configuration.adminId;
                co.Format = configuration.format;
                co.Type = configuration.type;
                co.Retention = configuration.retention;
                co.Frequency = configuration.frequency;
                co.Packages = configuration.packages;
                co.Alias = configuration.alias;
                this.context.Configurations.Add(co);
                this.context.SaveChanges();
                foreach (var item in configuration.sources)
                {
                    Sources so = new Sources();
                    so.ConfigId = co.Id;
                    so.Path = item;
                    this.context.Sources.Add(so);
                }
                foreach (var item in configuration.destinations)
                {
                    Destinations de = new Destinations();
                    de.ConfigurationId = co.Id;
                    de.Path = item.path;
                    de.Place = item.place;
                    de.FTPurl = item.host;
                    this.context.Destinations.Add(de);
                }
                foreach (var item in configuration.clientIds)
                {
                    Assignments ass = new Assignments();
                    ass.ConfigurationId = co.Id;
                    ass.StationId = item;
                    this.context.Assignments.Add(ass);

                }
                this.context.SaveChanges();
                return new JsonResult("Success") { StatusCode = StatusCodes.Status200OK };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new JsonResult("Failure") { StatusCode = StatusCodes.Status400BadRequest };
            }
        }


        [HttpPut("UpdateOne/{id}")]
        public JsonResult UpdateOne(ConfigurationInput configuration, int id)
        {

            DeleteOne(id);
            CreateOne(configuration);
            try
            {
                this.context.SaveChanges();
                return new JsonResult("Succes") { StatusCode = StatusCodes.Status200OK };
            }
            catch (Exception)
            {
                return new JsonResult("Failure") { StatusCode = StatusCodes.Status400BadRequest };
            }

        }

        [HttpDelete("DeleteOne/{id}")]
        public JsonResult DeleteOne(int id)
        {
            try
            {
                Configurations configurations = this.context.Configurations.Find(id);
                if (configurations == null)
                {
                    return new JsonResult("ID not found") { StatusCode = StatusCodes.Status400BadRequest };
                }
                this.context.Configurations.Remove(configurations);
                this.context.SaveChanges();
                return new JsonResult("Succes") { StatusCode = StatusCodes.Status200OK };
            }
            catch
            {
                return new JsonResult("Failure") { StatusCode = StatusCodes.Status400BadRequest };
            }

        }

    }
}
