using Common.Model;
using Core.API.ExtensionMethods;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
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
using Sample.Customer.Model;
using Sample.Customer.Model.Model;
using Sample.Customer.Model.Model.ParkingHeads;
using Sample.Customer.Model.Model.Reservation;

namespace Sample.Customer.HttpAggregator.Services.Reservation
{
    /// <summary>
    /// ReservationService
    /// </summary>
    public class ReservationService : IReservationService
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
        public ReservationService(HttpClient httpClient,
            IOptions<BaseUrlsConfig> config, IOptions<CommonConfig> commonConfig
            , ICommonHelper commonHelper
            )
        {
            Check.Argument.IsNotNull(nameof(httpClient), httpClient);
            Check.Argument.IsNotNull(nameof(config), config);
            this.httpClient = httpClient;
            this.urls = config.Value;
            _commonConfig = commonConfig.Value;
            _commonHelper = commonHelper;
        }

        /// <summary>
        /// CreateReservation
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<ParkingReserAmountAndLocationVM>> CreateReservation(AddUpdateFlightAndParkingReservationVM model, long userId, long accountId)
        {
            var responseResult = IsInputFormDataIsValid(model);
            if (responseResult.ResponseCode == ResponseCode.ValidationFailed) return responseResult;

            var finalModel = new AddUpdateFlightReservationVM()
            {
                UserId = userId==0 ? model.CustomUserId : userId,
                AccountId = accountId,
                DepaurtureAirportId = model.DepaurtureAirportId,
                DepaurtureDateTime = model.DepaurtureDateTime,
                FlyingToAirline = model.FlyingToAirline,
                FlyingToAirportld = model.FlyingToAirportld,
                FlyingToFlightNo = model.FlyingToFlightNo,
                IsBorrowingCarForRent = model.IsBorrowingCarForRent,
                IsHomeAirport = model.IsHomeAirport,
                ReturnDateTime = model.ReturnDateTime,
                ReturningToAirline = model.ReturningToAirline,
                ReturningToAirportld = model.ReturningToAirportld,
                ReturningToFlightNo = model.ReturningToFlightNo,
                FlightReservationStatus = 1,
            };

            #region [Vatildate Data, Get Price And Address]
            var postContent = new StringContent(JsonConvert.SerializeObject(finalModel), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.CalcuatePriceAndAddressDataByAirportID(), postContent);
            var priceResult = httpResponse.GetResponseResult<ParkingReservationAmountAndLocationVM>();

            responseResult.Message = priceResult.Message;
            responseResult.ResponseCode = priceResult.ResponseCode;
            responseResult.Data = priceResult.Data;
            responseResult.Error = priceResult.Error;

            if (priceResult.ResponseCode != ResponseCode.RecordFetched) return responseResult;
            #endregion

            #region [Create Reservation Kafak Call]
            finalModel.InTimeGap = priceResult.Data.InTimeGap;
            finalModel.OutTimeGap = priceResult.Data.OutTimeGap;
            finalModel.TotalFinalAmount = priceResult.Data.ParkingTotalAmount;
            finalModel.AirportsParkingId = priceResult.Data.AirportsParkingId;
            finalModel.ParkingProvidersLocationId = priceResult.Data.ParkingProvidersLocationId;

            responseResult.ResponseCode = ResponseCode.RecordSaved;
            responseResult.Message = ResponseMessage.RecordSaved;
            #endregion

            return responseResult;
        }

