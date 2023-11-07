using DemansAppWebPro.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DemansAppWebPro.Controllers
{
    [Authorize(Roles = "User")]
    public class U_ProfileDetailsController : Controller
    {
        private readonly EntitiesContext db;

        public U_ProfileDetailsController(EntitiesContext _db)
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
