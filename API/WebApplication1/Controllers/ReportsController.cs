using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using API_Serivce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Controllers;
using WebApplication1.Models.InputModels;

namespace API_Serivce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportsController : ControllerBase
    {
        private MyContext context = new MyContext();
        [Authorize]
        [HttpGet("GetAll")]
        public dynamic GetAll()
        {
            dynamic reports = (from r in context.Reports
                               join a in context.Assignments on r.AssignmentId equals a.Id
                               join s in context.Stations on a.StationId equals s.Id
                               join c in context.Configurations on a.ConfigurationId equals c.Id
                               select new { id = r.Id, station = s.Alias, config = c.Alias, time = r.Date, success = r.Status, message = r.Message }
                           );

            return reports;
        }
        [HttpGet("GetAllWithParamaters/{status}/{datefrom}/{dateto}")]
        public dynamic GetAllWithParamaters(bool status, DateTime datefrom, DateTime dateto)
        {
            dynamic reports = (from r in context.Reports
                               where dateto > r.Date && r.Date > datefrom && r.Status == status
                               join a in context.Assignments on r.AssignmentId equals a.Id
                               join s in context.Stations on a.StationId equals s.Id
                               join c in context.Configurations on a.ConfigurationId equals c.Id
                               select new { id = r.Id, station = s.Alias, config = c.Alias, time = r.Date, success = r.Status, message = r.Message }
                           );

            return reports;
        }
        [HttpGet("GetAllWithParamaters/{status}")]
        public dynamic GetAllWithParamaters(bool status)
        {
            dynamic reports = (from r in context.Reports
                               join a in context.Assignments on r.AssignmentId equals a.Id  
                               join s in context.Stations on a.StationId equals s.Id
                               join c in context.Configurations on a.ConfigurationId equals c.Id
                               where r.Status == status
                               select new { id = r.Id, station = s.Alias, config = c.Alias, time = r.Date, success = r.Status, message = r.Message }
                           );

            return reports;
        }
        [HttpGet("GetAllWithParamaters/{datefrom}/{dateto}")]
        public dynamic GetAllWithParamaters(DateTime datefrom, DateTime dateto)
        {
            dynamic reports = (from r in context.Reports
                               where dateto > r.Date && r.Date > datefrom
                               join a in context.Assignments on r.AssignmentId equals a.Id
                               join s in context.Stations on a.StationId equals s.Id
                               join c in context.Configurations on a.ConfigurationId equals c.Id
                               select new { id = r.Id, station = s.Alias, config = c.Alias, time = r.Date, success = r.Status, message = r.Message }
                           );

            return reports;
        }

        [HttpGet("GetAllErrors")]
        public dynamic GetAllErrors()
        {
            dynamic reports = (from r in context.Reports
                               join a in context.Assignments on r.AssignmentId equals a.Id
                               join s in context.Stations on a.StationId equals s.Id
                               join c in context.Configurations on a.ConfigurationId equals c.Id
                               where r.Status == false
                               select new { id = r.Id, station = s.Alias, config = c.Alias, time = r.Date, success = r.Status, message = r.Message }
                           );

            return reports;
        }


        [HttpPost("CreateOne")]
        public JsonResult CreateOne(ReportInput newReport)
        {
            try
            {
                Assignments myAssignment = context.Assignments.Where(x => x.StationId == newReport.StationID && x.ConfigurationId == newReport.ConfigID).First();
                if (myAssignment == null)
                {
                    return new JsonResult("ID not found") { StatusCode = StatusCodes.Status400BadRequest };
                }
                context.Reports.Add(new Reports()
                {
                    Status = newReport.Status,
                    AssignmentId = myAssignment.Id,
                    Date = newReport.Date,
                    Message = newReport.Message
                });

                context.SaveChanges();
                return new JsonResult("Succes") { StatusCode = StatusCodes.Status200OK };
            }
            catch 
            {   
                return new JsonResult("Failure") { StatusCode = StatusCodes.Status400BadRequest };
            }                  
        }
    }
}
