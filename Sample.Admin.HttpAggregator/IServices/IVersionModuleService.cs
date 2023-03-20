using Common.Model;
using Sample.Admin.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Admin.HttpAggregator.IServices
{
    /// <summary>
    /// Module Service Class
    ///</summary>
    public interface IVersionModuleService
    {
        /// <summary>
        /// VersionModule Service to get module list
        /// </summary>
        /// <returns></returns>
        Task<ResponseResultList> GetAllVersionModules(string ordering, int offset, int pageSize, int pageNumber, bool all);

        /// <summary>
        /// Get Module By Id
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<VersionModulesModel>> GetVersionModuleById(long versionModuleId);

        /// <summary>
        /// To Create new Version
        /// </summary>
        /// <param name="version">version object</param>
        /// <returns></returns>
        Task<ResponseResult<VersionModulesModel>> CreateVersionModule(VersionModulesModel version);
        /// <summary>
        /// To Update existing Version Module
        /// </summary>
        /// <param name="versionModule">versionModule object</param>
        /// /// <param name="versionModuleId">Unique Module Id</param>
        /// <returns></returns>

        Task<ResponseResult<VersionModulesModel>> UpdateVersionModule(long versionModuleId, VersionModulesModel versionModule);

        /// <summary>
        /// To Update existing Version Module
        /// </summary>
        /// <param name="versionModule">versionModule object</param>
        /// /// <param name="versionModuleId">Unique Module Id</param>
        /// <returns></returns>

        Task<ResponseResult<VersionModulesModel>> UpdatePartialVersionModule(long versionModuleId, VersionModulesModel versionModule);

        /// <summary>
        /// To Delete existing Module
        /// </summary>
        /// <param name="moduleId">module identifier</param>
        /// <returns></returns>
        Task<ResponseResult<VersionModulesModel>> DeleteVersionModule(long moduleId);
    }
}
