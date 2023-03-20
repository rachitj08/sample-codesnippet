using Sample.Admin.Service.Infrastructure.DataModels;
using Sample.Admin.Service.Infrastructure.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Admin.Service.ServiceWorker
{
    public class AuthenticationTypeService : IAuthenticationTypeService
    {
       
       
        private readonly IUnitOfWork unitOfWork;

        
        private readonly IAuthenticationTypeRepository authenticationRepository;

    

        /// <summary>
        /// Authentication Service constructor to Inject dependency
        /// </summary>
        /// <param name="unitOfWork">unit of work</param>
        /// <param name="authenticationRepository">authentication repository</param>
        public AuthenticationTypeService(IUnitOfWork unitOfWork, IAuthenticationTypeRepository authenticationRepository)
        {
            this.unitOfWork = unitOfWork;
            this.authenticationRepository = authenticationRepository;
        }

        /// <summary>
        /// Get All Authentication Types
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<AuthenticationTypes>> GetAllAuthenticationTypes()
        {

            var authenticationTypesData = await this.authenticationRepository.GetAllAuthenticationTypes();

            return authenticationTypesData;

        }

    }
}
