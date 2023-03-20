using Common.Model;
using System.Threading.Tasks;
using Sample.Customer.Model.Model;
using Sample.Customer.Model.VinDecoder;

namespace Sample.Customer.HttpAggregator.IServices.VehicleInfo
{
    /// <summary>
    /// 
    /// </summary>
    public interface IVehicleInfoService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseResult<VINDecoderResponseVM>> GetVehicleInfo(VINDecoderVM model);
    }
}
