using AutoMapper.Configuration;
using Common.Model;
using Sample.Admin.Service.Infrastructure.Repository;
using Sample.Admin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Admin.Service.ServiceWorker
{
    /// <summary>
    /// PasswordPolicyService
    /// </summary>
    public class PasswordPolicyService : IPasswordPolicyService
    {
        
        /// <summary>
        /// passwordPolicyRepository
        /// </summary>
        private readonly IPasswordPolicyRepository passwordPolicyRepository;
        /// <summary>
        /// PasswordPolicyService
        /// </summary>
        /// <param name="passwordPolicyRepository"></param>
        public PasswordPolicyService(IPasswordPolicyRepository passwordPolicyRepository)
        {
            this.passwordPolicyRepository = passwordPolicyRepository;
        }
        
        /// <summary>
        /// CreatePasswordPolicy
        /// </summary>
        /// <param name="passwordPolicy"></param>
        /// <returns></returns>
        public async Task<ResponseResult> CreateOrUpdatePasswordPolicy(PasswordPolicyVM passwordPolicy)
        {
            int result = await this.passwordPolicyRepository.CreateOrUpdatePasswordPolicy(passwordPolicy);
            if (result > 0)
            {
                return new ResponseResult()
                {
                    Message = ResponseMessage.RecordSaved,
                    ResponseCode = ResponseCode.RecordSaved,
                    Data = result
                };
            }
            else
            {
                return new ResponseResult()
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
        /// GetPasswordPolicy
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult<PasswordPolicyVM>> GetPasswordPolicy()
        {
            var result = await this.passwordPolicyRepository.GetPasswordPolicy();
            if (result != null)
            {
                return new ResponseResult<PasswordPolicyVM>()
                {
                    Message = ResponseMessage.RecordFetched,
                    ResponseCode = ResponseCode.RecordFetched,
                    Data = result
                };
            }
            else
            {
                return new ResponseResult<PasswordPolicyVM>()
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
       
    }
}
