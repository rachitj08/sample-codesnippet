using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class ReservationPaymentHistoryRepository : RepositoryBase<ReservationPaymentHistory>, IReservationPaymentHistoryRepository 
    {
        public ReservationPaymentHistoryRepository(CloudAcceleratorContext context) : base(context)
        {

        }

        public async Task<int> CreateReservationPaymentHistory(ReservationPaymentHistory objReservationPaymentHistory, long accountId)
        {
            return await CreateAndSaveAsync(objReservationPaymentHistory);
        }

        public async Task<List<ReservationPaymentHistory>> GetAllReservationPaymentHistory(long reservationId)
        {
            return await GetAll(x => x.ReservationId == reservationId);
        }

       

        public Task<int> UpdateReservationPaymentHistory(ReservationPaymentHistory objReservationPaymentHistory)
        {
            Update(objReservationPaymentHistory);
            return context.SaveChangesAsync();
        }

       
    }
}
