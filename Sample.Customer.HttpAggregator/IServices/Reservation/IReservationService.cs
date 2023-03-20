using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sample.Customer.Model.Model;
using Sample.Customer.Model;
using Sample.Customer.Model.Model.ParkingHeads;
using Sample.Customer.Model.Model.Reservation;

namespace Sample.Customer.HttpAggregator.IServices.Reservation
{
    /// <summary>
    /// IReservationService
    /// </summary>
    public interface IReservationService
    {
        /// <summary>
        /// Get All ReservationVMs
        /// </summary>
        /// <param name="ordering"></param>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="all"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<ResponseResultList<FlightAndParkingReservationVM>> GetAllReservations(string ordering, int offset, int pageSize, int pageNumber, bool all, long accountId);

        /// <summary>
        /// GetReservationById
        /// </summary>
        /// <param name="reservationid"></param>
        /// <returns></returns>
        Task<ResponseResult<FlightAndParkingReservationVM>> GetReservationById(long reservationid);


        /// <summary>
        /// To Create New ReservationVM
        /// </summary>
        /// <param name="model">new ReservationVM object</param>
        /// <param name="userId"></param>
        /// <param name="accountId"></param>
        /// <returns> ReservationVM object</returns>
        Task<ResponseResult<ParkingReserAmountAndLocationVM>> CreateReservation(AddUpdateFlightAndParkingReservationVM model, long userId, long accountId);

        /// <summary>
        /// UpdateReservation
        /// </summary>
        /// <param name="reservationid"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseResult<ParkingReserAmountAndLocationVM>> UpdateReservation(long reservationid, AddUpdateFlightAndParkingReservationVM model);


        /// <summary>
        /// To Delete ReservationVM
        /// </summary>
        /// <param name="reservationid">The Reservationid to delete</param>
        /// <returns></returns>
        Task<ResponseResult<FlightAndParkingReservationVM>> DeleteReservation(long reservationid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reservationid"></param>
        /// <returns></returns>
        Task<ResponseResult<bool>> CancelReservation(long reservationid);

        /// <summary>
        /// CreateReservationActivtyCode
        /// </summary>
        /// <param name="model"></param>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ResponseResult<ScannedResponseVM>> CreateReservationActivtyCode(ScannedVM model, long accountId, long userId);

        /// <summary>
        /// Get Email Itinerary Reservation For Review
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<List<FlightReservationVM>>> GetEmailItineraryReservationForReview();

        /// <summary>
        /// Confirm Email Itinerary Reservation
        /// </summary>
        /// <param name="flightReservationId"></param>
        /// <returns></returns>
        Task<ResponseResult<bool>> ConfirmEmailItineraryReservation(long flightReservationId);

        /// <summary>
        /// Get All Ongoing and Upcoming Trips
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<OngoingUpcomingTripVM>> GetAllOngoingUpcomingTrips();

        /// <summary>
        /// Get All Trips
        /// </summary>
        /// <param name="flag">ongoing|upcoming|completed</param>
        /// <returns></returns>
        Task<ResponseResult<List<TripDetailVM>>> GetAllTrips(string flag);

        /// <summary>
        /// Get Current Activity Code
        /// </summary>
        /// <param name="reservationId"></param>
        /// <returns></returns>
        Task<ResponseResultList<CurrentActivityVM>> GetCurrentActivityCode(long reservationId);
        /// <summary>
        /// GenerateReservationInvoiceById
        /// </summary>
        /// <param name="reservationid"></param>
        /// <returns></returns>
        Task<ResponseResult<bool>> GenerateReservationInvoiceById(long reservationid);

        /// <summary>
        /// Get Shuttle ETA
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<string>> ShuttleETA();

        /// <summary>
        /// GetReservationVehicleDetailsByVinNumber
        /// </summary>
        /// <param name="reservationId"></param>
        /// <param name="vinNumber"></param>
        /// <returns></returns>
        Task<ResponseResult<CarDetailVM>> GetReservationVehicleDetailsByVinNumber(long reservationId, string vinNumber);
        /// <summary>
        /// Update Parking Reservation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseResult<int>> UpdateParkingReservation(UpdateIsParkedVM model);
        /// <summary>
        /// Get Journey Completed list
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<OngoingUpcomingTripVM>> GetJourneyCompletedlist();
        
        /// <summary>
        /// Get Shuttle Boarded List
        /// </summary>
        /// <param name="terminalId"></param>
        /// <returns></returns>
        Task<ResponseResult<List<ShuttleBoardedListVM>>> GetShuttleBoardedList(long terminalId);
       /// <summary>
       /// 
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
        Task<ResponseResult<int>> DeBoardFromShuttle(DeBoardFromShuttleVM model);

        /// <summary>
        /// GetReservationsByParkingLocationId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseResult<List<ReservationHistoryVM>>> GetReservationsByParkingLocationId(ReservationSearchVM model);
        /// <summary>
        /// GetReservationsByUserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ResponseResult<List<FlightAndParkingReservationVM>>> GetReservationsByUserId(long userId);


        /// <summary>
        /// CreateReservationNew
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>

        Task<ResponseResult<ParkingReservationAmountAndLocationVM>> CreateReservationNew(AddUpdateFlightAndParkingReservationVM model, long userId, long accountId);
    }
}
