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
using Sample.Customer.Model.Model;
using Sample.Customer.Model.VinDecoder;

namespace Sample.Customer.HttpAggregator.IServices.VehicleInfo
{
    /// <summary>
    /// 
    /// </summary>
    public class VehicleInfoService : IVehicleInfoService
    {
        private readonly HttpClient httpClient;


        private readonly ILogger<VehicleInfoService> logger;
       

        private readonly BaseUrlsConfig urls;
      /// <summary>
      /// 
      /// </summary>
      /// <param name="httpClient"></param>
      /// <param name="logger"></param>
      /// <param name="config"></param>
        public VehicleInfoService(HttpClient httpClient, ILogger<VehicleInfoService> logger, IOptions<BaseUrlsConfig> config)
        {
            Check.Argument.IsNotNull(nameof(httpClient), httpClient);
            Check.Argument.IsNotNull(nameof(logger), logger);
            Check.Argument.IsNotNull(nameof(config), config);
            this.httpClient = httpClient;
            this.logger = logger;
            this.urls = config.Value;
            this.urls = config.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseResult<VINDecoderResponseVM>> GetVehicleInfo(VINDecoderVM model)
        {
            var postContent = new StringContent(JsonConvert.SerializeObject(model.VIN), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetVehicleInfo(), postContent);
            return httpResponse.GetResponseResult<VINDecoderResponseVM>();
        }
    }
}
