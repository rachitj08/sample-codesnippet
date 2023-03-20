using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IReservationPaymentHistoryRepository
    {
        /// <summary>
        /// GetAllReservationPaymentHistory
        /// </summary>
        /// <returns></returns>
        Task<List<ReservationPaymentHistory>> GetAllReservationPaymentHistory(long reservationId);

  

        /// <summary>
        /// To Create New ReservationPaymentHistory
        /// </summary>
        /// <param name="objReservationPaymentHistory">new ReservationPaymentHistory object</param>
        /// <returns> ReservationPaymentHistory object</returns>
        Task<int> CreateReservationPaymentHistory(ReservationPaymentHistory objReservationPaymentHistory, long accountId);

        /// <summary>
        /// To Update ReservationPaymentHistory
        /// </summary>
        /// <param name="objReservationPaymentHistory">New ReservationPaymentHistory object</param>
        /// <returns></returns>
        /// <returns></returns>
        Task<int> UpdateReservationPaymentHistory(ReservationPaymentHistory objReservationPaymentHistory);
        
    }
}
