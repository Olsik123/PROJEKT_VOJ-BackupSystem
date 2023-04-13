using System;
using System.Collections.Generic;
using System.Text;

using API_Serivce.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.InputModels;
using Microsoft.AspNetCore.Http;
using WebApplication1.Controllers;

namespace API_Serivce.Controllers
{

    [ApiController]
    //[Authorize]
    [Route("[controller]")]
    public class StationsController : ControllerBase
    {
        private MyContext context = new MyContext();


        [Authorize]
        [HttpGet("GetAll")]
        public dynamic GetAll()
        {
                dynamic reports = 
                              
                      from includedConfigs in (
                               from a in context.Assignments.ToList()
                               join s in context.Stations.ToList() on a.StationId equals s.Id
                               join c in context.Configurations.ToList() on a.ConfigurationId equals c.Id 
                               group c.Alias by s.Id into grp
                               select new { id = grp.Key , configsArray = grp }
                               )
                               join IdArray in (
                               from a in context.Assignments.ToList()
                               join s in context.Stations.ToList() on a.StationId equals s.Id
                               join c in context.Configurations.ToList() on a.ConfigurationId equals c.Id 
                               group c.Id by s.Id into grp2 
                               select new { id = grp2.Key, configIdsArray = grp2 }
                               ) on includedConfigs.id equals IdArray.id
                              
                               join stations in context.Stations on includedConfigs.id equals stations.Id
                               select new
                               {
                                   id = stations.Id,
                                   alias = stations.Alias,
                                   configIds = IdArray.configIdsArray,
                                   configs = includedConfigs.configsArray,
                                   mac = stations.Mac,
                                   ip = stations.Ip,
                                   verified = stations.Verified,
                                   ban = stations.Ban
                               };


            return reports;
        }
        [HttpGet("GetOne/{id}")]
        public dynamic GetOne(int id)
        {
            dynamic reports =

                  from includedConfigs in (
                           from a in context.Assignments.ToList()
                           join s in context.Stations.ToList() on a.StationId equals s.Id
                           join c in context.Configurations.ToList() on a.ConfigurationId equals c.Id
                           group c.Alias by s.Id into grp
                           select new { id = grp.Key, configsArray = grp }
                           )
                  join IdArray in (
                      from a in context.Assignments.ToList()
                           join s in context.Stations.ToList() on a.StationId equals s.Id
                           join c in context.Configurations.ToList() on a.ConfigurationId equals c.Id
                           group c.Id by s.Id into grp2
                           select new { id = grp2.Key, configIdsArray = grp2 }
                      ) on includedConfigs.id equals IdArray.id

                  join stations in context.Stations on includedConfigs.id equals stations.Id
                  where stations.Id == id
                  select new
                  {
                      id = stations.Id,
                      alias = stations.Alias,
                      configIds = IdArray.configIdsArray,
                      configs = includedConfigs.configsArray,
                      mac = stations.Mac,
                      ip = stations.Ip,
                      verified = stations.Verified,
                      ban = stations.Ban
                  };


            return reports;

        }
        [Authorize]
        [HttpGet("GetUnverified")]
        public dynamic GetUnverified()
        {
            dynamic reports =

                   from stations in this.context.Stations
                   where stations.Verified == false
                   select new
                  {
                      id = stations.Id,
                      alias = stations.Alias,
                      mac = stations.Mac,
                      ip = stations.Ip,
                      verify = stations.Verified
                  };


            return reports;
        }

        [Authorize]
        [HttpGet("GetVerifiedAndUnblocked")]
        public dynamic GetVerifiedAndUnblocked()
        {
            dynamic reports =

                    from client in this.context.Stations.ToList()
                    from a in context.Assignments.ToList().Where(x => x.StationId == client.Id).DefaultIfEmpty()
                    from c in context.Configurations.ToList().Where(x => (a != null ? x.Id == a.ConfigurationId : false)).DefaultIfEmpty()
                    group c by client.Id into grp
                    from clients in this.context.Stations.ToList().Where(x => x.Id == grp.Key && x.Ban == false && x.Verified == true)

                    select new
                    {
                        id = grp.Key,
                        alias = clients.Alias,
                        configsIds = grp.Select(x => x == null ? 0 : x.Id),
                        configs = grp.Select(x => x == null ? null : x.Alias),
                        mac = clients.Mac,
                        ip = clients.Ip,
                        verify = clients.Verified
                    };

            return reports;
        }




