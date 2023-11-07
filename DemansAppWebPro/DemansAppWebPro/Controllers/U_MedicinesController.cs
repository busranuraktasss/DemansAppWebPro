using DemansAppWebPro.Helper.DTO.Medicines;
using DemansAppWebPro.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DemansAppWebPro.Controllers
{
    [Authorize(Roles = "User")]
    public class U_MedicinesController : Controller
    {
        private readonly EntitiesContext db;

        public U_MedicinesController(EntitiesContext _db)
        {
            db = _db;
        }
        public IActionResult Index(int id)
        {
            var userInfo = db.Users.Where(w => w.Id == id).FirstOrDefault();

            return View(userInfo);
        }

        [HttpPost]
        public JsonResult ShowMedicines(int sId)
        {
            try
            {
                sId = 1;
                var _medicines_name = db.Medicines.Where(w => w.Id == sId).Select(s => s.Name).FirstOrDefault();
                var _medicines_list = db.Medicines.Where(w => w.Name == _medicines_name).ToList();

                var showMedicineRequest = new List<MedicineRequest>();
                for (var i = 0; i < _medicines_list.Count(); i++)
                {
                    var list = new MedicineRequest();
                    var _user_id = _medicines_list[i].UserId;

                    if (_user_id != null)
                    {
                        var medicine_list = db.Medicines.Where(w => w.Name == _medicines_name && w.UserId == _user_id).FirstOrDefault();
                        var user_list = db.Users.Where(w => w.Id == _user_id).Select(s => new { s.UserName, s.Surname }).FirstOrDefault();

                        //medicine Id
                        list.Id = medicine_list.Id;
                        list.UserName = _medicines_name;
                        list.Surname = user_list.Surname;

                        list.Moon = medicine_list.Moon == true ? "Sabah" : " ";
                        if (medicine_list.Moon == true)
                        {
                            list.Afternoon = medicine_list.Afternoon == true ? " - Öğle" : " ";
                        }
                        else
                        {
                            list.Afternoon = medicine_list.Afternoon == true ? "Öğle" : " ";
                        }
                        if (medicine_list.Moon == true || medicine_list.Afternoon == true)
                        {
                            list.Evening = medicine_list.Evening == true ? " - Akşam" : " ";
                        }
                        else
                        {
                            list.Evening = medicine_list.Evening == true ? "Akşam" : " ";
                        }
                        if (medicine_list.Moon == true || medicine_list.Afternoon == true || medicine_list.Evening == true)
                        {
                            list.Night = medicine_list.Night == true ? " - Gece" : " ";
                        }
                        else
                        {
                            list.Night = medicine_list.Night == true ? "Gece" : " ";
                        }

                        list.UsageDay = list.Moon + " " + list.Afternoon + " " + list.Evening + " " + list.Night;

                        TimeSpan results = medicine_list.EndDate - medicine_list.StartDate;
                        int UsageTime = (int)results.TotalDays;

                        list.UsageTime = UsageTime;

                        list.Time = medicine_list.MoonTime + " " + medicine_list.AfternoonTime + " " + medicine_list.EveningTime + " " + medicine_list.NightTime;

                        showMedicineRequest.Add(list);
                    }
                }
                var recordsTotal = showMedicineRequest.Count();

                return Json(new
                {
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = showMedicineRequest
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = true, data = "", messages = ex.Message });
            }
        }

        [HttpGet]
        public JsonResult Dropdown1(int sId)
        {
            sId = 1002; //UserId
            var _l_user_id_list = db.Medicines.Where(w => w.UserId == sId).Select(s => s.Id).ToList();


            var showMedicineUserRequest = new List<MedicineRequest>();



            for (var k = 0; k < _l_user_id_list.Count(); k++)
            {
                var list = new MedicineRequest();
                var l_user = _l_user_id_list[k];

                var user_control = db.Medicines.Where(w => w.Id == l_user).FirstOrDefault();



                list.Id = user_control.Id;
                list.UserName = user_control.Name;
                showMedicineUserRequest.Add(list);



            }

            return Json(new { data = showMedicineUserRequest });


        }
    }
}
