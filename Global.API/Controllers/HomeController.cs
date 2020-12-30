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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using AllowAnonymousAttribute = Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Environment = Gyan.Web.Identity.Data.Authentication.Environment;
using Global.Util;
using Global.API.Models;
using Microsoft.Extensions.Options;
using Global.DAO.Service;
using Global.DAO.Model;

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
        private readonly IEmailService _emailService;

        public HomeController(ILogger<HomeController> logger,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration config,
            IEmailService emailService
            )
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _config = config;
            _emailService = emailService;
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

        //Retorna tela de Login

        [AllowAnonymous]
        [HttpGet("Login")]
        public IActionResult Login()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("UserProfile", "Home");
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser();
                user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    // user is valid do whatever you want

                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

                    if (result.Succeeded)
                    {
                        // adicionar token 
                        var roles = await _userManager.GetRolesAsync(user);
                        var token = TokenService.GenerateToken(user, roles.ToList());

                        HttpContext.Response.Cookies
                            .Append("access_token", token, TokenService.GenerateCookies(_config.GetProperty<Environment>("APIConfig", "Environment")));

                        return RedirectToAction("UserProfile", "Home");
                    }
                                        
                }

                ViewBag.IsSuccess = false;
                return View(model);
            }
                        
            return View(model);

        }


        //Exemplo de perfil do usuário logado

        [Authorize]
        [HttpGet("UserProfile")]
        public IActionResult UserProfile()
        {

            return View();

        }


        //Fazer logout

        [Authorize]
        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            foreach (var cookie in HttpContext.Request.Cookies)
            {
                Response.Cookies.Delete(cookie.Key);
            }
            return RedirectToAction("Login", "Home");
        }


        //Redefinir a senha

        [HttpGet("ForgotPassword")]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                var user = await _userManager.FindByEmailAsync(model.Email);
                
                if (user != null && await _userManager.IsEmailConfirmedAsync(user))
                {
                    await _userManager.RemoveAuthenticationTokenAsync(user, TokenOptions.DefaultProvider, "ResetPassword");

                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                                        
                    var passwordResetLink = Url.Action("ResetPassword", "Home",
                            new { email = model.Email, token = token }, Request.Scheme);

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

                    _logger.Log(LogLevel.Warning, passwordResetLink);
                                        
                    return View("ForgotPasswordConfirmation");
                }
                                
                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        [HttpGet("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                return NotFound();
            }
            
            var user = await _userManager.FindByEmailAsync(email);

            bool result = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "ResetPassword", token);
            
            if (result)
            {
                return View();
            }

            return NotFound();
                       
        }

        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.RemoveAuthenticationTokenAsync(user, TokenOptions.DefaultProvider, "ResetPassword");
                        return View("ResetPasswordConfirmation");

                    }
                    
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
                                
                return View("ResetPasswordConfirmation");
            }
            
            return View(model);
        }


        //Mudar senha

        [Authorize]
        [Route("ChangePassword")]
        public IActionResult ChangePassword()
        {            
            return View();
        }
                
        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                
                if (result.Succeeded)
                {
                    ViewBag.IsSuccess = true;
                    ModelState.Clear();
                    return View();
                }
                
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                
            }
            return View(model);
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
                HttpContext.Response.Cookies.Append(cookie.Key, null, TokenService.GenerateCookies(_config.GetSection("ApiConfig").GetValue<Environment>("Environment")));
                //HttpContext.Response.Cookies.Delete(cookie.Key);
            }
            return Json("Logged Out");
        }
        
    }
}
