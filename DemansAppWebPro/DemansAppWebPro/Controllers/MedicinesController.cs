
using Microsoft.AspNetCore.Mvc;
using DemansAppWebPro.Helper.DTO.Medicines;
using DemansAppWebPro.Models;
using Microsoft.EntityFrameworkCore;
using DemansAppWebPro.Helper.DTO.Commands;
using DemansAppWebPro.Helper.DTO.Users;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.VisualBasic;

namespace DemansAppWebPro.Controllers
{
    public class MedicinesController : Controller
    {
        private readonly EntitiesContext db;

        public MedicinesController(EntitiesContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ShowMedicines()
        {
            try
            {
                Request.Form.TryGetValue("draw", out Microsoft.Extensions.Primitives.StringValues drawOut);

                var draw = drawOut.FirstOrDefault();

                Request.Form.TryGetValue("start", out Microsoft.Extensions.Primitives.StringValues startOut);
                var start = startOut.FirstOrDefault();

                Request.Form.TryGetValue("length", out Microsoft.Extensions.Primitives.StringValues lengthOut);
                var length = lengthOut.FirstOrDefault();

                Request.Form.TryGetValue("order[0][column]", out Microsoft.Extensions.Primitives.StringValues orderColumnOut);
                Request.Form.TryGetValue("columns[" + orderColumnOut.FirstOrDefault() + "][name]", out Microsoft.Extensions.Primitives.StringValues columnsNameOut);
                var sortColumn = columnsNameOut.FirstOrDefault();

                Request.Form.TryGetValue("order[0][dir]", out Microsoft.Extensions.Primitives.StringValues sortColumnDirOut);
                var sortColumnDir = sortColumnDirOut.FirstOrDefault();

                Request.Form.TryGetValue("search[value]", out Microsoft.Extensions.Primitives.StringValues searchValueOut);
                var searchValue = searchValueOut.FirstOrDefault() ?? "";

                //Paging Size 
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                var showMedicinesRequest = new List<showMedicinesRequest>();

                var _list = db.Medicines.ToList();
                for (var i = 0; i < _list.Count(); i++)
                {
                    var sId = _list[i].Id;
                    var sName = _list[i].Name;
                    var sUsageDuration = _list[i].UsageDuration;

                    var dd = new showMedicinesRequest();
                    var cc = showMedicinesRequest.FirstOrDefault(k => k.Name == sName);
                    if (cc != null)
                    {
                        cc.Name = sName;
                    }
                    else
                    {
                        dd.Id = sId;
                        dd.Name = sName;
                        dd.UsageDuration = sUsageDuration;
                        showMedicinesRequest.Add(dd);
                    }
                }
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn)
                    {
                        case "Name":
                            if (sortColumnDir == "desc")
                                showMedicinesRequest = (from o in showMedicinesRequest orderby o.Name descending select o).ToList();
                            else
                                showMedicinesRequest = (from o in showMedicinesRequest orderby o.Name ascending select o).ToList();
                            break;
                        case "Id":
                            if (sortColumnDir == "desc")
                                showMedicinesRequest = (from o in showMedicinesRequest orderby o.Id descending select o).ToList();
                            else
                                showMedicinesRequest = (from o in showMedicinesRequest orderby o.Id ascending select o).ToList();
                            break;
                        default:
                            if (sortColumnDir == "desc")
                                showMedicinesRequest = (from o in showMedicinesRequest orderby o.Name descending select o).ToList();
                            else
                                showMedicinesRequest = (from o in showMedicinesRequest orderby o.Name ascending select o).ToList();
                            break;
                    }
                }
                var recordsTotal = showMedicinesRequest.Count();

                return Json(new
                {
                    draw = draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = showMedicinesRequest
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = true, data = "", messages = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> AddMedicines(string Name, string UsageDuration)
        {
            try
            {
                var _medicines = new Medicines()
                {
                    Name = Name,
                    UsageDuration = UsageDuration,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    Status = true,

                };
                db.Medicines.Add(_medicines);
                await db.SaveChangesAsync();

                return Json(new { Status = true, Messages = "Ekleme işlemi hatalı." });
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", Messages = ex.Message });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetMedicines(int sId)
        {
            var _medicines_current = await db.Medicines.Where(f => f.Id == sId).Select(s => new { s.Id, s.Name, s.UsageDuration }).FirstOrDefaultAsync();
            return Json(new { Status = true, data = _medicines_current, Messages = "Success", Code = 200 });
        }
        [HttpPost]
        public async Task<JsonResult> UpdateMedicines(int Id, string Name, string UsageDuration)
        {
            try
            {
                var _s_medicines = db.Medicines.Where(w => w.Id == Id).FirstOrDefault();
                if (_s_medicines == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                var _s_medicines_list = db.Medicines.Where(w => w.Name == _s_medicines.Name).Select(s => s.Id).ToList();

                for (var _medicine_count = 0; _medicine_count < _s_medicines_list.Count(); _medicine_count++)
                {
                    var _medicine_ıd = _s_medicines_list[_medicine_count];
                    var _s_medicine_list_current = db.Medicines.Where(w => w.Id == _medicine_ıd).FirstOrDefault();

                    _s_medicine_list_current.Name = Name;
                    _s_medicine_list_current.UsageDuration = UsageDuration;

                    db.Medicines.Update(_s_medicine_list_current);
                    await db.SaveChangesAsync();
                }
                return Json(new { Status = true, Messages = "Değiştirme işlemi başarılı" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", Messages = ex.Message });
            }
        }

        [HttpGet]
        public async Task<JsonResult> DeleteMedicines(int pr)
        {
            //hard delete değiştri soft delete yap
            try
            {
                var _d_medicines = await db.Medicines.Where(w => w.Id == pr).FirstOrDefaultAsync();
                if (_d_medicines == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                var _d_medicines_list = db.Medicines.Where(w => w.Name == _d_medicines.Name).Select(s => s.Id).ToList();

                for (var _d_medicine_count = 0; _d_medicine_count < _d_medicines_list.Count(); _d_medicine_count++)
                {
                    var _medicine_ıd = _d_medicines_list[_d_medicine_count];

                    var _d_medicine_list_current = db.Medicines.Where(w => w.Id == _medicine_ıd).FirstOrDefault();

                    db.Medicines.Remove(_d_medicine_list_current);//Hard Delete
                    await db.SaveChangesAsync();
                }

                return Json(new { Status = true, data = "" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", messages = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ShowUsers(int sId)
        {
            try
            {
                var _medicines_name = db.Medicines.Where(w => w.Id == sId).Select(s => s.Name).FirstOrDefault();
                var _medicines_list = db.Medicines.Where(w => w.Name == _medicines_name).ToList();

                var showMedicineUserRequest = new List<showMedicineUserRequest>();
                for (var i = 0; i < _medicines_list.Count(); i++)
                {
                    var list = new showMedicineUserRequest();
                    var _user_id = _medicines_list[i].UserId;

                    if (_user_id != null)
                    {
                        var medicine_list = db.Medicines.Where(w => w.Name == _medicines_name && w.UserId == _user_id).FirstOrDefault();
                        var user_list = db.Users.Where(w => w.Id == _user_id).Select(s => new { s.UserName , s.Surname}).FirstOrDefault();

                        //medicine Id
                        list.Id = medicine_list.Id;
                        list.UserName = user_list.UserName;
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

                        showMedicineUserRequest.Add(list);
                    }
                }
                var recordsTotal = showMedicineUserRequest.Count();

                return Json(new
                {
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = showMedicineUserRequest
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
            var _l_medicine_name = db.Medicines.Where(w => w.Id == sId).Select(s => s.Name).FirstOrDefault();
            var _l_user_id_list = db.Medicines.Where(w => w.Name == _l_medicine_name).Select(s => s.UserId).ToList();


            var showMedicineUserRequest = new List<showMedicineUserRequest>();

            var l_user_list = db.Users.Where(w => w.Status == 1).Select(s => s.Id).ToList();

            for (var k = 0; k < l_user_list.Count(); k++)
            {
                var list = new showMedicineUserRequest();
                var l_user = l_user_list[k];

                var user_control = db.Medicines.Where(w => w.UserId == l_user).FirstOrDefault();

                if (user_control == null)
                {
                    var user = db.Users.Where(w => w.Id == l_user).Select(s =>  new { s.Id, s.UserName, s.Surname }).FirstOrDefault();

                    list.Id = user.Id;
                    list.UserName = user.UserName;
                    list.Surname = user.Surname;
                    showMedicineUserRequest.Add(list);

                }

            }

            return Json(new { data = showMedicineUserRequest });


        }

        [HttpPost]
        public async Task<JsonResult> AddUser(addMedicineUserRequest rq)
        {
            TimeSpan results = rq.EndDate - rq.StartDate;
            int UsagePurpose = (int)results.TotalDays;
            var _add_user = new Medicines()
            {
                Name = db.Medicines.Where(w => w.Id == rq.MedicineId).Select(s => s.Name).First(),
                UsagePurpose = UsagePurpose.ToString(),
                UsageDuration = db.Medicines.Where(w => w.Id == rq.MedicineId).Select(s => s.UsageDuration).First(),
                StartDate = rq.StartDate,
                EndDate = rq.EndDate,
                Moon = rq.Moon,
                Afternoon = rq.Afternoon,
                Evening = rq.Evening,
                Night = rq.Evening,
                MoonTime = rq.MoonTime,
                AfternoonTime = rq.AfternoonTime,
                EveningTime = rq.EveningTime,
                NightTime = rq.NightTime,
                UserId = rq.UserId,
                Status = true,
            };
            db.Medicines.Add(_add_user);
            await db.SaveChangesAsync();

            return Json(new { Status = true, Messages = "Success", Code = 200 });

        }

        [HttpGet]
        public async Task<JsonResult> DeleteUser(int pr)
        {
            try
            {var _d_user = await db.Medicines.Where(w => w.Id == pr).FirstOrDefaultAsync();
                if (_d_user == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                db.Medicines.Remove(_d_user);//Hard Delete
                await db.SaveChangesAsync();

                return Json(new { Status = true, data = "" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", messages = ex.Message });
            }
        }

    }
}
