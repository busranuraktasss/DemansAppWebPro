using DemansAppWebPro.Helper.DTO.Companions;
using DemansAppWebPro.Helper.DTO.Users;
using DemansAppWebPro.Models;
using Microsoft.AspNetCore.Mvc;
﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemansAppWebPro.Controllers
{
    public class UsersController : Controller
    {
        private readonly EntitiesContext db;

        public UsersController(EntitiesContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ShowUsers()
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
                var recordsTotal = db.Users.Where(W => W.Status == 1).Count();

                var _users_list = db.Users.Where(w => w.UserName.Contains(searchValue) && w.Status == 1)
                    .Select(s => new showUsersRequest()
                    {
                        Id = s.Id,
                        UserName = s.UserName,
                        Surname = s.Surname,
                        Email = s.Email,
                        Phone = s.Phone,
                        Sex = s.Sex,
                        EmergencyPhone = s.EmergencyPhone,

                    }).Skip(skip).Take(pageSize).ToList();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn)
                    {
                        case "RowId":
                            if (sortColumnDir == "desc")
                                _users_list = (from o in _users_list orderby o.Id ascending select o).ToList();
                            else
                                _users_list = (from o in _users_list orderby o.Id descending select o).ToList();
                            break;
                        case "UserName":
                            if (sortColumnDir == "desc")
                                _users_list = (from o in _users_list orderby o.UserName descending select o).ToList();
                            else
                                _users_list = (from o in _users_list orderby o.UserName ascending select o).ToList();
                            break;
                        case "Surname":
                            if (sortColumnDir == "desc")
                                _users_list = (from o in _users_list orderby o.Surname descending select o).ToList();
                            else
                                _users_list = (from o in _users_list orderby o.Surname ascending select o).ToList();
                            break;
                        default:
                            if (sortColumnDir == "desc")
                                _users_list = (from o in _users_list orderby o.Id descending select o).ToList();
                            else
                                _users_list = (from o in _users_list orderby o.Id ascending select o).ToList();
                            break;
                    }
                }

                return Json(new
                {
                    draw = draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = _users_list
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = true, data = "", messages = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> AddUser(addUsersRequest request)
        {
            try
            {
                var _users = new Users()
                {
                    UserName = request.UserName,
                    Surname = request.Surname,
                    Email = request.Email,
                    Sex = request.Sex,
                    Phone = request.Phone,
                    EmergencyPhone = request.EmergencyPhone,
                    Status = 1,
                };
                db.Users.Add(_users);
                await db.SaveChangesAsync();

                return Json(new { Status = true, Messages = "Ekleme işlemi hatalı." });
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", Messages = ex.Message });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetUsers(int sId)
        {
            var _users_current = await db.Users.Where(f => f.Id == sId).Select(s => new { s.Id, s.UserName, s.Surname, s.EmergencyPhone, s.Email, s.Phone, s.Sex}).FirstOrDefaultAsync();
            return Json(new { Status = true, data = _users_current, Messages = "Success", Code = 200 });
        }

        [HttpPost]
        public async Task<JsonResult> UpdateUser(updateUsersRequest request)
        {
            try
            {
                var _u_users = db.Users.Where(w => w.Id == request.Id).FirstOrDefault();
                if (_u_users == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                _u_users.UserName = request.UserName;
                _u_users.Surname = request.Surname;
                _u_users.Email = request.Email;
                _u_users.Phone = request.Phone;
                _u_users.EmergencyPhone = request.EmergencyPhone;
                _u_users.Sex = request.Sex;

                db.Users.Update(_u_users);
                await db.SaveChangesAsync();

                return Json(new { Status = true, Messages = "Değiştirme işlemi başarılı" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", Messages = ex.Message });
            }
        }

        [HttpGet]
        public async Task<JsonResult> DeleteUser(int pr)
        {
            try
            {
                var _d_users = await db.Users.Where(w => w.Id == pr).FirstOrDefaultAsync();
                if (_d_users == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                var _d_command_ids = db.Commands.Where(w => w.UserId == pr).Select(s => s.Id).ToList();

                if (_d_command_ids != null)
                {
                    for (var i = 0; i < _d_command_ids.Count(); i++)
                    {
                        var _d_command_id = _d_command_ids[i];
                        var _d_command = db.Commands.Where(w => w.Id == _d_command_id).FirstOrDefault();

                        _d_command.Status = false;

                        db.Commands.Update(_d_command);
                        await db.SaveChangesAsync();
                    }
                }

                var _d_audioBook_ids = db.AudioBooks.Where(w => w.UserId == pr).Select(s => s.Id).ToList();

                if (_d_audioBook_ids != null)
                {
                    for (var i = 0; i < _d_audioBook_ids.Count(); i++)
                    {
                        var _d_audioBook_id = _d_audioBook_ids[i];
                        var _d_audioBook = db.AudioBooks.Where(w => w.Id == _d_audioBook_id).FirstOrDefault();

                        _d_audioBook.Status = 0; // true:1 false:0

                        db.AudioBooks.Update(_d_audioBook);
                        await db.SaveChangesAsync();
                    }
                }

                var _d_companion_ids = db.Companions.Where(w => w.UserId == pr).Select(s => s.Id).ToList();

                if (_d_companion_ids != null)
                {
                    for (var i = 0; i < _d_companion_ids.Count(); i++)
                    {
                        var _d_companion_id = _d_companion_ids[i];
                        var _d_companion = db.Companions.Where(w => w.Id == _d_companion_id).FirstOrDefault();

                        _d_companion.Status = 0; // true:1 false:0

                        db.Companions.Update(_d_companion);
                        await db.SaveChangesAsync();
                    }
                }

                var _d_locInfo_ids = db.LocationInformation.Where(w => w.UserId == pr).Select(s => s.Id).ToList();

                if (_d_locInfo_ids != null)
                {
                    for (var i = 0; i < _d_locInfo_ids.Count(); i++)
                    {
                        var _d_locInfo_id = _d_locInfo_ids[i];
                        var _d_locInfo = db.LocationInformation.Where(w => w.Id == _d_locInfo_id).FirstOrDefault();

                        _d_locInfo.Status = false; // true:1 false:0

                        db.LocationInformation.Update(_d_locInfo);
                        await db.SaveChangesAsync();
                    }
                }

                var _d_medicine_ids = db.Medicines.Where(w => w.UserId == pr).Select(s => s.Id).ToList();

                if (_d_medicine_ids != null)
                {
                    for (var i = 0; i < _d_medicine_ids.Count(); i++)
                    {
                        var _d_medicine_id = _d_medicine_ids[i];
                        var _d_medicine = db.Medicines.Where(w => w.Id == _d_medicine_id).FirstOrDefault();

                        _d_medicine.Status = false; // true:1 false:0

                        db.Medicines.Update(_d_medicine);
                        await db.SaveChangesAsync();
                    }
                } 
                
                var _d_sentence_ids = db.MotivationSentences.Where(w => w.UserId == pr).Select(s => s.Id).ToList();

                if (_d_sentence_ids != null)
                {
                    for (var i = 0; i < _d_sentence_ids.Count(); i++)
                    {
                        var _d_sentence_id = _d_sentence_ids[i];
                        var _d_sentence = db.MotivationSentences.Where(w => w.Id == _d_sentence_id).FirstOrDefault();

                        _d_sentence.Status = false; // true:1 false:0

                        db.MotivationSentences.Update(_d_sentence);
                        await db.SaveChangesAsync();
                    }
                }

                var _d_picture_ids = db.Pictures.Where(w => w.UserId == pr).Select(s => s.Id).ToList();

                if (_d_picture_ids != null)
                {
                    for (var i = 0; i < _d_picture_ids.Count(); i++)
                    {
                        var _d_picture_id = _d_picture_ids[i];
                        var _d_picture = db.Pictures.Where(w => w.Id == _d_picture_id).FirstOrDefault();

                        _d_picture.Status = false; // true:1 false:0

                        db.Pictures.Update(_d_picture);
                        await db.SaveChangesAsync();
                    }
                }

                _d_users.Status = 0; // true:1 false:0
                db.Users.Update(_d_users);
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
