using Common.Model;
using Sample.Admin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Admin.Service.ServiceWorker
{
    public interface IPasswordPolicyService
    {
        /// <summary>
        /// Get Password Policy By PasswordPolicyId
        /// </summary>
        /// <param name="accountId">The accountId to get PasswordPolicy</param>
        /// <returns></returns>
        Task<ResponseResult<PasswordPolicyVM>> GetPasswordPolicy();

        /// <summary>l
        /// To Create Password Policy
        /// </summary>
        /// <param name="passwordPolicy">new passwordPolicy object/param>
        /// <returns></returns>
        Task<ResponseResult> CreateOrUpdatePasswordPolicy(PasswordPolicyVM passwordPolicy);

       
    }
}
