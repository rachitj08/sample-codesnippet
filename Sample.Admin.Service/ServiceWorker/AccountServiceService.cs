using Sample.Admin.Service.Infrastructure.DataModels;
using Sample.Admin.Service.Infrastructure.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Model;
using Sample.Admin.Model;

namespace Sample.Admin.Service.ServiceWorker
{
    public class AccountServiceService : IAccountServiceService
    {

        private readonly IUnitOfWork unitOfWork;

        private readonly IAccountServiceRepository accountServicesRepository;


        /// <summary>
        /// Account Service Service constructor to Inject dependency
        /// </summary>
        /// <param name="unitOfWork">unit of work</param>
        /// <param name="versionModulesRepository">version modules repository</param>
        public AccountServiceService(IUnitOfWork unitOfWork, IAccountServiceRepository accountServicesRepository)
        {
            this.unitOfWork = unitOfWork;
            this.accountServicesRepository = accountServicesRepository;
        }

        /// <summary>
        /// Get All AccountServices
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResultList<AccountServicesVM>> GetAllAccountServices(string ordering, int offset, int pageSize, int pageNumber, bool all) => await this.accountServicesRepository.GetAllAccountService(ordering, offset, pageSize, pageNumber, all);

        /// <summary>
        /// Get AccountService By Id
        /// </summary>
        /// <returns></returns>
        public async Task<AccountServicesVM> GetAccountServiceById(int accountServiceId) => await this.accountServicesRepository.GetAccountServiceById(accountServiceId);

        /// <summary>
        /// To Create New AccountService
        /// </summary>
        /// <param name="newAccountService">AccountService Param for accountService</param>
        /// <returns></returns>
        public async Task<ResponseResult<AccountServicesVM>> AddAccountService(AccountServicesModel accountService, int loggedInUserId)
        {
            // Validate Model
            var errorDetails = new Dictionary<string, string[]>();
            if (accountService.Account < 1)
            {
                errorDetails.Add("account", new string[] { "This field may not be blank." });
            }

            if (accountService.Service < 1)
            {
                errorDetails.Add("service", new string[] { "This field may not be blank." });
            }

            if (string.IsNullOrWhiteSpace(accountService.DbServer))
            {
                errorDetails.Add("dbServer", new string[] { "This field may not be blank." });
            }
            else if (accountService.DbServer.Length > 255)
            {
                errorDetails.Add("dbServer", new string[] { "Ensure this field has no more than 255 characters." });
            }

            if (string.IsNullOrWhiteSpace(accountService.DbName))
            {
                errorDetails.Add("dbName", new string[] { "This field may not be blank." });
            }
            else if (accountService.DbName.Length > 100)
            {
                errorDetails.Add("dbName", new string[] { "Ensure this field has no more than 100 characters." });
            }

            if (string.IsNullOrWhiteSpace(accountService.DbSchema))
            {
                errorDetails.Add("dbSchema", new string[] { "This field may not be blank." });
            }
            else if (accountService.DbSchema.Length > 100)
            {
                errorDetails.Add("dbSchema", new string[] { "Ensure this field has no more than 100 characters." });
            }

            if (string.IsNullOrWhiteSpace(accountService.UserName))
            {
                errorDetails.Add("userName", new string[] { "This field may not be blank." });
            }
            else if (accountService.UserName.Length > 200)
            {
                errorDetails.Add("userName", new string[] { "Ensure this field has no more than 200 characters." });
            }

            if (string.IsNullOrWhiteSpace(accountService.Password))
            {
                errorDetails.Add("password", new string[] { "This field may not be blank." });
            }
            else if (accountService.Password.Length > 200)
            {
                errorDetails.Add("password", new string[] { "Ensure this field has no more than 200 characters." });
            }

            if (errorDetails.Count > 0)
            {
                return new ResponseResult<AccountServicesVM>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Detail = errorDetails,
                        Message = ResponseMessage.ValidationFailed,
                    }
                };
            }

            var result = await this.accountServicesRepository.AddAccountService(accountService, loggedInUserId);

