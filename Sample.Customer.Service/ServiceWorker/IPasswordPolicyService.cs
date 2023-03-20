using Common.Model;
using System.Threading.Tasks;
using Sample.Customer.Model;

namespace Sample.Customer.Service.ServiceWorker
{
    public interface IPasswordPolicyService
    {
        /// <summary>
        /// Get Password Policy By AccountId
        /// </summary>
        /// <param name="accountId">The accountId to get PasswordPolicy</param>
        /// <returns></returns>
        Task<PasswordPolicyModel> GetPasswordPolicyByAccountId(long accountId);

        /// <summary>
        /// To Create new Password Policy
        /// </summary>
        /// <param name="passwordPolicy">passwordPolicy object</param>
        /// <returns></returns>
        Task<ResponseResult<PasswordPolicyModel>> CreatePasswordPolicy(PasswordPolicyModel passwordPolicy, long userId);

        /// <summary>
        /// To Update existing Password Policy
        /// </summary>
        /// <param name="accountId">accountId object</param>
        /// <returns></returns>

        Task<ResponseResult<PasswordPolicyModel>> UpdatePasswordPolicy(long accountId, PasswordPolicyModel passwordPolicy, long userId);

    }
}
