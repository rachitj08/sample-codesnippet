using Common.Model;
using Sample.Admin.Service.Helpers;
using Sample.Admin.Service.Infrastructure.DataModels;
using Sample.Admin.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Admin.Service.ServiceWorker
{
    public interface IVersionService
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
        Task<VersionModel> GetVersionById(int versionId);

        /// <summary>
        /// To Create new Version
        /// </summary>
        /// <param name="version">version object</param>
        /// <returns></returns>
        Task<ResponseResult<VersionsModel>> CreateVersion(VersionsModel version, int loggedInUserId);

        /// <summary>
        /// To Update existing Version
        /// </summary>
        /// <param name="version">version object</param>
        /// <returns></returns>

        Task<ResponseResult<VersionsModel>> UpdateVersion(long versionId, VersionsModel version, int loggedInUserId);

        /// <summary>
        /// To Update Version Partially
        /// </summary>
        /// /// <param name="versionId">Version Id</param>
        /// <param name="version">New version object</param>
        /// <returns></returns>
        Task<ResponseResult<VersionsModel>> UpdatePartialVersion(long versionId, VersionsModel version, int loggedInUserId);

        /// <summary>
        /// To Delete existing Version
        /// </summary>
        /// <param name="versionId">version identifier</param>
        /// <returns></returns>
        Task<long> DeleteVersion(long versionId);
    }
}
