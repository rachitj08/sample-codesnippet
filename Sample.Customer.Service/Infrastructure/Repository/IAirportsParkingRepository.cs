using System.Linq;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IAirportsParkingRepository
    {
        /// <summary>
        /// GetAllParkingProvidersLocations
        /// </summary>
        /// <returns></returns>
        Task<AirportsParking> GetAllAirportsParkingByAirportId(long airportId);

        /// <summary>
        /// Get All Airports Parking By AirportId
        /// </summary>
        /// <param name="airportId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        IQueryable<AirportsParking> GetAirportsParkingByAirportId(long airportId, long accountId);

        /// <summary>
        /// Get Airports Parking By Id
        /// </summary>
        /// <param name="airportsParkingId"></param>
        /// <param name="accountId"></param>
        Task<AirportsParking> GetAirportsParkingById(long airportsParkingId, long accountId);
        /// <summary>
        /// GetAirportsParkingByLocationId
        /// </summary>
        /// <param name="parkingLocation"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<AirportsParking> GetAirportsParkingByLocationId(long parkingLocation, long accountId);
    }
}
