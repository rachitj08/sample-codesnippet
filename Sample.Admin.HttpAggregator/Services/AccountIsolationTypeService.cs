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
    /// AccountIsolationTypeService
    /// </summary>
    public class AccountIsolationTypeService : IAccountIsolationTypeService
    {


        private readonly HttpClient httpClient;

        private readonly ILogger<AccountIsolationTypeService> logger;

        private readonly BaseUrlsConfig urls;


        /// <summary>
        /// Account Isolation Type service constructor to inject required dependency
        /// </summary>
        /// <param name="httpClient">The httpClient</param>
        /// <param name="logger">The logger</param>
        /// <param name="config">The config</param>

        public AccountIsolationTypeService(HttpClient httpClient,

            ILogger<AccountIsolationTypeService> logger,
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
        /// Get All Account Isolation Type
        /// </summary>
        /// <returns></returns>
        public async Task<List<AccountIsolationType>> GetAllAccountIsolationType()
        {
            var httpResponse = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminOperations.GetAccountIsolationTypes());
            if (httpResponse.IsSuccessStatusCode)
            {

                var accountIsolationTypesData = (httpResponse != null) ? JsonConvert.DeserializeObject<List<AccountIsolationType>>(httpResponse.Content.ReadAsStringAsync().Result) : null;
                if (accountIsolationTypesData != null && accountIsolationTypesData.Count > 0)
                {
                    return accountIsolationTypesData;
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
