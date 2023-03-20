using Common.Model;
using Core.API.ExtensionMethods;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities;
using Utility;
using Sample.Customer.HttpAggregator.Config.OperationsConfig;
using Sample.Customer.HttpAggregator.Config.UrlsConfig;
using Sample.Customer.HttpAggregator.IServices.Reservation;
using Sample.Customer.Model.Model;
using Sample.Customer.Model.Model.Reservation;

namespace Sample.Customer.HttpAggregator.Services.Reservation
{
    /// <summary>
    /// VehiclesService
    /// </summary>
    public class VehiclesService : IVehiclesService
    {
        private readonly HttpClient httpClient;
        private readonly BaseUrlsConfig urls;
        private readonly CommonConfig _commonConfig;
        private readonly ICommonHelper _commonHelper;

        /// <summary>
        ///  Resercation Service
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="_kafkaConfig"></param>
        /// <param name="config"></param>
        /// <param name="commonConfig"></param>
        /// <param name="commonHelper"></param>
        public VehiclesService(HttpClient httpClient,
            IOptions<BaseUrlsConfig> config, IOptions<CommonConfig> commonConfig, ICommonHelper commonHelper)
        {
            Check.Argument.IsNotNull(nameof(httpClient), httpClient);
            Check.Argument.IsNotNull(nameof(config), config);
            this.httpClient = httpClient;
            this.urls = config.Value;
            _commonConfig = commonConfig.Value;
            _commonHelper = commonHelper;
        }

       


        /// <summary>
        /// Get All Cars
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="parkingProviderLocationId"></param>
        /// <param name="searchType"></param>
        /// <returns></returns>
        public async Task<ResponseResult<InComingOutGoingCarsVM>> GetAllCars(string flag, long parkingProviderLocationId, int searchType)
        {
            var responseResult = new ResponseResult<InComingOutGoingCarsVM>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetAllCars(flag, parkingProviderLocationId,searchType));
            return httpResponse.GetResponseResult<InComingOutGoingCarsVM>();           
        }
        /// <summary>
        /// Get Car Count
        /// </summary>
        /// <param name="inputDateValue"></param>
        /// <param name="parkingProviderLocationId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<CarDetailCountVM>> GetCarCount(DateTime inputDateValue, long parkingProviderLocationId)
        {
            string date = inputDateValue.ToString("yyyy-MM-dd");
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetCarCount(date, parkingProviderLocationId));
            return httpResponse.GetResponseResult<CarDetailCountVM>();
           
        }
        /// <summary>
        /// Get Day Wise Count
        /// </summary>
        /// <param name="parkingProviderLocationId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<DayWiseCarCountVM>> GetDayWiseCount(long parkingProviderLocationId)
        {
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetDayWiseCount(parkingProviderLocationId));
            return httpResponse.GetResponseResult<DayWiseCarCountVM>();
           
        }

        /// <summary>
        /// GetVehicleDetailByTagId
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<VehicleDetailVM>> GetVehicleDetailByTagId(string tagId)
        {
            if (string.IsNullOrWhiteSpace(tagId))
            {
                return new ResponseResult<VehicleDetailVM>()
                {
                    Message = "Tag Id is null",
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed
                    }
                };
            }
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetVehicleDetailByTagId(tagId));
            return httpResponse.GetResponseResult<VehicleDetailVM>();
        }

        /// <summary>
        /// CreateReservationVehicle
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseResult<bool>> CreateReservationVehicle(CreateReservationVehicleReqVM model)
        {
            if (model == null)
            {
                return new ResponseResult<bool>()
                {
                    Message = "View Model is null",
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed
                    }
                };
            }

            var postContent = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.CreateReservationVehicle(), postContent);
            return httpResponse.GetResponseResult<bool>();
        }

        /// <summary>
        /// CreateVehicle
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseResult<bool>> CreateVehicle(CreateVehicleReqVM model)
        {
            if (model == null)
            {
                return new ResponseResult<bool>()
                {
                    Message = "View Model is null",
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed
                    }
                };
            }

            var postContent = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.CreateVehicle(), postContent);
            return httpResponse.GetResponseResult<bool>();
        }
       
    }
}
