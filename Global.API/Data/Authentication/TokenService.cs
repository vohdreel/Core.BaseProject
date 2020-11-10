using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Gyan.Web.Identity.Data.Authentication
{

    public static class Settings
    {
        public static string Secret = "fedaf7d8863b48e197b9287d492b708e";
    }

    public static class TokenService
    {
        public static string GenerateToken(IdentityUser user, List<string> CurrentRoles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            foreach (string role in CurrentRoles)
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));



            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}