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
        public async Task<JsonResult> AddCompanion(addCompanionsRequest request)
        {
            try
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
                await db.SaveChangesAsync();

                return Json(new { Status = true, Messages = "Ekleme işlemi hatalı." });
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", Messages = ex.Message });
            }
        }
    }
}
