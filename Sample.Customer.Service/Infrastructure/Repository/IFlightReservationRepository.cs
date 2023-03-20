using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sample.Customer.Model;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IFlightReservationRepository
    {

        IQueryable<FlightReservation> GetAllFlightReservation(long accountId, long userId);

        /// <summary>
        /// Get All Trips
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<TripDetailVM>> GetAllTrips(string flag, long accountId, long userId);

        /// <summary>
        /// Get All Flight Reservation For Review
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<FlightReservation>> GetAllFlightReservationForReview(long accountId, long userId);


        /// <summary>
        /// Get Flight Reservation By Id
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<FlightReservation> GetFlightReservationById(long flightReservationId, long accountId);

        /// <summary>
        /// Update Flight Reservation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        FlightReservation UpdateFlightReservation(FlightReservation model);
    }
}