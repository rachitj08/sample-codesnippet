
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
   public class TripPaxAndBagsRepository : RepositoryBase<TripPaxAndBags>, ITripPaxAndBagsRepository
    {
        public TripPaxAndBagsRepository(CloudAcceleratorContext context) : base(context)
        {
        }

        public async Task<int> SaveTripAndBags(TripPaxAndBags tripPaxAndBags, long reservationId, long userId)
        {
            base.context.Add(tripPaxAndBags);
            return await base.context.SaveChangesAsync();
        }
    }
}
