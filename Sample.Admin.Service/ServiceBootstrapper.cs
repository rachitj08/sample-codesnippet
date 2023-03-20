using Microsoft.Extensions.DependencyInjection;
using Utility;
using Sample.Admin.Service.Infrastructure.Repository;
using Sample.Admin.Service.ServiceWorker;

namespace Sample.Admin.Service
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
        public static void ConfigureServices(IServiceCollection services, string defaultConnection = "")
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICommonHelper, CommonHelper>();
            services.AddScoped<IApplicationConfigService, ApplicationConfigService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAccountServiceService, AccountServiceService>();
            services.AddScoped<IVersionService, VersionService>();
            services.AddScoped<IAuthenticationTypeService, AuthenticationTypeService>();
            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<IModuleService, ModuleService>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<IVersionModuleService, VersionModuleService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IAdminUserService, AdminUserService>();
            services.AddScoped<IAuthenticationConfigKeyService, AuthenticationConfigKeyService>();
            services.AddScoped<IPasswordPolicyService, PasswordPolicyService>();


            Bootstrapper.ConfigureServices(services, defaultConnection);
        }
    }
}
