using Common.Model;
using System;
using System.Threading.Tasks;
using Sample.Customer.Model;
using System.Collections.Generic;

namespace Sample.Customer.Service.ServiceWorker
{
    public interface IEmailParserCallBackService
    {
        Task<ResponseResult<EmailParseDetails>> GetDetailsFromMessageId(Guid messageId, long accountId);
        Task<ResponseResult<bool>> UpdateEmailParseDetail(long emailParseDetailId, long accountId, long userId, short status, string message);
        Task<ResponseResult<List<EmailParseDetails>>> GetAllEmailParseDetails(long accountId);
        Task<ResponseResult<bool>> EmailTravelItineraryCreateReservation(EmailParserReservationDetails model);
    }
}
