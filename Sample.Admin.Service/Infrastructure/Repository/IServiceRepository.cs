using Common.Model;
using Sample.Admin.Service.Infrastructure.DataModels;
using Sample.Admin.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Admin.Service.Infrastructure.Repository
{
    public interface IServiceRepository
    {
        /// <summary>
        /// Get All Services Info
        /// </summary>
        /// <returns></returns>
        Task<ResponseResultList<ServiceModel>> GetAllServices(int pageSize, int pageNumber, string ordering, string search, int offset, bool all);

        /// <summary>
        /// Get Services Details
        /// </summary>
        /// <returns></returns>
        Task<ServiceModel> GetServiceDetail(int serviceId);
    }
}
