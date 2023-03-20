using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IAirlineRepository
    {
        /// <summary>
        /// Get AllAirline
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        IQueryable<Airline> GetAllAirline();
    }
}
