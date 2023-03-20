using Common.Model;
using Logger;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.HttpAggregator.IServices.UserManagement;
using Sample.Customer.Model;

namespace Sample.Customer.HttpAggregator.Controllers.UserManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class VerifyEmailController : BaseApiController
    {
        private readonly IVerifyEmailService verifyEmailService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="verifyEmailService"></param>
        /// <param name="logger"></param>
        public VerifyEmailController(IVerifyEmailService verifyEmailService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(verifyEmailService), verifyEmailService);
            this.verifyEmailService = verifyEmailService;
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="token"></param>
       /// <param name="uid"></param>
       /// <returns></returns>
        [Route("confirmemail")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<SuccessMessageModel>), 200)]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token, [FromQuery] string uid)
        {

            return await Execute(async () =>
            {
                var result = await this.verifyEmailService.VerifyEmail(token, uid);

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
