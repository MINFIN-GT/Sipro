using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Dapper;
using Utilities;
using SiproModelCore.Models;
using SiproModelAnalyticCore.Models;
using Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.DataProtection;
using System.IO;
using System.Net;

namespace SPrestamo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var mapper = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(Prestamo)));
            SqlMapper.SetTypeMap(typeof(Prestamo), mapper);

            var mapper2 = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(AutorizacionTipo)));
            SqlMapper.SetTypeMap(typeof(AutorizacionTipo), mapper2);

            var mapper3 = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(InteresTipo)));
            SqlMapper.SetTypeMap(typeof(InteresTipo), mapper3);

            var mapper4 = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(TipoMoneda)));
            SqlMapper.SetTypeMap(typeof(TipoMoneda), mapper4);

            var mapper5 = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(EjecucionEstado)));
            SqlMapper.SetTypeMap(typeof(EjecucionEstado), mapper5);

            var mapper6 = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(Cooperante)));
            SqlMapper.SetTypeMap(typeof(Cooperante), mapper6);

            var mapper7 = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(UnidadEjecutora)));
            SqlMapper.SetTypeMap(typeof(UnidadEjecutora), mapper7);

            var mapper8 = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(PrestamoTipo)));
            SqlMapper.SetTypeMap(typeof(PrestamoTipo), mapper8);

            var mapper9 = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(Entidad)));
            SqlMapper.SetTypeMap(typeof(Entidad), mapper9);

            var mapper10 = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(Proyecto)));
            SqlMapper.SetTypeMap(typeof(Proyecto), mapper10);

            var mapper11 = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(DtmAvanceFisfinanEnp)));
            SqlMapper.SetTypeMap(typeof(DtmAvanceFisfinanEnp), mapper11);

            var mapper12 = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(DtmAvanceFisfinanCmp)));
            SqlMapper.SetTypeMap(typeof(DtmAvanceFisfinanCmp), mapper12);

            var mapper13 = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(Componente)));
            SqlMapper.SetTypeMap(typeof(Componente), mapper13);

            var mapper14 = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(ComponenteSigade)));
            SqlMapper.SetTypeMap(typeof(ComponenteSigade), mapper14);

            var mapper15 = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(PrestamoTipoPrestamo)));
            SqlMapper.SetTypeMap(typeof(PrestamoTipoPrestamo), mapper15);

            var mapper16 = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                .MakeGenericType(typeof(LineaBase)));
            SqlMapper.SetTypeMap(typeof(LineaBase), mapper16);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddIdentity<User, Rol>()
                .AddRoleStore<RoleStore>()
                .AddUserStore<UserPasswordStore>()
                .AddUserManager<CustomUserManager>()
                .AddDefaultTokenProviders();

            services.AddDataProtection()
                    .PersistKeysToFileSystem(new DirectoryInfo(@"/SIPRO"))
                    .SetApplicationName("SiproApp")
                    .DisableAutomaticKeyGeneration();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = ".AspNet.Sipro";
                options.Cookie.HttpOnly = true;

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
                options.AddPolicy("Préstamos o Proyectos - Visualizar",
                                  policy => policy.RequireClaim("sipro/permission", "Préstamos o Proyectos - Visualizar"));
                options.AddPolicy("Préstamos o Proyectos - Editar",
                                  policy => policy.RequireClaim("sipro/permission", "Préstamos o Proyectos - Editar"));
                options.AddPolicy("Préstamos o Proyectos - Eliminar",
                                  policy => policy.RequireClaim("sipro/permission", "Préstamos o Proyectos - Eliminar"));
                options.AddPolicy("Préstamos o Proyectos - Crear",
                                  policy => policy.RequireClaim("sipro/permission", "Préstamos o Proyectos - Crear"));
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                                 .AllowAnyHeader()
                                 .AllowAnyMethod();
                      });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseMvc();

            app.UseCors("AllowAllHeaders");
        }
    }
}
