using API_Serivce.Models;
using JWT.Algorithms;
using JWT.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class AuthenticationService
    {
        const string SECRET = "password";

        private MyContext context = new MyContext();

        private bool verifyPassword(string email1, string email2, string pass, string hash)
        {
            bool loginFound = email1 == email2;

            if (!loginFound)
                return false;

            return BCrypt.Net.BCrypt.Verify(pass, hash);
        }

        public string Authenticate(Credentials credentials)
        {
            Admins admin =
                this.context.Admins.ToList().Where(x => verifyPassword(credentials.Login, x.Email, credentials.Password, x.Password)).FirstOrDefault();

            if (admin == null)
                throw new Exception("Invalid admin");

            return JwtBuilder.Create()
                      .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                      .WithSecret(SECRET)
                      .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
                      .AddClaim("user_id", admin.Id)
                      .Encode();
        }
        public string GetId(Credentials credentials)
        {
            Admins admin =
                this.context.Admins.ToList().Where(x => verifyPassword(credentials.Login, x.Email, credentials.Password, x.Password)).FirstOrDefault();

            if (admin == null)
                throw new Exception("Invalid admin");

            return admin.Id.ToString();
        }

        public bool VerifyToken(string token)
        {
            try
            {
                string json = JwtBuilder.Create()
                             .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                             .WithSecret(SECRET)
                             .MustVerifySignature()
                             .Decode(token);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
