using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using API_Serivce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Controllers;
using WebApplication1.Models.InputModels;

namespace API_Serivce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminsController
    {

        private MyContext context = new MyContext();
        [HttpGet("GetAll")]
        public dynamic GetAll()
        {
            return this.context.Admins;
        }
        [HttpGet("GetByStationId/{id}")]
        public dynamic GetOne(int id)
        {
            Admins ad = new Admins();

            try
            {
                ad = this.context.Admins.ToList().FirstOrDefault(x => x.Id == id);
            }
            catch
            {
                throw new Exception("Invalid station");
            }
            try
            {
                if (ad == null)
                    throw new Exception("Invalid station");
            }
            catch
            {

            }

            return ad;
        }
        [HttpGet("GetById/{id}")]
        public dynamic GetById(int id)
        {
            Admins ad = new Admins();

            try
            {
                ad = this.context.Admins.ToList().FirstOrDefault(x => x.Id == id);

                if (ad == null)
                    throw new Exception("Invalid station");
            }
            catch
            {
                throw new Exception("Invalid station");

                if (ad == null)
                    throw new Exception("Invalid station");
            }

            return ad;
        }

        [HttpPost("CreateOne")]
        public JsonResult CreateOne(Admins newAdmine)
        {
            try
            {
                newAdmine.Password = BCrypt.Net.BCrypt.HashPassword(newAdmine.Password);

                this.context.Add(newAdmine);
                this.context.SaveChanges();
                return new JsonResult("Success") { StatusCode = StatusCodes.Status200OK};
            }
            catch
            {
                return new JsonResult("Failure") { StatusCode = StatusCodes.Status500InternalServerError };
            }

        }
        [HttpPut("UpdateOne/{id}")]
        public JsonResult UpdateOne(AdminInput updatedAdmin)
       {
        

            try
            {
                Admins ad = this.context.Admins.Find(updatedAdmin.Id);
                if (ad == null)
                    return new JsonResult("ID not found") { StatusCode = StatusCodes.Status400BadRequest };
                ad.Frequency = updatedAdmin.Frequency;
                ad.Email = updatedAdmin.Email;
                this.context.SaveChanges();
                return new JsonResult(updatedAdmin) { StatusCode = StatusCodes.Status200OK };
            }
            catch 
            {

                return new JsonResult("Failure") { StatusCode = StatusCodes.Status500InternalServerError };
            }         
        }
        [HttpDelete("DeleteOne/{id}")]
        public JsonResult DeleteOne(int id)
        {
            try
            {
                Admins ad = this.context.Admins.Find(id);
                if (ad == null)
                    return new JsonResult("ID not found") { StatusCode = StatusCodes.Status400BadRequest};
                this.context.Admins.Remove(ad);
                this.context.SaveChanges();
                return new JsonResult("Success") { StatusCode = StatusCodes.Status200OK};
            }
            catch
            {
                return new JsonResult("Failure") { StatusCode = StatusCodes.Status400BadRequest };  
            }

        }
    }
}