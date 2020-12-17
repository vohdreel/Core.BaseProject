using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Global.DAO.Model;
using Global.DAO.Service;
using Global.Util;
using Gyan.Web.Identity.Data.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Environment = Gyan.Web.Identity.Data.Authentication.Environment;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Global.API.Areas.Mobile.Controllers
{
    [Area("Mobile")]
    [Route("[area]/[controller]")]
    public class ContaController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _config;

        public ContaController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration config
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _config = config;
        }

        [HttpGet("Login")]
        public async Task<object> appLogin(string email, string password, bool ManterConectado)
        {
            password = System.Uri.UnescapeDataString(password);
            IdentityUser user = new IdentityUser();
            user = await _userManager.FindByEmailAsync(email);
            if (user != null && await _userManager.CheckPasswordAsync(user, password))
            {
                // user is valid do whatever you want

                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

                if (result.Succeeded)
                {
                    //await _signInManager.SignInAsync(user, true);
                    // adicionar token 
                    var roles = await _userManager.GetRolesAsync(user);
                    var token = TokenService.GenerateToken(user, roles.ToList());

                    HttpContext.Response.Cookies
                        .Append("access_token", token, TokenService.GenerateCookies(_config.GetProperty<Environment>("APIConfig","Environment")));
                    CandidatoService service = new CandidatoService();

                    if (ManterConectado)
                    {
                        service.AlternarMaterConectado(user.Id, true);

                    }
                    Candidato candidato = service.BuscarCandidato(user.Id);



                    return new {

                        IdCandidato = candidato.Id,
                        Ok = true, 
                        Message = "Logged in" 
                    };
                }

            }
            return new { Ok = false, Message = "Not Logged in" }; ;

        }

        [HttpGet("CheckToken")]
        public async Task<bool> CheckValidToken(int IdCandidato = 0)
        {

            //se o usuario tiver o manterconectado ativo, renova o token
            if (IdCandidato != 0)
            {
                CandidatoService service = new CandidatoService();
                Candidato candidato = service.BuscarCandidato(IdCandidato);
                if (candidato != null)
                {
                    bool manterConectado = new CandidatoService().VerificarManterConectado(IdCandidato);
                    if (manterConectado)
                    {
                        //await _signInManager.SignInAsync(user, true);

                        IdentityUser user = await _userManager.FindByEmailAsync(candidato.Email);
                        var roles = await _userManager.GetRolesAsync(user);
                        var token = TokenService.GenerateToken(user, roles.ToList());

                        HttpContext.Response.Cookies
                            .Append("access_token", token, TokenService.GenerateCookies(_config.GetProperty<Environment>("Environment")));

                        return true;
                    }
                }
            }

            string jwt = HttpContext.Request.Cookies["access_token"];
            if (string.IsNullOrEmpty(jwt))
                return false;
            else
            {
                JWTService helper = new JWTService();
                DateTime expiricy = helper.GetExpiryTimestamp(jwt);

                if (expiricy > DateTime.Now)
                    return true;
                else
                    return false;

            }

        }
        [Authorize]
        [HttpGet("Logout")]
        public async Task<bool> FakeUserLogout()
        {

            foreach (var cookie in HttpContext.Request.Cookies)
            {
                CookieOptions cookieOptions = TokenService.GenerateCookies(_config.GetProperty<Environment>("APIConfig", "Environment"));
                cookieOptions.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Response.Cookies.Append(cookie.Key, "", TokenService.GenerateCookies(_config.GetProperty<Environment>("APIConfig", "Environment")));
                //HttpContext.Response.Cookies.Delete(cookie.Key);
            }
            //await _signInManager.SignOutAsync();

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            CandidatoService service = new CandidatoService();
            service.AlternarMaterConectado(userId, false);






            return true;
        }




    }
}