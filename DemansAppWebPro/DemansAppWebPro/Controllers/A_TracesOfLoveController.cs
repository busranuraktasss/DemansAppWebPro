using DemansAppWebPro.Helper.DTO.LocationInformation;
using DemansAppWebPro.Helper.DTO.TraceOfLove;
using DemansAppWebPro.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DemansAppWebPro.Controllers
{
    [Authorize(Roles = "Admin")]
    public class A_TracesOfLoveController : Controller
    {
        private readonly EntitiesContext db;

        public A_TracesOfLoveController(EntitiesContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ShowTraceOfLove()
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
                var recordsTotal = db.TraceOfLoves.Count();

                var _tracesOfLove_list = db.TraceOfLoves.Where(w => w.PlaceName.Contains(searchValue) )
                    .Select(s => new showTraceOfLoveRequest()
                    {
                        Id = s.Id,
                        PlaceName = s.PlaceName,
                        Email = s.Email,
                        Phone = s.Phone,
                        Lat = s.Lat,
                        Lng = s.Lng,

                    }).Skip(skip).Take(pageSize).ToList();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn)
                    {
                        case "Id":
                            if (sortColumnDir == "desc")
                                _tracesOfLove_list = (from o in _tracesOfLove_list orderby o.Id ascending select o).ToList();
                            else
                                _tracesOfLove_list = (from o in _tracesOfLove_list orderby o.Id descending select o).ToList();
                            break;
                        case "PlaceName":
                            if (sortColumnDir == "desc")
                                _tracesOfLove_list = (from o in _tracesOfLove_list orderby o.PlaceName descending select o).ToList();
                            else
                                _tracesOfLove_list = (from o in _tracesOfLove_list orderby o.PlaceName ascending select o).ToList();
                            break;
                        case "Phone":
                            if (sortColumnDir == "desc")
                                _tracesOfLove_list = (from o in _tracesOfLove_list orderby o.Phone descending select o).ToList();
                            else
                                _tracesOfLove_list = (from o in _tracesOfLove_list orderby o.Phone ascending select o).ToList();
                            break;
                        default:
                            if (sortColumnDir == "desc")
                                _tracesOfLove_list = (from o in _tracesOfLove_list orderby o.Id descending select o).ToList();
                            else
                                _tracesOfLove_list = (from o in _tracesOfLove_list orderby o.Id ascending select o).ToList();
                            break;
                    }
                }

                return Json(new
                {
                    draw = draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = _tracesOfLove_list
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = true, data = "", messages = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult AddTraceOfLove(addTraceOfLoveRequest request)
        {
            try
            { 
                var _traceOfLove = new TraceOfLoves()
                {
                    PlaceName = request.PlaceName,
                    Email = request.Email,
                    Phone = request.Phone,
                    Lat = request.Lat,
                    Lng = request.Lng,
                };
                db.TraceOfLoves.Add(_traceOfLove);
                db.SaveChanges();

                return Json(new { Status = true });
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", Messages = ex.Message });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetTraceOfLove(int sId)
        {
            var _traceOfLove_current = await db.TraceOfLoves.Where(f => f.Id == sId).Select(s => new { s.Id, s.PlaceName, s.Email, s.Lat, s.Lng, s.Phone }).FirstOrDefaultAsync();
            return Json(new { Status = true, data = _traceOfLove_current, Messages = "Success", Code = 200 });
        }

        [HttpPost]
        public async Task<JsonResult> UpdateTraceOfLove(addTraceOfLoveRequest request)
        {
            try
            {
                var _u_traceOfLove = db.TraceOfLoves.Where(w => w.Id == request.Id).FirstOrDefault();
                if (_u_traceOfLove == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                _u_traceOfLove.PlaceName = request.PlaceName;
                _u_traceOfLove.Lat = request.Lat;
                _u_traceOfLove.Email = request.Email;
                _u_traceOfLove.Phone = request.Phone;
                _u_traceOfLove.Lng = request.Lng;

                db.TraceOfLoves.Update(_u_traceOfLove);
                await db.SaveChangesAsync();

                return Json(new { Status = true, Messages = "Değiştirme işlemi başarılı" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", Messages = ex.Message });
            }
        }

        [HttpGet]
        public async Task<JsonResult> DeleteTraceOfLove(int pr)
        {
            try
            {
                var _d_traceOfLove = await db.TraceOfLoves.Where(w => w.Id == pr).FirstOrDefaultAsync();
                if (_d_traceOfLove == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                db.TraceOfLoves.Remove(_d_traceOfLove);//Hard Delete
                await db.SaveChangesAsync();

                return Json(new { Status = true, data = "" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", messages = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ShiftMap(int sId)
        {
            var shiftMap = db.TraceOfLoves.Where(w => w.Id == sId)
                .Select(s => new shiftMapRequest
                {
                    Lat = s.Lat,
                    Lng = s.Lng,
                    PlaceName = s.PlaceName

                }).ToList();

            return Json(new { status = true, data = shiftMap });


        }
    }
}
