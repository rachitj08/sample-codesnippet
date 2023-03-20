using Common.Model;
using Sample.Admin.Model;
using System.Threading.Tasks;

namespace Sample.Admin.HttpAggregator.IServices
{
    /// <summary>
    /// ApplicationConfig Service Class
    /// </summary>
    public interface IApplicationConfigService
    {
        ///<summary>
        /// Get Application Config Details
        ///</summary>
        ///<returns></returns>
        Task<ResponseResult<ApplicationConfig>> GetApplicationConfig();

        ///<summary>
        /// Get Database Details
        ///</summary>
        ///<returns></returns>
        Task<ResponseResult<DatabaseDetails>> GetDatabaseDetails();
    }
}
