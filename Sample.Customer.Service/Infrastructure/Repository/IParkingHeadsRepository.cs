using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IParkingHeadsRepository
    {
        /// <summary>
        /// Get All ParkingHeadss
        /// </summary>
        /// <param name="ordering"></param>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="all"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        List<ParkingHeads> GetAllParkingHeads();

        /// <summary>
        /// Get Parking Heads List
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<List<ParkingHeads>> GetParkingHeadsList(long accountId);

        /// <summary>
        ///  Get ParkingHeads By ParkingHeadsID
        /// </summary>
        /// <param name="ParkingHeadsId"></param>
        /// <returns></returns>
        public ParkingHeads GetParkingHeadsById(long parkingHeadsId);

        /// <summary>
        /// To Create New ParkingHeads
        /// </summary>
        /// <param name="objParkingHeads">new ParkingHeads object</param>
        /// <returns> ParkingHeads object</returns>
        ParkingHeads CreateParkingHeads(ParkingHeads objParkingHeads, long accountId);

        /// <summary>
        /// To Update ParkingHeads
        /// </summary>
        /// <param name="objParkingHeads">New ParkingHeads object</param>
        /// <returns></returns>
        /// <returns></returns>
        ParkingHeads UpdateParkingHeads(long parkingHeadid, long accountId, ParkingHeads objParkingHeads);


        /// <summary>
        /// DeleteParkingHeads
        /// </summary>
        /// <param name="objParkingHeads"></param>
        /// <returns></returns>
        void DeleteParkingHeads(ParkingHeads objParkingHeads);

    }
}
