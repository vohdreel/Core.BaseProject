using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gyan.Web.Identity.Data.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.API.Controllers
{
    public class TokenController : ControllerBase
    {

        [AllowAnonymous]
        [Route("[controller]")]
        public object GenerateToken()
        {
            IdentityUser user = new IdentityUser();
            user.UserName = "googleAPI";
            user.Id = "googleAPI.ID";
            List<string> roles = new List<string>() { "GoogleAPIRole" };

            return new { token = TokenService.GenerateToken(user, roles)};
        }


    }
}
