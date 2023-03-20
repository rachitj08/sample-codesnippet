using Common.Model;
using System.Threading.Tasks;
using Sample.Customer.Model;

namespace Sample.Customer.HttpAggregator.IServices.UserManagement
{
    /// <summary>
    /// Authenticate Service
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Authenticate
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        ResponseResult<User> Authenticate(Login login);

        /// <summary>
        /// Service to Authenticate logged in users
        /// </summary>
        /// <param name="externalAuth">object of login parameter which is required to login</param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<ResponseResult> AuthenticateExternalUser(ExternalLoginVM externalAuth, long accountId);        
    }
}
