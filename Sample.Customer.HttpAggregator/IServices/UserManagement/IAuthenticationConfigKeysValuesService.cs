using Common.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Customer.HttpAggregator.IServices.UserManagement
{
    /// <summary>
    /// Authentication Config Servrice interface
    /// </summary>
    public interface IAuthenticationConfigKeysValuesService
    {

        /// <summary>
        /// Get Tenant Auth Settings
        /// </summary>
        /// <param name="authenticationType">Authentication Type</param>
        /// <returns></returns>
        Task<ResponseResult<Dictionary<string, string>>> GetTenantAuthSettings(string authenticationType);
    }
}
