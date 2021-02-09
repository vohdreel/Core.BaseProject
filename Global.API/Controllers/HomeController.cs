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
            _logger.LogInformation("Comi o cu de quem está lendo");
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
