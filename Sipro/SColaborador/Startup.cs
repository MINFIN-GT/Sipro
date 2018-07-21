using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Dapper;
using Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SiproModelCore.Models;
using Utilities;

namespace SColaborador
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var mapper = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(Colaborador)));
            SqlMapper.SetTypeMap(typeof(Colaborador), mapper);
            var mapper2 = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(UnidadEjecutora)));
            SqlMapper.SetTypeMap(typeof(UnidadEjecutora), mapper2);
            var mapper3 = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(Entidad)));
            SqlMapper.SetTypeMap(typeof(Entidad), mapper3);
            var mapper4 = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(AsignacionRaci)));
            SqlMapper.SetTypeMap(typeof(AsignacionRaci), mapper4);
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

            services.ConfigureApplicationCookie(options =>
            {
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
                options.AddPolicy("Colaboradores - Visualizar",
                                  policy => policy.RequireClaim("sipro/permission", "Colaboradores - Visualizar"));
                options.AddPolicy("Colaboradores - Crear",
                                  policy => policy.RequireClaim("sipro/permission", "Colaboradores - Crear"));
                options.AddPolicy("Colaboradores - Eliminar",
                                  policy => policy.RequireClaim("sipro/permission", "Colaboradores - Eliminar"));
                options.AddPolicy("Colaboradores - Editar",
                                  policy => policy.RequireClaim("sipro/permission", "Colaboradores - Editar"));
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
