using Common.Model;
using System.Threading.Tasks;
using Sample.Customer.Model;

namespace Sample.Customer.HttpAggregator.IServices.UserManagement
{
    /// <summary>
    /// Password Policy
    /// </summary>
    public interface IPasswordPolicyService
    {

        /// <summary>
        /// Get Password Policy By Account Id
        /// </summary>
        Task<ResponseResult<PasswordPolicyModel>> GetPasswordPolicyByAccountId(long accountId);

        /// <summary>
        /// Create password policy
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseResult<PasswordPolicyModel>> CreatePasswordPolicy(PasswordPolicyModel model);

        /// <summary>
        /// Update password policy
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseResult<PasswordPolicyModel>> UpdatePasswordPolicy(PasswordPolicyModel model);
    }
}
