using Common.Model;
using AutoMapper;
using System.Collections.Generic;
using Sample.Customer.Service.Infrastructure.Repository;
using System.Threading.Tasks;
using Sample.Customer.Model.Model;
using System.Linq;

namespace Sample.Customer.Service.ServiceWorker
{
   public class AirportAddressService : IAirportAddressService
    {
       
        private readonly IAirportsRepository airportsRepository;
        private readonly IAddressRepository addressRepository;

        public AirportAddressService( IAirportsRepository airportsRepository,IAddressRepository addressRepository)
        {
            this.airportsRepository = airportsRepository;
            this.addressRepository = addressRepository;
        }

        public ResponseResult<List<AirportAddressVM>> GetAllAirportAddress(long loggedInAccountId,long loggedInUserId)
        {
           // var addressTask =  addressRepository.GetAllAddress(loggedInAccountId, loggedInAccountId);
            var airports = airportsRepository.GetAllAiportsWithAddress(loggedInAccountId);
            var data = airports.Select(x => new AirportAddressVM
            {
                AirportId = x.AirportId,
                Name = x.Name,
                Code = x.Code,
                Streat1 = x.Address.Streat1,
                Streat2 = x.Address.Streat2,
                City = x.Address.City,
                State = x.Address.State,
                Zip = x.Address.Zip,
            }).ToList();
 

            return new ResponseResult<List<AirportAddressVM>>()
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = data
            };
        }

        public async Task<ResponseResult<List<AirportAddressVM>>> GetAirportDetails(long accountId, string[] airportCodes)
        {
            var airports = await airportsRepository.GetAirportDetails(accountId, airportCodes);
            var data = airports.Select(x => new AirportAddressVM
            {
                AirportId = x.AirportId,
                Name = x.Name,
                Code = x.Code,
                Streat1 = x.Address.Streat1,
                Streat2 = x.Address.Streat2,
                City = x.Address.City,
                State = x.Address.State,
                Zip = x.Address.Zip,
            }).ToList();


            return new ResponseResult<List<AirportAddressVM>>()
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = data
            };
        }
    }
}
