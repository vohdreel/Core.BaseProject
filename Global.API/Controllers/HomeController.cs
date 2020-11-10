using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Gyan.Web.Identity.Data.Authentication;

namespace Global.API.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private SignInManager<IdentityUser> _signInManager;

        public HomeController(ILogger<HomeController> logger,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
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
            user.UserName = "default";
            user.Email = "default@default.com";

            string userPWD = "Somepassword19+96+96";

            IdentityResult chkUser = await _userManager.CreateAsync(user, userPWD);

            //Add default User to Role Admin    
            if (chkUser.Succeeded)
            {
                await CreateRoles();
            }

            // creating Creating Manager role     
            var x = await _roleManager.RoleExistsAsync("Ozajin");
            if (!x)
            {
                var role = new IdentityRole();
                role.Name = "Ozajin";
                await _roleManager.CreateAsync(role);
            }

        }

        [HttpGet("CreateRoles")]
        public async Task CreateRoles()
        {

            // creating Creating Manager role     
            var x = await _roleManager.RoleExistsAsync("Ozajin");
            if (!x)
            {
                var role = new IdentityRole();
                role.Name = "Ozajin";
                await _roleManager.CreateAsync(role);
            }

            // creating Creating Manager role     
            x = await _roleManager.RoleExistsAsync("Manager");
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
                // user is valid do whatever you want

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
                    HttpContext.Response.Cookies.Append("access_token", token, new CookieOptions { HttpOnly = true });

                    return Json("Logged");
                }

            }
            return Json("Failed!");
        }



        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
