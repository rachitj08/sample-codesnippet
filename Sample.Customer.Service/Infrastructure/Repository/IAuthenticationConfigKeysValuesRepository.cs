using System.Collections.Generic;
using System.Threading.Tasks;
using Sample.Customer.Model;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    /// <summary>
    /// Authentication Config Keys Values Repository
    /// </summary>
    public interface IAuthenticationConfigKeysValuesRepository
    {
        /// <summary>
        /// Get Authentication Config Values By AccountId
        /// </summary>
        /// <param name="accountId">The accountId to get Authentication Config Keys Values</param>
        /// <returns></returns>
        Task<List<AuthenticationConfigKeyValueModel>> GetAuthenticationConfigKeysValues(long accountId);
    }
}
