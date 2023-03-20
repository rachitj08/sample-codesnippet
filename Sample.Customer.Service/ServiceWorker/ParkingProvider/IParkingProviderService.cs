using Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Model;
using Sample.Customer.Model.Model.ParkingHeads;
using Sample.Customer.Model.Model.ParkingProvider;
using Sample.Customer.Model.Model.Reservation;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.ServiceWorker.ParkingProvider
{
    public interface IParkingProviderService
    {
        ResponseResult<IEnumerable<DropDownMaster>> GetParkingProvider();
        ResponseResult<IEnumerable<DropDownMaster>> GetParkingProviderLocation(long providerId);
        ResponseResult<IEnumerable<DropDownMaster>> GetSubLocation(long parkingProviderlocationId);
        ResponseResult<IEnumerable<DropDownMaster>> GetActivityCode();
        ResponseResult<IEnumerable<DropDownMaster>> GetParkingSpotId(long subLocationId);
        ResponseResult<bool> UploadandSaveQR(QRUploadVM model,long loggedInUserId,long loggedInAccountId);
        ResponseResult<IEnumerable<QRListVM>> GetQRList(long parkingProviderId);
        ResponseResult<IEnumerable<Common.Model.SubLocationType>> GetSubLocationType();

        Task<ResponseResult<UpsertProviderReservationVM>> GetProviderReservation(long reservationId, long userId, long accountId);
        Task<ResponseResult<ParkingReserAmountAndLocationVM>> CreateReservation(UpsertProviderReservationVM model, long userId, long accountId);
        ResponseResult<IEnumerable<CountryVM>> GetAllCountry();
        Task<ResponseResult<IEnumerable<StateVM>>> GetStateByCountryId(long countryId);
        Task<ResponseResult<IEnumerable<CityVM>>> GetCityByStateId(long stateId);
        ResponseResult<IEnumerable<DropDownMaster>> GetSource();
        Task<ResponseResult<List<UpsertProviderReservationVM>>> GetAllProviderReservation(long userId, long accountId);

        Task<ResponseResult<ParkingReservationAmountAndLocationVM>> UpdateReservation(UpsertProviderReservationVM model, long userId, long accountId);
        ResponseResult<List<InvoicePriceDetailVM>> GetParkingPriceDetail(ParkingRateReqVM model, long accountId);

        ResponseResult<IEnumerable<DropDownMaster>> GetParkingSpotByLocationandSpotType(long providerLocationId, long spotType, long accountId);
        ResponseResult<IEnumerable<DropDownMaster>> GetParkingSpotType(long accountId);
        Task<ResponseResult<bool>> CheckInVehicle(CheckInCheckOutVM model, long userId, long accountId);
        Task<ResponseResult<bool>> CheckOutVehicle(CheckInCheckOutVM model, long userId, long accountId);

    }
}
