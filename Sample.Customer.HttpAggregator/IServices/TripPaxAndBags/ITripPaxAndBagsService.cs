using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sample.Customer.Model.Model;

namespace Sample.Customer.HttpAggregator.IServices.TripPaxAndBags
{
 /// <summary>
 /// 
 /// </summary>
    public interface ITripPaxAndBagsService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tripPaxAndBags"></param>
        /// <returns></returns>
        Task<ResponseResult<TripPaxAndBagsVM>> SaveTripPaxAndBags(TripPaxAndBagsVM tripPaxAndBags);
    }
}
