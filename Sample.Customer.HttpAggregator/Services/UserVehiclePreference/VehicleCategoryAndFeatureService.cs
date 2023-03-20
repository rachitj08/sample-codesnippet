using Common.Model;
using Core.API.ExtensionMethods;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities;
using Utility;
using Sample.Customer.HttpAggregator.Config.OperationsConfig;
using Sample.Customer.HttpAggregator.Config.UrlsConfig;
using Sample.Customer.HttpAggregator.IServices;
using Sample.Customer.Model;

namespace Sample.Customer.HttpAggregator.Services
{
    /// <summary>
    /// Vehicle category and feature service
    /// </summary>
    public class VehicleCategoryAndFeatureService: IVehicleCategoryAndFeatureService
    {
        private readonly HttpClient httpClient;

        private readonly ILogger<VehicleCategoryAndFeatureService> logger;

        private readonly ICommonHelper commonHelper;


        private readonly BaseUrlsConfig urls;



        /// <summary>
        /// Vehicle category and feature service constructor to inject required dependency
        /// </summary>
        /// <param name="httpClient">The httpClient</param>
        /// <param name="commonHelper">The Common Helper</param>
        /// <param name="logger">The logger service</param>
        /// <param name="config">The Base Urls config</param>

        public VehicleCategoryAndFeatureService(HttpClient httpClient,
           ICommonHelper commonHelper,
           ILogger<VehicleCategoryAndFeatureService> logger,
           IOptions<BaseUrlsConfig> config)
        {
            Check.Argument.IsNotNull(nameof(httpClient), httpClient);
            Check.Argument.IsNotNull(nameof(commonHelper), commonHelper);
            Check.Argument.IsNotNull(nameof(logger), logger);
            Check.Argument.IsNotNull(nameof(config), config);

            this.commonHelper = commonHelper;
            this.httpClient = httpClient;
            this.logger = logger;
            this.urls = config.Value;
        }

        /// <summary>
        /// Get Vehicle category and feature service method
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult<VehicleCategoryAndFeatures>> GetVehicleCategoryAndFeatures()
        {
            var responseResult = new ResponseResult<VehicleCategoryAndFeatures>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetVehicleCategoryAndFeature());

           return httpResponse.GetResponseResult<VehicleCategoryAndFeatures>();
            
        }
    }
}
