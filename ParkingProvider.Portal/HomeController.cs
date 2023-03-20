using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Net.Codecrete.QrCodeGenerator;
using ParkingProvider.Portal.Data;
using ParkingProvider.Portal.Model;
using System.Linq;
namespace ParkingProvider.Portal
{
    public class HomeController : Controller
    {

        private static readonly QrCode.Ecc[] errorCorrectionLevels = { QrCode.Ecc.Low, QrCode.Ecc.Medium, QrCode.Ecc.Quartile, QrCode.Ecc.High };
        
        public IActionResult Index()
        {
            string url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            
            return View();
        }
      
    }
}
