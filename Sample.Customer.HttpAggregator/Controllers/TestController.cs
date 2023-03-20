using Microsoft.AspNetCore.Mvc;

namespace Sample.Customer.HttpAggregator.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return Content("This is test controller");
        }
    }
}
