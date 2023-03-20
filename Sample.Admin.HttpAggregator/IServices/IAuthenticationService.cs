using Common.Model;
using Sample.Admin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Admin.HttpAggregator.IServices
{
    /// <summary>
    /// Authenticate Admin User Service
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Authenticate Admin User
        /// </summary>
        Task<ResponseResult<LoginAdminUserModel>> Authenticate(LoginModel login);
    }
}
