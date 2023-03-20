using Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Sample.Customer.Model.Model;

namespace Sample.Customer.Service.ServiceWorker
{
    public interface IAirlineService
    {
        /// <summary>
        /// Get All Airline List
        /// </summary>
        /// <param name="loggedInAccountId"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        ResponseResult<List<AirlineVM>> GetAllAirlineList(long loggedInAccountId, long loggedInUserId);
    }
}
