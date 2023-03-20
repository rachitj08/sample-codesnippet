using Sample.Admin.Service.Infrastructure.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Sample.Admin.Service.Infrastructure.Repository
{
    public class Bootstrapper
    {
        public static string defaultConnectionString;
        static Bootstrapper()
        {

        }
        /// <summary>
        /// To Configure services at a common place
        /// </summary>
        /// <param name="services">All service collection</param>
        /// <param name="defaultConnection">default connection string parameter</param>
        public static void ConfigureServices(IServiceCollection services, string defaultConnection="")
        {
            defaultConnectionString = defaultConnection;
            services.AddEntityFrameworkNpgsql();
            services.AddDbContextPool<CloudAcceleratorContext>(optionsBuilder =>
            {
                optionsBuilder.UseNpgsql(defaultConnection);
            });       

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAuthenticationConfigKeyRepository, AuthenticationConfigKeyRepository>();

            services.AddScoped<IApplicationConfigRepository, ApplicationConfigRepository>();

            services.AddScoped<IAccountServiceRepository, AccountServiceRepository>();

            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();

            services.AddScoped<IAccountRepository, AccountRepository>();

            services.AddScoped<IVersionRepository, VersionRepository>();

            services.AddScoped<IVersionModuleRepository, VersionModuleRepository>();

            services.AddScoped<IAdminUserRepository, AdminUserRepository>();

            services.AddScoped<ILoginHistoryRepository, LoginHistoryRepository>();

           // services.AddScoped<IAuthenticationTypeRepository, AuthenticationTypeRepository>();

            services.AddScoped<IServiceRepository, ServiceRepository>();

            services.AddScoped<IModuleRepository, ModuleRepository>();

            services.AddScoped<IPasswordPolicyRepository, PasswordPolicyRepository>();

            services.AddScoped<IVersionModuleRepository, VersionModuleRepository>();

            //services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();


        }
    }
}
