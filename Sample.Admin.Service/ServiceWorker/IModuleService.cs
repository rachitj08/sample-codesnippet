using Common.Model;
using Sample.Admin.Service.Infrastructure.DataModels;
using Sample.Admin.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Admin.Service.ServiceWorker
{
    public interface IModuleService
    {
        /// <summary>
        /// Get All Modules
        /// </summary>
        /// <returns></returns>
        Task<List<Module>> GetModuleList(bool isNavigationItem, long accountId, int serviceId);

        Task<ResponseResultList<ModulesModel>> GetAllModules(string ordering, string search, int offset, int pageSize, int pageNumber, string serviceName, bool all);

        /// <summary>
        /// Get Module By Id
        /// </summary>
        /// <returns></returns>
        Task<ModulesModel> GetModuleById(long moduleId);

        /// <summary>
        /// Get Sub Modules By ModuleId
        /// </summary>
        /// <param name="moduleId">module identifier</param>
        /// <returns></returns>
        Task<IEnumerable<Modules>> GetSubModulesByModuleId(long moduleId);

        /// <summary>
        /// To Create new Module
        /// </summary>
        /// <param name="module">module object</param>
        /// <returns></returns>
        Task<ResponseResult<ModulesModel>> CreateModule(ModulesModel module, int loggedInUserId);

        /// <summary>
        /// To Update existing Module
        /// </summary>
        /// <param name="module">module object</param>
        /// <returns></returns>
        
        Task<ResponseResult<ModulesModel>> UpdateModule(long moduleId, ModulesModel module, int loggedInUserId);

        /// <summary>
        /// To Update Module Partially
        /// </summary>
        /// /// <param name="moduleId">Module ID</param>
        /// <param name="module">New module object</param>
        /// <returns></returns>
        Task<ResponseResult<ModulesModel>> UpdatePartialModule(long moduleId, ModulesModel module, int loggedInUserId);

        /// <summary>
        /// To Delete existing Module
        /// </summary>
        /// <param name="moduleId">module identifier</param>
        /// <returns></returns>
        Task<long> DeleteModule(long moduleId);


        /// <summary>
        /// Get Modules For Service
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        Task<List<ModulesAllModel>> GetModulesForService(string serviceName);

        /// <summary>
        ///  Information for module by name
        /// </summary>
        /// <param name="accountId">account Id</param>
        /// <param name="serviceId">service Id</param>
        /// <param name="moduleName">module Name</param>
        /// <returns></returns>
        Task<long> GetModuleByName(long accountId, int serviceId, string moduleName);

        Task<List<Module>> GetModulesByAccountId(long accountId, bool isNavigationItem = true);
    }
}
