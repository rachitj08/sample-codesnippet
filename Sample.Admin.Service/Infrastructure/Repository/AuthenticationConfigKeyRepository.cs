using AutoMapper;
using Sample.Admin.Service.Infrastructure.DataModels;
using Sample.Admin.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace Sample.Admin.Service.Infrastructure.Repository
{
    public class AuthenticationConfigKeyRepository : RepositoryBase<AuthenticationConfigKeys>, IAuthenticationConfigKeyRepository
    {
        /// <summary>
        /// commonHelper Private Member
        /// </summary>
        private readonly ICommonHelper commonHelper;

        /// <summary>
        /// Auto mapper
        /// </summary>
        private readonly IMapper mapper;
        public AuthenticationConfigKeyRepository(ICommonHelper commonHelper, IMapper mapper, CloudAcceleratorContext context) : base(context)
        {
            this.commonHelper = commonHelper;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get All Authentication Config Keys
        /// </summary>
        /// <returns></returns>
        public async Task<List<AuthenticationConfigKeyModel>> GetAuthenticationConfigKeys(string authenticationType)
        {
            var result = await context.AuthenticationConfigKeys.Where(x => x.AuthenticationType == authenticationType).ToListAsync();

            return mapper.Map<List<AuthenticationConfigKeyModel>>(result);
        }
    }
}
