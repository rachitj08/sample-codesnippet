using Common.Model;
using Core.API.ExtensionMethods;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.HttpAggregator.Config.OperationsConfig;
using Sample.Customer.HttpAggregator.Config.UrlsConfig;
using Sample.Customer.HttpAggregator.IServices;
using Sample.Customer.Model;

namespace Sample.Customer.HttpAggregator.Services
{
    /// <summary>
    /// UserVehiclePreferenceCategoryFeaturesService
    /// </summary>
    public class UserVehiclePreferenceCategoryFeaturesService : IUserVehiclePreferenceCategoryFeaturesService
    {

        private readonly HttpClient httpClient;


        private readonly ILogger<UserVehiclePreferenceCategoryFeaturesService> logger;


        private readonly BaseUrlsConfig urls;


        /// <summary>
        /// UsersVehiclePreferenceCategoryFeatures Service constructor to inject required dependency
        /// </summary>
        /// <param name="httpClient">The httpClient</param>
        /// <param name="logger">The logger service</param>
        /// <param name="config">The Base Urls config</param>
        public UserVehiclePreferenceCategoryFeaturesService(HttpClient httpClient,

            ILogger<UserVehiclePreferenceCategoryFeaturesService> logger,
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
        /// Get User Vehicle Category Features
        /// </summary> 
        /// <returns></returns>
        public async Task<ResponseResult<VehicleCategoryAndFeatures>> GetUserVehicleCategoryFeatures()
        { 
            var httpResponseCustomerTask = this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetUserVehicleCategoryFeatures());
            var httpResponseAdmin = await this.httpClient.GetAsync(this.urls.CustomerAPI + AdminAPIOperations.GetVehicleCategoryAndFeatures());
            

            var adminResult = httpResponseAdmin.GetResponseResult<VehicleCategoryAndFeatures>();
            if(adminResult == null || adminResult.ResponseCode != ResponseCode.RecordFetched) return adminResult;

            var httpResponseCustomer = await httpResponseCustomerTask;
            var customeResult = httpResponseCustomer.GetResponseResult<UserVehicleCategoryFeaturesVM>();
            if (customeResult != null && customeResult.ResponseCode == ResponseCode.RecordFetched 
                && customeResult.Data != null)
            {
                if (adminResult.Data.VehicleCategory != null && customeResult.Data.VehicleCategory != null 
                    && adminResult.Data.VehicleCategory.Count() > 0 && customeResult.Data.VehicleCategory.Count() > 0)
                {
                    var finalData = from admin in adminResult.Data.VehicleCategory
                                    join cust in customeResult.Data.VehicleCategory on admin.VehicleCategoryId equals cust.VehicleCategoryId into custData
                                    from c in custData.DefaultIfEmpty()
                                    select new VehicleCategory
                                    {
                                        Icon = admin.Icon,
                                        Name = admin.Name,
                                        VehicleCategoryId = admin.VehicleCategoryId,
                                        SeqNo = (c != null ? c.SqnNo : (short)999)
                                    };
                    adminResult.Data.VehicleCategory = finalData.ToList().OrderBy(x=> x.SeqNo);
                }

                if (adminResult.Data.VehicleFeatures != null && customeResult.Data.VehicleFeatureId != null 
                    && adminResult.Data.VehicleFeatures.Count() > 0 && customeResult.Data.VehicleFeatureId.Count() > 0)
                {
                    var finalData = from admin in adminResult.Data.VehicleFeatures
                                    join cust in customeResult.Data.VehicleFeatureId on admin.VehicleFeatureId equals cust into custData
                                    from c in custData.DefaultIfEmpty()
                                    select new VehicleFeatures
                                    {
                                        Icon = admin.Icon,
                                        Name = admin.Name,
                                        VehicleFeatureId = admin.VehicleFeatureId,
                                        IsSelected = c > 0
                                    };
                    adminResult.Data.VehicleFeatures = finalData.ToList();
                }
            }
            return adminResult;
        }

        /// <summary>
        /// Save User Vehicle Category Features
        /// </summary>
        /// <param name="preferenceCategoryFeaturesVM"></param>
        /// <returns></returns>
        public async Task<ResponseResult<bool>> SaveUserVehicleCategoryFeatures(UserVehicleCategoryFeaturesVM preferenceCategoryFeaturesVM)
        {
            var postContent = new StringContent(JsonConvert.SerializeObject(preferenceCategoryFeaturesVM), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.SaveUserVehicleCategoryFeatures(), postContent);
             
            return httpResponse.GetResponseResult<bool>();
        }
    }
}
