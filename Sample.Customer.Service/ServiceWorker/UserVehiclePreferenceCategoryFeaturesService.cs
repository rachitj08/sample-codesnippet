using Common.Model;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.Model;
using Sample.Customer.Service.Infrastructure.DataModels;
using Sample.Customer.Service.Infrastructure.Repository;

namespace Sample.Customer.Service.ServiceWorker
{
    public class UserVehiclePreferenceCategoryFeaturesService : IUserVehiclePreferenceCategoryFeaturesService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserVehiclePreferenceFeaturesRepository userVehiclePreferenceFeaturesRepository;
        private readonly IUserVehiclePreferenceCategoryRepository userVehiclePreferenceCategoryRepository;
        public UserVehiclePreferenceCategoryFeaturesService(IUnitOfWork unitOfWork, IUserVehiclePreferenceFeaturesRepository userUserVehiclePreferenceFeaturesRepository, IUserVehiclePreferenceCategoryRepository userVehiclePreferenceCategoryRepository)
        {
            Check.Argument.IsNotNull(nameof(unitOfWork), unitOfWork);
            Check.Argument.IsNotNull(nameof(userVehiclePreferenceFeaturesRepository), userUserVehiclePreferenceFeaturesRepository);
            Check.Argument.IsNotNull(nameof(userVehiclePreferenceCategoryRepository), userVehiclePreferenceCategoryRepository);

            this.unitOfWork = unitOfWork;
            this.userVehiclePreferenceFeaturesRepository = userUserVehiclePreferenceFeaturesRepository;
            this.userVehiclePreferenceCategoryRepository = userVehiclePreferenceCategoryRepository;
        }


        /// <summary>
        /// Get User Vehicle Preference Category Features
        /// </summary> 
        /// <param name="loggedAccountId"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<UserVehicleCategoryFeaturesVM>> GetUserVehicleCategoryFeatures(long loggedAccountId, long loggedInUserId)
        {   
            if(loggedAccountId < 1 || loggedInUserId < 1)
            {
                return new ResponseResult<UserVehicleCategoryFeaturesVM>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed,
                    }
                };
            }
            var category = await userVehiclePreferenceCategoryRepository.GetUserVehiclePreferenceCategoryList(loggedAccountId, loggedInUserId);
            var features = await userVehiclePreferenceFeaturesRepository.GetUserVehiclePreferenceFeatures(loggedAccountId, loggedInUserId);
           
            if (category == null || features == null)
            {
                return new ResponseResult<UserVehicleCategoryFeaturesVM>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed,
                    }
                };
            }

            var data = new UserVehicleCategoryFeaturesVM()
            {
                VehicleCategory = category.Select(x => new VehicleCategoryVM() { SqnNo = x.SeqNo, VehicleCategoryId = x.VehicleCategoryId }).ToList(),
                VehicleFeatureId = features.Select(x => x.VehicleFeatureId).ToArray()
            };

            return new ResponseResult<UserVehicleCategoryFeaturesVM>()
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = data
            };

        }

        /// <summary>
        /// Add UserVehicle Preference Category Features
        /// </summary>
        /// <param name="model"></param>
        /// <param name="loggedAccountId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<bool>> SaveUserVehicleCategoryFeatures(UserVehicleCategoryFeaturesVM model, long loggedAccountId, long loggedInUserId)
        {
            if (model == null)
            {
                return new ResponseResult<bool>()
                {
                    ResponseCode = ResponseCode.ValidationFailed,
                    Message = ResponseMessage.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed
                    }
                };
            }

            var vehicleFeatures = model.VehicleFeatureId.Select(x => new UserVehiclePreferenceFeatures()
            {
                VehicleFeatureId = x,
                AccountId = loggedAccountId,
                LoggedInUserId = loggedInUserId,

                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
                CreatedBy = loggedInUserId,
                UpdatedBy = loggedInUserId,
            }).ToList();

            var vehicleCategories = model.VehicleCategory.Select(x => new UserVehiclePreferenceCategory()
            {
                VehicleCategoryId = x.VehicleCategoryId,
                AccountId = loggedAccountId,
                LoggedInUserId = loggedInUserId,
                SeqNo = x.SqnNo,

                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
                CreatedBy = loggedInUserId,
                UpdatedBy = loggedInUserId

            }).ToList();

            userVehiclePreferenceFeaturesRepository.DeletUserVehiclePreferenceFeatures(loggedAccountId, loggedInUserId);
            userVehiclePreferenceCategoryRepository.DeleteUserVehiclePreferenceCategory(loggedAccountId, loggedInUserId);

            await userVehiclePreferenceFeaturesRepository.AddUserVehiclePreferenceFeatures(vehicleFeatures);
            await userVehiclePreferenceCategoryRepository.AddUserVehiclePreferenceCategory(vehicleCategories);

            if (unitOfWork.CommitWithStatus() < 1)
            {
                return new ResponseResult<bool>()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                };
            }

            return new ResponseResult<bool>()
            {
                Message = ResponseMessage.RecordSaved,
                ResponseCode = ResponseCode.RecordSaved,
                Data = true
            };
          
        }
    }
}