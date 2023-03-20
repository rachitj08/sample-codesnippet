using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IParkingSpotsRepository
    {
        Task<ParkingSpots> GetParkingSpotById(long parkingSpotsId, long accountId);
        Task<int> UpdateParkingSpot(ParkingSpots parkingSpots);
        Task<ParkingSpots> GetParkingSpotByName(string name, long accountId);
    }
}
