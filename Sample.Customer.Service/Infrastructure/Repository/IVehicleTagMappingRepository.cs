using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IVehicleTagMappingRepository
    {
        /// <summary>
        /// GetAllVehicleTagMapping
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        IQueryable<VehicleTagMapping> GetAllVehicleTagMapping(long accountId);

        /// <summary>
        /// Create Vehicle Tag Mapping Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<VehicleTagMapping> CreateVehicleTagMappingDetails(VehicleTagMapping model);

        /// <summary>
        /// GetVehicleTagMappingById
        /// </summary>
        /// <param name="vehicleTagMappingId"></param>
        /// <returns></returns>
        Task<VehicleTagMapping> GetVehicleTagMappingById(long vehicleTagMappingId, long accountId);

        /// <summary>
        /// GetVehicleTagMappingByTagId
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        Task<VehicleTagMapping> GetVehicleTagMappingByTagId(string tagId, long accountId);


        /// <summary>
        /// GetVehicleTagMappingByTagIdAndVehicleId
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        Task<VehicleTagMapping> GetVehicleTagMappingByTagIdAndVehicleId(string tagId, long vehicleId, long accountId);
        /// <summary>
        /// GetVehicleLatestTag
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>

        VehicleTagMapping GetVehicleLatestTag(long vehicleId, long accountId);
        /// <summary>
        /// CreateVehicleTagMappingDetailsNew
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        Task<VehicleTagMapping> CreateVehicleTagMappingDetailsNew(VehicleTagMapping model);
    }
}
