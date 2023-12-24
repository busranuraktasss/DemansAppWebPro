using DemansAppWebPro.Helper.DTO.Companions;
using DemansAppWebPro.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DemansAppWebPro.Controllers
{
    [Authorize(Roles = "User")]
    public class U_CompanionDetailsController : Controller
    {
        private readonly EntitiesContext db;

        public U_CompanionDetailsController(EntitiesContext _db)
        {
            db = _db;
        }
        public IActionResult Index(int id)
        {
            var userInfo = db.Users.Where(w => w.Id == id).FirstOrDefault();

            return View(userInfo);
        }
        [HttpPost]
        public JsonResult ShowCompanion(int userId)
        {
            try
            {
                var _companion = db.Companions.Where(w => w.UserId == userId).FirstOrDefault();
                if(_companion == null) { return Json(new { status = false, msg = "Refakatçı Bilgisine Ulaşılamadı." }); }

                return Json(new { status = true, data =  _companion }); 
            }
            catch (Exception ex)
            {
                return Json(new {status = false, msg = ex.Message});
            }
        }

        [HttpPost]
        public JsonResult AddCompanion(addCompanionsRequest request)
        {
            try
            {
                var _c = db.Companions.Where(w => w.UserId == request.UserId).FirstOrDefault();
                if(_c == null)
                {
                    var _companions = new Companions()
                    {
                        Name = request.Name,
                        Surname = request.Surname,
                        Adress = request.Adress,
                        Email = request.Email,
                        Sex = request.Sex,
                        Phone = request.Phone,
                        UserId = request.UserId,
                        Status = 1
                    };
                    db.Companions.Add(_companions);
                    db.SaveChanges();
                }
                else
                {
                    _c.Name = request.Name;
                    _c.Surname = request.Surname;
                    _c.Adress = request.Adress;
                    _c.Email = request.Email;
                    _c.Sex = request.Sex;
                    _c.Phone = request.Phone;

                    db.Companions.Update(_c);
                    db.SaveChanges();
                }

                return Json(new { Status = true, Messages = "Refakatçı başarıyla güncellendi." });
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", Messages = ex.Message });
            }
        }
    }
}
