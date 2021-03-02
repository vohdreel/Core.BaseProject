using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Gyan.Web.Identity.Data.Authentication;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using AllowAnonymousAttribute = Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using Microsoft.Extensions.Configuration;
using Environment = Gyan.Web.Identity.Data.Authentication.Environment;
using Global.Util;
using Global.DAO.Model;
using Global.DAO.Service;
using Newtonsoft.Json.Linq;
using Global.Util.SystemEnumerations;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Global.API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _config;
        private IHttpContextAccessor _httpContextAccessor;


        public HomeController(ILogger<HomeController> logger,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration config,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _config = config;


        }
        [HttpGet]
        [Route("/")]
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpGet("UserAndRoles")]
        public async Task UserAndRoles()
        {
            var user = new IdentityUser();
            user.UserName = "default_global";
            user.Email = "default@global.com";

            string userPWD = "globalSomepassword20+20";

            IdentityResult chkUser = await _userManager.CreateAsync(user, userPWD);

            //Add default User to Role Admin    
            if (chkUser.Succeeded)
            {
                await CreateRoles();
            }

            // creating Creating Manager role     
            var x = await _roleManager.RoleExistsAsync("Manager");
            if (!x)
            {
                await _userManager.AddToRoleAsync(user, "Manager");
            }

        }

        [AllowAnonymous]
        [HttpGet("CreateRoles")]
        public async Task CreateRoles()
        {
            // creating Creating Manager role     
            var x = await _roleManager.RoleExistsAsync("Manager");
            if (!x)
            {
                var role = new IdentityRole();
                role.Name = "Manager";
                await _roleManager.CreateAsync(role);
            }

            // creating Creating Employee role     
            x = await _roleManager.RoleExistsAsync("Employee");
            if (!x)
            {
                var role = new IdentityRole();
                role.Name = "Employee";
                await _roleManager.CreateAsync(role);
            }

            x = await _roleManager.RoleExistsAsync("User");
            if (!x)
            {
                var role = new IdentityRole();
                role.Name = "User";
                await _roleManager.CreateAsync(role);
            }
        }

        [AllowAnonymous]
        [HttpGet("location")]
        public dynamic InnerRequest()
        {
            object parameters = new
            {
                address = "Rua+Manuel+Onha,+459+-+03192-100",
                key = "AIzaSyBDZN9proIwpDe18stl_EzVQjnxYTbdQLY"

            };
            var teste = HttpHelper
                .Get<JObject>(
                "https://maps.googleapis.com/maps/api/geocode/json",
                "json",
                parameters);


            return 0;

        }

        [AllowAnonymous]
        [HttpGet("FeedTest")]
        public dynamic FeedTest()
        {
            var feedArray = HttpHelper
                .Get<JObject>(
                "https://jobs.i-hunter.com/",
                "globalempregos/feed/indeed",
                isXML: true);


            JArray jobs = feedArray["source"]["jobs"]["job"] as JArray;

            foreach (JObject job in jobs)
            {
                ///primeiro verificar se a empresa existe
                Vaga vaga = new Vaga();
                Empresa empresa = new EmpresaService().BuscarPorNomeFantasia(job["company"].ToString());
                if (empresa == null)
                {
                    empresa = new Empresa()
                    {
                        NomeFantasia = job["company"]["#cdata-section"].ToString(),
                        EmailContato = job["email"]["#cdata-section"].ToString(),
                    };
                };

                string vagaText = job["title"]["#cdata-section"].ToString();

                Regex regex = new Regex(@".*?-\s+(?<cargo>.*?)\s+-");
                Match match = regex.Match(vagaText);
                if (match.Success)
                {
                    string nomeVaga = match.Groups["cargo"].Value;
                    CargoService cargoService = new CargoService();

                }






                ProcessoSeletivo processoSeletivo = new ProcessoSeletivo()
                {
                    DataInicioProcesso = Convert.ToDateTime(job["date"]["#cdata-section"].ToString()),
                    DataTerminoProcesso = Convert.ToDateTime(job["expiration_date"]["#cdata-section"].ToString()),
                    StatusProcesso = (int)StatusProcesso.EmAndamento,
                    NomeProcesso = job["title"]["#cdata-section"].ToString(),
                    IdEmpresaNavigation = empresa,
                    Vaga = new List<Vaga>() {
                        new Vaga()
                        {
                            Cidade = job["city"]["#cdata-section"].ToString(),
                            Estado = job["state"]["#cdata-section"].ToString(),
                            ReferenceNumber =job["referencenumber"]["#cdata-section"].Value<int>(),
                            Requisitos= job["description"]["#cdata-section"].ToString(),
                            UrlVaga = job["url"]["#cdata-section"].ToString()
                        }
                    }
                };                //regex para buscar modalidade se tiver

                //regex para buscar salario se tiver



            }




            return feedArray;


            //ver

        }


        [AllowAnonymous]
        [HttpGet("appLogin")]
        public async Task<JsonResult> appLogin(string email, string password)
        {
            IdentityUser user = new IdentityUser();
            user = await _userManager.FindByNameAsync(email);
            if (user != null && await _userManager.CheckPasswordAsync(user, password))
            {
                // user is valid do whatever you want

                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

                if (result.Succeeded)
                {
                    // adicionar token 
                    var roles = await _userManager.GetRolesAsync(user);
                    var token = TokenService.GenerateToken(user, roles.ToList());

                    HttpContext.Response.Cookies
                        .Append("access_token", token, TokenService.GenerateCookies(_config.GetProperty<Environment>("APIConfig", "Environment")));

                    return Json("Logged");
                }

            }
            return Json("Failed!");

        }
        [AllowAnonymous]
        [HttpGet("token")]
        public async Task<JsonResult> GenerateToken()
        {
            IdentityUser user = new IdentityUser();
            user = await _userManager.FindByNameAsync("default");

            if (user != null && await _userManager.CheckPasswordAsync(user, "Somepassword19+96+96"))
            {
                var result = await _signInManager.PasswordSignInAsync(user, "Somepassword19+96+96", false, false);

                if (result.Succeeded)
                {
                    try
                    {
                        var result1 = await _userManager.AddToRoleAsync(user, "Employee");
                        result1 = await _userManager.AddToRoleAsync(user, "Manager");
                    }
                    catch (Exception e)
                    {

                    }
                    // adicionar token 
                    var roles = await _userManager.GetRolesAsync(user);
                    var token = TokenService.GenerateToken(user, roles.ToList());
                    //O User estará vazio até que um JWT gerado pelo sistema seja encontrado na requisição
                    HttpContext
                        .Response
                        .Cookies
                        .Append("access_token",
                                token,
                               TokenService.GenerateCookies(_config.GetSection("ApiConfig").GetValue<Environment>("Environment")));

                    return Json("Logged");
                }

            }
            return Json("Failed!");
        }




        [AllowAnonymous]
        [HttpGet("FakeUserLogin")]
        public async Task<JsonResult> FakeUserLogin()
        {
            IdentityUser user = new IdentityUser();
            try
            {
                user = await _userManager.FindByNameAsync("default");
            }
            catch (Exception e)
            {


            }
            if (user != null && await _userManager.CheckPasswordAsync(user, "Somepassword19+96+96"))
            {
                var result = await _signInManager.PasswordSignInAsync(user, "Somepassword19+96+96", false, false);

                if (result.Succeeded)
                {
                    try
                    {
                        var result1 = await _userManager.AddToRoleAsync(user, "Employee");
                        result1 = await _userManager.AddToRoleAsync(user, "Manager");
                    }
                    catch (Exception e)
                    {

                    }
                    // adicionar token 
                    var roles = await _userManager.GetRolesAsync(user);
                    var token = TokenService.GenerateToken(user, roles.ToList());
                    //O User estará vazio até que um JWT gerado pelo sistema seja encontrado na requisição
                    HttpContext.Session.SetString("JWToken", token);

                    //HttpContext.Response.Cookies
                    //   .Append("access_token", token, TokenService.GenerateCookies(_config.GetProperty<Environment>("ApiConfig", "Environment"), HttpContext.Request.Headers["User-Agent"].ToString()));


                    return Json("Logged");
                }

            }
            return Json("Failed!");
        }

        [AllowAnonymous]
        [HttpGet("FakeUserLogout")]
        public async Task<JsonResult> FakeUserLogout()
        {
            foreach (var cookie in HttpContext.Request.Cookies)
            {
                CookieOptions cookieOptions = TokenService.GenerateCookies(_config.GetSection("ApiConfig").GetValue<Environment>("Environment"));
                cookieOptions.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Response.Cookies.Append(cookie.Key, "", TokenService.GenerateCookies(_config.GetSection("ApiConfig").GetValue<Environment>("Environment")));
                //HttpContext.Response.Cookies.Delete(cookie.Key);
            }

            HttpContext.Session.Clear();
            return Json("Logged Out");
        }

    }
}
