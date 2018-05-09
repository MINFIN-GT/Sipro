using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sipro.Utilities;
using Sipro.Utilities.Identity;
using SiproModel.Models;
using System.Net;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Sipro.Dao;

namespace Sipro
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Type[] types = typeof(Usuario).Assembly.GetTypes();
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
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.IdleTimeout = TimeSpan.FromMinutes(10);
            });

            services.AddIdentity<User, Rol>()
                .AddRoleStore<RoleStore>()
                .AddUserStore<UserPasswordStore>()
                .AddDefaultTokenProviders()
                .AddUserManager<CustomUserManager>();

			/*services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
            });*/

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/SignIn";
                options.LogoutPath = "/Login/Out";
                options.AccessDeniedPath = "/accesodenegado";
				options.Cookie.Domain = "localhost";
                options.Cookie.HttpOnly = true;
                //options.Cookie.Name = "Sipro.Cookie";
				options.Cookie.Expiration = TimeSpan.FromMinutes(60);
                options.Events.OnRedirectToLogin = context =>
                {
                    if (context.Request.Path.StartsWithSegments("/api") &&
                        context.Response.StatusCode == (int)HttpStatusCode.OK)
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
                    if (context.Request.Path.StartsWithSegments("/api") &&
                        context.Response.StatusCode == (int)HttpStatusCode.OK)
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
			        
            /*    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                    validateInterval: TimeSpan.FromMinutes(30),
                    regenerateIdentity: (manager, user) => {
                        var identity = manager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

                        //some additional claims and stuff specific to my needs
                        return Task.FromResult(identity);
                    })
                },
                CookieDomain = ".example.com"
            });*/

            /*services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/SignIn";
                options.LogoutPath = "/Login/Out";
                options.AccessDeniedPath = "/AccesoDenegado";
                //options.Cookie.HttpOnly = true;
                //options.Cookie.Name = "Sipro.Cookie";
                //options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.Events.OnRedirectToLogin = context =>
                {
                    if (context.Request.Path.StartsWithSegments("/api") &&
                        context.Response.StatusCode == (int)HttpStatusCode.OK)
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
                    if (context.Request.Path.StartsWithSegments("/api") &&
                        context.Response.StatusCode == (int)HttpStatusCode.OK)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                    else
                    {
                        context.Response.Redirect(context.RedirectUri);
                    }
                    return Task.CompletedTask;
                };
            });*/

            services.AddScoped<IUserClaimsPrincipalFactory<User>, ApplicationClaimsIdentityFactory>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("General", policy => policy.RequireRole("General"));

                List<Permiso> permisos = PermisoDAO.getPermisos();
                foreach (Permiso permiso in permisos)
                {
                    options.AddPolicy(permiso.nombre,
                                      policy => policy.RequireClaim(CustomClaimType.Permission, permiso.nombre));
                }
            });

            services.AddMvc();
            services.AddDistributedMemoryCache();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();
            //app.UseSession();
   
			app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


        }
    }
}
