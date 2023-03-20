using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sample.Customer.Model.Model;

namespace Sample.Customer.HttpAggregator.IServices.Reservation
{
    /// <summary>
    /// 
    /// </summary>
   public interface IAirportAddressService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<List<AirportAddressVM>>> GetAllAirportAddress();
    }
}
