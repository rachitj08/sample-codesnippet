using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PaymentService;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PaymentAPI
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
            services.AddControllers();

            //Stripe payment Setting
            var stripeConfig = Configuration.GetSection("Stripe").Get<PaymentConfig>();
            services.AddSingleton(stripeConfig);
            services.AddSingleton<IPaymentServices, PaymentServices>();

            //swagger settings            
            services.AddSwaggerGen(c =>
            {
                //Setting for Title/ Contact/ License / version details
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Payment API",
                    //Version = string.Format("{0}({1})", Configuration["ApiConfig:SiteVersion"], Environment.GetEnvironmentVariable("SiteVersion"))

                });

                ////Enable Jwt token support on Authorize button
                //c.AddSecurityDefinition("JWT", new OpenApiSecurityScheme
                //{
                //    Name = "Authorization",
                //    Type = SecuritySchemeType.ApiKey,
                //    Scheme = "Bearer",
                //    BearerFormat = "JWT",
                //    In = ParameterLocation.Header,
                //    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                //});

                ////Setting for reading token 
                //c.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //          new OpenApiSecurityScheme
                //            {
                //                Reference = new OpenApiReference
                //                {
                //                    Type = ReferenceType.SecurityScheme,
                //                    Id = "JWT"
                //                }
                //            },
                //            new string[] {}

                //    }
                //});

                //Enable Annotation support
                c.EnableAnnotations();

                //Use  fullname of schema
                c.CustomSchemaIds(x => x.FullName);


                ////AppDomain.CurrentDomain.BaseDirectory
                ////Enable reading of comments from controller files
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(Directory.GetCurrentDirectory(), xmlFile);
                //c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

            });

            // Add support for Newton json
            services.AddSwaggerGenNewtonsoftSupport();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //app.UseSwaggerAuthorized();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            //swagger settings           
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                c.DocExpansion(DocExpansion.None);               
            });
        }
    }
}
