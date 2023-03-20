using Common.Model;
using System.Threading.Tasks;
using Sample.Customer.Model.VinDecoder;

namespace Sample.Customer.Service.ServiceWorker
{
    /// <summary>
    /// 
    /// </summary>
    public interface IVehicleInfoService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vin"></param>
        /// <returns></returns>
        public Task<ResponseResult<VINDecoderResponseVM>> GetVehicleInfo(string vin);
    }
}
