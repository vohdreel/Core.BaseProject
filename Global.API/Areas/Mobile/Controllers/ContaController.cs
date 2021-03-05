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
using SendGrid;
using SendGrid.Helpers.Mail;
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
        private readonly IEmailService _emailService;


        public ContaController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration config,
            IEmailService emailService

            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _config = config;
            _emailService = emailService;

        }

        [HttpGet("Login")]
        public async Task<object> appLogin(string email, string password, bool ManterConectado)
        {
            password = System.Uri.UnescapeDataString(password);
            IdentityUser user = new IdentityUser();
            user = await _userManager.FindByEmailAsync(email);
            if(user == null)
                user = await _userManager.FindByNameAsync(email);
            if (user != null && await _userManager.CheckPasswordAsync(user, password))
            {
                // user is valid do whatever you want
                if (!await _userManager.IsEmailConfirmedAsync(user))
                    return new
                    {
                        unverified = true,
                        Ok = false,
                        Message = "Essa conta ainda não foi confirmada. Por favor verifique sua caixa de mensagens. (Em alguns casos, a mensagem pode ser marcado como spam)!"
                    };


                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

                if (result.Succeeded)
                {
                    //await _signInManager.SignInAsync(user, true);
                    // adicionar token 
                    var roles = await _userManager.GetRolesAsync(user);
                    var token = TokenService.GenerateToken(user, roles.ToList());

                    //HttpContext.Session.SetString("JWToken", token);

                    HttpContext.Response.Cookies
                        .Append("access_token", token, TokenService.GenerateCookies(_config.GetProperty<Environment>("ApiConfig", "Environment"), HttpContext.Request.Headers["User-Agent"].ToString()));

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
                    ok = false
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
                        message = "Sessão expirada"
                    };

            }

        }

        public async Task<bool> SendEmailForEmailConfirmation(string email, IdentityUser user)
        {
            try
            {
                await _userManager.UpdateSecurityStampAsync(user);

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var emailConfirmLink = Url.Action("ConfirmEmail", "Account",
                            new { email = email, token = token }, Request.Scheme);

                UserEmailOptions options = new UserEmailOptions
                {
                    ToEmails = new List<string>() { user.Email },
                    PlaceHolders = new List<KeyValuePair<string, string>>()
                        {
                            new KeyValuePair<string, string>("{{UserName}}", user.UserName),
                            new KeyValuePair<string, string>("{{Link}}", emailConfirmLink)
                        }
                };

                options = _emailService.ReturnConfirmationBody(options);
                //var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");
                //var client = new SendGridClient(apiKey);
                //var from = new EmailAddress("test@example.com", "Example User");
                //var subject = options.Subject;
                //var to = new EmailAddress(user.Email);
                //var plainTextContent = "and easy to do anywhere, even with C#";
                //var htmlContent = options.Body;
                //var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                //var response = await client.SendEmailAsync(msg);


                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }



        [HttpPost("EnviarLinkRedefinirSenha")]
        [AllowAnonymous]
        public async Task<object> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null && await _userManager.IsEmailConfirmedAsync(user))
            {
                await _userManager.RemoveAuthenticationTokenAsync(user, TokenOptions.DefaultProvider, "ResetPassword");

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var passwordResetLink = Url.Action("ResetPassword", "Account",
                        new { email = email, token = token }, Request.Scheme);

                UserEmailOptions options = new UserEmailOptions
                {
                    ToEmails = new List<string>() { user.Email },
                    PlaceHolders = new List<KeyValuePair<string, string>>()
                        {
                            new KeyValuePair<string, string>("{{UserName}}", user.UserName),
                            new KeyValuePair<string, string>("{{Link}}", passwordResetLink)
                        }
                };

                await _emailService.SendEmailForForgotPassword(options);

                //_logger.Log(LogLevel.Warning, passwordResetLink);

                return new
                {
                    ok = true,
                    message = "Enviamos pare esse endereço de email as instruções para redefinir sua senha.<br /><br />" +
                        "Por favor, verifique sua caixa de mensagens (Em alguns casos, a mensagem pode ser marcado como spam)!"

                };
            }

            return new
            {
                ok = false,
                message = "Não existe nenhuma conta registrada usando este email.<br /><br />" +
                        "Verifique se digitou o endereço de email corretamente e tente novamente!"

            };

        }

        [Authorize]
        [HttpGet("GetClaims")]
        public object GetClaims()
        {
            return new
            {
                UserId = this.User.FindFirstValue(ClaimTypes.Name),
                Email = this.User.FindFirstValue("IdAspNetUser")


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
        [HttpGet("VerificarCpf")]
        public async Task<object> VerifyCpf(string cpf)
        {
            using (var service = new CandidatoService())
            {
                return new
                {
                    ok = !service.ExisteCpfUsuario(cpf)
                };
            }
        }



        [AllowAnonymous]
        [HttpPost("CadastrarUsuario")]
        public async Task<object> SingUp([FromBody] dynamic userInfo)
        {
            var user = new IdentityUser();
            user.UserName = userInfo.Cpf;
            user.UserName = user.UserName.RemoveDiacritics();
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
                    Email = userInfo.Email,
                    TelefoneCandidato = new List<TelefoneCandidato>() {
                        new TelefoneCandidato(){
                            IdTelefoneNavigation = new Telefone(){
                                Numero = userInfo.Telefone
                            }
                        }
                    }

                };

                CandidatoService candidatoService = new CandidatoService();
                bool success = candidatoService.CadastrarCandidato(candidato);

                if (success)
                {
                    try
                    {
                        await SendEmailForEmailConfirmation(user.Email, user);
                        
                    }
                    catch (Exception e) { }
                }

                return new
                {
                    ok = success,
                    IdCandidato = candidato.Id

                };
            }
            else
                return new { ok = false };


        }

        [AllowAnonymous]
        [HttpGet("CarregarConfiguracoseInteresse")]
        public object CarregarConfiguracoseInteresse()
        {


            using (var areaService = new EnumAgrupamentoService())
            using (var cargoService = new CargoService())
            {
                EnumAgrupamento[] areas = areaService.BuscarTodos();
                Cargo[] cargos = cargoService.BuscarTodos();

                return new
                {
                    areas,
                    cargos

                };

            }
        }


        [AllowAnonymous]
        [HttpPost("SalvarPreferencias")]
        public object SalvarPreferencias([FromBody] ViewModel.Preferencias preferencias)
        {

            using (var areaService = new AreaInteresseService())
            using (var cargoService = new CargoInteresseService())
            {

                bool sucesso = true;
                foreach (var area in preferencias.areasInteresse)
                    sucesso = areaService.Salvar(area);

                foreach (var cargo in preferencias.cargosInteresse)
                    sucesso = cargoService.Salvar(cargo);

                return new
                {
                    ok = sucesso
                };


            }





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