using System.Threading.Tasks;
using Sample.Admin.Service.ServiceWorker;
using Sample.Admin.Model;
using Logger;
using Microsoft.AspNetCore.Mvc;
using Utilities;
using Sample.Admin.Service.Infrastructure.DataModels;

namespace Sample.Admin.API.Controllers
{
    [Route("api/accountServices")]
    [ApiController]
    public class AccountServiceController : BaseApiController
    {

        private readonly IAccountServiceService accountServicesService;


        /// <summary>
        /// Account Service Controller constructor to Inject dependency
        /// </summary>
        /// <param name="accountService"></param>
        public AccountServiceController(IAccountServiceService accountServicesService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(accountServicesService), accountServicesService);
            this.accountServicesService = accountServicesService;
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
        public async Task<IActionResult> GetAllAccountServices(string ordering, int offset, int pageSize, int pageNumber, bool all = false)
        {
            return await Execute(async () =>
            {
                var result = await this.accountServicesService.GetAllAccountServices(ordering, offset, pageSize, pageNumber, all);
                if (result != null)
                    return Ok(result);
                else
                    return BadRequest(result);
            });

        }

        /// <summary>
        ///  Information for accountService by id
        /// </summary>
        /// <param name="accountServiceId">A unique integer value identifying this AccountService.</param>
        /// <returns></returns>
        [Route("{accountServiceId}")]
        [HttpGet]
        public async Task<IActionResult> GetAccountServiceById([FromRoute] int accountServiceId)
        {
            return await Execute(async () =>
            {
                var result = await this.accountServicesService.GetAccountServiceById(accountServiceId);
                return Ok(result);
            });
        }

        /// <summary>
        /// This api is used for Creating new AccountService
        /// </summary>
        /// <param name="accountService">The new accountService object.</param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public async Task<IActionResult> AddAccountService([FromBody] AccountServicesModel accountService)
        {

            return await Execute(async () =>
            {
                var result = await this.accountServicesService.AddAccountService(accountService, loggedInUserId);
                return Ok(result);
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
        public async Task<IActionResult> UpdateAccountService([FromRoute] int accountServiceId, [FromBody] AccountServicesModel accountService)
        {
            return await Execute(async () =>
            {
                var result = await this.accountServicesService.UpdateAccountService(accountServiceId, accountService, loggedInUserId);
                return Ok(result);
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
        public async Task<IActionResult> UpdatePartialAccountService([FromRoute] int accountServiceId, [FromBody] AccountServicesModel accountService)
        {
            return await Execute(async () =>
            {
                var result = await this.accountServicesService.UpdatePartialAccountService(accountServiceId, accountService, loggedInUserId);
                return Ok(result);
            });

        }

        /// <summary>
        /// This api is used for deleing  AccountService
        /// </summary>
        /// <param name="accountServiceId">accountService identifier</param>
        /// <returns></returns>
        [Route("{accountServiceId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAccountService([FromRoute] int accountServiceId)
        {
            return await Execute(async () =>
            {
                var result = await this.accountServicesService.DeleteAccountService(accountServiceId);
                return Ok(result);
            });
        }

        // GET: api/<AccountServiceController>
        /// <summary>
        ///  Fetch  account service for all
        /// </summary>
        /// <returns></returns>
        [Route("getallaccountservices")]
        [HttpGet]
        public async Task<IActionResult> GetAllAccountService()
        {
            return await Execute(async () =>
                {
                    var result = await this.accountServicesService.GetAllAccountServices();
                    return Ok(result);
                });

        }


        // GET: api/<AccountServiceController>
        /// <summary>
        /// Get Account Service By their unique AccountId
        /// </summary>
        /// <param name="accountId">account identifier</param>
        /// <returns></returns>
        [Route("getaccountservicesbyaccountid/{accountId}")]
        [HttpGet]
        public async Task<IActionResult> GetAccountServiceByAccountId(long accountId)
        {

            return await Execute(async () =>
            {
                var result = await this.accountServicesService.GetAccountServicesByAccountId(accountId);
                return Ok(result);
            });


        }

        [HttpGet]
        [Route("GetAccountServiceByAccountId/{accountId}/{serviceId}")]
        //[HttpGet("")]
        public async Task<IActionResult> GetAccountServiceByAccountId([FromRoute] long accountId, [FromRoute] int serviceId)
        {
            return await Execute(async () =>
            {
                var result = await this.accountServicesService.GetAccountServiceByAccountId(accountId, serviceId);
                return Ok(result);
            });
        }

        // Post api/<AccountServiceController>/5
        /// <summary>
        /// This api is used for Creating accountService
        /// </summary>
        /// <param name="accountServices"> account service </param>
        /// <returns></returns>
        [Route("createaccountservice")]
        [HttpPost]
        public async Task<IActionResult> CreateAccountService(AccountServices accountServices)
        {

            return await Execute(async () =>
            {
                var result = await this.accountServicesService.CreateAccountService(accountServices, loggedInUserId);
                return Ok(result);
            });

        }
        
       
    }
}
