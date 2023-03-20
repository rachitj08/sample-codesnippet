using Common.Model;
using Sample.Admin.Service.Infrastructure.DataModels;
using Sample.Admin.Model;
using Sample.Admin.Model.Account.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Admin.Service.Infrastructure.Repository
{
    public interface IAccountRepository
    {
        /// <summary>
        /// Get Account List
        /// </summary>
        /// <returns></returns>
        Task<List<AccountDetail>> GetAccountList();

        /// <summary>
        /// Get All Account
        /// </summary>
        /// <returns></returns>
        Task<ResponseResultList<AccountsListVM>> GetAllAccount(string ordering, string search, int offset, int pageSize, int pageNumber, bool all);

        /// <summary>
        /// Get Account By ID
        /// </summary>
        /// <returns></returns>
        Task<AccountViewModel> GetAccountById(long accountId);

        /// <summary>
        /// To Create Accounts
        /// </summary>
        /// <param name="account">New Account Object</param>
        /// <returns></returns>
        Task<Accounts> CreateAccount(Accounts account);

        /// <summary>
        /// To Create Accounts
        /// </summary>
        /// <param name="account">New Account Object</param>
        /// <returns></returns>
        Task<ResponseResult<AccountsCreateVM>> AddAccount(AccountViewModel account);

        /// <summary>
        /// To Update Account
        /// </summary>
        /// <param name="account">New account object</param>
        /// <returns></returns>
        Task<ResponseResult<AccountViewModel>> UpdateAccount(long accountId, AccountViewModel account);

        /// <summary>
        /// To Delete Account
        /// </summary>
        /// <param name="accountId">The accountId to delete</param>
        /// <returns></returns>
        Task<long> DeleteAccount(long accountId);

        /// <summary>
        /// To Get Accounts Details for given Organisation Name
        /// </summary>
        /// <param name="organisationName">Name of Organisation</param>
        /// <returns></returns>

        Task<Accounts> CheckAccountDuplicateOrganisationName(string organisationName);

        /// <summary>
        /// To check if Account url already exists
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<Accounts> CheckAccountDuplicateUrl(string url);

        /// <summary>
        /// Get Account For TenantResolver
        /// </summary>
        /// <param name="accountIdentity">The accountIdentity to get account</param>
        /// <returns></returns>
        Task<Account> GetAccountForTenantResolver(string accountIdentity);

        /// <summary>
        /// Get Account full details
        /// </summary>
        /// <param name="accountId">The accountId to get account</param>
        /// <returns></returns>
        Task<AccountModel> GetAccount(long accountId);

        /// <summary>
        /// Get Account Details
        /// </summary>
        /// <param name="accountIdentity">The account Identity to get account</param>
        /// <returns></returns>
        Task<AccountDetail> GetAccountDetails(string accountIdentity);


        
    }
}
