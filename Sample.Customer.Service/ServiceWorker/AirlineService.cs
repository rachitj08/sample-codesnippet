using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sample.Customer.Model.Model;
using Sample.Customer.Service.Infrastructure.Repository;

namespace Sample.Customer.Service.ServiceWorker
{
    public class AirlineService : IAirlineService
    {
        private readonly IAirlineRepository airlineRepository;

        public AirlineService(IAirlineRepository airlineRepository, IAddressRepository addressRepository)
        {
            this.airlineRepository = airlineRepository;
        }

        public ResponseResult<List<AirlineVM>> GetAllAirlineList(long loggedInAccountId, long loggedInUserId)
        {
            var airlineList = airlineRepository.GetAllAirline().
                Select(x => new AirlineVM
                {
                    AirlineId = x.AirlineId,
                    Code = x.Code,
                    Country = x.Country,
                    Name = x.Name
                }).ToList();

            return new ResponseResult<List<AirlineVM>>
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = airlineList,
            };
        }
    }
}
