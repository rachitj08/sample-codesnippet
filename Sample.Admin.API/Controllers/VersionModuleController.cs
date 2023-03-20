using System.Threading.Tasks;
using Common.Model;
using Sample.Admin.Service.ServiceWorker;
using Sample.Admin.Model;
using Logger;
using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace Sample.Admin.API.Controllers
{
    /// <summary>
    /// Version Modules Controller
    /// </summary>
    [Route("api/versionModules")]
    [ApiController]
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
        public async Task<IActionResult> GetAllVersionModules(string ordering, int offset, int pageSize, int pageNumber, bool all = false)
        {

            return await Execute(async () =>
            {
                var result = await this.versionModulesService.GetAllVersionModules(ordering, offset, pageSize, pageNumber, all);
                if (result != null)
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
        public async Task<IActionResult> GetVersionModuleById([FromRoute] long versionModuleId)
        {
            return await Execute(async () =>
            {

                var result = await this.versionModulesService.GetVersionModuleById(versionModuleId);
                return Ok(result);

            });

        }

        /// <summary>
        /// Get Version Modules By VersionId
        /// </summary>
        /// <param name="versionId"> version identifier</param>
        /// <returns></returns>
        [Route("getversionmodulesbyversionid")]
        [HttpGet]
        public async Task<IActionResult> GetVersionModulesByVersionId(int versionId)
        {

            return await Execute(async () =>
            {
                var result = await this.versionModulesService.GetVersionModulesByVersionId(versionId);
                return Ok(result);
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

                var result = await this.versionModulesService.CreateVersionModule(versionModule);
                return Ok(result);

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
        public async Task<IActionResult> UpdateVersionModule([FromRoute] long versionModuleId, [FromBody] VersionModulesModel versionModule)
        {
            return await Execute(async () =>
            {

                var result = await this.versionModulesService.UpdateVersionModule(versionModuleId, versionModule);
                return Ok(result);

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
        
        public async Task<IActionResult> UpdatePartialVersionModule([FromRoute] long versionModuleId, [FromBody] VersionModulesModel versionModule)
        {
            return await Execute(async () =>
            {
                var result = await this.versionModulesService.UpdatePartialVersionModule(versionModuleId, versionModule);
                return Ok(result);
            });

        }

        /// <summary>
        /// This api is used for deleing Version Module
        /// </summary>
        /// <param name="versionModuleId">A unique integer value identifying this Version Module.</param>
        /// <returns></returns>
        [Route("{versionModuleId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteVersionModules(long versionModuleId)
        {

            return await Execute(async () =>
            {
                var result = await this.versionModulesService.DeleteVersionModules(versionModuleId);
                return Ok(result);
            });

        }

    }
}
