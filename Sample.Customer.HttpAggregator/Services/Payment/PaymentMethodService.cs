using Common.Model;
using Core.API.ExtensionMethods;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Stripe;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.HttpAggregator.Config.OperationsConfig;
using Sample.Customer.HttpAggregator.Config.UrlsConfig;
using Sample.Customer.HttpAggregator.IServices.Payment;
using Sample.Customer.Model.Model;

namespace Sample.Customer.HttpAggregator.Services.Payment
{
    /// <summary>
    /// 
    /// </summary>
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly HttpClient httpClient;
        private readonly BaseUrlsConfig urls;

        /// <summary>
        /// Payment Method Service
        /// </summary>
        /// <param name="httpClient"></param>
        public PaymentMethodService(HttpClient httpClient, IOptions<BaseUrlsConfig> config)
        {
            Check.Argument.IsNotNull(nameof(httpClient), httpClient);
            Check.Argument.IsNotNull(nameof(config), config);
            this.urls = config.Value;
            this.httpClient = httpClient;
        }
        /// <summary>
        /// Create Payment Method
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<ResponseResult<PaymentResponseVM>> CreatePaymentMethod(CardRequestVM model)
        {
            var postContent = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.CreatepaymentMethodNew(), postContent);
            return httpResponse.GetResponseResult<PaymentResponseVM>();
            
        }
        /// <summary>
        /// Check Valid Payment
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult<bool>> FetchValidPaymentMethod()
        {
            
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.FetchValidPaymentMethod());
            return httpResponse.GetResponseResult<bool>();
        }
    }
}
