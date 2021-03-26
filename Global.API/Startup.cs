using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
//using System.Web.Http;
using System.Net.Http;
using System.Net;
using Global.API.Data;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.Controllers;
using Gyan.Web.Identity.Data.Authentication;
using Global.Util;
using Global.DAO.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Global.DAO.Model;
using Global.DAO.Service;
using Global.API.Authentication;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Features;

namespace Global.API {
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (Configuration.GetProperty<bool>("ApiConfig", "useEntityCore"))
            {
                //seviços relacionados ao ASP .NET Identity

                services.AddDbContext<GlobalContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

                services.AddIdentity<IdentityUser, IdentityRole>()
                      .AddEntityFrameworkStores<GlobalContext>()
                      .AddDefaultTokenProviders();
            }

            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = long.MaxValue; // <-- ! long.MaxValue
                options.MultipartBoundaryLengthLimit = int.MaxValue;
                options.MultipartHeadersCountLimit = int.MaxValue;
                options.MultipartHeadersLengthLimit = int.MaxValue;
            });

            // Set token life span to 5 hours
            services.Configure<DataProtectionTokenProviderOptions>(o =>
                o.TokenLifespan = TimeSpan.FromMinutes(5));

            //Configuração do SMTP
            services.Configure<SMTPConfigModel>(Configuration.GetSection("SMTPConfig"));
            services.AddScoped<IEmailService, EmailService>();

            //Adicionando serviço de Cors e serviço de Controllers e Views(RazorPages)
            //Configuração do CORS (configurar para manter o httpResponse em plataformas diferentes [configurei para usar cookies no app Ionic])
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins("http://localhost:8100", "https://globalempregosapi.azurewebsites.net/", "https://globalempregosapi-dev.azurewebsites.net/")
                    .SetIsOriginAllowed(origin => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            if (Configuration.GetProperty<bool>("ApiConfig", "useMVC"))
            {
                services.AddMvc(options => options.EnableEndpointRouting = false);
                services.AddRazorPages();
                services.AddControllersWithViews()
                    .AddRazorRuntimeCompilation()
                    .AddNewtonsoftJson(options =>
                   options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
            }

            //Chave JWT (talvez colocar no ConfigurationManager??)            
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            //Configuração JWT
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {                    
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    
                };
                x.Events = new JwtBearerEvents

                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["access_token"];

                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context => 
                    {
                        var response = context.HttpContext.Response;
                        return Task.CompletedTask;
                    }
                };
            });


            
            //Configuração do Cookie Authentication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                      .AddCookie(options =>
                      {
                          options.ExpireTimeSpan = TimeSpan.FromDays(30);
                          options.SlidingExpiration = true;
                          options.ExpireTimeSpan = TimeSpan.FromDays(30);
                          options.LoginPath = "/Account/Login";
                          options.LogoutPath = "/Account/Logout";
                          options.AccessDeniedPath = "/Account/AccessDenied";

                          //Configuração geral dos cookies para validar no Azure Dev Ops
                          options.Cookie.SameSite = SameSiteMode.None;

                          if (Configuration.GetProperty<bool>("ApiConfig", "useEntityCore"))
                          {
                              //Trata todos od eventos relacionados ao Cookies Authetication
                              options.Events = new CookieAuthenticationEvents()
                              {
                                  OnRedirectToLogin = redirectContext =>
                                  {
                                      return Task.CompletedTask;

                                  },
                                  OnRedirectToAccessDenied = redirectContext =>
                                  {
                                      return Task.CompletedTask;

                                  },


                              };
                          }
                      });




            //serviço para usar os atributos de autorização
            services.AddAuthorization(options =>
            {
                //usando JWT Bearer e o esquema de Cookie Authentication juntos
                //havendo a necessidade de adicionar uma nova forma de autenticação, adicionar no metodo AutheticationSchemes
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes("Bearer", "Cookies")
                    .Build();
            });

            //Password Strength Setting  
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings  
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 5;

                // Lockout settings  
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings  
                options.User.RequireUniqueEmail = true;

                
            });

            //Configuração SWAGGER UI
            services.AddSwaggerGen(c =>
            {

                c.EnableAnnotations();
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "ASP ",
                        Version = "v1",
                        Description = "Exemplo de API REST criada com o ASP.NET Core 3.0 para aplicações web e mobile",
                        Contact = new OpenApiContact
                        {
                            Name = "Pico Araújo",
                            Url = new Uri("https://github.com/vohdreel")
                        }
                    });
                c.TagActionsBy(api =>
                {
                    if (api.GroupName != null)
                    {
                        return new[] { api.GroupName };
                    }
                     
                    var controllerActionDescriptor = api.ActionDescriptor as ControllerActionDescriptor;
                    if (controllerActionDescriptor != null)
                    {
                        return new[] { controllerActionDescriptor.ControllerName };
                    }

                    throw new InvalidOperationException("Unable to determine tag for endpoint.");
                });
                c.DocInclusionPredicate((name, api) => true);
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env )
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Versão Pré-Alpha");
            });

            app.UseStatusCodePages(context =>
            {
                var agent= context.HttpContext.Request.Headers["User-Agent"].ToString().ToLower();
                if (agent.Contains("android") || agent.Contains("iphone"))
                {
                    return Task.CompletedTask;

                }

                var response = context.HttpContext.Response;

                if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    response.Redirect("/Account/Login");
                }
                else
                {
                    response.Redirect($"/HttpError/{response.StatusCode}");
                }

                return Task.CompletedTask;

            });

            app.UseExceptionHandler("/error");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("CorsPolicy");
            app.UseAuthorization();
            app.UseAuthentication();

            if (Configuration.GetProperty<bool>("ApiConfig", "useMVC"))
            {
                app.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "areaRoute",
                        template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                });
            }
            else
            {
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapControllerRoute(
                       name: "areaRoute",
                       pattern: "{area:exists}/{controller}/{action}/{id?}");
                    //defaults: new { action = "Index" });
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller}/{action=swagger}/{id?}");

                });
            }
        }
    }
}