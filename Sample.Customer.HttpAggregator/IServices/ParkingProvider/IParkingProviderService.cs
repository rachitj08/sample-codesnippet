using Common.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sample.Customer.Model;
using Sample.Customer.Model.Model.ParkingHeads;
using Sample.Customer.Model.Model.ParkingProvider;
using Sample.Customer.Model.Model.Reservation;

namespace Sample.Customer.HttpAggregator.IServices.ParkingProvider
{
    /// <summary>
    /// 
    /// </summary>
    public interface IParkingProviderService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<List<DropDownMaster>>> GetParkingProvider();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerId"></param>
        /// <returns></returns>
        Task<ResponseResult<List<DropDownMaster>>> GetParkingProviderLocation(long providerId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parkingProviderlocationId"></param>
        /// <returns></returns>
        Task<ResponseResult<List<DropDownMaster>>> GetSubLocation(long parkingProviderlocationId);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<List<DropDownMaster>>> GetActivityCode();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="subLocationId"></param>
        /// <returns></returns>
        Task<ResponseResult<List<DropDownMaster>>> GetParkingSpotId(long subLocationId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseResult<bool>> UploadandSaveQR(QRUploadVM model);
        /// <summary>
        /// Get QRList
        /// </summary>
        /// <param name="parkingProviderId"></param>
        /// <returns></returns>
        Task<ResponseResult<List<QRListVM>>> GetQRList(long parkingProviderId);
        /// <summary>
        /// GetSubLocationType
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<List<SubLocationType>>> GetSubLocationType();
        /// <summary>
        /// GetCountry
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<List<CountryVM>>> GetCountry();
        /// <summary>
        /// GetState
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        Task<ResponseResult<List<StateVM>>> GetState(long countryId);
        /// <summary>
        /// GetCity
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        Task<ResponseResult<List<CityVM>>> GetCity(long stateId);
        /// <summary>
        /// GetSource
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<List<DropDownMaster>>> GetSource();
        /// <summary>
        /// To Create New ReservationVM
        /// </summary>
        /// <param name="model">new ReservationVM object</param>
        /// <returns> ReservationVM object</returns>
        Task<ResponseResult<ParkingReserAmountAndLocationVM>> CreateReservation(UpsertProviderReservationVM model);
        /// <summary>
        /// GetProviderReservation
        /// </summary>
        /// <param name="reservationId"></param>
        /// <returns></returns>
        Task<ResponseResult<UpsertProviderReservationVM>> GetProviderReservation(long reservationId);
        /// <summary>
        /// GetAllProviderReservation
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>

        Task<ResponseResult<List<UpsertProviderReservationVM>>> GetAllProviderReservation(long userId);
        Task<ResponseResult<ParkingReservationAmountAndLocationVM>> UpdateReservation(UpsertProviderReservationVM model);
        /// <summary>
        /// GetParkingPriceDetail
        /// </summary>
       
        /// <returns></returns>
        Task<ResponseResult<List<InvoicePriceDetailVM>>> GetParkingPriceDetail(ParkingRateReqVM model);
        /// <summary>
        /// GetParkingSpotByLocationandSpot
        /// </summary>
        /// <param name="providerLocationId"></param>
        /// <param name="spotType"></param>
        /// <returns></returns>
        Task<ResponseResult<IEnumerable<DropDownMaster>>> GetParkingSpotByLocationandSpotType(long providerLocationId, long spotType);
        /// <summary>
        /// GetParkingSpotType
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<IEnumerable<DropDownMaster>>> GetParkingSpotType();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseResult<bool>> CheckInVehicle(CheckInCheckOutVM model);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseResult<bool>> CheckOutVehicle(CheckInCheckOutVM model);
    }
}
