using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LaRosalinaAPI.Utils 
{ 

    public static class Tools
    {
        public static int GetUserByToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var claims = jwtToken.Claims;

            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid");

            return Convert.ToInt32(userId.Value);
        }

        private static Random randomS = new Random();

        public static string RandomString(int length)
        {           

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            //const string chars = "123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[randomS.Next(s.Length)]).ToArray());
        }
    }

}
