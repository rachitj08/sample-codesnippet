using Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.Model.Model;
using Sample.Customer.Service.Infrastructure.DataModels;
using Sample.Customer.Service.Infrastructure.Repository;

namespace Sample.Customer.Service.ServiceWorker
{
   public class TripPaxAndBagsService : ITripPaxAndBagsService
    {
        private readonly ITripPaxAndBagsRepository _tripPaxAndBagsRepository;

      public TripPaxAndBagsService(ITripPaxAndBagsRepository tripPaxAndBagsRepository)
        { 
            Check.Argument.IsNotNull(nameof(tripPaxAndBagsRepository), tripPaxAndBagsRepository);
            this._tripPaxAndBagsRepository = tripPaxAndBagsRepository;
        }

        public async Task<ResponseResult<TripPaxAndBagsVM>> SaveTripPaxAndBags(TripPaxAndBagsVM tripPaxAndBagsVM, long userId, long accountId)
        {
            ResponseResult<TripPaxAndBagsVM> responseResult = new ResponseResult<TripPaxAndBagsVM>();
            try
            {
                if (tripPaxAndBagsVM != null)
                {
                    TripPaxAndBags tripPaxAndBags = new TripPaxAndBags();
                    tripPaxAndBags.FlightReservationId = tripPaxAndBagsVM.FlightReservationId;
                    tripPaxAndBags.UserId = userId;
                    tripPaxAndBags.ActivityCodeId = tripPaxAndBagsVM.ActivityCodeId;
                    tripPaxAndBags.NoOfPassangers = tripPaxAndBagsVM.NoOfPassangers;
                    tripPaxAndBags.CreatedOn = DateTime.UtcNow;
                    tripPaxAndBags.CreatedBy = userId;
                    tripPaxAndBags.NoOfBags = tripPaxAndBagsVM.NoOfBags;
                    tripPaxAndBags.AccountId = accountId;

                    int result = await this._tripPaxAndBagsRepository.SaveTripAndBags(tripPaxAndBags, userId, accountId);
                    
                    if (result > 0)
                    {
                        responseResult.ResponseCode = ResponseCode.RecordSaved;
                        responseResult.Message = ResponseMessage.RecordSaved;
                        responseResult.Data = tripPaxAndBagsVM;
                    }
                    else
                    {
                        responseResult.ResponseCode = ResponseCode.SomethingWentWrong;
                        responseResult.Message = ResponseMessage.SomethingWentWrong;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return responseResult;
        }
    }
}
