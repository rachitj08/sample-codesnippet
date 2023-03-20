using Common.Model;
using Sample.Admin.Service.Infrastructure.Repository;
using Sample.Admin.Model;
using System.Threading.Tasks;

namespace Sample.Admin.Service.ServiceWorker
{
    public class ServiceService : IServiceService
    {        
        private readonly IUnitOfWork unitOfWork;
        private readonly IServiceRepository serviceRepository;
       
        /// <summary>
        /// Services Service constructor to Inject dependency
        /// </summary>
        /// <param name="unitOfWork">unit of work</param>
        /// <param name="serviceRepository">service repository</param>
        public ServiceService(IUnitOfWork unitOfWork, IServiceRepository serviceRepository)
        {
            this.unitOfWork = unitOfWork;
            this.serviceRepository = serviceRepository;
        }
        

        /// <summary>
        /// Get All Services Info
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResultList<ServiceModel>> GetAllServices(int pageSize, int pageNumber, string ordering, string search, int offset, bool all)
        { 
            return await this.serviceRepository.GetAllServices(pageSize, pageNumber, ordering, search, offset, all);
        }

        /// <summary>
        /// Get All Services Info
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceModel> GetServiceDetail(int serviceId)
        {
            return await this.serviceRepository.GetServiceDetail(serviceId);
        }

    }
}
