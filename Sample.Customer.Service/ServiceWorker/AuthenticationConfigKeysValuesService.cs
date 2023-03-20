using Sample.Customer.Service.Infrastructure.Repository;
using Sample.Customer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;

namespace Sample.Customer.Service.ServiceWorker
{
    /// <summary>
    /// Authentication Config Keys Values Service
    /// </summary>
    public class AuthenticationConfigKeysValuesService : IAuthenticationConfigKeysValuesService
    {
        private readonly IAuthenticationConfigKeysValuesRepository repository;

        /// <summary>
        /// Authentication Config Key sValues Service constructor to Inject dependency
        /// </summary>
        /// <param name="repository">AuthenticationConfigKeysValues repository</param>
        public AuthenticationConfigKeysValuesService(IAuthenticationConfigKeysValuesRepository repository)
        {
            Check.Argument.IsNotNull(nameof(repository), repository); 
            this.repository = repository;
        }

        /// <summary>
        /// Get Authentication Config Values By AccountId
        /// </summary>
        /// <param name="accountId">The accountId to get Authentication Config Keys Values</param>
        /// <returns></returns>
        public async Task<List<AuthenticationConfigKeyValueModel>> GetAuthenticationConfigKeysValues(long accountId)
        {
           return await repository.GetAuthenticationConfigKeysValues(accountId);
        } 
    }
}
