using DemansAppWebPro.Helper.DTO.LocationInformation;
using EsPark_WebApplication.Models;
using Microsoft.AspNetCore.Mvc;


namespace DemansAppWebPro.Controllers
{
    public class LocationInformation : Controller
    {
        private readonly EntitiesContext db;

        public LocationInformation(EntitiesContext _db)
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
            var shiftMap = db.LocationInformation.Select(s => new shiftMapRequest
            {
                Lat = s.Lat,
                Lng = s.Lng,
                UserName = db.Users.Where(w => w.Id == s.Id).Select(s => s.UserName).FirstOrDefault(),
                Surname = db.Users.Where(w => w.Id == s.Id).Select(s => s.Surname).FirstOrDefault(),

            }).ToList();

            return Json(new { status= true,  data = shiftMap });


        }
    }

    
}
