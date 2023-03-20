using Common.Model;
using Sample.Customer.Model.Model.ParkingHeads;

namespace Sample.Customer.Service.ServiceWorker
{
    public interface IParkingProvidersLocationsService
    {
        /// <summary>
        /// Get All ParkingProvidersLocations
        /// </summary>
        /// <param name="ordering"></param>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="all"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        ResponseResultList<ParkingProvidersLocationsVM> GetAllParkingProvidersLocations();

        /// <summary>
        ///  Get ParkingProvidersLocations By ParkingProvidersLocationsID
        /// </summary>
        /// <param name="ParkingProvidersLocationsId"></param>
        /// <returns></returns>
        public ResponseResult<ParkingProvidersLocationsVM> GetParkingProvidersLocationsById(long parkingProvidersLocationsId);

        /// <summary>
        /// To Create New ParkingProvidersLocations
        /// </summary>
        /// <param name="objParkingProvidersLocations">new ParkingProvidersLocations object</param>
        /// <returns> ParkingProvidersLocations object</returns>
        ParkingProvidersLocationsVM CreateParkingProvidersLocations(ParkingProvidersLocationsVM ojParkingProvidersLocationsVM, long accountId);

        /// <summary>
        /// To Update ParkingProvidersLocations
        /// </summary>
        /// <param name="objParkingProvidersLocations">New ParkingProvidersLocations object</param>
        /// <returns></returns>
        /// <returns></returns>
        ParkingProvidersLocationsVM UpdateParkingProvidersLocations(long parkingHeadid, long accountId, ParkingProvidersLocationsVM ojParkingProvidersLocationsVM);


        /// <summary>
        /// DeleteParkingProvidersLocations
        /// </summary>
        /// <param name="objParkingProvidersLocations"></param>
        /// <returns></returns>
        void DeleteParkingProvidersLocations(ParkingProvidersLocationsVM ojParkingProvidersLocationsVM);

    }
}
