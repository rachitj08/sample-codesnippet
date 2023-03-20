using Sample.Admin.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Admin.Service.ServiceWorker
{
    public interface IAuthenticationConfigKeyService
    {
        /// <summary>
        /// Get All Authentication Config Keys
        /// </summary>
        /// <returns></returns>
        Task<List<AuthenticationConfigKeyModel>> GetAuthenticationConfigKeys(string authenticationType);
    }
}
