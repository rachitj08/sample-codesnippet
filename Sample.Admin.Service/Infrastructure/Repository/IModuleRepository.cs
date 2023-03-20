using Common.Model;
using Sample.Admin.Service.Infrastructure.DataModels;
using Sample.Admin.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Admin.Service.Infrastructure.Repository
{
    public interface IModuleRepository
    {
        Task<List<Modules>> GetAllModules(bool isNavigationItem, long accountId, int serviceId);
        /// <summary>
        /// Get All Modules
        /// </summary>
        /// <returns></returns>
        Task<ResponseResultList<ModulesModel>> GetAllModules(string ordering, string search, int offset, int pageSize, int pageNumber, string serviceName, bool all);

        Task<Modules> GetModuleById(long moduleId);

        /// <summary>
        /// Get Sub Modules By ModuleId
        /// </summary>
        /// <param name="moduleId">The ModuleId to get sub module</param>
        /// <returns></returns>
        Task<IEnumerable<Modules>> GetSubModulesByModuleId(long moduleId);

        /// <summary>
        /// To Create Module
        /// </summary>
        /// <param name="module">new Module object/param>
        /// <returns></returns>
        Task<long> CreateModule(Modules module);

        /// <summary>
        /// To Update Module
        /// </summary>
        /// <param name="module">New module object</param>
        /// <returns></returns>
        Task<int> UpdateModule(Modules module); 

        /// <summary>
        /// To Delete Module
        /// </summary>
        /// <param name="moduleId">The moduleId to delete</param>
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
