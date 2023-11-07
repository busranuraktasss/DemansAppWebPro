using Microsoft.AspNetCore.Mvc;
using DemansAppWebPro.Helper.DTO.Companions;
using DemansAppWebPro.Helper.DTO.Users;
using DemansAppWebPro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace DemansAppWebPro.Controllers
{
    [Authorize(Roles = "Admin")]
    public class A_CompanionsController : Controller
    {
        private readonly EntitiesContext db;

        public A_CompanionsController(EntitiesContext _db)
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

                var showCompanionsRequest = db.Companions.Where(w => w.Name.Contains(searchValue) && w.Status == 1)
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
        public JsonResult Dropdown1(int sId)
        {
          
                var _c_user = db.Companions.Where(w => w.Id == sId).Select(s => s.UserId).FirstOrDefault();

                var _s_user = db.Users.Where(w => w.Status == 1)
                    .Select(s => new showUsersRequest()
                    {
                        Id = s.Id,
                        UserName = s.UserName,
                        Surname = s.Surname,
                    }).ToList();
                return Json(new { data= _s_user }) ;

           
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

        [HttpGet]
        public async Task<JsonResult> GetCompanions(int sId)
        {
            var _companions_current = await db.Companions.Where(f => f.Id == sId).Select(s => new { s.Id, s.Name, s.Surname, s.Adress, s.Email, s.Phone, s.Sex, s.UserId }).FirstOrDefaultAsync();
            return Json(new { Status = true, data = _companions_current, Messages = "Success", Code = 200 });
        }

        [HttpPost]
        public async Task<JsonResult> UpdateCompanions(updateCompanionsRequest request)
        {
            try
            {
                var _u_companions = db.Companions.Where(w => w.Id == request.Id).FirstOrDefault();
                if (_u_companions == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                _u_companions.Name = request.Name;
                _u_companions.Surname = request.Surname;
                _u_companions.Adress = request.Adress;
                _u_companions.Email = request.Email;
                _u_companions.Phone = request.Phone;
                _u_companions.Sex = request.Sex;

                db.Companions.Update(_u_companions);
                await db.SaveChangesAsync();
                
                return Json(new { Status = true, Messages = "Değiştirme işlemi başarılı" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", Messages = ex.Message });
            }
        }


        [HttpGet]
        public async Task<JsonResult> DeleteCompanions(int pr)
        {
            try
            {
                var _d_companions = await db.Companions.Where(w => w.Id == pr).FirstOrDefaultAsync();
                if (_d_companions == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                var _d_command_ids = db.Commands.Where(w => w.CompanionId == pr).Select(s => s.Id).ToList();

                if(_d_command_ids != null)
                {
                    for(var i = 0; i< _d_command_ids.Count(); i++)
                    {
                        var _d_command_id = _d_command_ids[i];
                        var _d_command = db.Commands.Where(w => w.Id == _d_command_id).FirstOrDefault();
                        db.Commands.Remove(_d_command);
                        await db.SaveChangesAsync();
                    }
                }

               db.Companions.Remove(_d_companions);//Hard Delete
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
