using DemansAppWebPro.Helper.DTO.AudioBooks;
using DemansAppWebPro.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DemansAppWebPro.Controllers
{
    [Authorize(Roles = "User")]
    public class U_AudioBooksController : Controller
    {
        private readonly EntitiesContext db;

        public U_AudioBooksController(EntitiesContext _db)
        {
            db = _db;
        }
        public IActionResult Index(int id)
        {
            var userInfo = db.Users.Where(w => w.Id == id).FirstOrDefault();

            return View(userInfo);
        }

        [HttpPost]
        public JsonResult ShowAudioBooks(int userId)
        {
            try
            {
                userId = 1002;
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

                var showAudioBooksRequest = db.AudioBooks/*.Where(w => w.UserId == userId)*/
                  .Select(s => new showAudioBooksRequest()
                  {
                      Id = s.Id,
                      Name = s.Name,
                      Subject = s.Subject,
                      Text = s.Text,
                      Url = s.Url,

                  }).Skip(skip).Take(pageSize).ToList();
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn)
                    {
                        case "Name":
                            if (sortColumnDir == "desc")
                                showAudioBooksRequest = (from o in showAudioBooksRequest orderby o.Name descending select o).ToList();
                            else
                                showAudioBooksRequest = (from o in showAudioBooksRequest orderby o.Name ascending select o).ToList();
                            break;
                        case "Subject":
                            if (sortColumnDir == "desc")
                                showAudioBooksRequest = (from o in showAudioBooksRequest orderby o.Subject descending select o).ToList();
                            else
                                showAudioBooksRequest = (from o in showAudioBooksRequest orderby o.Subject ascending select o).ToList();
                            break;
                        case "Id":
                            if (sortColumnDir == "desc")
                                showAudioBooksRequest = (from o in showAudioBooksRequest orderby o.Id descending select o).ToList();
                            else
                                showAudioBooksRequest = (from o in showAudioBooksRequest orderby o.Id ascending select o).ToList();
                            break;
                        default:
                            if (sortColumnDir == "desc")
                                showAudioBooksRequest = (from o in showAudioBooksRequest orderby o.Name descending select o).ToList();
                            else
                                showAudioBooksRequest = (from o in showAudioBooksRequest orderby o.Name ascending select o).ToList();
                            break;
                    }
                }
                var recordsTotal = showAudioBooksRequest.Count();

                return Json(new
                {
                    draw = draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = showAudioBooksRequest
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = true, data = "", messages = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult CheckAudioBook(int request)
        {
            try
            {
                var _c_audio_book = db.AudioBooks.Where(w => w.Id == request).FirstOrDefault();
                var _c_audio_book_list = db.AudioBooks.Where(w => w.Name == _c_audio_book.Name && w.Url == _c_audio_book.Url).Select(s => new { s.Status, s.Id }).ToList();

                for (var j = 0; j < _c_audio_book_list.Count(); j++)
                {
                    var _status = _c_audio_book_list[j].Status;
                    var _s_audio_book_id = _c_audio_book_list[j].Id;
                    var _s_audio_book = db.AudioBooks.Where(w => w.Id == _s_audio_book_id).FirstOrDefault();

                    if (_status == 1)
                    {
                        _s_audio_book.Status = 0;

                        db.AudioBooks.Update(_s_audio_book);
                        db.SaveChangesAsync();
                    }
                    else
                    {
                        _s_audio_book.Status = 1;

                        db.AudioBooks.Update(_s_audio_book);
                        db.SaveChangesAsync();
                    }
                }
                return Json(new { Status = true });
            }
            catch
            {
                return Json(new { Status = false });
            }
        }
    }
}
