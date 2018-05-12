using System;
using System.IO;
using Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Utilities;
using SiproModelCore.Models;
using Identity;
using Microsoft.AspNetCore.Identity;

namespace SCooperante
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
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

            services.ConfigureApplicationCookie(options => {
                options.Cookie.Name = ".AspNet.Sipro";
				options.Cookie.HttpOnly = true;
               
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Cooperantes - Visualizar",
                                  policy => policy.RequireClaim("sipro/permission", "Cooperantes - Visualizar"));
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
        }
    }
}
