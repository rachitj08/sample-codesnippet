using Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Model;
using Sample.Customer.Model.Model.ParkingHeads;

namespace Sample.Customer.Service.ServiceWorker
{
    public interface IParkingHeadsService
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
        ResponseResultList<ParkingHeadsVM> GetAllParkingHeads();

        /// <summary>
        ///  Get ParkingHeads By ParkingHeadsID
        /// </summary>
        /// <param name="ParkingHeadsId"></param>
        /// <returns></returns>
        public ParkingHeadsVM GetParkingHeadsById(long parkingHeadsId);

        /// <summary>
        /// To Create New ParkingHeads
        /// </summary>
        /// <param name="objParkingHeads">new ParkingHeads object</param>
        /// <returns> ParkingHeads object</returns>
        ParkingHeadsVM CreateParkingHeads(ParkingHeadsVM objParkingHeads, long accountId);

        /// <summary>
        /// To Update ParkingHeads
        /// </summary>
        /// <param name="objParkingHeads">New ParkingHeads object</param>
        /// <returns></returns>
        /// <returns></returns>
        ParkingHeadsVM UpdateParkingHeads(long parkingHeadid, long accountId, ParkingHeadsVM objParkingHeads);


        /// <summary>
        /// DeleteParkingHeads
        /// </summary>
        /// <param name="objParkingHeads"></param>
        /// <returns></returns>
        void DeleteParkingHeads(ParkingHeadsVM objParkingHeads);

        /// <summary>
        /// GetParkingPriceDetail
        /// </summary>
        /// <param name="parkingProviderLocationId"></param>
        /// <param name="reservationStartDateTime"></param>
        /// <param name="reservationEndDateTime"></param>
        /// <param name="accountId"></param>
        /// <param name="isCustom"></param>
        /// <param name="reservationId"></param>
        /// <returns></returns>
        ResponseResult<List<InvoicePriceDetailVM>> GetParkingPriceDetail(long parkingProviderLocationId, DateTime reservationStartDateTime, DateTime reservationEndDateTime, long accountId, bool isCustom, long reservationId);
    }
}
