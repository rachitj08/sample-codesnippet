using Sample.Admin.Service.Infrastructure.Repository;
using Sample.Admin.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Admin.Service.ServiceWorker
{
    public class AuthenticationConfigKeyService : IAuthenticationConfigKeyService
    {
        private readonly IAuthenticationConfigKeyRepository repository;

        public AuthenticationConfigKeyService(IAuthenticationConfigKeyRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<AuthenticationConfigKeyModel>> GetAuthenticationConfigKeys(string authenticationType)
        {
            return await repository.GetAuthenticationConfigKeys(authenticationType);
        }
    }
}
