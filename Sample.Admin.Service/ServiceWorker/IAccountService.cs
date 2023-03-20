using Common.Model;
using Sample.Admin.Model;
using Sample.Admin.Model.Account.Domain;
using Sample.Admin.Model.Account.New;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Admin.Service.ServiceWorker
{
    public interface IAccountService
    {
        Task<ResponseResultList<AccountsListVM>> GetAllAccounts(string ordering, string search, int offset, int pageSize, int pageNumber, bool all);

        /// <summary>
        /// Get account By Id
        /// </summary>
        /// <returns></returns>
        Task<AccountViewModel> GetAccountById(long accountId);

        /// <summary>
        /// To Create New account
        /// </summary>
        /// <param name="accountModel">account model</param>
        /// <returns></returns>
        Task<ResponseResult> CreateAccount(NewAccount accountModel, int loggedInUserId);

        /// <summary>
        /// To Create New account
        /// </summary>
        /// <param name="accountModel">account model</param>
        /// <returns></returns>
        Task<ResponseResult<AccountsCreateVM>> AddAccount(AccountViewModel accountModel, int loggedInUserId);



        /// <summary>
        /// To Update existing Account
        /// </summary>
        /// <param name="account">account object</param>
        /// <returns></returns>

        Task<ResponseResult<AccountViewModel>> UpdateAccount(long accountId, AccountViewModel account, int loggedInUserId);

        /// <summary>
        /// To Update Account Partially
        /// </summary>
        /// /// <param name="accountId">Account ID</param>
        /// <param name="account">New account object</param>
        /// <returns></returns>
        Task<ResponseResult<AccountViewModel>> UpdatePartialAccount(long accountId, AccountViewModel account, int loggedInUserId);

        /// <summary>
        /// To Delete existing Account
        /// </summary>
        /// <param name="accountId">account identifier</param>
        /// <returns></returns>
        Task<long> DeleteAccount(long accountId);

        /// <summary>
        /// Get Account For TenantResolver
        /// </summary>
        /// <param name="accountIdentity">The accountIdentity to get account</param>
        /// <returns></returns>
        Task<Account> GetAccountForTenantResolver(string accountIdentity);

        /// <summary>
        /// Get Account Details
        /// </summary>
        /// <param name="accountIdentity">The account Identity to get account</param>
        /// <returns></returns>
        Task<AccountDetail> GetAccountDetails(string accountIdentity);

        /// <summary>
        /// Get all Account details
        /// </summary>
        /// <param name="accountId">The accountId to get account</param>
        /// <returns></returns>
        Task<AccountModel> GetAccount(long accountId);

        /// <summary>
        /// Get Account List
        /// </summary>
        /// <returns></returns>
        Task<List<AccountDetail>> GetAccountList();
    }
}
