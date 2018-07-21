using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Dapper;
using Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SiproModelCore.Models;
using Utilities;

namespace SLogin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
			Type[] types = { typeof(Usuario), typeof(Permiso), typeof(Rol)};
            foreach (Type type in types)
            {
                var mapper = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                                .MakeGenericType(type));
                SqlMapper.SetTypeMap(type, mapper);
            }
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
                    .SetApplicationName("SiproApp");

			services.ConfigureApplicationCookie(options => {
				options.Cookie.Name = ".AspNet.Identity.Application";
				options.Cookie.HttpOnly = true;
				options.Cookie.Expiration = TimeSpan.FromMinutes(6000);
				options.SlidingExpiration = true;
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
                    return Task.CompletedTask;
                };
			});

            /*services.ConfigureApplicationCookie(options => {
                options.Cookie.Name = ".AspNet.Sipro";
                options.Cookie.HttpOnly = true;
				options.Cookie.Path = "/";
				options.Cookie.SameSite = SameSiteMode.None;
				options.Events.OnRedirectToAccessDenied = context =>
                {
                    if (context.Response.StatusCode == (int)HttpStatusCode.OK)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                    return Task.CompletedTask;
                };
            });*/

			services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                                 .AllowAnyHeader()
                                 .AllowAnyMethod()
					             .AllowCredentials();
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
