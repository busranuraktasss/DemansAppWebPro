using Microsoft.AspNetCore.Mvc;

namespace DemansAppWebPro.Controllers
{
    public class Pictures : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
