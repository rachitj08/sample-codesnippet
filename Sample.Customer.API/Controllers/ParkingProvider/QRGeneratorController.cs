using Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.Model;
using Sample.Customer.Service.ServiceWorker.ParkingProvider;

namespace Sample.Customer.API.Controllers.ParkingProvider
{
    [Route("api/[controller]")]
    [ApiController]
    public class QRGeneratorController : BaseApiController
    {
        private readonly IParkingProviderService parkingProviderService;

        public QRGeneratorController(IParkingProviderService parkingProviderService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(parkingProviderService), parkingProviderService);
            this.parkingProviderService = parkingProviderService;
        }

        [Route("getparkingprovider")]
        [HttpGet]
        public async Task<IActionResult> GetParkingProvider()
        {
            return await Execute(async () =>
            {
                var userGroups = this.parkingProviderService.GetParkingProvider();
                return Ok(userGroups);
            });

        }
        [Route("getactivitycode")]
        [HttpGet]
        public async Task<IActionResult> GetActivityCode()
        {
            return await Execute(async () =>
            {
                var userGroups = this.parkingProviderService.GetActivityCode();
                return Ok(userGroups);
            });

        }
        [Route("getparkingproviderlocation/{providerId}")]
        [HttpGet]
        public async Task<IActionResult> GetParkingProviderLocation(long providerId)
        {
            return await Execute(async () =>
            {
                var userGroups = this.parkingProviderService.GetParkingProviderLocation(providerId);
                return Ok(userGroups);
            });

        }
        [Route("getsublocation/{parkingProviderlocationId}")]
        [HttpGet]
        public async Task<IActionResult> GetSubLocation(long parkingProviderlocationId)
        {
            return await Execute(async () =>
            {
                var userGroups = this.parkingProviderService.GetSubLocation(parkingProviderlocationId);
                return Ok(userGroups);
            });

        }
        [Route("getparkingspotid/{subLocationId}")]
        [HttpGet]
        public async Task<IActionResult> GetParkingSpotId(long subLocationId)
        {
            return await Execute(async () =>
            {
                var userGroups = this.parkingProviderService.GetParkingSpotId(subLocationId);
                return Ok(userGroups);
            });

        }
        [Route("uploadandsaveqr")]
        [HttpPost]
        public async Task<IActionResult> UploadandSaveQR(QRUploadVM model)
        {
            return await Execute(async () =>
            {
                var userGroups = this.parkingProviderService.UploadandSaveQR(model,loggedInAccountId,loggedInUserId);
                return Ok(userGroups);
            });

        }
        [Route("getqrlist/{parkingProviderlocationId}")]
        [HttpGet]
        public async Task<IActionResult> GetQRList(long parkingProviderlocationId)
        {
            return await Execute(async () =>
            {
                var userGroups = this.parkingProviderService.GetQRList(parkingProviderlocationId);
                return Ok(userGroups);
            });

        }
        [Route("getsublocationtype")]
        [HttpGet]
        public async Task<IActionResult> GetSubLocationType()
        {
            return await Execute(async () =>
            {
                var userGroups = this.parkingProviderService.GetSubLocationType();
                return Ok(userGroups);
            });

        }
    }
}
