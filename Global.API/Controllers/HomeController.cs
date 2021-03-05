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
using System.Text.RegularExpressions;
using Global.Util.SystemEnumerations;
using System.Collections.Generic;
using System.Globalization;

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

        public HomeController(ILogger<HomeController> logger,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration config
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
            user.UserName = "paula_mattus";
            user.Email = "paula.mattus@globalempregos.com.br";

            string userPWD = "@Globalapp123";

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
        [HttpGet("FeedTest")]
        public dynamic FeedTest()
        {
            var feedArray = HttpHelper
                .Get<JObject>(
                "https://jobs.i-hunter.com/",
                "globalempregos/feed/indeed",
                isXML: true);


            ProcessoSeletivoService processoService = new ProcessoSeletivoService();
            VagaService vagaService = new VagaService();

            JArray jobs = feedArray["source"]["jobs"]["job"] as JArray;

            foreach (JObject job in jobs)
            {
                int referenceNumber = job["referencenumber"]["#cdata-section"].Value<int>();

                if (!vagaService.VerificarVagaPorReferenceNumber(referenceNumber))
                {
                    int enumTipoContratacao = 0;
                    decimal valueSalario = 0;
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
                    }
                    string rawVagaTitle = job["title"]["#cdata-section"].ToString();

                    //Regex regex = new Regex(@".*?-\s*(?<cargo>.*?)\s*-");
                    Regex regex = new Regex(@".*?-\s+(?<cargo>.*?(?(?=\s*-)\s*-|((?!\s-).*-|$)))");
                    Match match = regex.Match(rawVagaTitle);
                    Cargo cargoVaga = new Cargo();
                    if (match.Success)
                    {
                        string nameVaga = match.Groups["cargo"].Value.Replace("-", "").Trim();
                        CargoService cargoService = new CargoService();
                        cargoVaga = cargoService.BuscarCargoFeed(nameVaga);
                        if (cargoVaga == null)
                        {
                            cargoVaga = new Cargo()
                            {
                                NomeCargo = nameVaga,
                                IdEnumAgrupamento = new EnumAgrupamentoService().BuscarPorNome("Prestação de Serviços").Id

                            };
                            cargoService.Salvar(cargoVaga);
                        }

                    }
                    string rawVagaDescription = job["description"]["#cdata-section"].ToString().Replace("\r\n", " ");

                    regex = new Regex(@"^.*?Tipo de contratação.*?\s+(?<tipoContratacao>.*?\s+)");
                    match = regex.Match(rawVagaDescription);
                    if (match.Success)
                        enumTipoContratacao = (int)(VagaModalidade)Enum.Parse(typeof(VagaModalidade), match.Groups["tipoContratacao"].Value.RemoveDiacritics());

                    regex = new Regex(@"^.*?Salário.*?\s+(?<salario>.*?\s+)");
                    match = regex.Match(rawVagaDescription);
                    if (match.Success)
                    {
                        try
                        {
                            valueSalario = Convert.ToDecimal(match.Groups["salario"].Value.Replace(".", ""), new NumberFormatInfo() { NumberDecimalSeparator = "," });

                        }
                        catch (Exception e)
                        {
                            valueSalario = 0;
                        }


                    }
                    ProcessoSeletivo processoSeletivo = new ProcessoSeletivo()
                    {
                        DataInicioProcesso = Convert.ToDateTime(job["date"]["#cdata-section"].ToString()),
                        DataTerminoProcesso = Convert.ToDateTime(job["expiration_date"]["#cdata-section"].ToString()),
                        StatusProcesso = (int)StatusProcesso.EmAndamento,
                        NomeProcesso = job["title"]["#cdata-section"].ToString(),
                        IdEmpresaNavigation = empresa,
                        Vaga = new List<Vaga>()
                        {
                            new Vaga()
                            {
                                Cidade = job["city"]["#cdata-section"].ToString(),
                                Estado = job["state"]["#cdata-section"].ToString().ConverterEstados(),
                                ReferenceNumber =job["referencenumber"]["#cdata-section"].Value<int>(),
                                Requisitos= job["description"]["#cdata-section"].ToString(),
                                UrlVaga = job["url"]["#cdata-section"].ToString(),
                                DisponibilidadeTransferencia = (int)Disponibilidade.Negociavel,
                                DisponibilidadeViagem = (int)Disponibilidade.Negociavel,
                                Jornada = (int)DisponibilidadeHorario.Integral,
                                StatusVaga = (int)StatusVaga.Aberta,
                                Modalidade = enumTipoContratacao,
                                Salario = valueSalario,
                                IdCargo = cargoVaga.Id,


                            }
                        }
                    };

                    bool success = processoService.Salvar(processoSeletivo);
                }
            }




            return feedArray;


            //ver

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
                    //HttpContext.Session.SetString("JWToken", token);

                    HttpContext.Response.Cookies
                       .Append("access_token", token, TokenService.GenerateCookies(_config.GetProperty<Environment>("ApiConfig", "Environment"), HttpContext.Request.Headers["User-Agent"].ToString()));


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
            return Json("Logged Out");
        }

    }
}
