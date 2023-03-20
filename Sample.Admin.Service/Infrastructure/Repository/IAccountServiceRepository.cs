using Common.Model;
using Sample.Admin.Service.Infrastructure.DataModels;
using Sample.Admin.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Admin.Service.Infrastructure.Repository
{
    public interface IAccountServiceRepository
    {
        /// <summary>
        /// Get All AccountService
        /// </summary>
        /// <returns></returns>
        Task<ResponseResultList<AccountServicesVM>> GetAllAccountService(string ordering, int offset, int pageSize, int pageNumber, bool all);

        /// <summary>
        /// Get AccountService By ID
        /// </summary>
        /// <returns></returns>
        Task<AccountServicesVM> GetAccountServiceById(int accountServiceId);

        /// <summary>
        /// To Create AccountServices
        /// </summary>
        /// <param name="accountService">New AccountService Object</param>
        /// <returns></returns>
        Task<AccountServicesVM> AddAccountService(AccountServicesModel accountService, int userId);

        /// <summary>
        /// To Update AccountService
        /// </summary>
        /// <param name="accountService">New accountService object</param>
        /// <returns></returns>
        Task<AccountServicesVM> UpdateAccountService(int accountServiceId, AccountServicesModel accountService, int userId);

        /// <summary>
        /// To Update AccountService Partially
        /// </summary>
        /// /// <param name="accountServiceId">AccountService ID</param>
        /// <param name="accountService">New accountService object</param>
        /// <returns></returns>
        Task<AccountServicesVM> UpdatePartialAccountService(int accountServiceId, AccountServicesModel accountService, int userId);

        /// <summary>
        /// To Delete AccountService
        /// </summary>
        /// <param name="accountServiceId">The accountServiceId to delete</param>
        /// <returns></returns>
        Task<int> DeleteAccountService(int accountServiceId);

        /// <summary>
        /// Get All Account Services
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<AccountServices>> GetAllAccountServices();

        /// <summary>
        /// Get Account Services By AccountId
        /// </summary>
        /// <param name="accountId">The Account Id to get Account services</param>
        /// <returns></returns>
        Task<IEnumerable<AccountServices>> GetAccountServicesByAccountId(long accountId);

        /// <summary>
        /// Get acccount service for an account 
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        Task<AccountServicesVM> GetAccountServiceByAccountId(long accountId, int serviceId);

        /// <summary>
        /// To Create Account Service
        /// </summary>
        /// <param name="accountServices">account services model to save account services</param>
        /// <returns></returns>
        Task<AccountServices> CreateAccountService(AccountServices accountServices);
        
    }
}
