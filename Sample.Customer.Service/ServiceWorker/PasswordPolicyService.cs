using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.Repository;
using Sample.Customer.Model;
using Utilities;

namespace Sample.Customer.Service.ServiceWorker
{
    public class PasswordPolicyService : IPasswordPolicyService
    {
        /// <summary>
        /// passwordPolicyRepository Private Member
        /// </summary>
        private readonly IPasswordPolicyRepository passwordPolicyRepository;

        /// <summary>
        /// Passwor dPolicy Service constructor to Inject dependency
        /// </summary>
        /// <param name="passwordPolicyRepository"> password Policy repository for user data</param>
        public PasswordPolicyService(IPasswordPolicyRepository passwordPolicyRepository)
        { 
            Check.Argument.IsNotNull(nameof(passwordPolicyRepository), passwordPolicyRepository);            

            this.passwordPolicyRepository = passwordPolicyRepository;
        }

        /// <summary>
        /// Get Password Policy By AccountId
        /// </summary>
        /// <param name="accountId">The accountId to get PasswordPolicy</param>
        /// <returns></returns>
        public async Task<PasswordPolicyModel> GetPasswordPolicyByAccountId(long accountId)
        {
            return await this.passwordPolicyRepository.GetPasswordPolicyByAccountId(accountId);             
        }

        /// <summary>
        /// To Create new Password Policy
        /// </summary>
        /// <param name="passwordPolicy">passwordPolicy object</param>
        /// <returns></returns>
        public async Task<ResponseResult<PasswordPolicyModel>> CreatePasswordPolicy(PasswordPolicyModel passwordPolicy, long userId)
        {
            // Validate Model
            var errorDetails = new Dictionary<string, string[]>();
            if (passwordPolicy.MinPasswordLength <= 0)
            {
                errorDetails.Add("minPasswordLength", new string[] { "This field may not be blank." });
            }          

            if (passwordPolicy.ExpiryInDays <= 0)
            {
                errorDetails.Add("expiryInDays", new string[] { "This field may not be blank." });
            }
          
            if (errorDetails.Count > 0)
            {
                return new ResponseResult<PasswordPolicyModel>()
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

            return new ResponseResult<PasswordPolicyModel>()
            {
                Message = ResponseMessage.RecordSaved,
                ResponseCode = ResponseCode.RecordSaved,
                Data = await this.passwordPolicyRepository.CreatePasswordPolicy(passwordPolicy, userId)
            };

        }
        /// <summary>
        /// UpdatePasswordPolicy
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="passwordPolicy"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<PasswordPolicyModel>> UpdatePasswordPolicy(long accountId, PasswordPolicyModel passwordPolicy, long userId)
        {
            if(accountId < 1)
            {
                return new ResponseResult<PasswordPolicyModel>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = "Invalid account",
                    }
                };
            }
            // Validate Model
            var errorDetails = new Dictionary<string, string[]>();
             
            if (passwordPolicy.MinPasswordLength <= 0)
            {
                errorDetails.Add("minPasswordLength", new string[] { "This field may not be blank." });
            }

            if (passwordPolicy.ExpiryInDays <= 0)
            {
                errorDetails.Add("expiryInDays", new string[] { "This field may not be blank." });
            }

            if (errorDetails.Count > 0)
            {
                return new ResponseResult<PasswordPolicyModel>()
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

            return new ResponseResult<PasswordPolicyModel>()
            {
                Message = ResponseMessage.RecordSaved,
                ResponseCode = ResponseCode.RecordSaved,
                Data = await this.passwordPolicyRepository.UpdatePasswordPolicy(accountId, passwordPolicy, userId)
            };           
        }


    }
}
