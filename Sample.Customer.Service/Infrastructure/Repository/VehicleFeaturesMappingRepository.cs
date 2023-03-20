using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class VehicleFeaturesMappingRepository : RepositoryBase<VehicleFeaturesMapping>, IVehicleFeaturesMappingRepository
    {
        private readonly IMapper mapper;

        public VehicleFeaturesMappingRepository(CloudAcceleratorContext context, IMapper mapper) : base(context)
        {
            this.mapper = mapper;
        }
        public IQueryable<VehicleFeaturesMapping> GetAllVehicleFeaturesMapping(long accountId)
        {
            return GetQuery(x => x.AccountId == accountId);
        }

        public VehicleFeaturesMapping GetVehicleFeaturesMappingById(long vehicleFeaturesMappingId)
        {
            return base.context.VehicleFeaturesMapping.Where(x => x.VehicleFeaturesMappingId == vehicleFeaturesMappingId)
                  .Include(x => x.Vehicle)
                  .Include(x => x.VehicleFeature).FirstOrDefault();
        }

        public async Task<int> SaveVehicleFeaturesMappingDetails(VehicleFeaturesMapping objVehicleFeaturesMapping)
        {
            base.context.VehicleFeaturesMapping.Add(objVehicleFeaturesMapping);
            return await base.context.SaveChangesAsync();
        }
    }
}
