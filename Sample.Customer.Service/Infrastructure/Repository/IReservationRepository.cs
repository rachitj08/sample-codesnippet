using Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Model;
using Sample.Customer.Model.Model;
using Sample.Customer.Model.Model.Reservation;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IReservationRepository
    {
        /// <summary>
        /// Get All Reservations
        /// </summary>
        /// <param name="ordering"></param>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="all"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        List<Reservation> GetAllReservations();

        /// <summary>
        ///  Get Reservation By ReservationID
        /// </summary>
        /// <param name="reservationId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public Reservation GetReservationById(long reservationId, long accountId);

        /// <summary>
        /// To Create New Reservation
        /// </summary>
        /// <param name="objReservation">new Reservation object</param>
        /// <returns> Reservation object</returns>
        Task<int> CreateReservation(Reservation objReservation, long accountId);

        /// <summary>
        /// Create Reservation From SP
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<(long ReservationId, string ErrorMessage)> CreateReservationFromSP(AddFlightReservationVM model);

        /// <summary>
        /// To Update Reservation
        /// </summary>
        /// <param name="objReservation">New Reservation object</param>
        /// <returns></returns>
        /// <returns></returns>
        Task<int> UpdateReservation(long reservationid, long accountId, Reservation objReservation);

        /// <summary>
        /// DeleteReservation
        /// </summary>
        /// <param name="objReservation"></param>
        /// <returns></returns>
        Task<int> DeleteReservation(Reservation objReservation);

        /// <summary>
        /// CancelReservation
        /// </summary>
        /// <param name="reservationid"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> CancelReservation(long reservationid, long userId);

        /// <summary>
        /// UpdateReservation
        /// </summary>
        /// <param name="objReservation"></param>
        /// <returns></returns>
        Reservation UpdateReservation(Reservation objReservation);

        /// <summary>
        /// GetAllReservationDataByUserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Reservation GetAllReservationDataByUserId(long userId);

        /// <summary>
        /// Get Shuttle ETA
        /// </summary>
        /// <returns></returns>
        ResponseResult<string> ShuttleETA();

        /// <summary>
        /// Update Parking Reservation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reservationId"></param>
        /// <param name="accountId"></param>
        /// <param name="isParked"></param>
        /// <returns></returns>
        int Update_ParkingReservation(long reservationId, long accountId, bool isParked);

        /// <summary>
        /// Get Reservation Detail By Id
        /// </summary>
        /// <param name="reservationId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<ReservationDetailVM> GetReservationDetailById(long reservationId, long accountId);

        /// <summary>
        /// GetShuttleBoardedList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="terminalId"></param>
        /// <returns></returns>
        List<ShuttleBoardedListVM> GetShuttleBoardedList<T>(long terminalId);
        /// <summary>
        /// UpdateDeboard
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="reservationactivityCodeId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        int UpdateDeboard(long activityId, long reservationactivityCodeId, long userId);
        /// <summary>
        /// GetReservationsByParkingLocationId
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>

        List<ReservationHistoryVM> GetReservationsByParkingLocationId<T>(ReservationSearchVM model,long userId,long accountId);

        /// <summary>
        /// GetAllReservationListByUserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<Reservation> GetAllReservationListByUserId(long userId);

    }
}
