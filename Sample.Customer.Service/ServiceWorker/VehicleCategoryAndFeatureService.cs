using AutoMapper;
using Common.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utility;
using Sample.Customer.Model;
using Sample.Customer.Service.Infrastructure.Repository;

namespace Sample.Customer.Service.ServiceWorker
{
    public class VehicleCategoryAndFeatureService : IVehicleCategoryAndFeatureService
    {
        private readonly IMapper mapper;
        private readonly IVehicleCategoryRepository vehicleCategoryRepository;
        private readonly IVehicleFeaturesRepository vehicleFeaturesRepository;
        /// <summary>
        /// commonHelper Private Member
        /// </summary>
        private readonly ICommonHelper commonHelper;
      
        /// <summary>
        /// vehicle category and feature service constructor
        /// </summary>
        /// <param name="vehicleFeaturesRepository"></param>
        /// <param name="vehicleCategoryRepository"></param>
        /// <param name="commonHelper"></param>
        /// <param name="mapper"></param>
        public VehicleCategoryAndFeatureService(IVehicleFeaturesRepository vehicleFeaturesRepository, IVehicleCategoryRepository vehicleCategoryRepository, ICommonHelper commonHelper, IMapper mapper)
        {
            this.vehicleFeaturesRepository = vehicleFeaturesRepository;
            this.vehicleCategoryRepository = vehicleCategoryRepository;
            this.commonHelper = commonHelper;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get Vehicle category and feature service method
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult<VehicleCategoryAndFeatures>> GetVehicleCategoryAndFeatures(long accountId)
        {
            var result = new VehicleCategoryAndFeatures();
            var category = await this.vehicleCategoryRepository.GetAllVehiclesCategory(accountId);
            var features = await this.vehicleFeaturesRepository.GetAllVehiclesFeatures(accountId);
            if (category != null)
            {
                result.VehicleCategory = mapper.Map<IEnumerable<Model.VehicleCategory>>(category);
            }

            if (features != null)
            {
                result.VehicleFeatures = mapper.Map<IEnumerable<Model.VehicleFeatures>>(features);
            }

            if (result.VehicleCategory == null || result.VehicleFeatures == null)
            {
                return new ResponseResult<VehicleCategoryAndFeatures>()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound
                    }
                };
            }
            else
            {
                return new ResponseResult<VehicleCategoryAndFeatures>()
                {
                    Message = ResponseMessage.RecordFetched,
                    ResponseCode = ResponseCode.RecordFetched,
                    Data=result
                };
            }
        }

    }
}
