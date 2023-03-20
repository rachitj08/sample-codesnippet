using Common.Model;
using Sample.Admin.Service.Infrastructure.DataModels;
using Sample.Admin.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Admin.Service.ServiceWorker
{
    public interface IVersionModuleService
    {
        /// <summary>
        /// Get All Version Modules
        /// </summary>
        /// <returns></returns>
        Task<ResponseResultList> GetAllVersionModules(string ordering, int offset, int pageSize, int pageNumber, bool all);

        /// <summary>
        /// Get VersionModule By Id
        /// </summary>
        /// <returns></returns>
        Task<VersionModulesModel> GetVersionModuleById(long versionModuleId);

        /// <summary>
        /// Get Sub Version Modules By VersionModuleId
        /// </summary>
        /// <param name="versionModuleId">versionModule identifier</param>
        /// <returns></returns>
        Task<IEnumerable<VersionModules>> GetVersionModulesByVersionId(int versionModuleId);

        /// <summary>
        /// To Create new VersionModule
        /// </summary>
        /// <param name="versionModule">versionModule object</param>
        /// <returns></returns>
        Task<VersionModulesModel> CreateVersionModule(VersionModulesModel versionModule);

        /// <summary>
        /// To Update existing Module
        /// </summary>
        /// <param name="versionModule">versionModule object</param>
        /// <returns></returns>

        Task<VersionModulesModel> UpdateVersionModule(long versionModuleId, VersionModulesModel versionModule);

        /// <summary>
        /// To Update Module Partially
        /// </summary>
        /// /// <param name="versionModuleId">Version Module ID</param>
        /// <param name="versionModule">New versionModule object</param>
        /// <returns></returns>
        Task<VersionModulesModel> UpdatePartialVersionModule(long versionModuleId, VersionModulesModel versionModule);

        /// <summary>
        /// To Delete existing VersionModule
        /// </summary>
        /// <param name="versionModuleId">versionModule identifier</param>
        /// <returns></returns>
        Task<long> DeleteVersionModules(long versionModuleId);
    }
}
