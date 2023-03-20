using Logger;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.Service.ServiceWorker;

namespace Sample.Customer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailParserCallbackController : BaseApiController
    {
        private readonly IEmailParserCallBackService emailParserCallBackService;

        /// <summary>
        /// UserVehiclePreference Controller constructor to Inject dependency
        /// </summary>
        /// <param name="userService">user service class</param>
        public EmailParserCallbackController(IEmailParserCallBackService emailParserCallBackService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(emailParserCallBackService), emailParserCallBackService);

            this.emailParserCallBackService = emailParserCallBackService;
        }
         

        [HttpGet]
        [Route("getdetailsfrommessageid/{messageId}")]
        public async Task<IActionResult> GetDetailsFromMessageId([FromRoute]Guid messageId)
        {
            return await Execute(async () =>
            {
                var result = await this.emailParserCallBackService.GetDetailsFromMessageId(messageId, loggedInAccountId);
                return Ok(result);
            });
        }  
        
        [HttpGet]
        [Route("getallpendingemaildetails")]
        public async Task<IActionResult> GetAllPendingEmailDetails()
        {
            return await Execute(async () =>
            {
                var result = await this.emailParserCallBackService.GetAllEmailParseDetails(loggedInAccountId);
                return Ok(result);
            });
        }
    }
}
