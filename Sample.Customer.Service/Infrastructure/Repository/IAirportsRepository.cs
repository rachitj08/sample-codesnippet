using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IAirportsRepository
    {

        /// <summary>
        /// Get All Airports
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        IQueryable<Airports> GetAllAiportsWithAddress(long accountId);


        /// <summary>
        /// Get All Airports With airportCodes
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="airportCodes"></param>
        /// <returns></returns>
        Task<List<Airports>> GetAirportDetails(long accountId, string[] airportCodes);
    }
}
