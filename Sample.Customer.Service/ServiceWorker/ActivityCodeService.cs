using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.Model.Model;
using Sample.Customer.Service.Infrastructure.DataModels;
using Sample.Customer.Service.Infrastructure.Repository;

namespace Sample.Customer.Service.ServiceWorker
{
    public class ActivityCodeService : IActivityCodeService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IActivityCodeRepository _ActivityCodeRepository;
        private readonly IParkingProvidersSubLocationsRepository _parkingProvidersSubLocationsRepository;

        public ActivityCodeService(IUnitOfWork unitOfWork, IActivityCodeRepository ActivityCodeRepository, IParkingProvidersSubLocationsRepository parkingProvidersSubLocationsRepository)
        {
            Check.Argument.IsNotNull(nameof(unitOfWork), unitOfWork);
            Check.Argument.IsNotNull(nameof(ActivityCodeRepository), ActivityCodeRepository);
            Check.Argument.IsNotNull(nameof(parkingProvidersSubLocationsRepository), parkingProvidersSubLocationsRepository);
            this.unitOfWork = unitOfWork;
            this._ActivityCodeRepository = ActivityCodeRepository;
            _parkingProvidersSubLocationsRepository = parkingProvidersSubLocationsRepository;
        }

        public Task<ResponseResult<int>> CreateActivityCode(ActivityCodeVM objActivityCodeVM)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseResult<ActivityCodeVM>> DeleteActivityCode(long activityCodeid)
        {
            throw new NotImplementedException();
        }

        public ResponseResult<ActivityCodeVM> GetActivityCodeById(long activityCodeid)
        {
            ResponseResult<ActivityCodeVM> objResponseResult = new ResponseResult<ActivityCodeVM>();
            ActivityCodeVM objActivityCodeVM = new ActivityCodeVM();
            Infrastructure.DataModels.ActivityCode objActivityCode =  this._ActivityCodeRepository.GetActivityCodeById(activityCodeid);
            if(objActivityCode != null)
            {
                objActivityCodeVM.ActivityCodeId = objActivityCode.ActivityCodeId;
                objActivityCodeVM.Code = objActivityCode.Code;
                objActivityCodeVM.Odering = objActivityCode.Odering;
                objActivityCodeVM.CreatedOn = objActivityCode.CreatedOn;
                objActivityCodeVM.CreatedBy = objActivityCode.CreatedBy;
                objActivityCodeVM.UpdatedOn = objActivityCode.UpdatedOn;
                objActivityCodeVM.UpdatedBy = objActivityCode.UpdatedBy;
                objActivityCodeVM.AccountId = objActivityCode.AccountId;

                objResponseResult.ResponseCode = ResponseCode.RecordFetched;
                objResponseResult.Message = ResponseMessage.RecordFetched;
                objResponseResult.Data = objActivityCodeVM;
            }
            else
            {
                objResponseResult.ResponseCode = ResponseCode.SomethingWentWrong;
                objResponseResult.Message = ResponseMessage.SomethingWentWrong;
            }
            return objResponseResult;
        }

        public ResponseResult<List<ActivityCodeVM>> GetAllActivity(long parkingProviderLocationId,long loggedAccountId)
        {
            var activityData = _ActivityCodeRepository.GetAllActivity(loggedAccountId);
            var subLocation=_parkingProvidersSubLocationsRepository.GetSubLocationByParkingProvide(parkingProviderLocationId);

            List<ActivityCodeVM> result = activityData.Join(subLocation, x => x.ActivityCodeId, y => y.ActivityCodeId, (x, y) => new ActivityCodeVM()
            {
                ActivityCodeId = x.ActivityCodeId,
                Code = x.Code,
                Odering = x.Odering,
                CreatedOn = x.CreatedOn,
                CreatedBy = x.CreatedBy,
                UpdatedOn = x.UpdatedOn,
                UpdatedBy = x.UpdatedBy,
                AccountId = x.AccountId,
                Description = x.Description,
                Name = x.Name,
                EncryptedCode = y.QrcodeEncryptedValue,
                QRCode = y.QrcodeMapping
            }).ToList();
            
            if (result == null || result.Count == 0)
            {
                return new ResponseResult<List<ActivityCodeVM>>
                {
                    ResponseCode = ResponseCode.NoRecordFound,
                    Message = ResponseMessage.NoRecordFound,
                    Data = result,
                };
            }
            return new ResponseResult<List<ActivityCodeVM>>
            {
                ResponseCode = ResponseCode.RecordFetched,
                Message = ResponseMessage.RecordFetched,               
                Data = result,
            };
        }
           
            
        

        public ResponseResultList<ActivityCodeVM> GetAllActivityCodes(string ordering, int offset, int pageSize, int pageNumber, bool all, long accountId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseResult<int>> UpdateActivityCode(ActivityCodeVM objActivityCodeVM, long activityCodeid, long accountId, long userId)
        {
            throw new NotImplementedException();
        }
    }
}