        /// <summary>
        /// DeleteReservation
        /// </summary>
        /// <param name="reservationid"></param>
        /// <returns></returns>
        public async Task<ResponseResult<FlightAndParkingReservationVM>> DeleteReservation(long reservationid)
        {
            var responseResult = new ResponseResult<FlightAndParkingReservationVM>();
            var httpResponse = await this.httpClient.DeleteAsync(this.urls.CustomerAPI + CustomerAPIOperations.DeleteReservation(reservationid));
            if (httpResponse == null || !httpResponse.IsSuccessStatusCode || httpResponse.Content == null)
            {
                responseResult.Message = ResponseMessage.InternalServerError;
                responseResult.ResponseCode = ResponseCode.InternalServerError;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.InternalServerError
                };
                return responseResult;
            }
            responseResult = JsonConvert.DeserializeObject<ResponseResult<FlightAndParkingReservationVM>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (responseResult == null)
            {
                responseResult.Message = ResponseMessage.SomethingWentWrong;
                responseResult.ResponseCode = ResponseCode.SomethingWentWrong;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.SomethingWentWrong
                };
            }
            return responseResult;
        }
        
        /// <summary>
        /// GetAllReservations
        /// </summary>
        /// <param name="ordering"></param>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="all"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<ResponseResultList<FlightAndParkingReservationVM>> GetAllReservations(string ordering, int offset, int pageSize, int pageNumber, bool all, long accountId)
        {
            var responseResult = new ResponseResultList<FlightAndParkingReservationVM>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetAllReservation(ordering, offset, pageSize, pageNumber, all));
            if (httpResponse == null || !httpResponse.IsSuccessStatusCode || httpResponse.Content == null)
            {
                responseResult.Message = ResponseMessage.InternalServerError;
                responseResult.ResponseCode = ResponseCode.InternalServerError;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.InternalServerError
                };
                return responseResult;
            }
            responseResult = JsonConvert.DeserializeObject<ResponseResultList<FlightAndParkingReservationVM>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (responseResult == null)
            {
                responseResult.Message = ResponseMessage.SomethingWentWrong;
                responseResult.ResponseCode = ResponseCode.SomethingWentWrong;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.SomethingWentWrong
                };
            }
            return responseResult;
        }
        
        /// <summary>
        /// GetReservationById
        /// </summary>
        /// <param name="reservationid"></param>
        /// <returns></returns>
        public async Task<ResponseResult<FlightAndParkingReservationVM>> GetReservationById(long reservationid)
        {
            var responseResult = new ResponseResult<FlightAndParkingReservationVM>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetReservationById(reservationid));
            if (httpResponse == null || !httpResponse.IsSuccessStatusCode || httpResponse.Content == null)
            {
                responseResult.Message = ResponseMessage.InternalServerError;
                responseResult.ResponseCode = ResponseCode.InternalServerError;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.InternalServerError
                };
                return responseResult;
            }
            responseResult = JsonConvert.DeserializeObject<ResponseResult<FlightAndParkingReservationVM>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (responseResult == null)
            {
                responseResult.Message = ResponseMessage.SomethingWentWrong;
                responseResult.ResponseCode = ResponseCode.SomethingWentWrong;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.SomethingWentWrong
                };
            }
            return responseResult;
        }

        /// <summary>
        /// Update Reservation
        /// </summary>
        /// <param name="reservationid"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseResult<ParkingReserAmountAndLocationVM>> UpdateReservation(long reservationid, AddUpdateFlightAndParkingReservationVM model)
        {
            var responseResult = IsInputFormDataIsValid(model);
            if (responseResult.ResponseCode == ResponseCode.ValidationFailed) return responseResult;

            var postContent = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PutAsync(this.urls.CustomerAPI + CustomerAPIOperations.UpdateReservation(reservationid), postContent);
            return httpResponse.GetResponseResult<ParkingReserAmountAndLocationVM>();
        }

        /// <summary>
        /// Get Email Itinerary Reservation For Review
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult<List<FlightReservationVM>>> GetEmailItineraryReservationForReview()
        {
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetEmailItineraryReservationForReview());
            return httpResponse.GetResponseResult<List<FlightReservationVM>>();
        }

        /// <summary>
        /// Confirm Email Itinerary Reservation
        /// </summary>
        /// <param name="flightReservationId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<bool>> ConfirmEmailItineraryReservation(long flightReservationId)
        {
            var httpResponse = await this.httpClient.PutAsync(this.urls.CustomerAPI + CustomerAPIOperations.ConfirmEmailItineraryReservation(flightReservationId), null);
            return httpResponse.GetResponseResult<bool>();
        }

        /// <summary>
        /// Create Reservation Activty Code
        /// </summary>
        /// <param name="model"></param>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<ScannedResponseVM>> CreateReservationActivtyCode(ScannedVM model, long accountId, long userId)
        {
            var responseResult = new ResponseResult<ReservationActivityCodeVM>();
            var postContent = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
            var result = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.UpdateCreateReservationActivtyCode(), postContent);
            var response = result.GetResponseResult<ScannedResponseVM>();
            if (response.ResponseCode == ResponseCode.QRCodeInvalid)
            {
                return new ResponseResult<ScannedResponseVM>()
                {
                    Message = ResponseMessage.QRCodeInvalid,
                    ResponseCode = ResponseCode.QRCodeInvalid
                };
            }

            
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reservationid"></param>
        /// <returns></returns>
        public async Task<ResponseResult<bool>> CancelReservation(long reservationid)
        {

            var httpResponse = await this.httpClient.PutAsync(this.urls.CustomerAPI + CustomerAPIOperations.CancelReservation(reservationid), null);

            return httpResponse.GetResponseResult<bool>();

        }

        /// <summary>
        /// IsInputFormDataIsValid
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private ResponseResult<ParkingReserAmountAndLocationVM> IsInputFormDataIsValid(AddUpdateFlightAndParkingReservationVM model)
        {
            var responseResult = new ResponseResult<ParkingReserAmountAndLocationVM>();
            Dictionary<string, string[]> errorData = new Dictionary<string, string[]>();
            if (model != null)
            {
                if (model.DepaurtureDateTime == null || model.DepaurtureDateTime == DateTime.MinValue)
                    errorData.Add("depaurtureDateTime", new[] { "Please enter valid depaurture date time." });
                if (model.ReturnDateTime == null || model.ReturnDateTime == DateTime.MinValue)
                    errorData.Add("returnDateTime", new[] { " Please enter valid return date time." });

                //if (model.DepaurtureDateTime != null && model.DepaurtureDateTime < DateTime.UtcNow)
                //    errorData.Add("depaurtureDateTime", new[] { " Please enter valid depaurture date." });

                if (model.DepaurtureDateTime != null && model.ReturnDateTime != null
                    && model.DepaurtureDateTime > model.ReturnDateTime)
                    errorData.Add("returnDateTime", new[] { " Please enter valid return date." });

                if (model.DepaurtureAirportId == 0)
                    errorData.Add("depaurtureAirportId", new[] { "Please enter valid depaurture airport Id." });
                if (model.ReturningToAirportld == 0)
                    errorData.Add("returningToAirportld", new[] { "Please enter valid return airport Id." });
                if (model.FlyingToAirportld == 0)
                    errorData.Add("flyingToAirportld", new[] { "Please enter valid flying to airport Id." });
                if (string.IsNullOrWhiteSpace(model.FlyingToAirline))
                    errorData.Add("flyingToAirline", new[] { "Please enter valid flying to airline name." });
                if (string.IsNullOrWhiteSpace(model.FlyingToFlightNo))
                    errorData.Add("flyingToFlightNo", new[] { "Please enter valid flying to flight number." });
                if (string.IsNullOrWhiteSpace(model.ReturningToAirline))
                    errorData.Add("returningToAirline", new[] { "Please enter valid return to airline name." });
                if (string.IsNullOrWhiteSpace(model.ReturningToFlightNo))
                    errorData.Add("returningToFlightNo", new[] { "Please enter valid return to flight number." });

                if (errorData != null && errorData.Count > 0)
                {
                    responseResult.Message = ResponseMessage.ValidationFailed;
                    responseResult.ResponseCode = ResponseCode.ValidationFailed;
                    responseResult.Error = new ErrorResponseResult()
                    {
                        Detail = errorData
                    };
                }
            }
            else
            {
                responseResult.Message = "Invalid request. Data model is null";
                responseResult.ResponseCode = ResponseCode.ValidationFailed;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseCode.ValidationFailed
                };
            }
            return responseResult;
        }

        /// <summary>
        /// Get All Ongoing and Upcoming Trips
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult<OngoingUpcomingTripVM>> GetAllOngoingUpcomingTrips()
        {
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetAllOngoingUpcomingTrips());
            return httpResponse.GetResponseResult<OngoingUpcomingTripVM>();
        }

        /// <summary>
        /// Get All Trips
        /// </summary>
        /// <param name="flag">ongoing|upcoming|completed</param>
        /// <returns></returns>
        public async Task<ResponseResult<List<TripDetailVM>>> GetAllTrips(string flag)
        {
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetAllTrips(flag));
            return httpResponse.GetResponseResult<List<TripDetailVM>>();
        }

        /// <summary>
        /// Get Current Activity Code
        /// </summary>
        /// <param name="reservationId"></param>
        /// <returns></returns>
        public async Task<ResponseResultList<CurrentActivityVM>> GetCurrentActivityCode(long reservationId)
        {
            var responseResult = new ResponseResultList<CurrentActivityVM>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetCurrentActivityCode(reservationId));
            if (httpResponse == null || !httpResponse.IsSuccessStatusCode || httpResponse.Content == null)
            {
                responseResult.Message = ResponseMessage.InternalServerError;
                responseResult.ResponseCode = ResponseCode.InternalServerError;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.InternalServerError
                };
                return responseResult;
            }
            var detail = JsonConvert.DeserializeObject<ResponseResultList<CurrentActivityVM>>(httpResponse.Content.ReadAsStringAsync().Result);
            if (detail == null || detail.Data == null)
            {
                responseResult.Message = ResponseMessage.NoRecordFound;
                responseResult.ResponseCode = ResponseCode.NoRecordFound;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.NoRecordFound
                };
                return responseResult;
            }
            detail.Message = ResponseMessage.RecordFetched;
            detail.ResponseCode = ResponseCode.RecordFetched;
            return detail;

        }

        /// <summary>
        /// GenerateReservationInvoiceById
        /// </summary>
        /// <param name="reservationid"></param>
        /// <returns></returns>
        public async Task<ResponseResult<bool>> GenerateReservationInvoiceById(long reservationid)
        {
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.GenerateReservationInvoiceById(reservationid), null);
            return httpResponse.GetResponseResult<bool>();
        }

        /// <summary>
        /// GenerateReservationById
        /// </summary>
        /// <param name="reservationid"></param>
        /// <returns></returns>
        public async Task<ResponseResult<bool>> GenerateReservationById(long reservationid)
        {
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.GenerateReservationInvoiceById(reservationid), null);
            return httpResponse.GetResponseResult<bool>();
        }

        /// <summary>
        /// Get Shuttle ETA
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult<string>> ShuttleETA()
        {
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.ShuttleETA());
            return httpResponse.GetResponseResult<string>();
        }
        /// <summary>
        /// GetReservationVehicleDetailsByVinNumber
        /// </summary>
        /// <param name="reservationId"></param>
        /// <param name="vinNumber"></param>
        /// <returns></returns>
        public async Task<ResponseResult<CarDetailVM>> GetReservationVehicleDetailsByVinNumber(long reservationId, string vinNumber)
        {
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetReservationVehicleDetailsByVinNumber(reservationId, vinNumber));
            return httpResponse.GetResponseResult<CarDetailVM>();
        }
        /// <summary>
        /// Update Parking Reservation
        /// </summary>
        /// <param name="updateIsParkedVM"></param>
        /// <returns></returns>
        public async Task<ResponseResult<int>> UpdateParkingReservation(UpdateIsParkedVM updateIsParkedVM)
        {

            var postContent = new StringContent(JsonConvert.SerializeObject(updateIsParkedVM), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.UpdateParkingReservation(), postContent);
            return httpResponse.GetResponseResult<int>();
        }
        /// <summary>
        /// Get Journey Completed list
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult<OngoingUpcomingTripVM>> GetJourneyCompletedlist()
        {
            var responseResult = new ResponseResult<CarDetailVM>();
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetJourneyCompletedlist());
            return httpResponse.GetResponseResult<OngoingUpcomingTripVM>();
        }

        /// <summary>
        /// Get Shuttle Boarded List
        /// </summary>
        /// <param name="terminalId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<List<ShuttleBoardedListVM>>> GetShuttleBoardedList(long terminalId)
        {
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetShuttleBoardedList(terminalId));            
            return httpResponse.GetResponseResult<List<ShuttleBoardedListVM>>();
        }
        /// <summary>
        /// De-Board From Shuttle
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseResult<int>> DeBoardFromShuttle(DeBoardFromShuttleVM model)
        {
            var postContent = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.DeBoardFromShuttle(),postContent);            
            return httpResponse.GetResponseResult<int>();
        }
        /// <summary>
        /// GetReservationsByParkingLocationId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public async Task<ResponseResult<List<ReservationHistoryVM>>> GetReservationsByParkingLocationId(ReservationSearchVM model)
        {
            var postContent = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetReservationsByParkingLocationId(),postContent);
            return httpResponse.GetResponseResult<List<ReservationHistoryVM>>();
        }

        public async Task<ResponseResult<List<FlightAndParkingReservationVM>>> GetReservationsByUserId(long userId)
        {
            var httpResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetReservationsByUserId(userId));
            return httpResponse.GetResponseResult<List<FlightAndParkingReservationVM>>();
        }

        /// <summary>
        /// CreateReservation
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<ParkingReservationAmountAndLocationVM>> CreateReservationNew(AddUpdateFlightAndParkingReservationVM model, long userId, long accountId)
        {
            var responseResult = IsInputFormDataIsValid(model);




            if (responseResult.ResponseCode == ResponseCode.ValidationFailed)
            {
                var responseResultNew = new ResponseResult<ParkingReservationAmountAndLocationVM>();
                responseResultNew.Message = ResponseMessage.ValidationFailed;
                responseResultNew.ResponseCode = ResponseCode.ValidationFailed;
                if (responseResultNew.Error != null && responseResultNew.Error.Detail != null)
                {
                    responseResultNew.Error = new ErrorResponseResult()
                    {
                        Detail = responseResult.Error.Detail,
                    };
                }
                return responseResultNew;
            }
              

            var finalModel = new AddUpdateFlightReservationVM()
            {
                UserId = userId == 0 ? model.CustomUserId : userId,
                AccountId = accountId,
                DepaurtureAirportId = model.DepaurtureAirportId,
                DepaurtureDateTime = model.DepaurtureDateTime,
                FlyingToAirline = model.FlyingToAirline,
                FlyingToAirportld = model.FlyingToAirportld,
                FlyingToFlightNo = model.FlyingToFlightNo,
                IsBorrowingCarForRent = model.IsBorrowingCarForRent,
                IsHomeAirport = model.IsHomeAirport,
                ReturnDateTime = model.ReturnDateTime,
                ReturningToAirline = model.ReturningToAirline,
                ReturningToAirportld = model.ReturningToAirportld,
                ReturningToFlightNo = model.ReturningToFlightNo,
                FlightReservationStatus = 1,
            };

            
            var postContent = new StringContent(JsonConvert.SerializeObject(finalModel), System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await this.httpClient.PostAsync(this.urls.CustomerAPI + CustomerAPIOperations.CreateReservationNew(), postContent);
            return httpResponse.GetResponseResult<ParkingReservationAmountAndLocationVM>();
        }

    }
}
