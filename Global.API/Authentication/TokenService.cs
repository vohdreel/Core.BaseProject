using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using JWT.Serializers;
using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using Newtonsoft.Json.Linq;

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
                    //new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                    //new Claim("CustomClaim", "Random Value")
                }),
                Expires = DateTime.UtcNow.AddMinutes(400),
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
    public class JwtToken
    {
        public long exp { get; set; }
    }

    public class JWTService
    {
        private IJsonSerializer _serializer = new JsonNetSerializer();
        private IDateTimeProvider _provider = new UtcDateTimeProvider();
        private IBase64UrlEncoder _urlEncoder = new JwtBase64UrlEncoder();
        private IJwtAlgorithm _algorithm = new HMACSHA256Algorithm();

        public DateTime GetExpiryTimestamp(string accessToken)
        {
            try
            {
                IJwtValidator _validator = new JwtValidator(_serializer, _provider);
                IJwtDecoder decoder = new JwtDecoder(_serializer, _validator, _urlEncoder, _algorithm);
                var token = decoder.DecodeToObject<JwtToken>(accessToken);
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(token.exp);
                return dateTimeOffset.LocalDateTime;
            }
            catch (TokenExpiredException)
            {
                return DateTime.MinValue;
            }
            catch (SignatureVerificationException)
            {
                return DateTime.MinValue;
            }
            catch (Exception ex)
            {
                // ... remember to handle the generic exception ...
                return DateTime.MinValue;
            }
        }
    }


}