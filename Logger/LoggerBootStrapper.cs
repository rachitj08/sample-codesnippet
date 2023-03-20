using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using System.IO;

namespace Logger
{
    public static class LoggerBootStrapper
    {
        /// <summary>
        /// To add Services 
        /// </summary>
        /// <param name="services">The service collection </param>
        /// <returns></returns>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IFileLogger, FileLogger>();
            services.AddSingleton<Serilog.ILogger>
            (x => new LoggerConfiguration()
                  .MinimumLevel.Debug()
                  .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                  .MinimumLevel.Override("System", LogEventLevel.Warning)
                  .WriteTo.File(new JsonFormatter(),Path.Combine(Directory.GetCurrentDirectory(), "log.txt"),rollingInterval:Serilog.RollingInterval.Day)
                  .CreateLogger());
            return services;
        }

        /// <summary>
        /// To configure logger factory
        /// </summary>
        /// <param name="app">The Application Buider parameter</param>
        /// <param name="env">The Host Environment parameter</param>
        /// <param name="loggerFactory">The logger factory parameter</param>
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();
        }
    }
}
