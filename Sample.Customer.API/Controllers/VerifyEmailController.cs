using Common.Model;
using Logger;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.API.Controllers;
using Sample.Customer.Model;
using Sample.Customer.Service.ServiceWorker;

namespace Sample.Customer.HttpAggregator.Controllers.UserManagement
{
   
  [Route("api/[controller]")]
    [ApiController]
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
      /// <param name="model"></param>
      /// <returns></returns>
        [Route("verify")]
        [HttpPost]
        public async Task<IActionResult> Verify([FromBody] VerifyEmailVM model)
        {
            
            return await Execute(async () =>
            {
                var result = await this.verifyEmailService.ConfirmEmail(model,loggedInAccountId,loggedInUserId);
                return Ok(result);
            });
           
        }
    }
}
