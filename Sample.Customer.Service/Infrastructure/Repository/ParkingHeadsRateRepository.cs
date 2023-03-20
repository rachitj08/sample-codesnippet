using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Sample.Customer.Model;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class ParkingHeadsRateRepository : RepositoryBase<ParkingHeadsRate>, IParkingHeadsRateRepository
    {
        public ParkingHeadsRateRepository(CloudAcceleratorContext context) : base(context)
        {
        }

        /// <summary>
        /// Get All Parking Heads
        /// </summary>
        /// <returns></returns>
        public List<ParkingHeadsRateDetailVM> GetParkingHeadsRateList(DateTime applicableDate, long parkingProviderLocationId, long accountId)
        { 
            string sqlQuery = $"Select * From customer.fn_getparkingheadsratelist({accountId}, {parkingProviderLocationId},'{applicableDate}')";
            List<ParkingHeadsRateDetailVM> result = new List<ParkingHeadsRateDetailVM>();

            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {                 
                return connection.Query<ParkingHeadsRateDetailVM>(sqlQuery).ToList();
            }
        }
        public List<ParkingHeadsRateDetailVM> GetParkingCustomHeadsRateList(DateTime applicableDate, long parkingProviderLocationId, long accountId,long reservationId)
        { 
            string sqlQuery = $"Select * From customer.fn_getparkingheadscustomratelist({accountId}, {parkingProviderLocationId},'{applicableDate}',{reservationId})";
           
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {                 
                return connection.Query<ParkingHeadsRateDetailVM>(sqlQuery).ToList();
            }
        }
    }
}
