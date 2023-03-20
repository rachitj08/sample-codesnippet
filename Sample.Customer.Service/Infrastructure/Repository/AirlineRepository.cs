using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class AirlineRepository : RepositoryBase<Airline>, IAirlineRepository
    {

        public AirlineRepository(CloudAcceleratorContext context) : base(context)
        {

        }

        /// <summary>
        /// Get All Airports With Address
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public IQueryable<Airline> GetAllAirline()
        {
            return GetQuery();
        }

    }
}
