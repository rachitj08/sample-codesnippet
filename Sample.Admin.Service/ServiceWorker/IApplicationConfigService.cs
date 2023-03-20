using Sample.Admin.Model;
using System.Threading.Tasks;

namespace Sample.Admin.Service.ServiceWorker
{
    public interface IApplicationConfigService
    {
        /// <summary>
        /// Get Application Config Details
        /// </summary>
        /// <returns></returns>
        Task<ApplicationConfig> GetApplicationConfig();
    }
}
