using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Sample.Customer.Model;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class PaymentDetailsRepository : RepositoryBase<PaymentDetails>, IPaymentDetailsRepository
    {
        
        public PaymentDetailsRepository(CloudAcceleratorContext context) : base(context)
        {
        }

        public async Task<PaymentDetailVM> GetPaymentDetailsById(long paymentDetailId, long accountId)
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                var querySearch = @$" SELECT * FROM customer.""PaymentDetails"" 
                    WHERE ""AccountId"" = {accountId} AND ""PaymentDetailId"" = {paymentDetailId}; ";
                var data = await connection.QueryAsync<PaymentDetailVM>(querySearch);
                return data.FirstOrDefault();
            }
        }

        public async Task<int> CreatePaymentDetails(PaymentDetails model, long accountId, long userId)
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                var query = @$" INSERT INTO customer.""PaymentDetails""(""AccountId"", ""ReservationId"", ""IntentAmount"", ""PaymentIntentId"", 
	                ""ReceiptEmail"", ""CustomerId"", ""PaymentMethodId"", ""ApplicationFeeAmount"", 
                    ""MarginAmount"", ""ConnectedAccountId"", ""CreatedBy"", ""CreatedOn"")
	                VALUES ({accountId}, {model.ReservationId}, {model.IntentAmount}, '{model.PaymentIntentId}',
                    '{model.ReceiptEmail}', '{model.CustomerId}', '{model.PaymentMethodId}', '{(model.ApplicationFeeAmount != null ? Convert.ToString(model.ApplicationFeeAmount) : "null")}', 
                    {model.MarginAmount}, '{model.ConnectedAccountId}', {userId}, (now() at time zone 'utc')); ";
                return await connection.ExecuteAsync(query);
            }
        }

        public async Task<int> UpdatePaymentDetails(PaymentDetails model, long accountId, long userId)
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                var query = @$" UPDATE customer.""PaymentDetails"" SET
                        {(model.IntentAmount > 0 ? @$" ""IntentAmount"" = {model.IntentAmount} ,": "")}
                        ""ApplicationFeeAmount"" = {model.ApplicationFeeAmount},
                        ""FinalAmount"" = {(model.FinalAmount != null ? Convert.ToString(model.FinalAmount) : @$"null, ""MarginAmount"" = {model.MarginAmount}")},
                        ""PaymentDate"" = {(model.PaymentDate != null ? $"'{model.PaymentDate}'": "null" )},                        
                        ""UpdatedBy"" = {userId},
                        ""UpdatedOn"" = (now() at time zone 'utc') 
                        WHERE ""AccountId"" = {accountId} AND ""PaymentDetailId"" = {model.PaymentDetailId};";
                return await connection.ExecuteAsync(query);
            }
        }

    }
}
