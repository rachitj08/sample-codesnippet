using Sample.Admin.Service.ServiceWorker;
using Sample.Admin.Model;
using Logger;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;

namespace Sample.Admin.API.Controllers
{
    [Route("api/versions")]
    [ApiController]
    public class VersionController : BaseApiController
    {
        private readonly IVersionService versionService;

        /// <summary>
        /// Version Controller constructor to Inject dependency
        /// </summary>
        /// <param name="versionService">service for version</param>
        /// <param name="logger">logger for logging into file </param>
        public VersionController(IVersionService versionService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(versionService), versionService);
            this.versionService = versionService;
        }

        /// <summary>
        ///  Information for all existing versions
        /// </summary>
        /// <param name="ordering">Ordering Fields: (All Fields)</param>
        /// <param name="search">Search Fields: (ModuleId, Name, DisplayName)</param>
        /// <param name="offset">The initial index from which to return the results.</param>
        /// <param name="pageSize">Number of results to return per page.</param>
        /// <param name="pageNumber">Page Number of results.</param>
        /// <param name="all">It will return results (all pages) without pagination.</param>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetAllVersions(string ordering, string search, int offset, int pageSize, int pageNumber, bool all = false)
        {
            return await Execute(async () =>
            {
                var result = await this.versionService.GetAllVersions(ordering, search, offset, pageSize, pageNumber, all);
                if (result != null)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }

        /// <summary>
        ///  Information for verion by id
        /// </summary>
        /// <param name="versionId">A unique integer value identifying this version.</param>
        /// <returns></returns>
        [Route("{versionId}")]
        [HttpGet]
        public async Task<IActionResult> GetVersionById([FromRoute] int versionId)
        {
            return await Execute(async () =>
            {
                var result = await this.versionService.GetVersionById(versionId);
                return Ok(result);
            });
        }

        /// <summary>
        /// This api is used for Creating new Version
        /// </summary>
        /// <param name="version">The new version object.</param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public async Task<IActionResult> CreateVersion([FromBody] VersionsModel version)
        {
            return await Execute(async () =>
            {
                var result = await this.versionService.CreateVersion(version, loggedInUserId);
                return Ok(result);
            });
        }

        /// <summary>
        /// This api is used for Updating existing Version
        /// </summary>
        /// <param name="version">The existing version object.</param>
        /// <param name="versionId">A unique integer value identifying this Version.</param>
        /// <returns></returns>
        [Route("{versionId}")]
        [HttpPut]
        public async Task<IActionResult> UpdateVersion([FromRoute] long versionId, [FromBody] VersionsModel version)
        {
            return await Execute(async () =>
            {
                var result = await this.versionService.UpdateVersion(versionId, version, loggedInUserId);
                return Ok(result);
            });
        }

        /// <summary>
        /// This api is used for Updating existing Version partially
        /// </summary>
        /// <param name="version">The existing version object.</param>
        /// <param name="versionId">A unique integer value identifying this Version.</param>
        /// <returns></returns>
        [Route("{versionId}")]
        [HttpPatch]
        public async Task<IActionResult> UpdatePartialVersion([FromRoute] long versionId, [FromBody] VersionsModel version)
        {
            return await Execute(async () =>
            {
                var result = await this.versionService.UpdatePartialVersion(versionId, version, loggedInUserId);
                return Ok(result);
            });
        }

        /// <summary>
        /// This api is used for deleing Version
        /// </summary>
        /// <param name="versionId">version identifier</param>
        /// <returns></returns>
        [Route("{versionId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteVersion([FromRoute] long versionId)
        {
            return await Execute(async () =>
            {
                var result = await this.versionService.DeleteVersion(versionId);
                return Ok(result);
            });
        }



        






















        /// <summary>
        /// Get Version Modules By VersionId
        /// </summary>
        /// <returns></returns>
        //[Route("getversionmodulesbyversionid")]
        //[HttpGet]
        //public async Task<IActionResult> GetVersionModulesByVersionId(int? versionId = 0)
        //{

        //    return await Execute(async () =>
        //    {
        //        var result = await this.versionService.GetVersionModulesByVersionId(versionId);
        //        return Ok(result);
        //    });

        //}


        /// <summary>
        ///  Get All Versions With All Permisssions given
        /// </summary>
        /// <returns></returns>
        //[Route("getallversionswithallpermission/{VersionId:int=0}")]
        //[HttpGet]
        //public async Task<IActionResult> GetAllVersionsWithAllPermission(int? VersionId = 0)
        //{

        //    return await Execute(async () =>
        //    {
        //        var result = await this.versionService.GetAllVersionsWithAllPermission(VersionId);
        //        return Ok(result);
        //    });

        //}

        /// <summary>
        /// This api is used for Creating Version
        /// </summary>
        /// <param name="version">new version object</param>
        /// <returns></returns>
        //[Route("createversion")]
        //[HttpPost]
        //public async Task<IActionResult> CreateVersion([FromBody] Sample.Admin.Service.Infrastructure.DataModels.Version version)
        //{

        //    return await Execute(async () =>
        //    {
        //        var result = await this.versionService.CreateVersion(version);
        //        return Ok(result);
        //    });

        //}


        /// <summary>
        /// This api is used for Updating Version
        /// </summary>
        /// <param name="version">new version object</param>
        /// <returns></returns>
        //[Route("updateversion")]
        //[HttpPut]
        //public async Task<IActionResult> UpdateVersion([FromBody] Sample.Admin.Service.Infrastructure.DataModels.Version version)
        //{

        //    return await Execute(async () =>
        //    {
        //        var result = await this.versionService.UpdateVersion(version);
        //        return Ok(result);
        //    });

        //}


        /// <summary>
        /// This api is used for deleting Version
        /// </summary>
        /// <param name="versionId">version identifier</param>
        /// <returns></returns>
        //[Route("deleteversion")]
        //[HttpDelete]
        //public async Task<IActionResult> DeleteVersion(int versionId)
        //{

        //    return await Execute(async () =>
        //    {
        //        var result = await this.versionService.DeleteVersion(versionId);
        //        return Ok(result);
        //    });

        //}

        ///// <summary>
        ///// Get version with all features(Modules, Sub Modules, and Permissions) for given VersionId
        ///// </summary>
        ///// <param name="versionId">The VersionId</param>
        ///// <returns></returns>
        //[Route("GetVersionByVersionId")]
        //[HttpGet]
        //public async Task<IActionResult> GetVersionByVersionId(int versionId = 0)
        //{
        //    return await Execute(async () =>
        //    {
        //        var version = await this.versionService.GetVersionByVersionId(versionId);
        //        if (version == null)
        //        {
        //            return BadRequest(new ResponseResult()
        //            {
        //                Message = ResponseMessage.InternalServerError,
        //                ResponseCode = ResponseCode.InternalServerError
        //            });
        //        }
        //        return Ok(version);
        //    });
        //}

        /// <summary>
        /// Get all versions with all features(Modules, Sub Modules, and Permissions) 
        /// </summary>
        /// <returns></returns>
        //[Route("GetVersions")]
        //[HttpGet]
        //public async Task<IActionResult> GetVersions()
        //{
        //    return await Execute(async () =>
        //    {
        //        var versions = await this.versionService.GetVersions();
        //        if (versions == null)
        //        {
        //            return BadRequest(new ResponseResult()
        //            {
        //                Message = ResponseMessage.InternalServerError,
        //                ResponseCode = ResponseCode.InternalServerError
        //            });
        //        }
        //        return Ok(versions);
        //    });

        //}

    }
}
