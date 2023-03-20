using Sample.Admin.Service.Infrastructure.DataModels;
using System.Threading.Tasks;

namespace Sample.Admin.Service.Infrastructure.Repository
{
    public interface IApplicationConfigRepository
    {
        Task<ApplicationConfigs> GetApplicationConfig();
    }
}
