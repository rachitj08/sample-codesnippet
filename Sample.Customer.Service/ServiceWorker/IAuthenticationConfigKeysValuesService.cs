using Sample.Customer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Customer.Service.ServiceWorker
{
    /// <summary>
    /// Authentication Config Keys Values Service
    /// </summary>
    public interface IAuthenticationConfigKeysValuesService
    {
        // <summary>
        /// Get Authentication Config Values By AccountId
        /// </summary>
        /// <param name="accountId">The accountId to get Authentication Config Keys Values</param>
        /// <returns></returns>
        Task<List<AuthenticationConfigKeyValueModel>> GetAuthenticationConfigKeysValues(long accountId);
    }
}
