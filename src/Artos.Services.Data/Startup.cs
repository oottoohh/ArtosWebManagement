using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Artos.Tools;
using Swashbuckle.AspNetCore.Swagger;
using Artos.Services.Data.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Artos.Services.Data
{
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
            var sqlConnectionString = Configuration.GetConnectionString("MySqlCon");
            
            services.AddDbContext<ArtosDB>(options =>
                options.UseMySql(
                    sqlConnectionString,
                    b => b.MigrationsAssembly("Artos.Services.Data")
                )
            );
            RedisDB.RedisConStr = Configuration.GetConnectionString("RedisCon");
            
            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    policy.AllowAnyOrigin();
                });
            });

            // Add framework services.
            //services.AddMvc();
            services.AddMvcCore()
            .AddAuthorization()
            .AddJsonFormatters()
            .AddApiExplorer();
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Configuration.GetValue<string>("server:identityurl");
                    options.RequireHttpsMetadata = false;
                    options.ApiName = Configuration.GetValue<string>("server:apiname"); 
                });

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Artos API",
                    Version = "v1",
                    Description = "Rest API for accessing master data",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "Artos Team", Email = "team@artos.com", Url = "http://twitter.com/artos" },
                    License = new License { Name = "For developers only", Url = "http://artos.com" }
                });

            });
            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.CookieHttpOnly = true;
            });

            ObjectContainer.Register<RedisDB>(new RedisDB());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            /*
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc();*/
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseSession();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            //app.UseIdentity();
            app.UseWebSockets();
            //app.UseSignalR();
            /*
            app.UseGoogleAuthentication(new GoogleOptions()
            {
                ClientId = Configuration["Authentication:Google:ClientId"],
                ClientSecret = Configuration["Authentication:Google:ClientSecret"]
            });
            app.UseFacebookAuthentication(new FacebookOptions()
            {
                AppId = Configuration["Authentication:Facebook:ClientId"],
                AppSecret = Configuration["Authentication:Facebook:ClientSecret"]
            });
            
            // Configure the HTTP request pipeline.
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "Cookie",
                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });*/
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Home}/{id?}"
                );
                /*
                routes.MapRoute(
                       name: "api",
                       template: "api/{controller=Home}/{action=Home}/{id?}"
               );*/
            });
            if (env.IsDevelopment())
            {

                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUi(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Artos API V1");
            });
            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
            //app.UseCors(builder => builder.WithOrigins("http://murahaje.azurewebsites.net"));
           
        }
    }
}
