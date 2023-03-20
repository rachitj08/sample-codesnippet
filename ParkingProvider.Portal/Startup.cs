using Core.Storage.ServiceWorker.IMediaUploadServices;
using Core.Storage.ServiceWorker.MediaUploadServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Core.Storage.ServiceWorker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using ParkingProvider.Portal.Data;
using Core.Storage.Helper;
using uhv.Customer.Model.Model;
using Common.Model;

namespace ParkingProvider.Portal
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env,IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
           .SetBasePath(env.ContentRootPath)
           .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
           .AddEnvironmentVariables();
            Configuration = builder.Build();
            //Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IImageUploadService, ImageUploadService>();
            services.AddSingleton<IUploadFileService, UploadFileService>();
            services.AddSingleton<IS3Client, S3Client>();
            services.AddSingleton<IS3AzureClient, S3AzureClient>();
            services.AddAWSService<IAmazonS3>();
            services.AddSingleton<IDatabaseContext, DatabaseContext>();
            services.AddRazorPages();
            // Configuration for Storage
            services.Configure<AwsConfig>(Configuration.GetSection("AWS"));
            services.Configure<AzureConfig>(Configuration.GetSection("Storage:AZURE"));
            services.Configure<AppSettings>(Configuration.GetSection("Storage:AppSettings"));
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();
            services.Configure<CustomerBaseUrlsConfig>(Configuration.GetSection("urls"));
            services.Configure<CommonConfig>(Configuration.GetSection("Common"));
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UsePathBase("/provider-web");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapRazorPages();
            //});
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=QRGenerator}/{action=Index}/{id?}");
            });
        }
    }
}
