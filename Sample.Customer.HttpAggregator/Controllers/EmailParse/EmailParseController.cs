using Common.Model;
using Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.HttpAggregator.IServices.EmailParse;
using Sample.Customer.Model;

namespace Sample.Customer.HttpAggregator.Controllers.EmailParse
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/EmailParse")]
    [ApiController]
    public class EmailParseController : BaseApiController
    {
        private readonly IEmailParserService emailParserService;
        /// <summary>
        /// Email Parse
        /// </summary>
        /// <param name="emailParserService"></param>
        /// <param name="logger"></param>
        public EmailParseController(IEmailParserService emailParserService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(emailParserService), emailParserService);

            this.emailParserService = emailParserService;
        }

        /// <summary>
        /// Email Parser Call Back Request
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("emailparsercallback")]
        [ProducesResponseType(typeof(ResponseResult<bool>), 200)]
        public async Task<IActionResult> EmailParserCallBack([FromBody] EmailParserCallBackResponse model)
        {
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + DateTime.Now.Ticks.ToString() + "emailparsercallback.txt", JsonConvert.SerializeObject(model));
            Check.Argument.IsNotNull(nameof(emailParserService), emailParserService);

            var authorizationKey = "";
            if (Request.Headers.ContainsKey("Authorization"))
            {
                authorizationKey = Convert.ToString(HttpContext.Request.Headers["Authorization"]);
            }

            return await Execute(async () =>
            {
                var result = await emailParserService.EmailParserCallBack(model, authorizationKey, headerAccountId);
                if (result.ResponseCode == ResponseCode.RecordSaved)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            });
        }


        /// <summary>
        /// process all pending emails
        /// </summary> 
        /// <returns></returns>
        [HttpPost]
        [Route("processallpendingemail")]
        [ProducesResponseType(typeof(ResponseResult<bool>), 200)]
        public async Task<IActionResult> ProcessAllPendingEmail()
        {
            //Check.Argument.IsNotNull(nameof(emailParserService), emailParserService);

            var authorizationKey = "";
            if (Request.Headers.ContainsKey("Authorization"))
            {
                authorizationKey = Convert.ToString(HttpContext.Request.Headers["Authorization"]);
            }

            return await Execute(async () =>
            {
                var result = await emailParserService.ProcessAllPendingEmail(authorizationKey, headerAccountId);
                if (result.ResponseCode == ResponseCode.RecordFetched)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            });
        }

    }
}
