using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using System.IO;
using Utilities.EmailHelper;

namespace Utilities
{
    public static class EmailConfigBootStrapper
    {
        /// <summary>
        /// configuration interface
        /// </summary>
        public static IConfiguration Configuration { get; }
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IEmailHelper, EmailHelper.EmailHelper>();
            services.Configure<SmtpConfiguration>(configuration.GetSection("SmtpConfiguration"));
            return services;
        }
    }
}
