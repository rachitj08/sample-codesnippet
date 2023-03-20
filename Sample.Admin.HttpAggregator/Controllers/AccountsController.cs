using System;
using System.Threading.Tasks;
using Common.Model;
using Sample.Admin.Model.Account.New;
using Logger;
using Microsoft.AspNetCore.Mvc;
using Sample.Admin.HttpAggregator.IServices;
using Utilities;
using System.Collections.Generic;
using Sample.Admin.Model.Account.Domain;
using Sample.Admin.Model;
using Microsoft.AspNetCore.Authorization;

namespace Sample.Admin.HttpAggregator.Controllers
{
    /// <summary>
    /// Account Controller
    /// </summary>
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : BaseApiController
    {
        private readonly IAccountService accountsService;

        /// <summary>
        /// Account Controller constructor
        /// </summary>
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
        [ProducesResponseType(typeof(ResponseResultList), 200)]
        [Authorize]
        public async Task<IActionResult> GetAllAccounts([FromQuery] string ordering, [FromQuery] string search, [FromQuery] int offset, [FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] bool all = false)
        {
            return await Execute(async () =>
            {
                var accounts = await accountsService.GetAllAccounts(HttpContext, ordering, search, offset, pageSize, pageNumber, all);
                if (accounts.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(accounts);
                else
                    return BadRequest(accounts);
            });

        }

        /// <summary>
        ///  Information for account by id
        /// </summary>
        /// <param name="accountId">A unique integer value identifying this Account.</param>
        /// <returns></returns>
        [Route("{accountId}")]
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ResponseResult<AccountViewModel>), 200)]
        public async Task<IActionResult> GetAccountById([FromRoute] long accountId)
        {
            return await Execute(async () =>
            {
                var result = await this.accountsService.GetAccountById(accountId);
                if (result != null)
                {
                    if (result.ResponseCode == ResponseCode.RecordFetched)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest(result);
                    }
                }
                else
                {
                    return BadRequest(new ResponseResult<AccountViewModel>()
                    {
                        ResponseCode = ResponseCode.InternalServerError,
                        Message = ResponseMessage.InternalServerError,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.InternalServerError
                        }
                    });
                }
            });
        }

        /// <summary>
        /// This api is used for Creating new Account
        /// </summary>
        /// <param name="account">The new account object.</param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult<AccountsCreateVM>), 201)]
        public async Task<IActionResult> AddAccount([FromBody] AccountViewModel account)
        {
            return await Execute(async () =>
            {
                if (account != null)
                {
                    var result = await this.accountsService.AddAccount(account);
                    if (result != null)
                    {
                        if (result.ResponseCode == ResponseCode.RecordSaved)
                        {
                            return Created("api/accounts/", result);
                        }
                        else
                        {
                            return BadRequest(result);
                        }
                    }
                    else
                    {
                        return BadRequest(new ResponseResult<AccountsCreateVM>()
                        {
                            ResponseCode = ResponseCode.InternalServerError,
                            Message = ResponseMessage.InternalServerError,
                            Error = new ErrorResponseResult()
                            {
                                Message = ResponseMessage.InternalServerError
                            }
                        });
                    }
                }

                return BadRequest(new ResponseResult<AccountsCreateVM>
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
        /// Create new account.
        /// </summary>
        /// <param name="newAccount">The new account object.</param>
        /// <returns></returns>
        [Route("createaccount")]
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ResponseResult), 200)]
        public async Task<IActionResult> CreateAccount([FromBody] NewAccount newAccount)
        {
            //TODO - Need to remove account name parameter.
            Check.Argument.IsNotNull(nameof(newAccount), newAccount);
            Check.Argument.IsNotNull(nameof(newAccount.PasswordPolicy), newAccount.PasswordPolicy);
            Check.Argument.IsNotNull(nameof(newAccount.Subscription), newAccount.Subscription);
            Check.Argument.IsNotNull(nameof(newAccount.User), newAccount.User);

            return await Execute(async () =>
            {
                var account = await accountsService.CreateAccount(newAccount);
                if (account.ResponseCode == ResponseCode.RecordSaved)
                    return Ok(account);
                else
                    return BadRequest(account);
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
        [Authorize]
        [ProducesResponseType(typeof(ResponseResult<AccountViewModel>), 200)]
        public async Task<IActionResult> UpdateAccount([FromRoute] long accountId, [FromBody] AccountViewModel account)
        {
            return await Execute(async () =>
            {
                if (accountId != 0 && account != null)
                {
                    var result = await this.accountsService.UpdateAccount(accountId, account);
                    if (result != null)
                    {
                        if (result.ResponseCode == ResponseCode.RecordSaved)
                        {
                            return Ok( result);
                        }
                        else
                        {
                            return BadRequest(result);
                        }
                    }
                    else
                    {
                        return BadRequest(new ResponseResult<AccountViewModel>()
                        {
                            ResponseCode = ResponseCode.InternalServerError,
                            Message = ResponseMessage.InternalServerError,
                            Error = new ErrorResponseResult()
                            {
                                Message = ResponseMessage.InternalServerError
                            }
                        });
                    }
                }

                return BadRequest(new ResponseResult<AccountViewModel>
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
        /// This api is used for Updating existing Account partially
        /// </summary>
        /// <param name="account">The existing account object.</param>
        /// <param name="accountId">A unique integer value identifying this Account.</param>
        /// <returns></returns>
        [Route("{accountId}")]
        [HttpPatch]
        [Authorize]
        [ProducesResponseType(typeof(ResponseResult<AccountViewModel>), 200)]
        public async Task<IActionResult> UpdatePartialAccount([FromRoute] long accountId, [FromBody] AccountViewModel account)
        {
            return await Execute(async () =>
            {
                if (accountId != 0 && account != null && ModelState.IsValid)
                {
                    var result = await this.accountsService.UpdatePartialAccount(accountId, account);
                    if (result != null)
                    {
                        if (result.ResponseCode == ResponseCode.RecordSaved)
                        {
                            return Ok(result);
                        }
                        else
                        {
                            return BadRequest(result);
                        }
                    }
                    else
                    {
                        return BadRequest(new ResponseResult<AccountsCreateVM>()
                        {
                            ResponseCode = ResponseCode.InternalServerError,
                            Message = ResponseMessage.InternalServerError,
                            Error = new ErrorResponseResult()
                            {
                                Message = ResponseMessage.InternalServerError
                            }
                        });
                    }
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
        /// This api is used for deleing  Account
        /// </summary>
        /// <param name="accountId">A unique integer value identifying this Account.</param>
        /// <returns></returns>
        [Route("{accountId}")]
        [HttpDelete]
        [Authorize]
        [ProducesResponseType(typeof(ResponseResult), 200)]
        public async Task<IActionResult> DeleteAccount([FromRoute] long accountId)
        {

            return await Execute(async () =>
            {
                var result = await this.accountsService.DeleteAccount(accountId);
                 
                if (result != null)
                {
                    if (result.ResponseCode == ResponseCode.RecordDeleted)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest(result);
                    }
                }
                else
                {
                    return BadRequest(new ResponseResult()
                    {
                        ResponseCode = ResponseCode.InternalServerError,
                        Message = ResponseMessage.InternalServerError,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.InternalServerError
                        }
                    });
                } 
            });

        }


        /// <summary>
        /// Get Account Details
        /// </summary>
        /// <param name="accountId">A unique integer value identifying this Account.</param>
        /// <returns></returns>
        [Route("getaccountdetails/{accountId}")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<AccountDetail>), 200)]
        public async Task<IActionResult> GetAccountDetail([FromRoute] long accountId)
        {
            return await Execute(async () =>
            {
                if (accountId < 1)
                {
                    return BadRequest(new ResponseResult()
                    {
                        ResponseCode = ResponseCode.ValidationFailed,
                        Message = ResponseMessage.NoRecordFound,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.NoRecordFound
                        }
                    });
                }

                var account = await accountsService.GetAccountDetails(accountId);
                if (account.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(account);
                else
                    return BadRequest(account);
            });
        }

        /// <summary>
        /// Get Account Details based on header and domain name - used for Tenant Application
        /// </summary>
        /// <returns></returns>
        [Route("getAccountDetail")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<AccountDetail>), 200)]
        public async Task<IActionResult> GetAccountDetail()
        {
            var accountIdentity = GetAccountIdentity();
            if (string.IsNullOrEmpty(accountIdentity))
            {
                return BadRequest(new ResponseResult()
                {
                    ResponseCode = ResponseCode.ValidationFailed,
                    Message = ResponseMessage.NoRecordFound,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound
                    }
                });
            }

            var account = await accountsService.GetAccountDetails(accountIdentity);
            if (account.ResponseCode == ResponseCode.RecordFetched)
                return Ok(account);
            else
                return BadRequest(account);
        }

        /// <summary>
        /// Get Account List for Tenant Application
        /// </summary>
        /// <returns></returns>
        [Route("getAccountList")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<List<AccountDetail>>), 200)]
        public async Task<IActionResult> GetAccountList()
        {
            return await Execute(async () =>
            {
                var account = await accountsService.GetAccountList();
                if (account.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(account);
                else
                    return BadRequest(account);
            });
        }

        /// <summary>
        /// Private Method to Get Account Identity Type
        /// </summary>
        [NonAction]
        private string GetAccountIdentity()
        {
            string accountIdentity = string.Empty;
            if (HttpContext.Request.Headers.ContainsKey("api-key"))
            {
                accountIdentity = Convert.ToString(HttpContext.Request.Headers["api-key"]);
            }
            else if (HttpContext.Request.Headers.ContainsKey("accountid"))
            {
                accountIdentity = Convert.ToString(HttpContext.Request.Headers["accountid"]);
            }
            else if (HttpContext.Request.Query.ContainsKey("accountid"))
            {
                accountIdentity = Convert.ToString(HttpContext.Request.Query["accountid"]);
            }
            else if (HttpContext.Request.Headers.TryGetValue("Origin", out var origin))
            {
                accountIdentity = origin;
            }

            return accountIdentity;
        }

    }
}
