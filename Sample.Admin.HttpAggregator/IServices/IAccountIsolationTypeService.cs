using Sample.Admin.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Admin.HttpAggregator.IServices
{
    /// <summary>
    /// IAccountIsolationTypeService
    /// </summary>
    public interface IAccountIsolationTypeService
    {
       /// <summary>
       /// Get List of All Account Isolation Type
       /// </summary>
       /// <returns></returns>
        Task<List<AccountIsolationType>> GetAllAccountIsolationType();
    }
}