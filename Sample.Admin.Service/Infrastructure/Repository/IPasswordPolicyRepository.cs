using Sample.Admin.Service.Infrastructure.DataModels;
using Sample.Admin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Admin.Service.Infrastructure.Repository
{
    public interface IPasswordPolicyRepository
    {
        /// <summary>
        /// Get Password Policy By PasswordPolicyId
        /// </summary>
        /// <param name="accountId">The accountId to get PasswordPolicy</param>
        /// <returns></returns>
        Task<PasswordPolicyVM> GetPasswordPolicy();

       
        /// <summary>
        /// CreatePasswordPolicy
        /// </summary>
        /// <param name="passwordPolicy"></param>
        /// <returns></returns>
        Task<int> CreateOrUpdatePasswordPolicy(PasswordPolicyVM passwordPolicy);
    }
}
