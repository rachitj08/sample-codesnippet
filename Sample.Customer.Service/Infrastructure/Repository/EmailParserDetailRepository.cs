using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class EmailParserDetailRepository : RepositoryBase<EmailParserDetail>, IEmailParserDetailRepository
    {

        public EmailParserDetailRepository(CloudAcceleratorContext context) : base(context)
        {
            
        }


        public async Task<(bool Satus, string ErrorMessage)> InsertCallBackResult(EmailParserDetail model)
        {
            try
            {
                string sqlQuery = @$"INSERT INTO customer.""EmailParserDetail""(
                ""AccountId"", ""MessageId"", ""RequestId"", ""Mobile"", 
                ""MobileCode"", ""Status"", ""CreatedOn"", ""CreatedBy"")
                VALUES({model.AccountId}, '{model.MessageId}', '{model.RequestId}', '{model.Mobile}',
                '{model.MobileCode}', '{model.Status}', '{model.CreatedOn}', '{model.CreatedBy}'); ";
                using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
                {
                    await connection.ExecuteAsync(sqlQuery);
                    return (true, "");
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public EmailParserDetail UpdateCallBackResult(EmailParserDetail model)
        {
            return Update(model);
        }

        public async Task<EmailParserDetail> GetResultByMessageId(Guid msgId, long accountId)
        {
            return await SingleAsnc(x => x.MessageId == msgId && x.AccountId == accountId && x.Status == 0); 
        }

        public async Task<List<EmailParserDetail>> GetAllPendingEmailDetails(long accountId)
        {
            return await GetAll(x => x.AccountId == accountId && x.Status == 0); 
        }

        public async Task<EmailParserDetail> GetById(long accountId, long emailParserDetailId)
        {
            return await base.SingleAsnc(x => x.AccountId == accountId && x.EmailParserDetailId == emailParserDetailId);
        }
    }
}
