using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Model.Model;

namespace Sample.Customer.Service.ServiceWorker
{
    public interface IAirportsParkingService
    {
        /// <summary>
        /// GetAllAirportsParkingByAirportId
        /// </summary>
        /// <param name="airportId"></param>
        /// <returns></returns>
        public Task<AirportsParkingVM> GetAllAirportsParkingByAirportId(long airportId);

        /// <summary>
        /// Select Airports Parking By AirportId for reservation
        /// </summary>
        /// <param name="airportId"></param>
        /// <param name="accountId"></param>
        /// <param name="airportsParkingId"></param>
        /// <returns></returns>
        Task<AirportParkingVM> SelectAirportsParkingByAirportId(long airportId, long accountId, long airportsParkingId);
    }
}