            if(result.AccountServiceId > 0)
            {
                return new ResponseResult<AccountServicesVM>()
                {
                    Message = ResponseMessage.RecordSaved,
                    ResponseCode = ResponseCode.RecordSaved,
                    Data = result
                };
            }
            else
            {
                return new ResponseResult<AccountServicesVM>()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                };
            }
        }

        /// <summary>
        /// To Update existing AccountService
        /// </summary>
        /// <param name="accountService">accountService object</param>
        /// <returns></returns>
        public async Task<ResponseResult<AccountServicesVM>> UpdateAccountService(int accountServiceId, AccountServicesModel accountService, int loggedInUserId)
        {
            // Validate Model
            var errorDetails = new Dictionary<string, string[]>();
            if (accountService.Account < 1)
            {
                errorDetails.Add("account", new string[] { "This field may not be blank." });
            }

            if (accountService.Service < 1)
            {
                errorDetails.Add("service", new string[] { "This field may not be blank." });
            }

            if (string.IsNullOrWhiteSpace(accountService.DbServer))
            {
                errorDetails.Add("dbServer", new string[] { "This field may not be blank." });
            }
            else if (accountService.DbServer.Length > 255)
            {
                errorDetails.Add("dbServer", new string[] { "Ensure this field has no more than 255 characters." });
            }

            if (string.IsNullOrWhiteSpace(accountService.DbName))
            {
                errorDetails.Add("dbName", new string[] { "This field may not be blank." });
            }
            else if (accountService.DbName.Length > 100)
            {
                errorDetails.Add("dbName", new string[] { "Ensure this field has no more than 100 characters." });
            }

            if (string.IsNullOrWhiteSpace(accountService.DbSchema))
            {
                errorDetails.Add("dbSchema", new string[] { "This field may not be blank." });
            }
            else if (accountService.DbSchema.Length > 100)
            {
                errorDetails.Add("dbSchema", new string[] { "Ensure this field has no more than 100 characters." });
            }

            if (string.IsNullOrWhiteSpace(accountService.UserName))
            {
                errorDetails.Add("userName", new string[] { "This field may not be blank." });
            }
            else if (accountService.UserName.Length > 200)
            {
                errorDetails.Add("userName", new string[] { "Ensure this field has no more than 200 characters." });
            }

            if (string.IsNullOrWhiteSpace(accountService.Password))
            {
                errorDetails.Add("password", new string[] { "This field may not be blank." });
            }
            else if (accountService.Password.Length > 200)
            {
                errorDetails.Add("password", new string[] { "Ensure this field has no more than 200 characters." });
            }

            if (errorDetails.Count > 0)
            {
                return new ResponseResult<AccountServicesVM>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Detail = errorDetails,
                        Message = ResponseMessage.ValidationFailed,
                    }
                };
            }

            return new ResponseResult<AccountServicesVM>()
            {
                Message = ResponseMessage.RecordSaved,
                ResponseCode = ResponseCode.RecordSaved,
                Data = await this.accountServicesRepository.UpdateAccountService(accountServiceId, accountService, loggedInUserId)
            };
        }

        /// <summary>
        /// To Update AccountService Partially
        /// </summary>
        /// /// <param name="accountServiceId">AccountService ID</param>
        /// <param name="accountService">New accountService object</param>
        /// <returns></returns>
        public async Task<ResponseResult<AccountServicesVM>> UpdatePartialAccountService(int accountServiceId, AccountServicesModel accountService, int loggedInUserId)
        {
            // Validate Model
            var errorDetails = new Dictionary<string, string[]>();
            if (accountService.Account < 1)
            {
                errorDetails.Add("account", new string[] { "This field may not be blank." });
            }

            if (accountService.Service < 1)
            {
                errorDetails.Add("service", new string[] { "This field may not be blank." });
            }

            if (string.IsNullOrWhiteSpace(accountService.DbServer))
            {
                errorDetails.Add("dbServer", new string[] { "This field may not be blank." });
            }
            else if (accountService.DbServer.Length > 255)
            {
                errorDetails.Add("dbServer", new string[] { "Ensure this field has no more than 255 characters." });
            }

            if (string.IsNullOrWhiteSpace(accountService.DbName))
            {
                errorDetails.Add("dbName", new string[] { "This field may not be blank." });
            }
            else if (accountService.DbName.Length > 100)
            {
                errorDetails.Add("dbName", new string[] { "Ensure this field has no more than 100 characters." });
            }

            if (string.IsNullOrWhiteSpace(accountService.DbSchema))
            {
                errorDetails.Add("dbSchema", new string[] { "This field may not be blank." });
            }
            else if (accountService.DbSchema.Length > 100)
            {
                errorDetails.Add("dbSchema", new string[] { "Ensure this field has no more than 100 characters." });
            }

            if (string.IsNullOrWhiteSpace(accountService.UserName))
            {
                errorDetails.Add("userName", new string[] { "This field may not be blank." });
            }
            else if (accountService.UserName.Length > 200)
            {
                errorDetails.Add("userName", new string[] { "Ensure this field has no more than 200 characters." });
            }

            if (string.IsNullOrWhiteSpace(accountService.Password))
            {
                errorDetails.Add("password", new string[] { "This field may not be blank." });
            }
            else if (accountService.Password.Length > 200)
            {
                errorDetails.Add("password", new string[] { "Ensure this field has no more than 200 characters." });
            }

            if (errorDetails.Count > 0)
            {
                return new ResponseResult<AccountServicesVM>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Detail = errorDetails,
                        Message = ResponseMessage.ValidationFailed,
                    }
                };
            }

            return new ResponseResult<AccountServicesVM>()
            {
                Message = ResponseMessage.RecordSaved,
                ResponseCode = ResponseCode.RecordSaved,
                Data = await this.accountServicesRepository.UpdatePartialAccountService(accountServiceId, accountService, loggedInUserId)
            };

        }

        /// <summary>
        /// To delete existing AccountService
        /// </summary>
        /// <param name="accountServiceId">accountService identifier</param>
        /// <returns></returns>
        public async Task<int> DeleteAccountService(int accountServiceId) => await this.accountServicesRepository.DeleteAccountService(accountServiceId);

        /// <summary>
        /// Get information for All Account Service
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<AccountServices>> GetAllAccountServices()
        {

            var modulesData = await this.accountServicesRepository.GetAllAccountServices();
            return modulesData;


        }

        /// <summary>
        /// Get Account Services By AccountId
        /// </summary>
        /// <param name="accountId">account identifier</param>
        /// <returns></returns>
        public async Task<IEnumerable<AccountServices>> GetAccountServicesByAccountId(long accountId)
        {

            var modulesData = await this.accountServicesRepository.GetAccountServicesByAccountId(accountId);
            return modulesData;

        }
        /// <summary>
        /// Get account sevice for an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public async Task<AccountServicesVM> GetAccountServiceByAccountId(long accountId, int serviceId) => await this.accountServicesRepository.GetAccountServiceByAccountId(accountId, serviceId);

      

        /// <summary>
        /// To Create  New Account Service
        /// </summary>
        /// <param name="accountServices">account service</param>
        /// <returns></returns>
        public async Task<AccountServices> CreateAccountService(AccountServices accountServices, int loggedInUserId)
        {
            accountServices.CreatedOn = System.DateTime.UtcNow;
            accountServices.CreatedBy = loggedInUserId;
            var result = await this.accountServicesRepository.CreateAccountService(accountServices);
            return result;
        }

    }
}
