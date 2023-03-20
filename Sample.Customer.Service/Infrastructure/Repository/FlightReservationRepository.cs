using Common.Model;
using Sample.Customer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Sample.Customer.Service.Infrastructure.DataModels;
using Sample.Customer.Model.Model.Reservation;
using Npgsql;
using System.Data;
using Dapper;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class FlightReservationRepository : RepositoryBase<FlightReservation>, IFlightReservationRepository
    {
        
        public FlightReservationRepository(CloudAcceleratorContext context) : base(context)
        {
        }

        public IQueryable<FlightReservation> GetAllFlightReservation(long accountId, long userId)
        {
            return GetQuery(x => x.AccountId == accountId && x.Reservation.IsCancelled == false
                && x.Reservation.UserId == userId);//, "Reservation"
        }


        public async Task<List<TripDetailVM>> GetAllTrips(string flag, long accountId, long userId)
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                var querySearch = $"SELECT  * FROM customer.fn_gettriplist({accountId}, {userId}, '{flag}')";
                var data = await connection.QueryAsync<TripDetailVM>(querySearch);
                return data.ToList();
            }
        }

        /// <summary>
        /// Get All Flight Reservation For Review
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<FlightReservation>> GetAllFlightReservationForReview(long accountId, long userId)
        { 
            //"Status:- 0~Pending|1~Confirm"
            return await GetQuery(x => x.AccountId == accountId && x.Reservation.IsCancelled == false
                && x.Reservation.UserId == userId && x.Status < 1, "DepaurtureAirport,FlyingToAirportldNavigation,ReturningToAirportldNavigation").ToListAsync();
        }

        /// <summary>
        /// Get Flight Reservation By Id
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<FlightReservation> GetFlightReservationById(long flightReservationId, long accountId)
        {
            //"Status:- 0~Pending|1~Confirm"
            return await SingleAsnc(x => x.AccountId == accountId && x.FlightReservationId == flightReservationId);
        }

        /// <summary>
        /// Update Flight Reservation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FlightReservation UpdateFlightReservation(FlightReservation model)
        {
            return Update(model);
        }
    }
}
