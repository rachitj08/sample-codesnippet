using Common.Model;
using Sample.Admin.Model;
using Sample.Admin.Model.Account.Domain;
using Sample.Admin.Model.Account.New;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Admin.HttpAggregator.IServices
{
    /// <summary>
    /// Account Service Interface
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Account Service to get account list
        /// </summary>
        /// <returns></returns>
        Task<ResponseResultList<AccountsListVM>> GetAllAccounts(HttpContext httpContext, string ordering, string search, int offset, int pageSize, int pageNumber, bool all);

        /// <summary>
        /// Get Account By Id
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<AccountViewModel>> GetAccountById(long accountId);

        /// <summary>
        /// To Create new Account
        /// </summary>
        /// <param name="account">account object</param>
        /// <returns></returns>
        Task<ResponseResult<AccountsCreateVM>> AddAccount(AccountViewModel account);

        /// <summary>
        /// Account Service to create new account
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Task<ResponseResult> CreateAccount(NewAccount account);

        /// <summary>
        /// To Update existing Account
        /// </summary>
        /// <param name="account">account object</param>
        /// /// <param name="accountId">Unique Account Id</param>
        /// <returns></returns>

        Task<ResponseResult<AccountViewModel>> UpdateAccount(long accountId, AccountViewModel account);

        /// <summary>
        /// To Update existing Account
        /// </summary>
        /// <param name="account">account object</param>
        /// /// <param name="accountId">Unique Account Id</param>
        /// <returns></returns>

        Task<ResponseResult<AccountViewModel>> UpdatePartialAccount(long accountId, AccountViewModel account);

        /// <summary>
        /// To Delete existing Account
        /// </summary>
        /// <param name="accountId">account identifier</param>
        /// <returns></returns>
        Task<ResponseResult> DeleteAccount(long accountId);

        /// <summary>
        /// Account Service to get account details
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<ResponseResult<AccountDetail>> GetAccount(long accountId);

        /// <summary>
        /// Account Service to get account list
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<List<AccountDetail>>> GetAccountList();

        /// <summary>
        /// Account Service to get account details
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<ResponseResult<AccountDetail>> GetAccountDetails(long accountId);

        /// <summary>
        /// Account Service to get account details
        /// </summary>
        /// <param name="accountIdentity"></param>
        /// <returns></returns>
        Task<ResponseResult<AccountDetail>> GetAccountDetails(string accountIdentity);

    }
}
