using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{

    /// <summary>
    /// VehicleTagMappingRepository
    /// </summary>
    public class VehicleTagMappingRepository : RepositoryBase<VehicleTagMapping>, IVehicleTagMappingRepository
    {
        public VehicleTagMappingRepository(CloudAcceleratorContext context) : base(context)
        {

        }

        /// <summary>
        /// GetAllActivityCodes
        /// </summary>
        /// <param name="ordering"></param>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="all"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public IQueryable<VehicleTagMapping> GetAllVehicleTagMapping(long accountId)
        {
            //return (List<Vehicles>)base.GetAll();
            //return GetQuery(x => x.AccountId == accountId);
            return null;
        }

        /// <summary>
        /// Create Vehicle Tag Mapping Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<VehicleTagMapping> CreateVehicleTagMappingDetails(VehicleTagMapping model)
        {
            return await CreateAsync(model);
        }

        /// <summary>
        /// GetVehicleTagMappingById
        /// </summary>
        /// <param name="vehicleTagMappingId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<VehicleTagMapping> GetVehicleTagMappingById(long vehicleTagMappingId, long accountId)
        {
            return await GetQuery(x => x.VehicleTagMappingId == vehicleTagMappingId && x.AccountId == accountId, "Vehicle").FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get Vehicle Tag Mapping By TagId
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<VehicleTagMapping> GetVehicleTagMappingByTagId(string tagId, long accountId)
        {
            return await GetQuery(x => x.TagId == tagId && x.AccountId == accountId, "Vehicle").FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get Vehicle Tag Mapping By TagId and VehicleId
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<VehicleTagMapping> GetVehicleTagMappingByTagIdAndVehicleId(string tagId, long vehicleId, long accountId)
        {
            return await GetQuery(x => x.TagId == tagId && x.AccountId == accountId && x.VehicleId == vehicleId, "Vehicle").FirstOrDefaultAsync();
        }
        /// <summary>
        /// Get Vehicle Tag Mapping By TagId and VehicleId
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public VehicleTagMapping GetVehicleLatestTag( long vehicleId, long accountId)
        {
            return Find(x =>  x.AccountId == accountId && x.VehicleId == vehicleId ).OrderByDescending(x=> x.CreatedOn).FirstOrDefault();
        }

        public async Task<VehicleTagMapping> CreateVehicleTagMappingDetailsNew(VehicleTagMapping model)
        {
            var VehicleTagMapping= await CreateAsync(model);
            await context.SaveChangesAsync();
            return VehicleTagMapping;
        }
    }
}
