using Sample.Admin.Service.Infrastructure.DataModels;
using Sample.Admin.Model;
using System;
using System.Threading.Tasks;

namespace Sample.Admin.Service.ServiceWorker
{
    public interface ITokenService
    {
        AuthResultModel GenerateAuthToken(AdminUsers user);
        string GenerateJwtToken(AdminUsers user, out Guid tokenId);
        string VerifyToken(RefreshTokenModel storedToken);
    }
}
