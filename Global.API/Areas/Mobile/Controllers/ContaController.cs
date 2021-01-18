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
using FromBodyAttribute = Microsoft.AspNetCore.Mvc.FromBodyAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
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
                        .Append("access_token", token, TokenService.GenerateCookies(_config.GetProperty<Environment>("ApiConfig", "Environment")));
                    CandidatoService service = new CandidatoService();

                    if (ManterConectado)
                    {
                        service.AlternarMaterConectado(user.Id, true);

                    }
                    Candidato candidato = service.BuscarCandidato(user.Id);



                    return new
                    {

                        IdCandidato = candidato.Id,
                        Ok = true,
                        Message = "Logged in"
                    };
                }

            }
            return new { Ok = false, Message = "Not Logged in" }; ;

        }

        [HttpGet("CheckToken")]
        public async Task<object> CheckValidToken(int IdCandidato = 0)
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
                        IdentityUser user = await _userManager.FindByEmailAsync(candidato.Email);
                        var roles = await _userManager.GetRolesAsync(user);
                        var token = TokenService.GenerateToken(user, roles.ToList());

                        HttpContext.Response.Cookies
                            .Append("access_token", token, TokenService.GenerateCookies(_config.GetProperty<Environment>("ApiConfig", "Environment")));

                        return new
                        {
                            ok = true
                        };
                    }
                }
            }

            string jwt = HttpContext.Request.Cookies["access_token"];
            if (string.IsNullOrEmpty(jwt))
            {
                return new
                {
                    ok = false,
                    message = "Session Expired"
                };
            }
            else
            {
                JWTService helper = new JWTService();
                DateTime expiricy = helper.GetExpiryTimestamp(jwt);

                if (expiricy > DateTime.Now)
                    return new
                    {
                        ok = true
                    };
                else
                    return new
                    {
                        ok = false,
                        message = "Session Expired"
                    };

            }

        }


        [Authorize]
        [HttpGet("GetClaims")]
        public object GetClaims()
        {
            return new
            {
                UserId = this.User.FindFirstValue(ClaimTypes.Name),
                Email = this.User.FindFirstValue("IdAspNetUser"),
            };
        }


        [AllowAnonymous]
        [HttpGet("VerificarEmail")]
        public async Task<object> VerifyEmailAdress(string emailAdress)
        {
            return new
            {
                ok = await _userManager.FindByEmailAsync(emailAdress) == null
            };
        }


        [AllowAnonymous]
        [HttpPost("CadastrarUsuario")]
        public async Task<object> SingUp([FromBody] dynamic userInfo)
        {
            var user = new IdentityUser();
            user.UserName = userInfo.Nome;
            user.Email = userInfo.Email;

            string userPWD = userInfo.Password;

            IdentityResult chkUser = await _userManager.CreateAsync(user, userPWD);
            if (chkUser.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");

                Candidato candidato = new Candidato()
                {
                    IdAspNetUsers = user.Id,
                    Cpf = userInfo.Cpf,
                    Nome = userInfo.Nome,
                    Email = userInfo.Email
                };

                CandidatoService candidatoService = new CandidatoService();
                candidatoService.CadastrarCandidato(candidato);

                return new
                {
                    ok = true,
                    IdCandidato = candidato.Id

                };
            }
            else
                return new { ok = false };


        }


        [Authorize]
        [HttpGet("Logout")]
        public async Task<bool> Logout()
        {

            var userId = this.User.FindFirstValue("IdAspNetUser");
            CandidatoService service = new CandidatoService();
            service.AlternarMaterConectado(userId, false);


            foreach (var cookie in HttpContext.Request.Cookies)
            {
                CookieOptions cookieOptions = TokenService.GenerateCookies(_config.GetProperty<Environment>("ApiConfig", "Environment"));
                cookieOptions.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Response.Cookies.Append(cookie.Key, "", TokenService.GenerateCookies(_config.GetProperty<Environment>("ApiConfig", "Environment")));
                //HttpContext.Response.Cookies.Delete(cookie.Key);
            }


            return true;
        }




    }
}