using Common.Model;
using Sample.Admin.Service.Infrastructure.DataModels;
using Sample.Admin.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Admin.Service.Infrastructure.Repository
{
    public interface IVersionRepository
    {
        /// <summary>
        /// Get All Versions
        /// </summary>
        /// <returns></returns>
        Task<ResponseResultList<VersionsModel>> GetAllVersions(string ordering, string search, int offset, int pageSize, int pageNumber, bool all);

        /// <summary>
        /// Get Version By Id
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        Task<Versions> GetVersionById(int versionId);

        /// <summary>
        /// Get Version Modules By VersionId
        /// </summary>
        /// <param name = "versionId" > The VersionId to get version modules</param>
        /// <returns></returns>
        Task<List<Modules>> GetVersionModulesByVersionId(int versionId);

        /// <summary>
        /// To Create Version
        /// </summary>
        /// <param name="version">new Version object/param>
        /// <returns></returns>
        Task<VersionsModel> CreateVersion(VersionsModel version, int userId);

        /// <summary>
        /// To Update Version
        /// </summary>
        /// <param name="version">New version object</param>
        /// <returns></returns>
        Task<VersionsModel> UpdateVersion(long versionId, VersionsModel version, int userId);
    
        /// <summary>
        /// To Update Version Partially
        /// </summary>
        /// /// <param name="versionId">Version Id</param>
        /// <param name="version">New version object</param>
        /// <returns></returns>
        Task<VersionsModel> UpdatePartialVersion(long versionId, VersionsModel version, int userId);

        /// <summary>
        /// To Delete Version
        /// </summary>
        /// <param name="versionId">The versionId to delete</param>
        /// <returns></returns>
        Task<long> DeleteVersion(long versionId);


        /// <summary>
        /// Get All Versions
        /// </summary>
        /// <returns></returns>
        //Task<IEnumerable<DATAMODEL.Versions>> GetAllVersions();


        /// <summary>
        /// Get All Modules
        /// </summary>
        /// <returns></returns>
        //Task<IEnumerable<Modules>> GetAllModules();

        /// <summary>
        /// Get All Version Modules
        /// </summary>
        /// <param name="VersionId">The VersionId to get version</param>
        /// <returns></returns>
        //Task<IEnumerable<VersionModules>> GetAllVersionModules(int VersionId);

        /// <summary>
        /// To Create Version
        /// </summary>
        /// <param name="version">The new version object</param>
        /// <returns></returns>
        //Task<DATAMODEL.Versions> CreateVersion(DATAMODEL.Versions version);

        /// <summary>
        /// To Update Version
        /// </summary>
        /// <param name="version">The new version object</param>
        /// <returns></returns>
        //Task<DATAMODEL.Versions> UpdateVersion(DATAMODEL.Versions versionModel);

        /// <summary>
        /// To Delete Version
        /// </summary>
        /// <param name="versionId">Version Id to delete Version</param>
        /// <returns></returns>
        //Task<int> DeleteVersion(int versionId);

        /// <summary>
        /// To get all versions with all features(Modules, Sub Modules, and Permissions)
        /// </summary>
        /// <returns></returns>
        //Task<IEnumerable<Model.Versions>> GetVersions();

        /// <summary>
        ///  To get version details with all features(Modules, Sub Modules, and Permissions) for given Version Id
        /// </summary>
        /// <param name="VersionId">Version Id to get version details</param>
        /// <returns></returns>
        //Task<IEnumerable<Model.Versions>> GetVersionByVersionId(int VersionId);
    }
}
