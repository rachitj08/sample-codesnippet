using Logger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Utilities;
using Sample.Customer.Service;
using Common.Model;
using PaymentService;

namespace Sample.Customer.API
{
    public class Startup
    {
        public static string ConnectionString { get; private set; }


        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
           .SetBasePath(env.ContentRootPath)
           .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
           .AddEnvironmentVariables();

            Configuration = builder.Build();
            //Console.WriteLine("Environment Variable :-" + env.EnvironmentName);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().AddNewtonsoftJson();
            services.Configure<CommonConfig>(Configuration.GetSection("Common"));
            ConnectionString = Configuration.GetConnectionString("default");
            services.Configure<PaymentConfig>(Configuration.GetSection("Stripe"));
            ServiceBootstrapper.ConfigureServices(services, Configuration, ConnectionString);

            LoggerBootStrapper.AddServices(services);
            //services.AddEntityFrameworkNpgsql();
            //services.AddDbContextPool<CloudAcceleratorContext>(optionsBuilder =>
            //{
            //    optionsBuilder.UseNpgsql(ConnectionString);
            //});
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                    builder.SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });



            //services.UseSchemaPerTenant(Configuration);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCors();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
