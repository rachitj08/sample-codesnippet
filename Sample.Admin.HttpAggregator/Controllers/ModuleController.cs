using Sample.Admin.HttpAggregator.IServices;
using Common.Model;
using Sample.Admin.Model;
using Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;

namespace Sample.Admin.HttpAggregator.Controllers
{
    /// <summary>
    /// Modules Controller
    /// </summary>
    [Route("api/modules")]
    [ApiController]
    [Authorize]
    public class ModuleController : BaseApiController
    {

        private readonly IModuleService moduleService;

        /// <summary>
        /// Module Controller constructor to Inject dependency
        /// </summary>
        /// <param name="moduleService">module service </param>
        /// <param name="logger">logger service </param>
        public ModuleController(IModuleService moduleService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(moduleService), moduleService);
            this.moduleService = moduleService;
        }

        /// <summary>
        ///  Information for all existing modules
        /// </summary>
        /// <param name="ordering">Ordering Fields: (All Fields)</param>
        /// <param name="search">Search Fields: (ModuleId, Name, DisplayName)</param>
        /// <param name="offset">The initial index from which to return the results.</param>
        /// <param name="pageSize">Number of results to return per page.</param>
        /// <param name="pageNumber">Page Number of results.</param>
        ///  <param name="serviceName">Service Name of results.</param>
        /// <param name="all">It will return results (all pages) without pagination.</param>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResultList), 200)]
        public async Task<IActionResult> GetAllModules([FromQuery]string ordering, [FromQuery] string search, [FromQuery] int offset, [FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] string serviceName, [FromQuery] bool all)
        {
            return await Execute(async () =>
            {
                var modules = await moduleService.GetAllModules(HttpContext, ordering, search,offset,pageSize,pageNumber, serviceName, all);
                if (modules.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(modules);
                else
                    return BadRequest(modules);
            });

        }

        /// <summary>
        ///  Information for all existing modules
        /// </summary> 
        /// <param name="serviceName">Service Name of results.</param> 
        /// <returns></returns>
        [Route("all")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<List<ModulesAllModel>>), 200)]
        public async Task<IActionResult> GetModulesForService([FromQuery] string serviceName)
        {
            return await Execute(async () =>
            {
                var modules = await moduleService.GetModulesForService(serviceName);
                if (modules.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(modules);
                else
                    return BadRequest(modules);
            });

        }

        /// <summary>
        ///  Information for module by id
        /// </summary>
        /// <param name="moduleId">A unique integer value identifying this Module.</param>
        /// <returns></returns>
        [Route("{moduleId}")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<ModulesModel>), 200)]
        public async Task<IActionResult> GetModuleById([FromRoute] long moduleId)
        {
            return await Execute(async () =>
            {
                    var result = await this.moduleService.GetModuleById(moduleId);
                    if (result.ResponseCode == ResponseCode.RecordFetched)
                        return Ok(result);
                    else
                        return BadRequest(result);
            });
        }

        /// <summary>
        /// This api is used for Creating new Module
        /// </summary>
        /// <param name="module">The new module object.</param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult<ModulesModel>), 201)]
        public async Task<IActionResult> CreateModule([FromBody] ModulesModel module)
        {

            return await Execute(async () =>
            {
                if (module != null)
                {
                    var result = await this.moduleService.CreateModule(module);
                    if (result != null)
                        return Created("api/modules/", result);
                    else
                        return BadRequest(result);
                }

                return BadRequest(new ResponseResult<ModulesModel>
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
        /// This api is used for Updating existing Module
        /// </summary>
        /// <param name="module">The existing module object.</param>
        /// <param name="moduleId">A unique integer value identifying this Module.</param>
        /// <returns></returns>
        [Route("{moduleId}")]
        [HttpPut]
        [ProducesResponseType(typeof(ResponseResult), 200)]
        public async Task<IActionResult> UpdateModule([FromRoute] long moduleId, [FromBody] ModulesModel module)
        {
            return await Execute(async () =>
            {
                if (moduleId != 0 && module != null)
                {
                    var result = await this.moduleService.UpdateModule(moduleId, module);
                    if (result != null)
                        return Ok(result);
                    else
                        return BadRequest(result);
                }

                return BadRequest(new ResponseResult<ModulesModel>
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
        /// This api is used for Updating existing Module partially
        /// </summary>
        /// <param name="module">The existing module object.</param>
        /// <param name="moduleId">A unique integer value identifying this Module.</param>
        /// <returns></returns>
        [Route("{moduleId}")]
        [HttpPatch]
        [ProducesResponseType(typeof(ResponseResult), 200)]
        public async Task<IActionResult> UpdatePartialModule([FromRoute] long moduleId, [FromBody] ModulesModel module)
        {
            return await Execute(async () =>
            {
                if (moduleId != 0 && module != null)
                {
                    var result = await this.moduleService.UpdatePartialModule(moduleId, module);
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
        /// This api is used for deleing  Module
        /// </summary>
        /// <param name="moduleId">A unique integer value identifying this Module.</param>
        /// <returns></returns>
        [Route("{moduleId}")]
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseResult), 200)]
        public async Task<IActionResult> DeleteModule([FromRoute] long moduleId)
        {

            return await Execute(async () =>
            {
                var result = await this.moduleService.DeleteModule(moduleId);

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

        ///// <summary>
        ///// Build Navigation
        ///// </summary>
        ///// <returns></returns>
        //[Route("buildNavigation")]
        //[ProducesResponseType(typeof(ResponseResult<ModuleNavigationModel>), 200)]
        //[HttpGet]
        //public async Task<IActionResult> BuildUserNavigationByUserId()
        //{
        //    return await Execute(async () =>
        //    {
        //        var navigations = await moduleService.BuildNavigation();
        //        if (navigations.ResponseCode == ResponseCode.RecordFetched)
        //            return Ok(navigations);
        //        else
        //            return BadRequest(navigations);
        //    });
        //}

    }
}
