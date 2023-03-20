using Common.Model;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Sample.Customer.Model;
using Sample.Customer.Model.Model;
using Sample.Customer.Model.Model.Reservation;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class ReservationRepository : RepositoryBase<Reservation>, IReservationRepository
    {
        public ReservationRepository(CloudAcceleratorContext context) : base(context)
        {

        }


        /// <summary>
        /// GetAllReservations
        /// </summary>
        /// <param name="ordering"></param>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="all"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public List<Reservation> GetAllReservations()
        {
            return (List<Reservation>)base.GetAll();
        }

        /// <summary>
        /// GetReservationById
        /// </summary>
        /// <param name="reservationId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public Reservation GetReservationById(long reservationId, long accountId)
        {
            //return base.GetById(reservationId);
            return base.context.Reservation.Where(x => x.ReservationId == reservationId && x.AccountId == accountId)
                .Include(x => x.User)
                .Include(x => x.FlightReservation)
                .Include(x => x.ParkingReservation)
                .Include(x => x.RentalReservation)
                .Include(x => x.RentalReservationOption)
                .Include(x => x.ReservationVehicle)
                .Include(x => x.User)
                .Include(x => x.PaymentDetails)
                .FirstOrDefault();
        }

        /// <summary>
        /// CreateReservation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> CreateReservation(Reservation model, long accountId)
        {  
            base.context.Reservation.Add(model);
            return await base.context.SaveChangesAsync();
        }

        /// <summary>
        /// Create Reservation From SP
        /// </summary>
        /// <param name="model"></param> 
        /// <returns></returns>
        public async Task<(long ReservationId, string ErrorMessage)> CreateReservationFromSP(AddFlightReservationVM model)
        {
            try
            {
                string sqlQuery = $"call customer.sp_create_reservation({model.AccountId}::bigint, {model.UserId}::bigint, " +
                    $" '{model.ReservationCode}'::character varying, {model.FlyingToAirportld}::bigint, '{model.FlyingToAirline}'::character varying," +
                    $" '{model.FlyingToFlightNo}'::character varying, {model.DepaurtureAirportId}::bigint, '{model.DepaurtureDateTime}'::timestamp with time zone, " +
                    $" {model.ReturningToAirportld}::bigint, '{model.ReturningToAirline}'::character varying, '{model.ReturningToFlightNo}'::character varying," +
                    $" '{model.ReturnDateTime}'::timestamp with time zone, {model.IsHomeAirport}::boolean, {model.IsBorrowingCarForRent}::boolean," +
                    $" {model.FlightReservationStatus}::integer, {model.AirportsParkingId}::bigint, {model.ParkingProvidersLocationId }::bigint, " +
                    $" {model.InTimeGap}::integer, {model.OutTimeGap}::integer, '{model.SourceId}'::integer,  '{model.Comment}'::character varying,  '{model.BookingConfirmationNo}'::character varying,  '{model.ComingFrom}'::character varying, 0::bigint)";

                using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
                {
                    var reservationId = Convert.ToInt64(await connection.ExecuteScalarAsync(sqlQuery));
                    return (reservationId, "");
                }
            }
            catch(Exception ex)
            {
                return (0, ex.Message);
            }
        }
        
        /// <summary>
        /// UpdateReservation
        /// </summary>
        /// <param name="reservationId"></param>
        /// <param name="objReservation"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> UpdateReservation(long reservationid, long accountId, Reservation objReservation)
        {
            base.context.Reservation.Update(objReservation);
            return await base.context.SaveChangesAsync();

        }

        /// <summary>
        /// Delete Reservation
        /// </summary>
        /// <param name="reservationId"></param>
        /// <returns></returns>
        public async Task<int> DeleteReservation(Reservation model)
        {
            base.context.ReservationActivityCode.RemoveRange(base.context.ReservationActivityCode.Where(x => x.ReservationId == model.ReservationId));
            base.context.ReservationVehicle.RemoveRange(base.context.ReservationVehicle.Where(x => x.ReservationId == model.ReservationId));
            base.context.ParkingReservation.RemoveRange(base.context.ParkingReservation.Where(x => x.ReservationId == model.ReservationId));
            base.context.FlightReservation.RemoveRange(base.context.FlightReservation.Where(x => x.ReservationId == model.ReservationId));
            base.context.Reservation.Remove(model);
            return await base.context.SaveChangesAsync();
        }

        public async Task<int> CancelReservation(long reservationid, long userId)
        {
            var result= await base.context.Reservation.FindAsync(reservationid);
            if (result != null)
            {
                result.IsCancelled = true;
                result.UpdatedOn = DateTime.UtcNow;
                result.UpdatedBy = userId;
            }
            return await base.context.SaveChangesAsync();

        }

        public  Reservation UpdateReservation( Reservation objReservation)
        {
            return Update(objReservation);

        }

        /// <summary>
        /// GetAllReservationDataByUserId
        /// </summary>
        /// <param name="reservationId"></param>
        /// <returns></returns>
        public Reservation GetAllReservationDataByUserId(long userId)
        {
            return base.context.Reservation.Where(x => x.UserId == userId)
                .Include(x => x.User)
                 .Include(x => x.FlightReservation)
                  .Include(x => x.ParkingReservation)
                  .Include(x => x.RentalReservation)
                  .Include(x => x.RentalReservationOption)
                   .Include(x => x.ReservationVehicle)
                 .Include(x => x.User).FirstOrDefault();
        }

        /// <summary>
        /// Get Shuttle ETA
        /// </summary>
        /// <returns></returns>
        public ResponseResult<string> ShuttleETA()
        {
            return ShuttleETA();
        }

        public int Update_ParkingReservation(long reservationId, long accountId, bool isParked)
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                var querySearch = @$"UPDATE customer.""ParkingReservation"" SET ""IsParked""={isParked} where ""AccountId""={accountId} and ""ReservationId""={reservationId};";
               
                return connection.Execute(querySearch);
            }
        }

        public async Task<ReservationDetailVM> GetReservationDetailById(long reservationId, long accountId)
        {
            List<ReservationDetailVM> result = new List<ReservationDetailVM>();
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                var querySearch = @$"Select * from customer.fn_getresevationdetails({accountId},{reservationId});";

                using (var reader = await connection.ExecuteReaderAsync(querySearch))
                {
                    while (reader.Read())
                    {
                        result.Add(new ReservationDetailVM()
                        {
                            EmailAddress = Convert.ToString(reader["EmailAddress"]),
                            EndDateTime = Convert.ToDateTime(reader["EndDateTime"]),
                            StartDateTime = Convert.ToDateTime(reader["StartDateTime"]),
                            Mobile = Convert.ToString(reader["Mobile"]),
                            MobileCode = reader["MobileCode"] != DBNull.Value ? Convert.ToString(reader["MobileCode"]) : "",
                            ParkingProvidersLocationId = Convert.ToInt64(reader["ParkingProvidersLocationId"]),
                            ParkingReservationId = Convert.ToInt64(reader["ParkingReservationId"]),
                            ReservationCode = Convert.ToString(reader["ReservationCode"]),
                            UserFirstName = Convert.ToString(reader["UserFirstName"]),
                            UserLastName = Convert.ToString(reader["UserLastName"]),
                            DepaurtureAirportCode = Convert.ToString(reader["DepaurtureAirportCode"]),
                            ParkingProvidersLocationName = Convert.ToString(reader["ParkingProvidersLocationName"]),
                            PaymentDetailId = reader["PaymentDetailId"] != DBNull.Value ? Convert.ToInt64(reader["PaymentDetailId"]) : 0,
                            PaymentIntentId = reader["PaymentIntentId"] != DBNull.Value ? Convert.ToString(reader["PaymentIntentId"]) : "",
                        });
                    }
                }
            }
            return result.FirstOrDefault();
        }

        public List<ShuttleBoardedListVM> GetShuttleBoardedList<T>(long terminalId)
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                var querySearch = @$"select * from customer.fn_getShuttleBoardingList({terminalId},1)";
                return connection.Query<ShuttleBoardedListVM>(querySearch).ToList();
            }
        }
        public int UpdateDeboard(long activityId,long reservationactivityCodeId,long userId)
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                var querySearch = @$"update customer.""ReservationActivityCode"" set ""IsDropped""=true, ""UpdatedBy""={userId}, ""UpdatedOn""='{DateTime.UtcNow}' Where ""ReservationActivityCodeId""={reservationactivityCodeId} ";
                return connection.Execute(querySearch);
            }
        }

        public List<ReservationHistoryVM> GetReservationsByParkingLocationId<T>(ReservationSearchVM model,long userId,long accountId)
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                var querySearch = @$"select * from customer.fn_getreservationHistory({accountId},{userId},'{model.ReservaionLocationId}','{model.FirstName}','{model.LastName}'
,'{model.EmailId}','{model.MobNo}','{model.FromDate}','{model.ToDate}','{model.SourceName}')";
                return connection.Query<ReservationHistoryVM>(querySearch).ToList();
            }
        }

        public List<Reservation> GetAllReservationListByUserId(long userId)
        {
            return base.context.Reservation.Where(x => x.UserId == userId)
                .Include(x => x.User)
                 .Include(x => x.FlightReservation)
                  .Include(x => x.ParkingReservation)
                  .Include(x => x.RentalReservation)
                  .Include(x => x.RentalReservationOption)
                   .Include(x => x.ReservationVehicle)
                 .Include(x => x.User).ToList();
        }
    }
}
