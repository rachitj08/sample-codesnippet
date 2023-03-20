using Common.Model;
using Logger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PaymentService;
using Sample.Customer.HttpAggregator.Core;
using Sample.Customer.HttpAggregator.IServices.ActivityCode;
using Sample.Customer.HttpAggregator.IServices.Airline;
using Sample.Customer.HttpAggregator.IServices.EmailParse;
using Sample.Customer.HttpAggregator.IServices.FormsMaster;
using Sample.Customer.HttpAggregator.IServices.ParkingProvider;
using Sample.Customer.HttpAggregator.IServices.Payment;
using Sample.Customer.HttpAggregator.IServices.Reservation;
using Sample.Customer.HttpAggregator.IServices.Services.CommonUtility;
using Sample.Customer.HttpAggregator.IServices.TripPaxAndBags;
using Sample.Customer.HttpAggregator.IServices.UserManagement;
using Sample.Customer.HttpAggregator.IServices.VehicleInfo;
using Sample.Customer.HttpAggregator.IServices;
using Sample.Customer.HttpAggregator.Middleware;
using Sample.Customer.HttpAggregator.Services.ActivityCode;
using Sample.Customer.HttpAggregator.Services.Airline;
using Sample.Customer.HttpAggregator.Services.EmailParse;
using Sample.Customer.HttpAggregator.Services.FormsMaster;
using Sample.Customer.HttpAggregator.Services.ParkingProvider;
using Sample.Customer.HttpAggregator.Services.Payment;
using Sample.Customer.HttpAggregator.Services.Reservation;
using Sample.Customer.HttpAggregator.Services.Services.CommonUtility;
using Sample.Customer.HttpAggregator.Services.TripPaxAndBags;
using Sample.Customer.HttpAggregator.Services.UserManagement;
using Sample.Customer.HttpAggregator.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Utility;
using Sample.Customer.HttpAggregator.Config.UrlsConfig;
using Sample.Customer.Model;

namespace Sample.Customer.HttpAggregator
{
    public class Startup
    {
        /// <summary>
        /// startup class constructor to inject configuration dependency.
        /// </summary>
        /// <param name="configuration">configuration parameter to set configuration for project</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// configuration interface
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Service collection to add all services</param>       
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //register delegating handlers
            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IOptionsMonitor<JwtBearerOptions>, TenantBasedJWTOptions>();

