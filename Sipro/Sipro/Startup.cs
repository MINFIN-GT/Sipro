﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Identity;
using SiproModelCore.Models;
using System.Net;
using Sipro.Dao;
using Microsoft.AspNetCore.DataProtection;
using System.IO;
using Utilities;

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

        public void ConfigureServices(IServiceCollection services)
        {
            string[] files = Directory.GetFiles(@"C:\SIPRO");
    //        foreach (string file in files)
				//if (file.Contains("key-"))
    //                File.Delete(file);

			services.AddDataProtection()
                    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\SIPRO"))
                    .SetApplicationName("SiproApp");
			
			services.AddIdentity<User, Rol>()
                .AddRoleStore<RoleStore>()
                .AddUserStore<UserPasswordStore>()
                .AddDefaultTokenProviders()
                .AddUserManager<CustomUserManager>();

            services.ConfigureApplicationCookie(options =>
            {
				options.LoginPath = "/login";
                options.LogoutPath = "/api/Login/Out";
                options.AccessDeniedPath = "/accesodenegado";
				options.Cookie.HttpOnly = true;
                options.Cookie.Name = ".AspNet.Sipro";
				//options.Cookie.Domain = "localhost";
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
            
			app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });




        }
    }
}
