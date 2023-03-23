using Microsoft.AspNetCore.Mvc;

namespace DemansAppWebPro.Controllers
{
    public class CommandsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
