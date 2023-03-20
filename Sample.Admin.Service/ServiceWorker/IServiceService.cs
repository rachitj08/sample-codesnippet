using Common.Model;
using Sample.Admin.Model;
using System.Threading.Tasks;

namespace Sample.Admin.Service.ServiceWorker
{
    public interface IServiceService
    {
        /// <summary>
        /// Get All Services Info
        /// </summary>
        /// <returns></returns>
        Task<ResponseResultList<ServiceModel>> GetAllServices(int pageSize, int pageNumber, string ordering, string search, int offset, bool all);

        /// <summary>
        /// Get Services Detail
        /// </summary>
        /// <returns></returns>
        Task<ServiceModel> GetServiceDetail(int serviceId);
    }
}
