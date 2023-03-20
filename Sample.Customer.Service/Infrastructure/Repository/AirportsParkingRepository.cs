using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class AirportsParkingRepository :RepositoryBase<AirportsParking>, IAirportsParkingRepository 
    {

        public AirportsParkingRepository(CloudAcceleratorContext context) : base(context)
        {

        }

        /// <summary>
        /// GetAllAirportsParkingByAirportId
        /// </summary>
        /// <returns></returns>
        public async Task<AirportsParking> GetAllAirportsParkingByAirportId(long airportId)
        {
            return await SingleAsnc(x => x.AirportId == airportId, "Airport");
        }

        /// <summary>
        /// Get All Airports Parking By AirportId
        /// </summary>
        /// <param name="airportId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public IQueryable<AirportsParking> GetAirportsParkingByAirportId(long airportId, long accountId)
        {
            return base.context.AirportsParking.Where(x => x.AirportId == airportId && x.AccountId == accountId)
                .Include(s => s.Airport).Include(s => s.ParkingProvidersLocation).ThenInclude(x => x.Address);
        }


        /// <summary>
        /// Get Airports Parking By Id
        /// </summary>
        /// <param name="airportsParkingId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<AirportsParking> GetAirportsParkingById(long airportsParkingId, long accountId)
        {
            return await base.context.AirportsParking.Where(x => x.AirportsParkingId == airportsParkingId && x.AccountId == accountId)
                .Include(s => s.Airport).Include(s => s.ParkingProvidersLocation).ThenInclude(x => x.Address).FirstOrDefaultAsync();
        }
        /// <summary>
        /// GetAirportsParkingByLocationId
        /// </summary>
        /// <param name="locationId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<AirportsParking> GetAirportsParkingByLocationId(long parkingLocation, long accountId)
        {
            return await base.context.AirportsParking.Where(x => x.ParkingProvidersLocationId == parkingLocation && x.AccountId == accountId)
                .Include(s => s.Airport).Include(s => s.ParkingProvidersLocation).ThenInclude(x => x.Address).FirstOrDefaultAsync();
        }
    }
}
