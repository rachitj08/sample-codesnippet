using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class ParkingReservationRepository : RepositoryBase<ParkingReservation>, IParkingReservationRepository
    {
        public ParkingReservationRepository(CloudAcceleratorContext context) : base(context)
        {

        }

        public IQueryable<ParkingReservation> GetParkingReservationQuery(long accountId)
        {
            return base.GetQuery(x => x.AccountId == accountId);//, "ParkingProvidersLocation"
        }

        public async Task<ParkingReservation> GetParkingReservationByReservationId(long reservationId, long accountId)
        {
            return await SingleAsnc(x => x.AccountId == accountId && x.ReservationId == reservationId);
        }

        public ParkingReservation UpdateParkingReservation(ParkingReservation model)
        {
            return Update(model);
        }
        public async Task<int> UpdateParkingReservationNew(ParkingReservation model)
        {
            base.context.ParkingReservation.Update(model);
            return await base.context.SaveChangesAsync();
        }

        public IQueryable<ParkingReservation> GetParkingReservationQueryByDate(long accountId)
        {
            return base.GetQuery(x => x.AccountId == accountId && x.EndDateTime<=DateTime.UtcNow);//, "ParkingProvidersLocation"
        }
    }
}
