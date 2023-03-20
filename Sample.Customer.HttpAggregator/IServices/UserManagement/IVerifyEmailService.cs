using Common.Model;
using System.Threading.Tasks;
using Sample.Customer.Model;

namespace Sample.Customer.HttpAggregator.IServices.UserManagement
{
    /// <summary>
    /// 
    /// </summary>
    public interface IVerifyEmailService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        Task<ResponseResult<SuccessMessageModel>> VerifyEmail(string token, string uid);
    }
}