            var key = Encoding.ASCII.GetBytes(Configuration["Authentication:JWT_IS:Secret"]);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("jwtBearerIS", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateAudience = false,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = false,
                    ValidIssuer = Configuration["Authentication:JWT_IS:IssuerURL"]
                };
                options.RequireHttpsMetadata = false;
            })
            .AddJwtBearer("jwtBearerAD", opt =>
            {
                var audience = Configuration["Authentication:JWT:Audience"];
                var validAudiences = Configuration["Authentication:JWT:ValidAudiences"];
                var validIssuers = Configuration["Authentication:JWT:ValidIssuers"];

                if (!string.IsNullOrWhiteSpace(audience)) opt.Audience = audience;

                opt.Authority = Configuration["Authentication:JWT:Authority"];
                var tokenParams = new TokenValidationParameters
                {
                    ValidateIssuer = Convert.ToBoolean(Configuration["Authentication:JWT:ValidateIssuer"]),
                    ValidateAudience = Convert.ToBoolean(Configuration["Authentication:JWT:ValidateAudience"]),
                };

                if (!string.IsNullOrEmpty(validAudiences))
                {
                    tokenParams.ValidAudiences = new List<string>
                    {
                        validAudiences
                    };
                }

                if (!string.IsNullOrEmpty(validIssuers))
                {
                    tokenParams.ValidIssuers = new List<string>
                    {
                        validIssuers
                    };
                }

                opt.RequireHttpsMetadata = false;
                opt.TokenValidationParameters = tokenParams;
            });

            // Creating policies that wraps the authorization requirements
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes("jwtBearerIS", "jwtBearerAD")
                    .Build();
            });

            IdentityModelEventSource.ShowPII = true;

            services.AddTransient<IRequestLogger, Logger.RequestLogger>();
            services.AddHttpClient<IAuthenticationService, AuthenticationService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<IUserService, UserService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<IPasswordPolicyService, PasswordPolicyService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<IGroupService, GroupService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<IAuthenticationConfigKeysValuesService, AuthenticationConfigKeysValuesService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            // services.AddHttpClient<Sample.Customer.HttpAggregator.IServices.ReportingManagement.IReportService, Sample.Customer.HttpAggregator.Services.ReportingManagement.ReportService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<IFormMasterService, FormsMasterService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<IUserVehiclePreferenceCategoryFeaturesService, UserVehiclePreferenceCategoryFeaturesService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<IVerifyEmailService, VerifyEmailService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<IVehicleInfoService, VehicleInfoService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            //services.AddHttpClient<Sample.Customer.HttpAggregator.IServices.ReportingManagement.IReportJobsService, Sample.Customer.HttpAggregator.Services.ReportingManagement.ReportJobsService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<IVehicleCategoryAndFeatureService, VehicleCategoryAndFeatureService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<IEmailParserService, EmailParserService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<IVehiclesService, VehiclesService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<IPaymentMethodService, PaymentMethodService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();

            LoggerBootStrapper.AddServices(services);

            // Get Configuration
            services.Configure<BaseUrlsConfig>(Configuration.GetSection("urls"));
            services.Configure<BaseConfig>(Configuration.GetSection("baseTokenConfig"));
            services.Configure<ResetPasswordConfig>(Configuration.GetSection("Common"));
            services.Configure<PaymentConfig>(Configuration.GetSection("Stripe"));
            services.Configure<GoogleConfig>(Configuration.GetSection("Authentication:Google"));
            services.Configure<FacebookConfig>(Configuration.GetSection("Authentication:Facebook"));
            services.Configure<SendVerificationMailConfig>(Configuration.GetSection("EmailVerification"));
            services.Configure<CommonConfig>(Configuration.GetSection("Common"));

            services.AddSingleton<ICommonHelper, CommonHelper>();
            services.AddSingleton<IServiceCommonUtility, ServiceCommonUtility>();
            //services.AddSingleton<IReservationService, ReservationService>();
            services.AddHttpClient<IReservationService, ReservationService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<IAirlineService, AirlineService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<IAirportAddressService, AirportAddressService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();

            services.AddHttpClient<IActivityCodeService, ActivityCodeService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();

            services.AddHttpClient<ITripPaxAndBagsService, TripPaxAndBagsService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<IParkingProviderService, ParkingProviderService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();



            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });

            services.AddRouting();
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()

                );
            });
            services.AddMvc();
            services.AddControllers().AddNewtonsoftJson();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "Sample Tenant Gateway API", Version = "v1" });
                // To Enable authorization using Swagger (JWT)  
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });


                swagger.AddSecurityDefinition("api-key", new OpenApiSecurityScheme()
                {
                    Name = "api-key",
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Description = "api-key",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "api-key"
                                }
                            },
                            new string[] {}

                    }
                });

                var filePath = "";

#if DEBUG

                filePath = Path.Combine(System.AppContext.BaseDirectory, "Sample.Customer.HttpAggregator.xml");
#else
            filePath = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
#endif
                swagger.IncludeXmlComments(filePath);
            });
            services.AddSwaggerGenNewtonsoftSupport();
        }



        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">application builder property to configure</param>
        /// <param name="env">environment variable property to configure</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRequestLog();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("./v1/swagger.json", "v1");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                name: "default",
                template: "{controller=Values}/{action=Get}/{id?}");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute("default", "{controller=Values}/{action=Get}/{id?}");
            });

        }
    }
}
