using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
   public class AirportsRepository: RepositoryBase<Airports>, IAirportsRepository 
    {

        public AirportsRepository(CloudAcceleratorContext context) : base(context)
        {
            
        }

        /// <summary>
        /// Get All Airports With Address
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public IQueryable<Airports> GetAllAiportsWithAddress(long accountId)
        {
            return base.GetQuery(x=> x.AccountId == accountId);// , "Address"
        }


        /// <summary>
        /// Get Airports With airportCodes
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="airportCodes"></param>
        /// <returns></returns>
        public async Task<List<Airports>> GetAirportDetails(long accountId, string[] airportCodes)
        {
            airportCodes = airportCodes.Select(x => x.ToLower()).ToArray();
            return await base.GetQuery(x => x.AccountId == accountId && airportCodes.Contains(x.Code.ToLower()), "Address").ToListAsync();
        }
    }
}
