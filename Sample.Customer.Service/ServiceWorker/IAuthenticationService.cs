using Common.Model;
using System.Threading.Tasks;
using Sample.Customer.Model;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.ServiceWorker
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// To Authenticate user
        /// </summary>
        /// <param name="login"> login data for user</param>
        /// <param name="ipAddress">IP Address</param>
        /// <param name="requestHeaders">Request Headers</param>
        /// <returns></returns>
        Task<ResponseResult> Authenticate(Login login, string ipAddress, string requestHeaders);


        /// <summary>
        /// To Authenticate External User
        /// </summary>
        /// <param name="login"> login data for user</param>
        /// <param name="ipAddress">IP Address</param>
        /// <param name="requestHeaders">Request Headers</param>
        /// <returns></returns>
        Task<ResponseResult> AuthenticateExternalUser(ExternalUserVM login, string ipAddress, string requestHeaders);

        /// <summary>
        /// Logout
        /// </summary>
        /// <param name="model">Token Request Model</param>
        /// <returns></returns>
        Task<ResponseResult<SuccessMessageModel>> Logout(TokenRequestModel model);

        /// <summary>
        /// Refresh Token
        /// </summary>
        /// <param name="refreshToken">Refresh Token</param>
        /// <returns></returns>
        Task<ResponseResult<RefreshTokenResultModel>> RefreshToken(string refreshToken);

        /// <summary>
        /// Verify Token
        /// </summary>
        /// <param name="tokenModel">Token Model</param>
        /// <param name="accountId">AccountId</param>
        /// <returns></returns>
        Task<ResponseResult> VerifyToken(VerifyTokenModel tokenModel, long accountId);
        /// <summary>
        /// Authenticate
        /// </summary>
        /// <param name="user"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<User> AuthenticateWithMobile(Users user, long accountId);


    }
}
