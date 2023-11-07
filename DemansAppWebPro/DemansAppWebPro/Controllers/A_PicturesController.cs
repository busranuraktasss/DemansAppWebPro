using DemansAppWebPro.Helper.DTO.Pictures;
using DemansAppWebPro.Helper.DTO.Users;
using DemansAppWebPro.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DemansAppWebPro.Controllers
{
    [Authorize(Roles = "Admin")]
    public class A_PicturesController : Controller
    {
        private readonly EntitiesContext db;

        public A_PicturesController(EntitiesContext _db)
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

                var _pictures_list = db.Users.Where(w => w.UserName.Contains(searchValue) && w.Status == 1)
                    .Select(s => new showUsersRequest()
                    {
                        Id = s.Id,
                        UserName = s.UserName,
                        Surname = s.Surname,

                    }).Skip(skip).Take(pageSize).ToList();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn)
                    {
                        case "UserName":
                            if (sortColumnDir == "desc")
                                _pictures_list = (from o in _pictures_list orderby o.UserName ascending select o).ToList();
                            else
                                _pictures_list = (from o in _pictures_list orderby o.UserName descending select o).ToList();
                            break;
                        
                        case "Surname":
                            if (sortColumnDir == "desc")
                                _pictures_list = (from o in _pictures_list orderby o.Surname descending select o).ToList();
                            else
                                _pictures_list = (from o in _pictures_list orderby o.Surname ascending select o).ToList();
                            break;
                        default:
                            if (sortColumnDir == "desc")
                                _pictures_list = (from o in _pictures_list orderby o.Id descending select o).ToList();
                            else
                                _pictures_list = (from o in _pictures_list orderby o.Id ascending select o).ToList();
                            break;
                    }
                }

                return Json(new
                {
                    draw = draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = _pictures_list
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = true, data = "", messages = ex.Message });
            }
        }
        
        [HttpPost]
        public JsonResult ShowPicture(int sId)
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
                var recordsTotal = db.Pictures.Where(w => w.Status == true && w.UserId == sId).Count();

                var _pictures_list = db.Pictures.Where(w => w.Text.Contains(searchValue) && w.Status == true && w.UserId == sId)
                    .Select(s => new showPicturesRequest()
                    { 
                        Id = s.Id,
                        Url = s.Url,
                        Text = s.Text,

                    }).Skip(skip).Take(pageSize).ToList();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn)
                    {
                        case "Text":
                            if (sortColumnDir == "desc")
                                _pictures_list = (from o in _pictures_list orderby o.Text ascending select o).ToList();
                            else
                                _pictures_list = (from o in _pictures_list orderby o.Text descending select o).ToList();
                            break;
                        
                        case "Url":
                            if (sortColumnDir == "desc")
                                _pictures_list = (from o in _pictures_list orderby o.Url descending select o).ToList();
                            else
                                _pictures_list = (from o in _pictures_list orderby o.Url ascending select o).ToList();
                            break;
                        default:
                            if (sortColumnDir == "desc")
                                _pictures_list = (from o in _pictures_list orderby o.Id descending select o).ToList();
                            else
                                _pictures_list = (from o in _pictures_list orderby o.Id ascending select o).ToList();
                            break;
                    }
                }

                return Json(new
                {
                    draw = draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = _pictures_list
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = true, data = "", messages = ex.Message });
            }
        }


        [HttpGet]
        public async Task<JsonResult> DeletePicture(int pr)
        {
            try
            {
                var _d_picture = await db.Pictures.Where(w => w.Id == pr).FirstOrDefaultAsync();
                if (_d_picture == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                _d_picture.Status = false;

                    db.Pictures.Update(_d_picture); //Soft Delete
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
