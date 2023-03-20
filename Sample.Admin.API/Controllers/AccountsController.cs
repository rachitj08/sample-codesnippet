using Common.Model;
using Logger;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;
using Sample.Admin.Model;
using Sample.Admin.Model.Account.New;
using Sample.Admin.Service.ServiceWorker;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sample.Admin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseApiController
    {

        private readonly IAccountService accountsService;

        /// <summary>
        /// Account Controller constructor to Inject dependency
        /// </summary>
        /// <param name="accountService">Account Service </param>
        /// <param name="logger">Logger for file</param>
        public AccountsController(IAccountService accountsService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(accountsService), accountsService);
            this.accountsService = accountsService;
        }

        /// <summary>
        ///  Information for all existing accounts
        /// </summary>
        /// <param name="ordering">Ordering Fields: (All Fields)</param>
        /// <param name="search">Search Fields: (AccountId, Name, DisplayName)</param>
        /// <param name="offset">The initial index from which to return the results.</param>
        /// <param name="pageSize">Number of results to return per page.</param>
        /// <param name="pageNumber">Page Number of results.</param>
        /// <param name="all">It will return results (all pages) without pagination.</param>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetAllAccounts(string ordering, string search, int offset, int pageSize, int pageNumber, bool all = false)
        {
            return await Execute(async () =>
            {
                var result = await this.accountsService.GetAllAccounts(ordering, search, offset, pageSize, pageNumber, all);
                if (result != null)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }

        /// <summary>
        ///  Information for account by id
        /// </summary>
        /// <param name="accountId">A unique integer value identifying this Account.</param>
        /// <returns></returns>
        [Route("{accountId}")]
        [HttpGet]
        public async Task<IActionResult> GetAccountById([FromRoute] long accountId)
        {
            return await Execute(async () =>
            {
                var result = await this.accountsService.GetAccountById(accountId);
                return Ok(result);
            });
        }

        /// <summary>
        /// This api is used for Creating new Account
        /// </summary>
        /// <param name="account">The new account object.</param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public async Task<IActionResult> AddAccount([FromBody] AccountViewModel account)
        {

            return await Execute(async () =>
            {
                var result = await this.accountsService.AddAccount(account, loggedInUserId);
                return Ok(result);
            });

        }

        /// <summary>
        /// To Create New Account
        /// </summary>
        /// <param name="account"> The New Account object</param>
        /// <returns></returns>
        [Route("createaccount")]
        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] NewAccount account)
        {
            Check.Argument.IsNotNull(nameof(account), account);
            return await Execute(async () =>
            {
                var accounts = await this.accountsService.CreateAccount(account, loggedInUserId);
                if (accounts == null)
                {
                    return BadRequest(new ResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                        ResponseCode = ResponseCode.InternalServerError
                    });
                }
                return Ok(accounts);

            });
        }

        /// <summary>
        /// This api is used for Updating existing Account
        /// </summary>
        /// <param name="account">The existing account object.</param>
        /// <param name="accountId">A unique integer value identifying this Account.</param>
        /// <returns></returns>
        [Route("{accountId}")]
        [HttpPut]
        public async Task<IActionResult> UpdateAccount([FromRoute] long accountId, [FromBody] AccountViewModel account)
        {
            return await Execute(async () =>
            {
                var result = await this.accountsService.UpdateAccount(accountId, account, loggedInUserId);
                return Ok(result);
            });

        }

        /// <summary>
        /// This api is used for Updating existing Account partially
        /// </summary>
        /// <param name="account">The existing account object.</param>
        /// <param name="accountId">A unique integer value identifying this Account.</param>
        /// <returns></returns>
        [Route("{accountId}")]
        [HttpPatch]
        public async Task<IActionResult> UpdatePartialAccount([FromRoute] long accountId, [FromBody] AccountViewModel account)
        {
            return await Execute(async () =>
            {
                var result = await this.accountsService.UpdatePartialAccount(accountId, account, loggedInUserId);
                return Ok(result);
            });

        }

        /// <summary>
        /// This api is used for deleing  Account
        /// </summary>
        /// <param name="accountId">account identifier</param>
        /// <returns></returns>
        [Route("{accountId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAccount([FromRoute] long accountId)
        {
            return await Execute(async () =>
            {
                var result = await this.accountsService.DeleteAccount(accountId);
                return Ok(result);
            });
        }

        /// <summary>
        /// Get Account For TenantResolver
        /// </summary>
        /// <param name="accountIdentity">The accountIdentity to get account</param>
        /// <returns></returns>
        [Route("getaccountfortenantresolver")]
        [HttpGet]
        public async Task<IActionResult> GetAccountForTenantResolver(string accountIdentity)
        {
            return await Execute(async () =>
            {
                var result = await this.accountsService.GetAccountForTenantResolver(accountIdentity);
                return Ok(result);
            });
        }

        /// <summary>
        /// Get Account Details
        /// </summary>
        /// <param name="accountIdentity">The account Identity to get account</param>
        /// <returns></returns>
        [Route("getaccountdetails")]
        [HttpGet]
        public async Task<IActionResult> GetAccountDetails(string accountIdentity)
        {
            return await Execute(async () =>
            {
                var result = await this.accountsService.GetAccountDetails(accountIdentity);
                return Ok(result);
            });
        }

        /// <summary>
        /// Get Account all Details
        /// </summary>
        /// <param name="accountId">The accountId to get account</param>
        /// <returns></returns>
        [Route("getaccount")]
        [HttpGet]
        public async Task<IActionResult> GetAccount(long accountId)
        {
            return await Execute(async () =>
            {
                var result = await this.accountsService.GetAccount(accountId);
                return Ok(result);
            });
        }

        /// <summary>
        /// Get Account List
        /// </summary>
        /// <returns></returns>
        [Route("getaccounts")]
        [HttpGet]
        public async Task<IActionResult> GetAccountList()
        {
            return await Execute(async () =>
            {
                var result = await this.accountsService.GetAccountList();
                return Ok(result);
            });
        }
    }
}
