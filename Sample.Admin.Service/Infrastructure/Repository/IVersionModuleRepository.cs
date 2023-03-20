using Common.Model;
using Sample.Admin.Service.Infrastructure.DataModels;
using Sample.Admin.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Admin.Service.Infrastructure.Repository
{
    public interface IVersionModuleRepository
    {

        /// <summary>
        /// Get All Version Modules
        /// </summary>
        /// <returns></returns>
        Task<ResponseResultList> GetAllVersionModules(string ordering, int offset, int pageSize, int pageNumber, bool all);

        Task<VersionModulesModel> GetVersionModuleById(long versionmModuleId);

        /// <summary>
        /// Get Version  Modules By VersionId
        /// </summary>
        /// <param name="versionId">The VersionId to get version modules </param>
        /// <returns></returns>
        Task<IEnumerable<VersionModules>> GetVersionModulesByVersionId(int versionId);

        /// <summary>
        /// To Create version Module
        /// </summary>
        /// <param name="versionModule">The new version module object</param>
        /// <returns></returns>
        Task<VersionModulesModel> CreateVersionModule(VersionModulesModel versionModule);

        Task<VersionModulesModel> UpdateVersionModule(long versionModuleId, VersionModulesModel versionModule);

        Task<VersionModulesModel> UpdatePartialVersionModule(long versionmModuleId, VersionModulesModel versionModule);

        /// <summary>
        /// To Delete Versions Module
        /// </summary>
        /// <param name="versionModuleId">The versionModuleId to delete versionmodule</param>
        /// <returns></returns>
        Task<long> DeleteVersionModules(long versionModuleId);

        Task<long> DeleteVersionModulesByVersionId(long versionId);
    }
}
