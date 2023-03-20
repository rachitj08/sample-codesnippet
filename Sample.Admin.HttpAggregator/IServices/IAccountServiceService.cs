using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Model;
using Sample.Admin.Model;
using Microsoft.AspNetCore.Http;

namespace Sample.Admin.HttpAggregator.IServices
{
    /// <summary>
    /// IAccountServiceService
    /// </summary>
    public interface IAccountServiceService
    {
        /// <summary>
        /// AccountService Service to get accountService list
        /// </summary>
        /// <returns></returns>
        Task<ResponseResultList<AccountServicesVM>> GetAllAccountServices(HttpContext httpContext, string ordering, int offset, int pageSize, int pageNumber, bool all);

        /// <summary>
        /// Get AccountService By Id
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<AccountServicesVM>> GetAccountServiceById(int accountServiceId);

        /// <summary>
        /// To Create new AccountService
        /// </summary>
        /// <param name="accountService">accountService object</param>
        /// <returns></returns>
        Task<ResponseResult<AccountServicesVM>> AddAccountService(AccountServicesModel accountService);

        /// <summary>
        /// To Update existing AccountService
        /// </summary>
        /// <param name="accountService">accountService object</param>
        /// /// <param name="accountServiceId">Unique AccountService Id</param>
        /// <returns></returns>

        Task<ResponseResult<AccountServicesVM>> UpdateAccountService(int accountServiceId, AccountServicesModel accountService);

        /// <summary>
        /// To Update existing AccountService
        /// </summary>
        /// <param name="accountService">accountService object</param>
        /// /// <param name="accountServiceId">Unique AccountService Id</param>
        /// <returns></returns>

        Task<ResponseResult<AccountServicesVM>> UpdatePartialAccountService(int accountServiceId, AccountServicesModel accountService);

        /// <summary>
        /// To Delete existing AccountService
        /// </summary>
        /// <param name="accountServiceId">accountService identifier</param>
        /// <returns></returns>
        Task<ResponseResult<AccountServicesVM>> DeleteAccountService(int accountServiceId);

        /// <summary>
        /// Service to Get List of All Account Service
        /// </summary>
        /// <returns></returns>
        Task<List<AccountService>> GetAllAccountServices();

        /// <summary>
        /// GetAccountServiceByAccountId
        /// </summary>
        /// <param name="accountId">account Id</param>
        /// <param name="serviceId">service Id</param>
        /// <returns></returns>
        Task<ResponseResult<AccountServicesVM>> GetAccountServiceByAccountId(long accountId, int serviceId);
    }
}
