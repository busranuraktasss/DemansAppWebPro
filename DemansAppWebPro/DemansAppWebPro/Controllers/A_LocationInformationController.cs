using DemansAppWebPro.Helper.DTO.LocationInformation;
using DemansAppWebPro.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;


namespace DemansAppWebPro.Controllers
{
    [Authorize(Roles = "Admin")]
    public class A_LocationInformationController : Controller
    {
        private readonly EntitiesContext db;

        public A_LocationInformationController(EntitiesContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ShiftMap()
        {
            var shiftMap = db.LocationInformation
                .Select(s => new shiftMapRequest
            {
                Lat = s.Lat,
                Lng = s.Lng,
                UserName = db.Users.Where(w => w.Id == s.UserId).Select(s => s.UserName).FirstOrDefault(),
                Surname = db.Users.Where(w => w.Id == s.UserId).Select(s => s.Surname).FirstOrDefault(),

            }).ToList();

            return Json(new { status= true,  data = shiftMap });


        }
    }

    
}
