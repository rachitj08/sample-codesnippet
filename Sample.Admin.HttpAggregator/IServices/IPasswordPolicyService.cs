using Common.Model;
using Sample.Admin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Admin.HttpAggregator.IServices
{
    /// <summary>
    /// IPasswordPolicyService
    /// </summary>
    public interface IPasswordPolicyService
    {
        /// <summary>
        /// GetPasswordPolicy
        /// </summary>
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
