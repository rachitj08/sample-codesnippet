using Common.Model;
using Sample.Admin.Model;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Admin.HttpAggregator.IServices
{
    /// <summary>
    /// IVersionService
    /// </summary>
    public interface IVersionService
    {
        /// <summary>
        /// ModuVersionle Service to get version list
        /// </summary>
        /// <returns></returns>
        Task<ResponseResultList<VersionsModel>> GetAllVersion(HttpContext httpContext, string ordering, string search, int offset, int pageSize, int pageNumber, bool all);

        /// <summary>
        /// Get Version By Id
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<VersionModel>> GetVersionById(long versionId);

        /// <summary>
        /// To Create new Version
        /// </summary>
        /// <param name="version">version object</param>
        /// <returns></returns>
        Task<ResponseResult<VersionsModel>> CreateVersion(VersionsModel version);

        /// <summary>
        /// To Update existing Version
        /// </summary>
        /// <param name="version">version object</param>
        /// /// <param name="versionId">Unique Version Id</param>
        /// <returns></returns>
        Task<ResponseResult<VersionsModel>> UpdateVersion(long versionId, VersionsModel version);

        /// <summary>
        /// To Update existing Version
        /// </summary>
        /// <param name="version">version object</param>
        /// /// <param name="versionId">Unique Version Id</param>
        /// <returns></returns>

        Task<ResponseResult<VersionsModel>> UpdatePartialVersion(long versionId, VersionsModel version);

        /// <summary>
        /// To Delete existing Version
        /// </summary>
        /// <param name="versionId">version identifier</param>
        /// <returns></returns>
        Task<ResponseResult<VersionsModel>> DeleteVersion(long versionId);








        ///// <summary>
        ///// Service to Get List of Version Modules by their unique id
        ///// </summary>
        ///// <param name="VersionId"> Version identifier</param>
        ///// <returns></returns>
        // Task<List<VersionInfo>> GetVersionModulesByVersionId(int VersionId);

        // /// <summary>
        // /// Service to get versions with all features(Modules, Sub Modules, and Permissions) for given Version Id
        // /// </summary>
        // /// <param name="versionId"></param>
        // /// <returns></returns>
        // Task<ResponseResult> GetVersionByVersionId(int versionId);

        // /// <summary>
        // ///  Service to get all versions with all features(Modules, Sub Modules, and Permissions)
        // /// </summary>
        // /// <returns></returns>
        // Task<ResponseResult> GetVersions();
    }
}
