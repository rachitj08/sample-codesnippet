using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Sample.Customer.HttpAggregator.Core
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class TenantBasedSocialLoginOptionsBase<TOptions> : OptionsMonitor<TOptions>
    where TOptions : class, new()
    {
        private readonly IOptionsFactory<TOptions> _factory; 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="sources"></param>
        /// <param name="cache"></param>
        protected TenantBasedSocialLoginOptionsBase(
            IOptionsFactory<TOptions> factory,
            IEnumerable<IOptionsChangeTokenSource<TOptions>> sources,
            IOptionsMonitorCache<TOptions> cache //you can use that to cache settings.
            ) : base(factory, sources, cache)
        {
           // AbpSession = NullAbpSession.Instance;
            _factory = factory;
           // _configuration = configuration;
        }

        /// <summary>
        /// </summary>
        /// <returns>if tenant has social login settings for that auth provider.</returns>
        protected abstract bool DoesCurrentTenantHasSettings();

        /// <summary>
        ///	Set option's properties for current tenant
        /// </summary>
        protected abstract void SetTenantSettings(TOptions options);

        /// <summary>
        ///	Set option's properties for the host
        /// </summary>
        protected abstract void SetHostSettings(TOptions options);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override TOptions Get(string name)
        {
            var options = _factory.Create(name);
            if (name == "jwtBearerIS")
            {
                SetHostSettings(options);
            }
            else
            {
                if (DoesCurrentTenantHasSettings())
                {
                    SetTenantSettings(options);
                }
                else
                {
                    SetHostSettings(options);
                }
            }
            return options;
        }
    }
}
