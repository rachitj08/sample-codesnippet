using Common.Model;
using Core.API.ExtensionMethods;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.HttpAggregator.Config.OperationsConfig;
using Sample.Customer.HttpAggregator.Config.UrlsConfig;
using Sample.Customer.HttpAggregator.IServices.ParkingProvider;
using Sample.Customer.Model;
using Sample.Customer.Model.Model.ParkingHeads;
using Sample.Customer.Model.Model.ParkingProvider;
using Sample.Customer.Model.Model.Reservation;

namespace Sample.Customer.HttpAggregator.Services.ParkingProvider
{
    /// <summary>
    /// 
    /// </summary>
    public class ParkingProviderService : IParkingProviderService
    {
        private readonly HttpClient httpClient;
        private readonly BaseUrlsConfig urls;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config"></param>
        public ParkingProviderService(HttpClient httpClient, IOptions<BaseUrlsConfig> config)
        {
            Check.Argument.IsNotNull(nameof(httpClient), httpClient);
            Check.Argument.IsNotNull(nameof(config), config);
            this.httpClient = httpClient;
            this.urls = config.Value;
        }
        /// <summary>
        /// CreateReservation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseResult<ParkingReserAmountAndLocationVM>> CreateReservation(UpsertProviderReservationVM model)
        {
            var postContent = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + ParkingProviderAPIOperations.CreateReservation(), postContent);
            return httpResponse.GetResponseResult<ParkingReserAmountAndLocationVM>();
        }
        /// <summary>
        /// UpdateReservation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseResult<ParkingReservationAmountAndLocationVM>> UpdateReservation(UpsertProviderReservationVM model)
        {
            var postContent = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + ParkingProviderAPIOperations.UpdateReservation(), postContent);
            return httpResponse.GetResponseResult<ParkingReservationAmountAndLocationVM>();
        }
       

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<ResponseResult<List<DropDownMaster>>> GetActivityCode()
        {
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetActivityCode());
            return httpResponse.GetResponseResult<List<DropDownMaster>>();
        }

        public async Task<ResponseResult<List<CityVM>>> GetCity(long stateId)
        {
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + ParkingProviderAPIOperations.GetCity(stateId));
            return httpResponse.GetResponseResult<List<CityVM>>();
        }

        public async Task<ResponseResult<List<CountryVM>>> GetCountry()
        {
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + ParkingProviderAPIOperations.GetCountry());
            return httpResponse.GetResponseResult<List<CountryVM>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        
        public async Task<ResponseResult<List<DropDownMaster>>> GetParkingProvider()
        {
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetParkingProvider());
            return httpResponse.GetResponseResult<List<DropDownMaster>>();
        }
        
        public async Task<ResponseResult<List<InvoicePriceDetailVM>>> GetParkingPriceDetail(ParkingRateReqVM model)
        {
            var postContent = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + ParkingProviderAPIOperations.GetParkingPriceDetail(), postContent);
            return httpResponse.GetResponseResult<List<InvoicePriceDetailVM>>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerId"></param>
        /// <returns></returns>
        
        public async Task<ResponseResult<List<DropDownMaster>>> GetParkingProviderLocation(long providerId)
        {
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetParkingProviderLocation(providerId));
            return httpResponse.GetResponseResult<List<DropDownMaster>>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="subLocationId"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<ResponseResult<List<DropDownMaster>>> GetParkingSpotId(long subLocationId)
        {
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetParkingSpotId(subLocationId));
            return httpResponse.GetResponseResult<List<DropDownMaster>>();
        }

        public async Task<ResponseResult<UpsertProviderReservationVM>> GetProviderReservation(long reservationId)
        {
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + ParkingProviderAPIOperations.GetProviderReservation(reservationId));
            return httpResponse.GetResponseResult<UpsertProviderReservationVM>();
        }
    

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parkingProviderlocationId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<List<QRListVM>>> GetQRList(long parkingProviderlocationId)
        {
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetQRList(parkingProviderlocationId));
            return httpResponse.GetResponseResult<List<QRListVM>>();
        }

        public async Task<ResponseResult<List<DropDownMaster>>> GetSource()
        {
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + ParkingProviderAPIOperations.GetSource());
            return httpResponse.GetResponseResult<List<DropDownMaster>>();
        }

        public async Task<ResponseResult<List<StateVM>>> GetState(long countryId)
        {
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + ParkingProviderAPIOperations.GetState(countryId));
            return httpResponse.GetResponseResult<List<StateVM>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parkingProviderlocationId"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<ResponseResult<List<DropDownMaster>>> GetSubLocation(long parkingProviderlocationId)
        {
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetSubLocation(parkingProviderlocationId));
            return httpResponse.GetResponseResult<List<DropDownMaster>>();
        }

        public async Task<ResponseResult<List<SubLocationType>>> GetSubLocationType()
        {
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetSubLocationType());
            return httpResponse.GetResponseResult<List<SubLocationType>>();
        }

        /// <summary>
        /// Upload and SaveQR
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<ResponseResult<bool>> UploadandSaveQR(QRUploadVM model)
        {
            var postContent = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.UploadandSaveQR(),postContent);
            return httpResponse.GetResponseResult<bool>();
        }

        public async Task<ResponseResult<List<UpsertProviderReservationVM>>> GetAllProviderReservation(long userId)
        {
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + ParkingProviderAPIOperations.GetAllProviderReservation(userId));
            return httpResponse.GetResponseResult<List<UpsertProviderReservationVM>>();
        }
        public async Task<ResponseResult<IEnumerable<DropDownMaster>>> GetParkingSpotByLocationandSpotType(long providerLocationId, long spotType)
        {
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + ParkingProviderAPIOperations.GetParkingSpotByLocationandSpotType(providerLocationId,spotType));
            return httpResponse.GetResponseResult<IEnumerable<DropDownMaster>>();
        }
        public async Task<ResponseResult<IEnumerable<DropDownMaster>>> GetParkingSpotType()
        {
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + ParkingProviderAPIOperations.GetParkingSpotType());
            return httpResponse.GetResponseResult<IEnumerable<DropDownMaster>>();
        }
        /// <summary>
        ///CheckInVehicle 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseResult<bool>> CheckInVehicle(CheckInCheckOutVM model)
        {
            var postContent = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + ParkingProviderAPIOperations.CheckInVehicle(), postContent);
            return httpResponse.GetResponseResult<bool>();
        }
        /// <summary>
        /// CheckOutVehicle
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseResult<bool>> CheckOutVehicle(CheckInCheckOutVM model)
        {
            var postContent = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + ParkingProviderAPIOperations.CheckOutVehicle(), postContent);
            return httpResponse.GetResponseResult<bool>();
        }

    }
}
