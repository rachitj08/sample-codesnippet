using Common.Model;
using Logger;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;
using Sample.Admin.Service.ServiceWorker;

namespace Sample.Admin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationConfigController : BaseApiController
    {
        private readonly IApplicationConfigService applicationConfigService;
        public ApplicationConfigController(IApplicationConfigService applicationConfigService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(applicationConfigService), applicationConfigService);
            this.applicationConfigService = applicationConfigService;
        }

        /// <summary>
        /// Get Application Config Details
        /// </summary>
        /// <returns></returns>
        //[Route("application")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await Execute(async () =>
            {
                var result = await this.applicationConfigService.GetApplicationConfig();
                if (result == null)
                {
                    return BadRequest(new ResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                        ResponseCode = ResponseCode.InternalServerError
                    });
                }
                return Ok(result);
            });
        }
    }
}
