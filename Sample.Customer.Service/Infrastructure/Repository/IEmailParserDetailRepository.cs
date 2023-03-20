using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IEmailParserDetailRepository
    {
        Task<(bool Satus, string ErrorMessage)> InsertCallBackResult(EmailParserDetail model);
        EmailParserDetail UpdateCallBackResult(EmailParserDetail model);
        Task<EmailParserDetail> GetResultByMessageId(Guid msgId, long accountId);
        Task<List<EmailParserDetail>> GetAllPendingEmailDetails(long accountId);
        Task<EmailParserDetail> GetById(long accountId, long emailParserDetailId);
    }
}
