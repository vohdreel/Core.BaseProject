using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Gyan.Web.Identity.Data.Authentication
{

    public enum Environment { Development, Production }

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
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            foreach (string role in CurrentRoles)
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static CookieOptions GenerateCookies(Environment enviorment)
        {
            CookieOptions cookie = new CookieOptions();
            cookie.HttpOnly = true;
            if (enviorment == Environment.Production)
            {
                cookie.SameSite = SameSiteMode.None;
                cookie.Secure = true;
            }
            return cookie;
        }
    }


}