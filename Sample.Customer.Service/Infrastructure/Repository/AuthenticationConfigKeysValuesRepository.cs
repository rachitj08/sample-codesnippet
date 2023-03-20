using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sample.Customer.Model;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    /// <summary>
    /// Authentication Config Keys Values Repository
    /// </summary>
    public class AuthenticationConfigKeysValuesRepository : RepositoryBase<AuthenticationConfigKeysValues>, IAuthenticationConfigKeysValuesRepository
    {
        public AuthenticationConfigKeysValuesRepository(CloudAcceleratorContext context) : base(context)
        {
        } 

        /// <summary>
        /// Get Authentication Config Values By AccountId
        /// </summary>
        /// <param name="accountId">The accountId to get Authentication Config Keys Values</param>
        /// <returns></returns>
        public async Task<List<AuthenticationConfigKeyValueModel>> GetAuthenticationConfigKeysValues(long accountId)
        {
            return await base.context.AuthenticationConfigKeysValues
                .Where(x => x.AccountId == accountId)
                .Select(x=> new AuthenticationConfigKeyValueModel()
                {
                    AccountId = x.AccountId,
                    AuthenticationConfigKeyId = x.AuthenticationConfigKeyId,
                    AuthenticationConfigKeysValueId = x.AuthenticationConfigKeysValueId,
                    ConfigKeyValue = x.ConfigKeyValue
                })
                .ToListAsync();
        }
    }
}
