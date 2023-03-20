using Common.Model;
using Logger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Utilities;
using Utility;
using Sample.Customer.Model;
using Sample.Customer.Service.Infrastructure.Repository;
using Sample.Customer.Service.ServiceWorker;
using Core.Email.EmailNotification;
using Sample.Customer.Model.Model;
using PaymentService;
using Sample.Customer.Service.ServiceWorker.ParkingProvider;

namespace Sample.Customer.Service
{
    public class ServiceBootstrapper
    {

        static ServiceBootstrapper()
        {

        }
        /// <summary>
        /// To Configure services at a common place
        /// </summary>
        /// <param name="services">All service collection</param>
        /// <param name="defaultConnection">default connection string parameter</param>
        public static void ConfigureServices(IServiceCollection services, IConfiguration Configuration, string defaultConnection = "")
        {
            services.Configure<CustomerBaseUrlsConfig>(Configuration.GetSection("urls"));
            // Set Configuration
            services.Configure<AuthenticationConfig>(Configuration.GetSection("AuthenticationConfig"));
            services.Configure<ResetPasswordConfig>(Configuration.GetSection("ResetPassword"));
            services.Configure<SendVerificationMailConfig>(Configuration.GetSection("EmailVerification"));
            services.Configure<SMSConfig>(Configuration.GetSection("SMSConfig"));
            services.Configure<EmailSenderConfig>(Configuration.GetSection("EmailProvider"));
            var stripeConfig = Configuration.GetSection("Stripe").Get<PaymentConfig>();
            if (stripeConfig != null)
            {
                services.Configure<PaymentConfig>(Configuration.GetSection("Stripe"));
                services.AddSingleton(stripeConfig);
            }
            var awsCommon = Configuration.GetSection("Common");
            if (awsCommon != null)
            {
                services.Configure<CommonConfig>(Configuration.GetSection("Common"));
            }
            // Configuration for Storage
            var awsConfig = Configuration.GetSection("AWS");
            var storageConfig = Configuration.GetSection("Storage:AppSettings");

            //  SmtpConfiguration
            EmailConfigBootStrapper.AddServices(services, Configuration);

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // DI
            services.AddSingleton<IPaymentServices, PaymentServices>();
            //var stripeConfig = Configuration.GetSection("Stripe").Get<PaymentConfig>();
            //services.AddSingleton(stripeConfig);


            services.AddScoped<IFileLogger, FileLogger>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IPasswordPolicyService, PasswordPolicyService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IAuthenticationConfigKeysValuesService, AuthenticationConfigKeysValuesService>();
            services.AddScoped<ICommonHelper, CommonHelper>();
            services.AddScoped<IFormMasterService, FormsMasterService>();
            
            services.AddScoped<ITokenService, TokenService>();
            services.AddSingleton<ISMSHelper, SMSHelper>();
            services.AddScoped<IUserVehiclePreferenceCategoryFeaturesService, UserVehiclePreferenceCategoryFeaturesService>();
            services.AddScoped<IVerifyEmailService, VerifyEmailService>();
            services.AddScoped<IEmailParserCallBackService, EmailParserCallBackService>();

            services.AddScoped<IVehicleCategoryAndFeatureService, VehicleCategoryAndFeatureService>();
            services.AddScoped<IParkingHeadsService, ParkingHeadsService>();
            services.AddScoped<IParkingProvidersLocationsService, ParkingProvidersLocationsService>();
            services.AddScoped<IAirportsParkingService, AirportsParkingService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IAirportAddressService, AirportAddressService>();
            services.AddScoped<IAirportAddressService, AirportAddressService>();
            services.AddScoped<ITripPaxAndBagsService, TripPaxAndBagsService>();


            services.AddScoped<IActivityCodeService, ActivityCodeService>();

            services.AddScoped<IVehiclesService, VehiclesService>();
            services.AddScoped<IUserVehiclePreferenceCategoryFeaturesService, UserVehiclePreferenceCategoryFeaturesService>();
            services.AddScoped<IPaymentIntentService, PaymentIntentService>();
            services.AddScoped<IAirlineService, AirlineService>();
            services.AddScoped<IParkingProviderService, ParkingProviderService>();
            
            Bootstrapper.ConfigureServices(services, defaultConnection);
        }
    }
}
