using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Utilities;
using SiproModelCore.Models;
using Identity;
using Microsoft.AspNetCore.Identity;
using Dapper;
using Microsoft.AspNetCore.DataProtection;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace SAutorizacionTipo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var mapper = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(AutorizacionTipo)));
            SqlMapper.SetTypeMap(typeof(AutorizacionTipo), mapper);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<User, Rol>()
                .AddRoleStore<RoleStore>()
                .AddUserStore<UserPasswordStore>()
                .AddUserManager<CustomUserManager>()
                .AddDefaultTokenProviders();

            services.AddScoped<IUserClaimsPrincipalFactory<User>, ApplicationClaimsIdentityFactory>();

            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultAuthenticateScheme = "Identity.Application";
                sharedOptions.DefaultSignInScheme = "Identity.Application";
                // sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            });

            services.AddDataProtection()
                    .PersistKeysToFileSystem(new DirectoryInfo(@"/SIPRO"))
                    .SetApplicationName("SiproApp")
                    .DisableAutomaticKeyGeneration();

            services.ConfigureApplicationCookie(options => {
                options.Cookie.Name = ".AspNet.Identity.Application";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.Path = "/";
                options.Events.OnRedirectToLogin = context =>
                {
                    if (context.Response.StatusCode == (int)HttpStatusCode.OK)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                    else
                    {
                        context.Response.Redirect(context.RedirectUri);
                    }
                    return Task.CompletedTask;
                };

                options.Events.OnRedirectToAccessDenied = context =>
                {
                    if (context.Response.StatusCode == (int)HttpStatusCode.OK)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                    else
                    {
                        context.Response.Redirect(context.RedirectUri);
                    }
                    return Task.CompletedTask;
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Autorización Tipo - Visualizar",
                                  policy => policy.RequireClaim("sipro/permission", "Autorización Tipo - Visualizar"));
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                                 .AllowAnyHeader()
                                 .AllowCredentials()
                                 .AllowAnyMethod();
                      });
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseCors("AllowAllHeaders");
            app.UseMvc();            
        }
    }
}
