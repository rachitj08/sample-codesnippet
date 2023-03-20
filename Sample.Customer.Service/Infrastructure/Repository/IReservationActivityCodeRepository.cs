using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Model.Model;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IReservationActivityCodeRepository
    {
        /// <summary>
        /// Create Reservation Activty Code
        /// </summary>
        /// <param name="objReservationActivityCode"></param>
        /// <returns>ReservationActivityCode</returns>
        public Task<ReservationActivityCode> CreateReservationActivtyCode(ReservationActivityCode objReservationActivityCode);


        IQueryable<ReservationActivityCode> GetReservationActivityCodeQuery(long accountId);

        IQueryable<ReservationActivityCode> GetCurrentActivityCode(long reservationid, long accountId);

        bool CheckAlreadyExist(long activityId, long reservationId, long accountId);

        ScannedResponseVM GetCurrentAndNextActivityCode<T>(long reservationId,long accountId, string scannedBy);
        Task<ReservationActivityCode> CreateReservationActivtyCodeAndSave(ReservationActivityCode objReservationActivityCode);
    }
}
