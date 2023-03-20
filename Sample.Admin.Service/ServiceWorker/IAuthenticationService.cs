using Common.Model;
using Sample.Admin.Model;
using System.Threading.Tasks;

namespace Sample.Admin.Service.ServiceWorker
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Authenticate User
        /// </summary>
        /// <param name="login"></param>
        /// <param name="ipAddress"></param>
        /// <param name="requestHeaders"></param>
        /// <returns></returns>
        Task<ResponseResult<LoginAdminUserModel>> Authenticate(LoginModel login, string ipAddress, string requestHeaders);


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
    }
}
