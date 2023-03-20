using Common.Model;
using Sample.Admin.HttpAggregator.IServices;
using Sample.Admin.Model;
using Logger;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using Microsoft.AspNetCore.Authorization;

namespace Sample.Admin.HttpAggregator.Controllers
{
    /// <summary>
    /// Version Modules Controller
    /// </summary>
    [Route("api/versionModules")]
    [ApiController]
    [Authorize]
    public class VersionModuleController : BaseApiController
    {

        private readonly IVersionModuleService versionModulesService;

        /// <summary>
        /// Version Module Controller constructor to Inject dependency
        /// </summary>
        /// <param name="versionModulesService">module service </param>
        /// <param name="logger">logger service </param>
        public VersionModuleController(IVersionModuleService versionModulesService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(versionModulesService), versionModulesService);
            this.versionModulesService = versionModulesService;
        }

        /// <summary>
        ///  Information for all existing version modules
        /// </summary>
        /// <param name="ordering">Ordering Fields: (All Fields)</param>
        /// <param name="offset">The initial index from which to return the results.</param>
        /// <param name="pageSize">Number of results to return per page.</param>
        /// <param name="pageNumber">Page Number of results.</param>
        /// <param name="all">It will return results (all pages) without pagination.</param>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<VersionModulesModel>), 200)]
        public async Task<IActionResult> GetAllVersionModules(string ordering, int offset, int pageSize, int pageNumber, bool all = false)
        {

            return await Execute(async () =>
            {
                var result = await this.versionModulesService.GetAllVersionModules(ordering, offset, pageSize, pageNumber, all);
                if (result.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }

        /// <summary>
        ///  Information for versionmodule by id
        /// </summary>
        /// <param name="versionModuleId">A unique integer value identifying this Module.</param>
        /// <returns></returns>
        [Route("{versionModuleId}")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<VersionModulesModel>), 200)]
        public async Task<IActionResult> GetVersionModuleById([FromRoute] long versionModuleId)
        {
            return await Execute(async () =>
            {
                var result = await this.versionModulesService.GetVersionModuleById(versionModuleId);
                if (result.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(result);
                else
                    return BadRequest(result);
            });

        }

        /// <summary>
        /// This api is used for Creating new Version Module
        /// </summary>
        /// <param name="versionModule">The new version module object.</param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult), 200)]
        public async Task<IActionResult> CreateVersionModule([FromBody] VersionModulesModel versionModule)
        {

            return await Execute(async () =>
            {
                if (versionModule != null)
                {
                    var result = await this.versionModulesService.CreateVersionModule(versionModule);
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
        /// This api is used for Updating existing Version Module
        /// </summary>
        /// <param name="versionModule">The existing version module object.</param>
        /// <param name="versionModuleId">A unique integer value identifying this Version Module.</param>
        /// <returns></returns>
        [Route("{versionModuleId}")]
        [HttpPut]
        [ProducesResponseType(typeof(ResponseResult), 200)]
        public async Task<IActionResult> UpdateVersionModule([FromRoute] long versionModuleId, [FromBody] VersionModulesModel versionModule)
        {
            return await Execute(async () =>
            {
                if (versionModuleId != 0 && versionModule != null)
                {
                    var result = await this.versionModulesService.UpdateVersionModule(versionModuleId, versionModule);
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
        /// This api is used for Updating existing Version Module partially
        /// </summary>
        /// <param name="versionModule">The existing version module object.</param>
        /// <param name="versionModuleId">A unique integer value identifying this Version Module.</param>
        /// <returns></returns>
        [Route("{versionModuleId}")]
        [HttpPatch]
        [ProducesResponseType(typeof(ResponseResult), 200)]
        public async Task<IActionResult> UpdatePartialVersionModule([FromRoute] long versionModuleId, [FromBody] VersionModulesModel versionModule)
        {
            return await Execute(async () =>
            {
                if (versionModuleId != 0 && versionModule != null)
                {
                    var result = await this.versionModulesService.UpdatePartialVersionModule(versionModuleId, versionModule);
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
        /// This api is used for deleing Version Module
        /// </summary>
        /// <param name="versionModuleId">A unique integer value identifying this Version Module.</param>
        /// <returns></returns>
        [Route("{versionModuleId}")]
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseResult), 200)]
        public async Task<IActionResult> DeleteVersionModules(long versionModuleId)
        {

            return await Execute(async () =>
            {
                var result = await this.versionModulesService.DeleteVersionModule(versionModuleId);
                if (result != null)
                    return Ok(result);

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

    }
}
