using System.Threading.Tasks;
using Common.Model;
using Logger;
using Microsoft.AspNetCore.Mvc;
using Sample.Admin.HttpAggregator.IServices;
using Utilities;
using Sample.Admin.Model;
using Microsoft.AspNetCore.Authorization;

namespace Sample.Admin.HttpAggregator.Controllers
{
    /// <summary>
    /// Versions
    /// </summary>
    [Route("api/versions")]
    [ApiController]
    [Authorize]
    public class VersionController : BaseApiController
    {
        private readonly IVersionService versionService;

        /// <summary>
        /// Version Controller Constructor to inject services
        /// </summary>
        /// <param name="versionService">The Version Service</param>
        /// <param name="logger">The file logger</param>
        public VersionController(IVersionService versionService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(versionService), versionService);
            this.versionService = versionService;
        }

        /// <summary>
        ///  Information for all existing versions
        /// </summary>
        /// <param name="ordering">Ordering Fields: (All Fields)</param>
        /// <param name="search">Search Fields: (VersionId, Name, DisplayName)</param>
        /// <param name="offset">The initial index from which to return the results.</param>
        /// <param name="pageSize">Number of results to return per page.</param>
        /// <param name="pageNumber">Page Number of results.</param>
        /// <param name="all">It will return results (all pages) without pagination.</param>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResultList), 200)]
        public async Task<IActionResult> GetAllVersions([FromQuery]string ordering, [FromQuery] string search, [FromQuery] int offset, [FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] bool all = false)
        {
            return await Execute(async () =>
            {
                var versions = await versionService.GetAllVersion(HttpContext, ordering, search, offset, pageSize, pageNumber, all);
                if (versions.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(versions);
                else
                    return BadRequest(versions);
            });
        }

        /// <summary>
        ///  Information for version by id
        /// </summary>
        /// <param name="versionId">A unique integer value identifying this Version.</param>
        /// <returns></returns>
        [Route("{versionId}")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<VersionModel>), 200)]
        public async Task<IActionResult> GetVersionById([FromRoute] long versionId)
        {
            return await Execute(async () =>
            {
                var result = await this.versionService.GetVersionById(versionId);
                if (result.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }

        /// <summary>
        /// This api is used for Creating new Version
        /// </summary>
        /// <param name="version">The new version object.</param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult<VersionsModel>), 201)]
        public async Task<IActionResult> CreateVersion([FromBody] VersionsModel version)
        {
            return await Execute(async () =>
            {
                if (version != null)
                {
                    var result = await this.versionService.CreateVersion(version);
                    if (result != null)
                        return Created("api/version/", result);
                    else
                        return BadRequest(result);
                }

                return BadRequest(new ResponseResult<VersionsModel>
                {
                    ResponseCode = ResponseCode.ValidationFailed,
                    Message = ResponseMessage.NoRecordFound,
                    Error = new ErrorResponseResult
                    {
                        Message = ResponseMessage.ValidationFailed
                    }
                });

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
        [ProducesResponseType(typeof(ResponseResult), 200)]
        public async Task<IActionResult> UpdateVersion([FromRoute] long versionId, [FromBody] VersionsModel version)
        {
            return await Execute(async () =>
            {
                if (versionId != 0 && version != null)
                {
                    var result = await this.versionService.UpdateVersion(versionId, version);
                    if (result != null)
                        return Ok(result);
                    else
                        return BadRequest(result);
                }

                return BadRequest(new ResponseResult<VersionsModel>
                {
                    ResponseCode = ResponseCode.ValidationFailed,
                    Message = ResponseMessage.NoRecordFound,
                    Error = new ErrorResponseResult
                    {
                        Message = ResponseMessage.ValidationFailed
                    }
                });
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
        [ProducesResponseType(typeof(ResponseResult), 200)]
        public async Task<IActionResult> UpdatePartialVersion([FromRoute] long versionId, [FromBody] VersionsModel version)
        {
            return await Execute(async () =>
            {
                if (versionId != 0 && version != null)
                {
                    var result = await this.versionService.UpdatePartialVersion(versionId, version);
                    if (result != null)
                        return Ok(result);
                    else
                        return BadRequest(result);
                }

                return BadRequest(new ResponseResult
                {
                    ResponseCode = ResponseCode.ValidationFailed,
                    Message = ResponseMessage.NoRecordFound,
                    Error = new ErrorResponseResult
                    {
                        Message = ResponseMessage.NoRecordFound
                    }
                });
            });
        }


        /// <summary>
        /// This api is used for deleing Version
        /// </summary>
        /// <param name="versionId">A unique integer value identifying this Version.</param>
        /// <returns></returns>
        [Route("{versionId}")]
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseResult), 200)]
        public async Task<IActionResult> DeleteVersion([FromRoute] long versionId)
        {
            return await Execute(async () =>
            {
                var result = await this.versionService.DeleteVersion(versionId);

                if (result != null)
                    return Ok(result);

                return BadRequest(new ResponseResult
                {
                    ResponseCode = ResponseCode.NoRecordFound,
                    Message = ResponseMessage.NoRecordFound,
                    Error = new ErrorResponseResult
                    {
                        Message = ResponseMessage.NoRecordFound
                    }
                });
            });
        }       
    }
}
