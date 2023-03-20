using Sample.Admin.HttpAggregator.Config.OperationsConfig;
using Sample.Admin.HttpAggregator.Config.UrlsConfig;
using Sample.Admin.HttpAggregator.IServices;
using Common.Model;
using Sample.Admin.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities;
using Core.API.ExtensionMethods;

namespace Sample.Admin.HttpAggregator.Services
{
    /// <summary>
    /// Authentication Service
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient httpClient;

        private readonly ILogger<AuthenticationService> logger;

        private readonly BaseUrlsConfig urls;

        /// <summary>
        /// Users Service constructor to inject required dependency
        /// </summary>
        /// <param name="httpClient">The httpClient</param>
        /// <param name="logger">The logger service</param>
        /// <param name="config">The Base Urls config</param>
        /// <param name="baseConfig">The Base config</param>
        public AuthenticationService(HttpClient httpClient, ILogger<AuthenticationService> logger, IOptions<BaseUrlsConfig> config, 
            IOptions<BaseConfig> baseConfig)
        {
            Check.Argument.IsNotNull(nameof(httpClient), httpClient);
            Check.Argument.IsNotNull(nameof(logger), logger);
            Check.Argument.IsNotNull(nameof(config), config);
            Check.Argument.IsNotNull(nameof(baseConfig), baseConfig);
            this.httpClient = httpClient;
            this.logger = logger;
            this.urls = config.Value;
        }

        /// <summary>
        /// To Authenticate user
        /// </summary>
        /// <param name="login">object of login parameter which is required to login</param>
        /// <returns></returns>
        public async Task<ResponseResult<LoginAdminUserModel>> Authenticate(LoginModel login)
        {
            var postContent = new StringContent(JsonConvert.SerializeObject(login), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.AdminAPI + AdminOperations.AuthenticateUser(), postContent);
            return httpResponse.GetResponseResult<LoginAdminUserModel>();
        }
    }
}
