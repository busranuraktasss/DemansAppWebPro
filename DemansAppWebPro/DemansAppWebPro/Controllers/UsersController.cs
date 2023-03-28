using DemansAppWebPro.Helper.DTO.Users;
using EsPark_WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
﻿using Microsoft.AspNetCore.Mvc;

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
                var recordsTotal = db.Users.Count();

                var _users_list = db.Users.Where(w => w.UserName.Contains(searchValue) )
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

    }
}
