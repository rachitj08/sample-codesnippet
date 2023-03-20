using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IParkingProvidersLocationsRepository
    {
       
        /// <summary>
        /// Get All ParkingProvidersLocationss
        /// </summary>
        /// <param name="ordering"></param>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="all"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        List<ParkingProvidersLocations> GetAllParkingProvidersLocations();

        /// <summary>
        ///  Get ParkingProvidersLocations By ParkingProvidersLocationsID
        /// </summary>
        /// <param name="ParkingProvidersLocationsId"></param>
        /// <returns></returns>
        public List<ParkingProvidersLocations> GetParkingProvidersLocationsById(long parkingProvidersLocationsId);

        /// <summary>
        ///  Get ParkingProvidersLocations By ParkingProvidersLocationsID
        /// </summary>
        /// <param name="ParkingProvidersLocationsId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<ParkingProvidersLocations> GetParkingProvidersLocationsById(long parkingProvidersLocationsId, long accountId);

        /// <summary>
        /// To Create New ParkingProvidersLocations
        /// </summary>
        /// <param name="objParkingProvidersLocations">new ParkingProvidersLocations object</param>
        /// <returns> ParkingProvidersLocations object</returns>
        ParkingProvidersLocations CreateParkingProvidersLocations(ParkingProvidersLocations objParkingProvidersLocations, long accountId);

        /// <summary>
        /// To Update ParkingProvidersLocations
        /// </summary>
        /// <param name="objParkingProvidersLocations">New ParkingProvidersLocations object</param>
        /// <returns></returns>
        /// <returns></returns>
        ParkingProvidersLocations UpdateParkingProvidersLocations(long parkingHeadid, long accountId, ParkingProvidersLocations objParkingProvidersLocations);


        /// <summary>
        /// DeleteParkingProvidersLocations
        /// </summary>
        /// <param name="objParkingProvidersLocations"></param>
        /// <returns></returns>
        void DeleteParkingProvidersLocations(ParkingProvidersLocations objParkingProvidersLocations);

        /// <summary>
        /// GetParkingProvidersLocationsQuery
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        IQueryable<ParkingProvidersLocations> GetParkingProvidersLocationsQuery(long accountId);
    }
}
