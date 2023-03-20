using Common.Model;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Model.Model;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{

    public class ReservationActivityCodeRepository : RepositoryBase<ReservationActivityCode>, IReservationActivityCodeRepository
    {
        public ReservationActivityCodeRepository(CloudAcceleratorContext context) : base(context)
        {

        }

        public async Task<ReservationActivityCode> CreateReservationActivtyCode(ReservationActivityCode objReservationActivityCode)
        {
            return await CreateAsync(objReservationActivityCode);
        }

        public IQueryable<ReservationActivityCode> GetCurrentActivityCode(long reservationid, long accountId)
        {
            return GetQuery(x => x.ReservationId == reservationid && x.AccountId == accountId).OrderBy(x => x.ActivityCodeId);
        }

        public IQueryable<ReservationActivityCode> GetReservationActivityCodeQuery(long accountId)
        {
            return base.GetQuery(x => x.AccountId == accountId);
        }

        public bool CheckAlreadyExist(long activityId, long reservationId, long accountId)
        {
            return base.Any(x => x.ActivityCodeId == activityId && x.ReservationId == reservationId && x.AccountId == accountId);
        }

        public ScannedResponseVM GetCurrentAndNextActivityCode<T>(long reservationId, long accountId, string scannedBy)
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                var querySearch = @$"
                Select Distinct ac.""Code"" as ""CurrentActivityCode"", ac.""Odering"",ac.""ActivityCodeId"",
                (select acs.""Code"" from customer.""ActivityCode"" as acs where ""ActivityCodeFor"" = '{scannedBy}' and
                 acs.""Odering"" > ac.""Odering""  ORDER BY acs.""Odering"" ASC limit 1) as ""NextActivityCode""
                FROM customer.""ActivityCode"" as ac
                inner JOIN customer.""ReservationActivityCode"" as rac
                ON ac.""ActivityCodeId"" = rac.""ActivityCodeId""
                where rac.""ReservationId"" = {reservationId} and rac.""AccountId"" = {accountId} and ac.""ActivityCodeFor"" = '{scannedBy}'
                ORDER BY ac.""Odering"" DESC limit 1";
                return connection.QueryFirst<ScannedResponseVM>(querySearch);
            }
        }

        public async Task<ReservationActivityCode> CreateReservationActivtyCodeAndSave(ReservationActivityCode objReservationActivityCode)
        {
            ReservationActivityCode reservationActivityCode = await CreateAsync(objReservationActivityCode);
            context.SaveChanges();
            return reservationActivityCode;
        }

    }
}
