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
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Text;
using Utility;
using Sample.Admin.HttpAggregator.Config.UrlsConfig;
using Sample.Admin.HttpAggregator.Core;
using Sample.Admin.HttpAggregator.IServices;
using Sample.Admin.HttpAggregator.Services;
using Utility;

namespace Sample.Admin.HttpAggregator
{
    /// <summary>
    /// Start Up
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// startup class constructor to inject configuration dependency.
        /// </summary>
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
            services.AddMvcCore().AddApiExplorer();

            services.AddControllers();

            var issuerUrl = Configuration["Authentication:JWT_IS:Issuer"];
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
                    ValidIssuer = issuerUrl
                };
                options.RequireHttpsMetadata = false;
            });

            // Creating policies that wraps the authorization requirements
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes("jwtBearerIS")
                    .Build();
            });

            //IdentityModelEventSource.ShowPII = true;

            //register delegating handlers
            services.AddScoped<ICommonHelper, CommonHelper>();
            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddHttpClient<IApplicationConfigService, ApplicationConfigService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<IAuthenticationService, AuthenticationService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<IVersionService, VersionService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<IAccountService, AccountService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<IAccountServiceService, AccountServiceService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<IAuthenticationTypeService, AuthenticationTypeService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<ICurrencyService, CurrencyService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<IServiceService, ServiceService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<IVersionModuleService, VersionModuleService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<ISubscriptionService, SubscriptionService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<IModuleService, ModuleService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<IGroupService, GroupService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<IAdminUsersService, AdminUsersService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<IPasswordPolicyService, PasswordPolicyService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<ICommonService, CommonService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();

            LoggerBootStrapper.AddServices(services);
            services.Configure<BaseUrlsConfig>(Configuration.GetSection("urls"));
            services.Configure<BaseConfig>(Configuration.GetSection("baseTokenConfig"));
            services.Configure<ResetPasswordConfig>(Configuration.GetSection("Common"));
            services.AddSingleton<ICommonHelper, CommonHelper>();
            //TenantResolverBootStrapper.AddServices(services, Configuration);

            services.AddMvc().AddMvcOptions(options => options.EnableEndpointRouting = false);
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
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "Sample Admin Gateway API", Version = "v1" });
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
                                },
                            },
                            new string[] {}

                    }
                });
                var filePath = "";

#if DEBUG
                filePath = Path.Combine(System.AppContext.BaseDirectory, "Sample.Admin.HttpAggregator.xml");
#else
            filePath = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
#endif
                swagger.IncludeXmlComments(filePath);
            });
            services.AddSwaggerGenNewtonsoftSupport();
            services.AddSwaggerGen();
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
                app.UseExceptionHandler("/Error");
                app.UseHsts();
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
