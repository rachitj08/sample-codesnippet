using Common.Model;
using System.Threading.Tasks;
using Sample.Customer.Model;

namespace Sample.Customer.HttpAggregator.IServices.EmailParse
{
    /// <summary>
    /// IEmail Parser Service
    /// </summary>
    public interface IEmailParserService
    {
        /// <summary>
        /// Email Parser CallBack
        /// </summary>
        /// <param name="model"></param>
        /// <param name="authorizationKey"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<ResponseResult<bool>> EmailParserCallBack(EmailParserCallBackResponse model, string authorizationKey, long accountId);


        /// <summary>
        /// Process All Pending Email
        /// </summary>
        /// <param name="authorizationKey"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<ResponseResult<bool>> ProcessAllPendingEmail(string authorizationKey, long accountId);
    }
}
