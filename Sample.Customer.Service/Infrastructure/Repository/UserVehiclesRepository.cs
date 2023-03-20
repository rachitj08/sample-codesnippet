using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Sample.Customer.Model.Model;
using Sample.Customer.Service.Infrastructure.DataModels;
using System.Threading.Tasks;

namespace Sample.Customer.Service.Infrastructure.Repository
{

    public class UserVehiclesRepository : RepositoryBase<UserVehicles>, IUserVehiclesRepository
    {
        public UserVehiclesRepository(CloudAcceleratorContext context) : base(context)
        {

        }

        public IQueryable<UserVehicles> GetUserVehiclesRepositoryQuery(long accountId)
        {
            return GetQuery(x => x.AccountId == accountId);
        }

        #region Out Going
        public List<CarDetailVM> GetAllOutgoing<T>(string flag, long accountId, long parkingProviderLocationId)
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                string querySearch = $@"SELECT * from customer.fn_getAllOutgoingCarList({accountId},{parkingProviderLocationId},'{flag.ToLower().Trim()}')";
                 return connection.Query<CarDetailVM>(querySearch).ToList();
            }
        }
        public CarDetailCountVM GetAllOutgoingCount<T>(DateTime inputDateValue, long accountId, long parkingProviderLocationId)
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                string querySearch = $@"select * from  customer.fn_getAllOutgoingCount({accountId},{parkingProviderLocationId})";
                return connection.QueryFirstOrDefault<CarDetailCountVM>(querySearch);
            }
        }
        public List<DayWiseOutgoingCount> GetOutgoingDayWiseCount<T>(int dayCount, long accountId, long parkingProviderLocationId)
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                var querySearch = @$"
               select * from customer.fn_GetOutgoingDayWiseCount({dayCount},{accountId},{parkingProviderLocationId}) ";
                return connection.Query<DayWiseOutgoingCount>(querySearch).ToList();
            }
        }
        public CarDetailCountVM GetAllOutgoingCountByDate<T>(DateTime inputDateValue, long accountId, long parkingProviderLocationId)
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                var querySearch = @$"select * from  customer.fn_getAllOutgoingCountByDate('{inputDateValue}','{inputDateValue.AddHours(24)}' ,{accountId},{parkingProviderLocationId})";
                return connection.QueryFirstOrDefault<CarDetailCountVM>(querySearch);
            }
        }
        #endregion

        #region In Coming
        public List<CarDetailVM> GetAllInComing<T>(string flag, long accountId, long parkingProviderLocationId)
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                string querySearch = $@"SELECT * FROM customer.fn_getallincomingcarlist({accountId},{parkingProviderLocationId}, '{flag.ToLower().Trim()}')";
                return connection.Query<CarDetailVM>(querySearch).ToList();
            }
        }
        public CarDetailCountVM GetAllInComingCount<T>(DateTime inputDateValue, long accountId, long parkingProviderLocationId)
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                string querySearch = $@"select * from  customer.fn_getAllInComingCarCount({accountId},{parkingProviderLocationId})";
                return connection.QueryFirstOrDefault<CarDetailCountVM>(querySearch);
            }
        }
        
        public List<DayWiseIncomingCount> GetIncomingDayWiseCount<T>(int dayCount, long accountId, long parkingProviderLocationId)
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                var querySearch = @$"
               select * from customer.fn_GetIncomingDayWiseCount({dayCount},{accountId},{parkingProviderLocationId}) ";
                return connection.Query<DayWiseIncomingCount>(querySearch).ToList();
            }
        }
        public CarDetailCountVM GetAllInComingCountByDate<T>(DateTime inputDateValue, long accountId, long parkingProviderLocationId)
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                var querySearch = @$"select * from  customer.fn_getAllInComingCountByDate('{inputDateValue}','{inputDateValue.AddHours(24)}' ,{accountId},{parkingProviderLocationId})";
                
                return connection.QueryFirstOrDefault<CarDetailCountVM>(querySearch);
            }
        }

        #endregion

        /// <summary>
        /// GetUserVehiclesDataByvehicleIdAndUserID
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserVehicles GetUserVehiclesDataByVehicleIdAndUserID(long vehicleId, long accountId, long userId)
        {
            return base.context.UserVehicles.Where(x => x.VehicleId == vehicleId && x.UserId == userId && x.AccountId == accountId)
                   .Include(x => x.User)
                   .Include(x => x.Vehicle).FirstOrDefault();
        }

        /// <summary>
        /// Craete UserVehicleDetails
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<UserVehicles> CreateUserVehicleDetails(UserVehicles model)
        {
            return await CreateAsync(model);
        }

        public List<CarDetailVM> GetJourneyCompletedlist<T>(string flag, long accountId, long parkingProviderLocationId)
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                string querySearch = $@"SELECT * from customer.fn_getJourneyCompletedlist({accountId},{parkingProviderLocationId},'{flag.ToLower().Trim()}')";
                return connection.Query<CarDetailVM>(querySearch).ToList();
            }
        }
        public async Task<UserVehicles> CreateUserVehicleDetailsNew(UserVehicles model)
        {
            var UserVehicles= await CreateAsync(model);
            await context.SaveChangesAsync();
            return UserVehicles;

        }
    }
}
