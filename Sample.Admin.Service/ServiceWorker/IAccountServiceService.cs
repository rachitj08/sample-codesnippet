using Common.Model;
using Sample.Admin.Service.Infrastructure.DataModels;
using Sample.Admin.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Admin.Service.ServiceWorker
{
    public interface IAccountServiceService
    {
        Task<ResponseResultList<AccountServicesVM>> GetAllAccountServices(string ordering, int offset, int pageSize, int pageNumber, bool all);

        /// <summary>
        /// Get accountService By Id
        /// </summary>
        /// <returns></returns>
        Task<AccountServicesVM> GetAccountServiceById(int accountServiceId);

        /// <summary>
        /// To Create New accountService
        /// </summary>
        /// <param name="accountServiceModel">accountService model</param>
        /// <returns></returns>
        Task<ResponseResult<AccountServicesVM>> AddAccountService(AccountServicesModel accountServiceModel, int loggedInUserId);

        /// <summary>
        /// To Update existing AccountService
        /// </summary>
        /// <param name="accountService">accountService object</param>
        /// <returns></returns>

        Task<ResponseResult<AccountServicesVM>> UpdateAccountService(int accountServiceId, AccountServicesModel accountService, int loggedInUserId);

        /// <summary>
        /// To Update AccountService Partially
        /// </summary>
        /// /// <param name="accountServiceId">AccountService ID</param>
        /// <param name="accountService">New accountService object</param>
        /// <returns></returns>
        Task<ResponseResult<AccountServicesVM>> UpdatePartialAccountService(int accountServiceId, AccountServicesModel accountService, int loggedInUserId);

        /// <summary>
        /// To Delete existing AccountService
        /// </summary>
        /// <param name="accountServiceId">accountService identifier</param>
        /// <returns></returns>
        Task<int> DeleteAccountService(int accountServiceId);


        /// <summary>
        /// Get All Account Service
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<AccountServices>> GetAllAccountServices();
        /// <summary>
        /// Get Account Services By AccountId
        /// </summary>
        /// <param name="accountId">account identifier</param>
        /// <returns></returns>
        Task<IEnumerable<AccountServices>> GetAccountServicesByAccountId(long accountId);

        /// <summary>
        /// Get account sevice for an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        Task<AccountServicesVM> GetAccountServiceByAccountId(long accountId, int serviceId);

        /// <summary>
        /// To Create Account Service
        /// </summary>
        /// <param name="accountServices">account service</param>
        /// <returns></returns>
        Task<AccountServices> CreateAccountService(AccountServices accountServices, int loggedInUserId);
       
    }
}
