using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class ParkingHeadsCustomRateRepository : RepositoryBase<ParkingHeadsCustomRate>, IParkingHeadsCustomRateRepository 
    {
        public ParkingHeadsCustomRateRepository(CloudAcceleratorContext context) : base(context)
        {

        }

        public async Task<int> CreateParkingHeadsCustomRate(ParkingHeadsCustomRate objParkingHeadsCustomRate, long accountId)
        {
            return await CreateAndSaveAsync(objParkingHeadsCustomRate);
        }

        public async Task<List<ParkingHeadsCustomRate>> GetAllParkingHeadsCustomRate(long reservationId)
        {
            return await GetAll(x => x.ReservationId == reservationId);
        }

       

        public Task<int> UpdateParkingHeadsCustomRate(ParkingHeadsCustomRate objParkingHeadsCustomRate)
        {
            Update(objParkingHeadsCustomRate);
            return context.SaveChangesAsync();
        }

       
    }
}
