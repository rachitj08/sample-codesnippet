using Common.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sample.Customer.Model.Model;

namespace Sample.Customer.HttpAggregator.IServices.Airline
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAirlineService
    {
        /// <summary>
        /// Get Airline List
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<List<AirlineVM>>> GetAirlineList();
    }
}
