using Microsoft.AspNetCore.Mvc;

namespace DemansAppWebPro.Controllers
{
    public class AudioBooksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
