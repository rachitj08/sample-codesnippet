using Sample.Admin.Service.ServiceWorker;
using Sample.Admin.Model;
using Logger;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;

namespace Sample.Admin.API.Controllers
{
    [Route("api/modules")]
    [ApiController]
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
        public async Task<IActionResult> GetAllModules(string ordering, string search, int offset, int pageSize, int pageNumber,string serviceName, bool all = false)
        {
            return await Execute(async () =>
            {
                var result = await this.moduleService.GetAllModules(ordering, search, offset, pageSize, pageNumber, serviceName, all);
                if (result != null)
                    return Ok(result);
                else
                    return BadRequest(result);
            });

        }

        /// <summary>
        ///  Information for module by id
        /// </summary>
        /// <param name="moduleId">A unique integer value identifying this Module.</param>
        /// <returns></returns>
        [Route("{moduleId}")]
        [HttpGet]
        public async Task<IActionResult> GetModuleById([FromRoute] long moduleId)
        {
            return await Execute(async () =>
            {
                var result = await this.moduleService.GetModuleById(moduleId);
                return Ok(result);
            });
        }

        /// <summary>
        ///  Information for module by name
        /// </summary>
        /// <param name="accountId">account Id</param>
        /// <param name="serviceId">service Id</param>
        /// <param name="moduleName">module Name</param>
        /// <returns></returns>
        [HttpGet]
        [Route("getmodulebyname")]
        public async Task<IActionResult> GetModuleByName(long accountId, int serviceId, string moduleName)
        {
            return await Execute(async () =>
            {
                var result = await this.moduleService.GetModuleByName(accountId, serviceId, moduleName);
                return Ok(result);
            });
        }

        /// <summary>
        ///  Information for all existing modules
        /// </summary>
        /// <returns></returns>
        [Route("getmodules")]
        [HttpGet]
        public async Task<IActionResult> GetModules(bool isNavigationItem, long accountId, int serviceId)
        {
            return await Execute(async () =>
            {
                var result = await this.moduleService.GetModuleList(isNavigationItem, accountId, serviceId);
                return Ok(result);
            });
        }

        /// <summary>
        /// Get Sub Modules By their unique ModuleId
        /// </summary>
        /// <param name="moduleId">module identifier</param>
        /// <returns></returns>
        [Route("getsubmodulesbymoduleid")]
        [HttpGet]
        public async Task<IActionResult> GetSubModulesByModuleId(long moduleId)
        {

            return await Execute(async () =>
            {
                var result = await this.moduleService.GetSubModulesByModuleId(moduleId);
                return Ok(result);
            });


        }

        /// <summary>
        /// This api is used for Creating new Module
        /// </summary>
        /// <param name="module">The new module object.</param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public async Task<IActionResult> CreateModule([FromBody] ModulesModel module)
        {

            return await Execute(async () =>
            {
                var result = await this.moduleService.CreateModule(module, loggedInUserId);
                return Ok(result);
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
        public async Task<IActionResult> UpdateModule([FromRoute] long moduleId, [FromBody] ModulesModel module)
        {
            return await Execute(async () =>
            {
                var result = await this.moduleService.UpdateModule(moduleId, module, loggedInUserId);
                return Ok(result);
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
        public async Task<IActionResult> UpdatePartialModule([FromRoute] long moduleId, [FromBody] ModulesModel module)
        {
            return await Execute(async () =>
            {
                var result = await this.moduleService.UpdatePartialModule(moduleId, module, loggedInUserId);
                return Ok(result);
            });

        }

        /// <summary>
        /// This api is used for deleing  Module
        /// </summary>
        /// <param name="moduleId">module identifier</param>
        /// <returns></returns>
        [Route("{moduleId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteModule([FromRoute] long moduleId)
        {
            return await Execute(async () =>
            {
                var result = await this.moduleService.DeleteModule(moduleId);
                return Ok(result);
            });
        }


        /// <summary>
        /// This api is used for get all Module for service
        /// </summary>
        /// <param name="serviceName">Service Name</param>
        /// <returns></returns>
        [Route("all")]
        [HttpGet]
        public async Task<IActionResult> GetModulesForService([FromQuery] string serviceName)
        {
            return await Execute(async () =>
            {
                var result = await this.moduleService.GetModulesForService(serviceName);
                return Ok(result);
            });
        }

        [Route("{accountId}/{isNavigationItem}/getmodulesbyaccountid")]
        [HttpGet]
        public async Task<IActionResult> GetModulesByAccountId([FromRoute] long accountId, bool isNavigationItem = true)
        {
            return await Execute(async () =>
            {
                var result = await this.moduleService.GetModulesByAccountId(accountId, isNavigationItem);
                return Ok(result);
            });
        }
    }
}
