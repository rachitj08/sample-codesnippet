using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class ParkingSpotsRepository : RepositoryBase<ParkingSpots>, IParkingSpotsRepository
    {
        public ParkingSpotsRepository(CloudAcceleratorContext context) : base(context)
        {

        }

        public async Task<ParkingSpots> GetParkingSpotById(long parkingSpotsId, long accountId)
        {
            return await SingleAsnc(x => x.ParkingSpotId == parkingSpotsId && x.AccountId == accountId);
        } 
        public async Task<int> UpdateParkingSpot(ParkingSpots parkingSpots)
        {
            base.context.ParkingSpots.Update(parkingSpots);
            return await base.context.SaveChangesAsync();
        }

        public async Task<ParkingSpots> GetParkingSpotByName(string name, long accountId)
        {
            return await SingleAsnc(x => x.Name == name && x.AccountId == accountId);
        }
    }
}
