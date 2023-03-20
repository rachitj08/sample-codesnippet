using Sample.Admin.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Admin.HttpAggregator.IServices
{
    /// <summary>
    /// IAuthenticationTypeService
    /// </summary>
    public interface IAuthenticationTypeService
    {
       /// <summary>
       /// Service to Get List of All Authentication Type
       /// </summary>
       /// <returns></returns>
        Task<List<AuthenticationType>> GetAllAuthenticationTypes();
    }
}
