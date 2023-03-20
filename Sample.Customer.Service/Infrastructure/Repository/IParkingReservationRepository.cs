using System.Linq;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IParkingReservationRepository
    {
        IQueryable<ParkingReservation> GetParkingReservationQuery(long accountId);

        Task<ParkingReservation> GetParkingReservationByReservationId(long reservationId, long accountId);

        ParkingReservation UpdateParkingReservation(ParkingReservation model);

        IQueryable<ParkingReservation> GetParkingReservationQueryByDate(long accountId);

        Task<int> UpdateParkingReservationNew(ParkingReservation model);
    }
}
