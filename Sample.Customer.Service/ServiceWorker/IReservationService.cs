using Common.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sample.Customer.Model;
using Sample.Customer.Model.Model;
using Sample.Customer.Model.Model.ParkingHeads;
using Sample.Customer.Model.Model.Reservation;

namespace Sample.Customer.Service.ServiceWorker
{

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
        ResponseResultList<FlightAndParkingReservationVM> GetAllReservations(string ordering, int offset, int pageSize, int pageNumber, bool all, long accountId);

        /// <summary>
        ///  Get ReservationVM By Reservationid
        /// </summary>
        /// <param name="Reservationid"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        ResponseResult<FlightAndParkingReservationVM> GetReservationById(long reservationid, long accountId);


        /// <summary>
        /// Get Email Itinerary Reservation For Review
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ResponseResult<List<FlightReservationVM>>> GetEmailItineraryReservationForReview(long accountId, long userId);

        /// <summary>
        /// Confirm Email Itinerary Reservation
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ResponseResult<bool>> ConfirmEmailItineraryReservation(long flightReservationId, long accountId, long userId);


        /// <summary>
        /// To Create New ReservationVM
        /// </summary>
        /// <param name="model">new ReservationVM object</param>
        /// <returns> ReservationVM object</returns>
        Task<ResponseResult<bool>> CreateReservation(AddUpdateFlightReservationVM model);

        /// <summary>
        /// GetReservationPriceAndAddressData
        /// </summary>
        /// <param name="model"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<ResponseResult<ParkingReservationAmountAndLocationVM>> GetReservationPriceAndAddressData(AddUpdateFlightAndParkingReservationVM model, long accountId, long airportsParkingId = 0);

        /// <summary>
        /// To Update ReservationVM
        /// </summary>
        /// <param name="model">Update ReservationVM object</param>
        /// <returns></returns>
        Task<ResponseResult<ParkingReservationAmountAndLocationVM>> UpdateReservation(AddUpdateFlightAndParkingReservationVM model, long reservationId, long accountId, long userId);


        /// <summary>
        /// To Delete ReservationVM
        /// </summary>
        /// <param name="reservationId">The Reservationid to delete</param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<ResponseResult<FlightAndParkingReservationVM>> DeleteReservation(long reservationId, long accountId);

        /// <summary>
        /// Cancel Reservation
        /// </summary>
        /// <param name="reservationid"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ResponseResult<bool>> CancelReservation(long reservationid, long accountId, long userId);

        /// <summary>
        /// Get All Ongoing and Upcoming Trips
        /// </summary>
        /// <param name="accountId">Account Id</param>
        /// <param name="userId">User Id</param>
        /// <returns></returns>
        Task<ResponseResult<OngoingUpcomingTripVM>> GetAllOngoingUpcomingTrips(long accountId, long userId);

        /// <summary>
        /// Get All Trip Details
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>        
        Task<ResponseResult<List<TripDetailVM>>> GetAllTrips(string flag, long accountId, long userId);

        /// <summary>
        /// Get Current Activity Code
        /// </summary>
        /// <param name="reservationId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        ResponseResult<IEnumerable<CurrentActivityVM>> GetCurrentActivityCode(long reservationId,long accountId);

        /// <summary>
        /// Generate Reservation Invoice By Id
        /// </summary>
        /// <param name="reservationid"></param>
        /// <returns></returns>
        Task<ResponseResult<bool>> GenerateReservationInvoiceById(CreateInvoiceVM model);

        /// <summary>
        /// Generate Reservation By Id
        /// </summary>
        /// <param name="reservationid"></param>
        /// <returns></returns>
        Task<ResponseResult<string>> GenerateReservationById(CreateInvoiceVM model);

        /// <summary>
        /// Get Shuttle ETA
        /// </summary>
        /// <returns></returns>
        ResponseResult<string> ShuttleETA();

        /// <summary>
        /// Update Create Reservation Activty Code
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseResult<ScannedResponseVM>> UpdateCreateReservationActivtyCode(ScannedVM model, long accountId, long userId);
        
        /// <summary>
        /// GetReservationVehicleDetailsByVinNumber
        /// </summary>
        /// <param name="reservationId"></param>
        /// <param name="vinNumber"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        ResponseResult<CarDetailVM> GetReservationVehicleDetailsByVinNumber(long reservationId, string vinNumber, long accountId);

        /// <summary>
        /// Update Parking Reservation
        /// </summary>
        /// <param name="model"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<ResponseResult<int>> UpdateParkingReservation(UpdateIsParkedVM model,long accountId, long userId);
        /// <summary>
        /// Get JourneyCompleted list
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ResponseResult<OngoingUpcomingTripVM>> GetJourneyCompletedlist(long accountId, long userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="terminalId"></param>
        /// <returns></returns>
        ResponseResult<List<ShuttleBoardedListVM>> GetShuttleBoardedList(int terminalId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        ResponseResult<int> DeBoardFromShuttle(DeBoardFromShuttleVM model, long accountId, long userId);
        /// <summary>
        /// GetReservationsByParkingLocationId
        /// </summary>
        /// <param name="model"></param>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>        
        Task<ResponseResult<List<ReservationHistoryVM>>> GetReservationsByParkingLocationId(ReservationSearchVM model, long accountId, long userId);
        /// <summary>
        /// GetReservationsByUserId
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ResponseResult<List<FlightAndParkingReservationVM>>> GetReservationsByUserId(long accountId, long userId);

        Task<ResponseResult<ParkingReserAmountAndLocationVM>> CreateReservationNew(AddUpdateFlightReservationVM model, long accountId);
        /// <summary>
        /// UpdateReservationFromProviderPortal
        /// </summary>
        /// <param name="model"></param>
        /// <param name="reservationId"></param>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ResponseResult<ParkingReservationAmountAndLocationVM>> UpdateReservationFromProviderPortal(AddUpdateFlightAndParkingReservationVM model, long reservationId, long accountId, long userId);
    }
}
