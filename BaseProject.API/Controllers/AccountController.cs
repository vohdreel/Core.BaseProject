using BaseProject.API.Models;
using BaseProject.DAO.Model;
using BaseProject.DAO.Service;
using BaseProject.Util;
using Gyan.Web.Identity.Data.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Environment = Gyan.Web.Identity.Data.Authentication.Environment;

namespace BaseProject.API.Controllers
{
    //[Authorize]
    [Route("[controller]")]
    public class AccountController : Controller
    {

        private readonly ILogger<AccountController> _logger;
        private UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;

        public AccountController(ILogger<AccountController> logger,
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

        //Cadastrar conta

        [AllowAnonymous]
        [HttpGet("SignUp")]
        public IActionResult SignUp()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {

            if (ModelState.IsValid)
            {
                var usuarioexiste = await _userManager.FindByNameAsync(model.UserName);
                var emailexiste = await _userManager.FindByEmailAsync(model.Email);

                if (usuarioexiste != null || emailexiste != null)
                {
                    ModelState.Clear();
                    if (emailexiste != null)
                    {
                        ModelState.AddModelError("", "Esse email já está vinculado a uma conta!");
                    }
                    if (usuarioexiste != null)
                    {
                        ModelState.AddModelError("", "Esse nome de usuário já existe!");
                    }
                    return View(model);
                }

                var user = new IdentityUser();
                user.UserName = model.UserName;
                user.Email = model.Email;

                string userPWD = model.Password;

                IdentityResult chkUser = await _userManager.CreateAsync(user, userPWD);

                if (!chkUser.Succeeded)
                {

                    foreach (var errorMessage in chkUser.Errors)
                    {
                        ModelState.AddModelError("", errorMessage.Description);
                    }

                    return View(model);
                }



                //Enviando email de confirmação no cadastro


                ModelState.Clear();
                EmailConfirmViewModel modelEmailConfirm = new EmailConfirmViewModel
                {
                    Email = model.Email
                };

                user = await _userManager.FindByEmailAsync(modelEmailConfirm.Email);

                if (user != null)
                {
                    var result = await SendEmailForEmailConfirmation(modelEmailConfirm, user);

                    ModelState.Clear();
                    if (result)
                    {
                        modelEmailConfirm.EmailSent = true;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Não foi possivel enviar o email de confirmação, tente mais tarde!");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Esse email não está registrado!");
                }

                return View("ConfirmEmail", modelEmailConfirm);

            }
            foreach (var cookie in HttpContext.Request.Cookies)
            {
                CookieOptions cookieOptions = TokenService.GenerateCookies(_config.GetProperty<Environment>("ApiConfig", "Environment"));
                cookieOptions.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Response.Cookies.Append(cookie.Key, "", TokenService.GenerateCookies(_config.GetProperty<Environment>("ApiConfig", "Environment")));
                //HttpContext.Response.Cookies.Delete(cookie.Key);
            }


            return View(model);
        }

        //Confirmar email

        [AllowAnonymous]
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {


            EmailConfirmViewModel model = new EmailConfirmViewModel
            {
                Email = email
            };

            if (!string.IsNullOrEmpty(token))
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                bool verifyToken = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "EmailConfirmation", token);

                if (!verifyToken)
                {
                    ModelState.Clear();
                    return Redirect("/TokenExpired");
                }

                token = token.Replace(' ', '+');

                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {

                    await _userManager.UpdateSecurityStampAsync(user);
                    model.EmailVerified = true;
                }
            }
            foreach (var cookie in HttpContext.Request.Cookies)
            {
                CookieOptions cookieOptions = TokenService.GenerateCookies(_config.GetProperty<Environment>("ApiConfig", "Environment"));
                cookieOptions.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Response.Cookies.Append(cookie.Key, "", TokenService.GenerateCookies(_config.GetProperty<Environment>("ApiConfig", "Environment")));
                //HttpContext.Response.Cookies.Delete(cookie.Key);
            }


            return View(model);
        }

        public async Task<bool> SendEmailForEmailConfirmation(EmailConfirmViewModel model, IdentityUser user)
        {
            try
            {
                await _userManager.UpdateSecurityStampAsync(user);

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var emailConfirmLink = Url.Action("ConfirmEmail", "Account",
                            new { email = model.Email, token = token }, Request.Scheme);

                UserEmailOptions options = new UserEmailOptions
                {
                    ToEmails = new List<string>() { user.Email },
                    PlaceHolders = new List<KeyValuePair<string, string>>()
                        {
                            new KeyValuePair<string, string>("{{UserName}}", user.UserName),
                            new KeyValuePair<string, string>("{{Link}}", emailConfirmLink)
                        }
                };

                await _emailService.SendEmailForEmailConfirmation(options);
                return true;
            }
            catch(Exception e)
            {
                return false;
            }

        }

        [AllowAnonymous]
        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmViewModel model)
        {

            var user = await _userManager.FindByEmailAsync(model.Email);


            if (user != null)
            {
                if (await _userManager.IsEmailConfirmedAsync(user))
                {
                    model.IsConfirmed = true;
                    return View(model);
                }

                if (user.EmailConfirmed)
                {
                    model.EmailVerified = true;
                    return View(model);
                }

                var result = await SendEmailForEmailConfirmation(model, user);

                ModelState.Clear();
                if (result)
                {
                    model.EmailSent = true;
                }
                else
                {
                    ModelState.AddModelError("", "Não foi possivel enviar o email de confirmação, tente mais tarde!");
                }
            }
            else
            {
                ModelState.AddModelError("", "Esse email não está registrado!");
            }
            return View(model);
        }

        //Fazer Login

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
                return RedirectToAction("Index", "Home");
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
                            .Append("access_token", token, TokenService.GenerateCookies(Environment.WebDeploy));



                        return RedirectToAction("Index", "Home");
                    }

                }

                ModelState.Clear();
                ModelState.AddModelError("", "Email ou senha incorreto!");
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
            return RedirectToAction("Login", "Account");
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

                    var passwordResetLink = Url.Action("ResetPassword", "Account",
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

                    //await _emailService.SendEmailForForgotPassword(options);

                    options = _emailService.ReturnForgotPasswordBody(options);
                    var client = new SendGridClient("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");
                    var from = new EmailAddress("management.globalempregos@gmail.com", "Global Empregos");
                    var subject = options.Subject;
                    var to = new EmailAddress(user.Email);
                    //var plainTextContent = "and easy to do anywhere, even with C#";
                    var htmlContent = options.Body;
                    var msg = MailHelper.CreateSingleEmail(from, to, subject, htmlContent, htmlContent);
                    var response = await client.SendEmailAsync(msg);



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

            return Redirect("/TokenExpired");

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

                    ModelState.Clear();
                    ViewBag.IsSuccess = true;
                    return View();
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View(model);
        }


    }
}
