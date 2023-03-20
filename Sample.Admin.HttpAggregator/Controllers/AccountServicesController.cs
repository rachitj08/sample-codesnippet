using Sample.Admin.HttpAggregator.IServices;
using Common.Model;
using Sample.Admin.Model;
using Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;

namespace Sample.Admin.HttpAggregator.Controllers
{
    /// <summary>
    /// AccountServices Controller
    /// </summary>
    [Route("api/accountServices")]
    [ApiController]
    [Authorize]
    public class AccountServiceController : BaseApiController
    {

        private readonly IAccountServiceService accountServiceService;

        /// <summary>
        /// AccountService Controller constructor to Inject dependency
        /// </summary>
        /// <param name="accountServiceService">accountService service </param>
        /// <param name="logger">logger service </param>
        public AccountServiceController(IAccountServiceService accountServiceService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(accountServiceService), accountServiceService);
            this.accountServiceService = accountServiceService;
        }

        /// <summary>
        ///  Information for all existing accountServices
        /// </summary>
        /// <param name="ordering">Ordering Fields: (All Fields)</param>
        /// <param name="offset">The initial index from which to return the results.</param>
        /// <param name="pageSize">Number of results to return per page.</param>
        /// <param name="pageNumber">Page Number of results.</param>
        /// <param name="all">It will return results (all pages) without pagination.</param>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResultList), 200)]
        public async Task<IActionResult> GetAllAccountServices([FromQuery]string ordering, [FromQuery] int offset, [FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] bool all = false)
        {
            return await Execute(async () =>
            {
                var accountServices = await accountServiceService.GetAllAccountServices(HttpContext,ordering, offset,pageSize,pageNumber,all);
                if (accountServices.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(accountServices);
                else
                    return BadRequest(accountServices);
            });

        }

        /// <summary>
        ///  Information for accountService by id
        /// </summary>
        /// <param name="accountServiceId">A unique integer value identifying this AccountService.</param>
        /// <returns></returns>
        [Route("{accountServiceId}")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<AccountServicesModel>), 200)]
        public async Task<IActionResult> GetAccountServiceById([FromRoute] int accountServiceId)
        {
            return await Execute(async () =>
            {
                    var result = await this.accountServiceService.GetAccountServiceById(accountServiceId);
                    if (result.ResponseCode == ResponseCode.RecordFetched)
                        return Ok(result);
                    else
                        return BadRequest(result);
            });
        }

        /// <summary>
        /// This api is used for Creating new AccountService
        /// </summary>
        /// <param name="accountService">The new accountService object.</param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult<AccountServicesModel>), 201)]
        public async Task<IActionResult> CreateAccountService([FromBody] AccountServicesModel accountService)
        {
            return await Execute(async () =>
            {
                if (accountService != null)
                {
                    var result = await this.accountServiceService.AddAccountService(accountService);
                    if (result != null && result.ResponseCode == ResponseCode.RecordSaved)
                        return Created("api/accountServices/", result);
                    else
                        return BadRequest(result);
                }

                return BadRequest(new ResponseResult<AccountServicesModel>
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
        /// This api is used for Updating existing AccountService
        /// </summary>
        /// <param name="accountService">The existing accountService object.</param>
        /// <param name="accountServiceId">A unique integer value identifying this AccountService.</param>
        /// <returns></returns>
        [Route("{accountServiceId}")]
        [HttpPut]
        [ProducesResponseType(typeof(ResponseResult), 200)]
        public async Task<IActionResult> UpdateAccountService([FromRoute] int accountServiceId, [FromBody] AccountServicesModel accountService)
        {
            return await Execute(async () =>
            {
                if (accountServiceId != 0 && accountService != null)
                {
                    var result = await this.accountServiceService.UpdateAccountService(accountServiceId, accountService);
                    if (result != null)
                        return Ok(result);
                    else
                        return BadRequest(result);
                }

                return BadRequest(new ResponseResult<AccountServicesModel>
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
        /// This api is used for Updating existing AccountService partially
        /// </summary>
        /// <param name="accountService">The existing accountService object.</param>
        /// <param name="accountServiceId">A unique integer value identifying this AccountService.</param>
        /// <returns></returns>
        [Route("{accountServiceId}")]
        [HttpPatch]
        [ProducesResponseType(typeof(ResponseResult), 200)]
        public async Task<IActionResult> UpdatePartialAccountService([FromRoute] int accountServiceId, [FromBody] AccountServicesModel accountService)
        {
            return await Execute(async () =>
            {
                if (accountServiceId != 0 && accountService != null)
                {
                    var result = await this.accountServiceService.UpdatePartialAccountService(accountServiceId, accountService);
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
        /// This api is used for deleing  AccountService
        /// </summary>
        /// <param name="accountServiceId">A unique integer value identifying this AccountService.</param>
        /// <returns></returns>
        [Route("{accountServiceId}")]
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseResult), 200)]
        public async Task<IActionResult> DeleteAccountService([FromRoute] int accountServiceId)
        {

            return await Execute(async () =>
            {
                var result = await this.accountServiceService.DeleteAccountService(accountServiceId);

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
