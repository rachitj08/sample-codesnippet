using Sample.Admin.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sample.Admin.HttpAggregator.Config.OperationsConfig;
using Sample.Admin.HttpAggregator.Config.UrlsConfig;
using Sample.Admin.HttpAggregator.IServices;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities;

namespace Sample.Admin.HttpAggregator.Services
{
    /// <summary>
    /// AuthenticationTypeService
    /// </summary>
    public class AuthenticationTypeService : IAuthenticationTypeService
    {


        private readonly HttpClient httpClient;


        private readonly ILogger<AuthenticationTypeService> logger;


        private readonly BaseUrlsConfig urls;


        /// <summary>
        /// Authentication Service constructor to inject required dependency
        /// </summary>
        /// <param name="httpClient">The httpClient</param>
        /// <param name="logger">The logger service</param>
        /// <param name="config">The Base Urls config</param>
        public AuthenticationTypeService(HttpClient httpClient,

            ILogger<AuthenticationTypeService> logger,
            IOptions<BaseUrlsConfig> config)
        {
            Check.Argument.IsNotNull(nameof(httpClient), httpClient);
            Check.Argument.IsNotNull(nameof(logger), logger);
            Check.Argument.IsNotNull(nameof(config), config);
            this.httpClient = httpClient;
            this.logger = logger;
            this.urls = config.Value;
        }



        /// <summary>
        /// To Get List of All Authentication Types
        /// </summary>
        /// <returns></returns>
        public async Task<List<AuthenticationType>> GetAllAuthenticationTypes()
        {
            var httpResponse = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminOperations.GetAllAuthenticationTypes());
            if (httpResponse.IsSuccessStatusCode)
            {

                var authenticationTypesData = (httpResponse != null) ? JsonConvert.DeserializeObject<List<AuthenticationType>>(httpResponse.Content.ReadAsStringAsync().Result) : null;
                if (authenticationTypesData != null && authenticationTypesData.Count > 0)
                {
                    return authenticationTypesData;
                }
                return null;
            }
            else
            {
                return null;
            }
        }

    }
}
