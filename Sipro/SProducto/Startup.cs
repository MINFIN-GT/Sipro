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

namespace SProducto
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var mapper = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(Producto)));
            SqlMapper.SetTypeMap(typeof(Producto), mapper);
            var mapper2 = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(AcumulacionCosto)));
            SqlMapper.SetTypeMap(typeof(AcumulacionCosto), mapper2);
            var mapper3 = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(ProductoTipo)));
            SqlMapper.SetTypeMap(typeof(ProductoTipo), mapper3);
            var mapper4 = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(Componente)));
            SqlMapper.SetTypeMap(typeof(Componente), mapper4);
            var mapper5 = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(Subcomponente)));
            SqlMapper.SetTypeMap(typeof(Subcomponente), mapper5);
            var mapper6 = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(UnidadEjecutora)));
            SqlMapper.SetTypeMap(typeof(UnidadEjecutora), mapper6);
            var mapper7 = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(Entidad)));
            SqlMapper.SetTypeMap(typeof(Entidad), mapper7);
            var mapper8 = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(PagoPlanificado)));
            SqlMapper.SetTypeMap(typeof(PagoPlanificado), mapper8);
            var mapper9 = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(ProductoPropiedad)));
            SqlMapper.SetTypeMap(typeof(ProductoPropiedad), mapper9);
            var mapper10 = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(ProductoPropiedadValor)));
            SqlMapper.SetTypeMap(typeof(ProductoPropiedadValor), mapper10);
            var mapper11 = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(Proyecto)));
            SqlMapper.SetTypeMap(typeof(Proyecto), mapper11);
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
                options.AddPolicy("Productos - Visualizar",
                                  policy => policy.RequireClaim("sipro/permission", "Productos - Visualizar"));
                options.AddPolicy("Productos - Editar",
                                  policy => policy.RequireClaim("sipro/permission", "Productos - Editar"));
                options.AddPolicy("Productos - Eliminar",
                                  policy => policy.RequireClaim("sipro/permission", "Productos - Eliminar"));
                options.AddPolicy("Productos - Crear",
                                  policy => policy.RequireClaim("sipro/permission", "Productos - Crear"));
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
