using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IParkingHeadsCustomRateRepository
    {
        /// <summary>
        /// GetAllParkingHeadsCustomRate
        /// </summary>
        /// <returns></returns>
        Task<List<ParkingHeadsCustomRate>> GetAllParkingHeadsCustomRate(long reservationId);

  

        /// <summary>
        /// To Create New ParkingHeadsCustomRate
        /// </summary>
        /// <param name="objParkingHeadsCustomRate">new ParkingHeadsCustomRate object</param>
        /// <returns> ParkingHeadsCustomRate object</returns>
        Task<int> CreateParkingHeadsCustomRate(ParkingHeadsCustomRate objParkingHeadsCustomRate, long accountId);

        /// <summary>
        /// To Update ParkingHeadsCustomRate
        /// </summary>
        /// <param name="objParkingHeadsCustomRate">New ParkingHeadsCustomRate object</param>
        /// <returns></returns>
        /// <returns></returns>
        Task<int> UpdateParkingHeadsCustomRate(ParkingHeadsCustomRate objParkingHeadsCustomRate);
        
    }
}
