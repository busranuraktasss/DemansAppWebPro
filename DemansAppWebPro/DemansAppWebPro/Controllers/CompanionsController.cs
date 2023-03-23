using Microsoft.AspNetCore.Mvc;

namespace DemansAppWebPro.Controllers
{
    public class CompanionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
