using DemansAppWebPro.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DemansAppWebPro.Controllers
{
    [Authorize(Roles = "User")]
    public class U_CommandsController : Controller
    {
        private readonly EntitiesContext db;

        public U_CommandsController(EntitiesContext _db)
        {
            db = _db;
        }
        public IActionResult Index(int id)
        {
            var userInfo = db.Users.Where(w => w.Id == id).FirstOrDefault();

            return View(userInfo);
        }
    }
}
