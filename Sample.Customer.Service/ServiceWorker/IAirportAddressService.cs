using Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Model.Model;

namespace Sample.Customer.Service.ServiceWorker
{
    public interface IAirportAddressService
    {
        ResponseResult<List<AirportAddressVM>> GetAllAirportAddress(long loggedInAccountId,long loggedInUserId);

        Task<ResponseResult<List<AirportAddressVM>>> GetAirportDetails(long accountId, string[] airportCodes);
    }
}
