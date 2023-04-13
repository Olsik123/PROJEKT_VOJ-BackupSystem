using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private AuthenticationService auth = new AuthenticationService();

        [HttpPost]
        public JsonResult Login(Credentials credentials)
        {
            try
            {
                return new JsonResult(this.auth.Authenticate(credentials));
            }
            catch (Exception e)
            {
                return new JsonResult("Invalid username or password") { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
        [HttpPost("GetId")]
        public JsonResult GetId(Credentials credentials)
        {
            try
            {
                return new JsonResult(this.auth.GetId(credentials));
            }
            catch (Exception e)
            {
                return new JsonResult("Invalid username or password") { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
