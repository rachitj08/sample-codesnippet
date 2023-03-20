using AutoMapper;
using Common.Model;
using Logger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
//using Microsoft.IdentityModel.Logging;
//using MongoDBLogger;
using System;
//using Utilities;
using Sample.Admin.Model;
using Sample.Admin.Service;

namespace Sample.Admin.API
{
    public class Startup
    {
        /// <summary>
        /// DB connection string variable
        /// </summary>
        public static string ConnectionString { get; private set; }

        /// <summary>
        /// startup class constructor to inject configuration dependency.
        /// </summary>
        /// <param name="env">env parameter to set host environment for project</param>
        /// <param name="configuration">configuration parameter to set configuration for project</param>
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                .AddConfiguration(configuration)
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
               .AddEnvironmentVariables();

            Configuration = builder.Build();
            //Console.WriteLine("Environment Variable :-" + env.EnvironmentName);
        }

        /// <summary>
        /// configuration variable
        /// </summary>
        public IConfiguration Configuration { get; }


        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Service collection to add all services</param>
        public void ConfigureServices(IServiceCollection services)
        {
            //IdentityModelEventSource.ShowPII = true;
            //services.AddControllers().AddNewtonsoftJson();
            //services.Configure<ResetPasswordConfig>(Configuration.GetSection("ResetPassword"));
            //services.Configure<AuthenticationConfig>(Configuration.GetSection("AuthenticationConfig"));

            //ConnectionString = Configuration.GetConnectionString("default");
            //ServiceBootstrapper.ConfigureServices(services, ConnectionString);

            //LoggerBootStrapper.AddServices(services);
            ////  SmtpConfiguration
            //EmailConfigBootStrapper.AddServices(services, Configuration);
            //services.Configure<MongoDatabaseSettings>(Configuration.GetSection("MongoDatabaseSettings"));
            //services.AddControllers();
            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //services.AddMvc();
        }


        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">application builder property to configure</param>
        /// <param name="env">environment variable property to configure</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            var fordwardedHeaderOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            };
            fordwardedHeaderOptions.KnownNetworks.Clear();
            fordwardedHeaderOptions.KnownProxies.Clear();

            app.UseForwardedHeaders(fordwardedHeaderOptions);
            //app.UseAuthentication();
            //app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
