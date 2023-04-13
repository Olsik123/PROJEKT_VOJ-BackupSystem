using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private AuthenticationService auth = new AuthenticationService();

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string token = context.HttpContext.Request.Headers["Authorization"].ToString().Split(' ').Last();

            if (!this.auth.VerifyToken(token))
            {
                context.Result = new JsonResult("Authentication failed") { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
