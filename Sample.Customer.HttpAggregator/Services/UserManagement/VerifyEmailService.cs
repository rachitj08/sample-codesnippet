using Common.Model;
using Core.API.ExtensionMethods;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.HttpAggregator.Config.OperationsConfig;
using Sample.Customer.HttpAggregator.Config.UrlsConfig;
using Sample.Customer.HttpAggregator.IServices.UserManagement;
using Sample.Customer.Model;

namespace Sample.Customer.HttpAggregator.Services.UserManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class VerifyEmailService : IVerifyEmailService
    {
        private readonly HttpClient httpClient;


        private readonly ILogger<UserService> logger;


        private readonly BaseUrlsConfig urls;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="logger"></param>
        /// <param name="config"></param>
        public VerifyEmailService(HttpClient httpClient,
            ILogger<UserService> logger,
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
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<ResponseResult<SuccessMessageModel>> VerifyEmail(string token, string uid)
        {
            VerifyEmailVM model = new VerifyEmailVM()
            {
                Token = token,
                Uid = uid
            };
            var postContent = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.VerifyEmail(), postContent);
            return httpResponse.GetResponseResult<SuccessMessageModel>();

            
        }
    }
}
