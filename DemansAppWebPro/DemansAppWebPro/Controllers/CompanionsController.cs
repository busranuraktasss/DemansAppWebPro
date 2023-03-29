using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using EsPark_WebApplication.Models;
using DemansAppWebPro.Helper.DTO.Companions;
using DemansAppWebPro.Helper.DTO.Users;
using DemansAppWebPro.Helper.DTO.AudioBooks;
using DemansAppWebPro.Models;

namespace DemansAppWebPro.Controllers
{
    public class CompanionsController : Controller
    {
        private readonly EntitiesContext db;

        public CompanionsController(EntitiesContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ShowCompanions()
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

                var showCompanionsRequest = db.Companions.Where(w => w.Name.Contains(searchValue))
                  .Select(s => new showCompanionsRequest()
                  {
                      Id = s.Id,
                      Name = s.Name,
                      Surname = s.Surname,
                      Email = s.Email,
                      Phone = s.Phone,
                      Adress = s.Adress,
                      Sex = s.Sex,
                      UserName = db.Users.Where(w => w.Id == s.UserId).Select(s => s.UserName).First(),
                      UserSurname = db.Users.Where(w => w.Id == s.UserId).Select(s => s.Surname).First(),


                  }).Skip(skip).Take(pageSize).ToList();
            
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn)
                    {
                        case "Name":
                            if (sortColumnDir == "desc")
                                showCompanionsRequest = (from o in showCompanionsRequest orderby o.Name descending select o).ToList();
                            else
                                showCompanionsRequest = (from o in showCompanionsRequest orderby o.Name ascending select o).ToList();
                            break;
                        case "Adress":
                            if (sortColumnDir == "desc")
                                showCompanionsRequest = (from o in showCompanionsRequest orderby o.Adress descending select o).ToList();
                            else
                                showCompanionsRequest = (from o in showCompanionsRequest orderby o.Adress ascending select o).ToList();
                            break;
                        case "Id":
                            if (sortColumnDir == "desc")
                                showCompanionsRequest = (from o in showCompanionsRequest orderby o.Id descending select o).ToList();
                            else
                                showCompanionsRequest = (from o in showCompanionsRequest orderby o.Id ascending select o).ToList();
                            break;
                        default:
                            if (sortColumnDir == "desc")
                                showCompanionsRequest = (from o in showCompanionsRequest orderby o.Name descending select o).ToList();
                            else
                                showCompanionsRequest = (from o in showCompanionsRequest orderby o.Name ascending select o).ToList();
                            break;
                    }
                }
                var recordsTotal = showCompanionsRequest.Count();

                return Json(new
                {
                    draw = draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = showCompanionsRequest
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = true, data = "", messages = ex.Message });
            }
        }

        [HttpGet]
        public JsonResult Dropdown1()
        {
            var s_users = db.Companions.Select(s => s.UserId).ToList();
            var user_list = db.Users.Where(t => t.UserName != null)
                .Select(s => new showUsersRequest()
                {
                    Id = s.Id,
                    UserName = s.UserName,
                    Surname = s.Surname,

                }).ToList();

            return Json(new { data = user_list , selected = user_list });
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
  