        private dynamic VerifyStations(string MAC1, string MAC2)
        {
            bool loginFound = MAC1 == MAC2;

            if (!loginFound)
                return false;

            return true;

   
        }
        [HttpGet("CheckBlock/{id}")]
        public int CheckID(string id)
        {

            Stations station = new Stations();

            //-1 STATION DOESNT EXISTS
            // 0 UNBLOCKED
            // 1 BLOCKED


            try
            {
                station =
                this.context.Stations.ToList().Where(x => VerifyStations(id, x.Id.ToString())).FirstOrDefault();
                if (station == null)
                    return -1;
                if (station.Ban == true)             
                    return 1;                         
                return 0;   
            }
            catch
            {
                return -1;
            }

        }

        [HttpGet("CheckID/{ip}/{mac}")]
        public dynamic CheckID(string ip,string mac)
        {
            
            Stations station = new Stations();

            StationOutput sto = new StationOutput();
            sto.Ip = ip;
            sto.Mac = mac;


            try
            {
                station =
                this.context.Stations.ToList().Where(x => VerifyStations(sto.Mac, x.Mac) && VerifyStations(sto.Ip, x.Ip)).FirstOrDefault();
            }
            catch
            {
                return null;
            }


            if (station == null)
                return null;




            return station;


        }

        [HttpPost("CreateOne")]
        public JsonResult CreateOne(StationInputClient newStation)
        {
            try
            {
                Stations stat = new Stations();
                stat.Alias = newStation.Alias;
                stat.Ip = newStation.Ip;
                stat.Mac = newStation.Mac;
                context.Stations.Add(stat);
                context.SaveChanges();
                return new JsonResult("Succes") { StatusCode = StatusCodes.Status200OK };

            }
            catch
            {
                return new JsonResult("Failure") { StatusCode = StatusCodes.Status400BadRequest };
            }

        }

        [Authorize]
        [HttpPut("UpdateOne/{id}")]
        public JsonResult UpdateOne(StationsInput updatedStation)
        {   
            DeleteOne(updatedStation.Id);

            try
            {
                Stations beiingUpdated = new Stations();

                beiingUpdated.Id = updatedStation.Id;
                beiingUpdated.Mac = updatedStation.Mac;
                beiingUpdated.Ip = updatedStation.Ip;
                beiingUpdated.Alias = updatedStation.Alias;
                beiingUpdated.Verified = updatedStation.Verified;
                beiingUpdated.Ban = updatedStation.Ban;
                this.context.Stations.Add(beiingUpdated);
                context.SaveChanges();
                foreach (var item in updatedStation.configIds)
                {
                    Assignments ass = new Assignments();
                    ass.ConfigurationId = item;
                    ass.StationId = updatedStation.Id;
                    this.context.Assignments.Add(ass);
                }
                context.SaveChanges();
                return new JsonResult("Succes") { StatusCode = StatusCodes.Status200OK };
            }
            catch 
            {
                return new JsonResult("Failure") { StatusCode = StatusCodes.Status400BadRequest };

            }
          
        }
        private dynamic TrustStation(int MAC1, int MAC2)
        {
            bool loginFound = MAC1 == MAC2;

            if (!loginFound)
                return false;

            return true;


        }

        [HttpPut("Trust/{id}")]
        public JsonResult Trust(int id)
        {
             Stations station =
               this.context.Stations.ToList().Where(x => TrustStation(id, x.Id)).FirstOrDefault();
            try
            {
                station.Verified = true;
                this.context.SaveChanges();
                return new JsonResult("Succes") { StatusCode = StatusCodes.Status200OK };
            }
            catch
            {
                return new JsonResult("Failure") { StatusCode = StatusCodes.Status400BadRequest };

            }

        }

        [Authorize]
        [HttpDelete("DeleteOne/{id}")]
        public JsonResult DeleteOne(int id)
        {
            try
            {
                Stations st = this.context.Stations.Find(id);
                if (st == null)
                {
                    return new JsonResult("ID not found") { StatusCode = StatusCodes.Status400BadRequest };
                }
                this.context.Stations.Remove(st);
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
