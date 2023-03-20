using Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Model.Model;

namespace Sample.Customer.Service.ServiceWorker
{
   public interface ITripPaxAndBagsService
    {
        Task<ResponseResult<TripPaxAndBagsVM>> SaveTripPaxAndBags(TripPaxAndBagsVM tripPaxAndBagsVM, long userId, long accountId);
    }
}
