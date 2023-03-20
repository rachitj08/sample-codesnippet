using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sample.Customer.Model;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    /// <summary>
    /// IPasswordPolicyRepository
    /// </summary>
    public interface IPasswordPolicyRepository
    {
        /// <summary>
        /// Get Password Policy By PasswordPolicyId
        /// </summary>
        /// <param name="accountId">The accountId to get PasswordPolicy</param>
        /// <returns></returns>
        Task<PasswordPolicyModel> GetPasswordPolicyByAccountId(long accountId);

        /// <summary>l
        /// To Create Password Policy
        /// </summary>
        /// <param name="passwordPolicy">new passwordPolicy object/param>
        /// <returns></returns>
        Task<PasswordPolicyModel> CreatePasswordPolicy(PasswordPolicyModel passwordPolicy, long userId);

        /// <summary>
        /// Update password policy;
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="passwordPolicy"></param>
        /// <returns></returns>
        Task<PasswordPolicyModel> UpdatePasswordPolicy(long accountId, PasswordPolicyModel passwordPolicy, long userId);
        /// <summary>
        /// CreatePasswordPolicy
        /// </summary>
        /// <param name="passwordPolicy"></param>
        /// <returns></returns>
        Task CreatePasswordPolicy(PasswordPolicyModel passwordPolicy);
    }
}
