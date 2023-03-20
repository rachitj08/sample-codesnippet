using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface ITripPaxAndBagsRepository
    {
        Task<int> SaveTripAndBags(TripPaxAndBags tripPaxAndBags, long userId, long accountId);
    }
}
