using Sample.Admin.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Admin.Service.Infrastructure.Repository
{
    public interface IAuthenticationConfigKeyRepository
    {
        /// <summary>
        /// Get All Authentication Config Keys
        /// </summary>
        /// <returns></returns>
        Task<List<AuthenticationConfigKeyModel>> GetAuthenticationConfigKeys(string authenticationType);
    }
}
