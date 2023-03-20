
using System;
using Sample.Customer.Model.Model.Reservation;

namespace Sample.Customer.HttpAggregator.Config.OperationsConfig
{
    /// <summary>
    /// User Management Operations
    /// </summary>
    public static class ParkingProviderAPIOperations
    {
        #region Parking Provider
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetParkingProvider() => $"/api/qrgenerator/getparkingprovider/";
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetActivityCode() => $"/api/qrgenerator/getactivitycode/";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerId"></param>
        /// <returns></returns>
        public static string GetParkingProviderLocation(long providerId) => $"/api/qrgenerator/getparkingproviderlocation/{providerId}";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parkingProviderlocationId"></param>
        /// <returns></returns>
        public static string GetSubLocation(long parkingProviderlocationId) => $"/api/qrgenerator/getsublocation/{parkingProviderlocationId}";
        /// <summary>
        /// GetSubLocationType
        /// </summary>
        /// <returns></returns>
        public static string GetSubLocationType() => $"/api/qrgenerator/getsublocationtype/";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="subLocationId"></param>
        /// <returns></returns>
        public static string GetParkingSpotId(long subLocationId) => $"/api/qrgenerator/getparkingspotid/{subLocationId}";
        /// <summary>
        /// Upload and SaveQR
        /// </summary>
        /// <returns></returns>
        public static string UploadandSaveQR() => $"/api/qrgenerator/uploadandsaveqr/";
        /// <summary>
        /// GetQRList
        /// </summary>
        /// <param name="parkingProviderlocationId"></param>
        /// <returns></returns>
        public static string GetQRList(long parkingProviderlocationId) => $"/api/qrgenerator/getqrlist/{parkingProviderlocationId}";

        public static string GetSource() => $"/api/ParkingProvider/getsource/";
        public static string GetCountry() => $"/api/ParkingProvider/getcountry/";
        public static string GetState(long countryId) => $"/api/ParkingProvider/getstate/{countryId}";
        public static string GetCity(long stateId) => $"/api/ParkingProvider/getcity/{stateId}";
        public static string CreateReservation() => $"/api/ParkingProvider/createreservation/";
        public static string UpdateReservation() => $"/api/ParkingProvider/updatereservation/";
        public static string GetProviderReservation(long reservationId) => $"/api/ParkingProvider/getproviderreservation/{reservationId}";
        public static string GetAllProviderReservation(long userId) => $"/api/ParkingProvider/getallproviderreservation/{userId}";
        /// <summary>
        /// GetParkingPriceDetail
        /// </summary>
        /// <returns></returns>
        public static string GetParkingPriceDetail() => $"/api/ParkingProvider/getparkingpricedetail/";
        public static string GetParkingSpotByLocationandSpotType(long providerLocationId, long spotType) => $"/api/ParkingProvider/getparkingspotbylocationandspottype/{providerLocationId}/{spotType}";
        public static string GetParkingSpotType() => $"/api/ParkingProvider/getparkingspottype/";
        public static string CheckInVehicle() => $"/api/ParkingProvider/checkinvehicle/";
        public static string CheckOutVehicle() => $"/api/ParkingProvider/checkoutvehicle/";
        #endregion
    }
}
