using Common.Model;
using Sample.Admin.Model;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Admin.HttpAggregator.IServices
{
    /// <summary>
    /// Module Service interface
    /// </summary>
    public interface IModuleService
    {
        /// <summary>
        /// Module Service to get module list
        /// </summary>
        /// <returns></returns>
        Task<ResponseResultList<ModulesModel>> GetAllModules(HttpContext httpContext, string ordering, string search, int offset, int pageSize, int pageNumber, string serviceName, bool all);

        /// <summary>
        /// Get Module By Id
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<ModulesModel>> GetModuleById(long moduleId);

        /// <summary>
        /// To Create new Module
        /// </summary>
        /// <param name="module">module object</param>
        /// <returns></returns>
        Task<ResponseResult<ModulesModel>> CreateModule(ModulesModel module);
        /// <summary>
        /// To Update existing Module
        /// </summary>
        /// <param name="module">module object</param>
        /// /// <param name="moduleId">Unique Module Id</param>
        /// <returns></returns>

        Task<ResponseResult<ModulesModel>> UpdateModule(long moduleId, ModulesModel module);

        /// <summary>
        /// To Update existing Module
        /// </summary>
        /// <param name="module">module object</param>
        /// /// <param name="moduleId">Unique Module Id</param>
        /// <returns></returns>

        Task<ResponseResult<ModulesModel>> UpdatePartialModule(long moduleId, ModulesModel module);

        /// <summary>
        /// To Delete existing Module
        /// </summary>
        /// <param name="moduleId">module identifier</param>
        /// <returns></returns>
        Task<ResponseResult<ModulesModel>> DeleteModule(long moduleId);

        /// <summary>
        /// Build Navigation
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<List<ModuleNavigationModel>>> BuildNavigation();

        /// <summary>
        /// Get Modules For Service
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        Task<ResponseResult<List<ModulesAllModel>>> GetModulesForService(string serviceName);
        /// <summary>
        /// GetModulesByAccountId
        /// </summary>
        /// <param name="accounID"></param>
        /// <returns></returns>
        Task<ResponseResult<List<Module>>> GetModulesByAccountId(long accounID);
    }
}
