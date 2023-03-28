using DemansAppWebPro.Helper.DTO.AudioBooks;
using DemansAppWebPro.Helper.DTO.Users;
using DemansAppWebPro.Models;
using EsPark_WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DemansAppWebPro.Controllers
{
    public class AudioBooksController : Controller
    {
        private readonly EntitiesContext db;

        public AudioBooksController(EntitiesContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ShowAudioBooks()
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
                var showAudioBooksRequest = new List<showAudioBooksRequest>();

                var _list = db.AudioBooks.ToList();
                for (var aa = 0; aa< _list.Count(); aa++)
                {
                    var sName = _list[aa].Name;
                    var sId = _list[aa].Id;
                    var sText = _list[aa].Text;
                    var sUrl = _list[aa].Url;
                    var sSubject = _list[aa].Subject;
                    var sStatus = _list[aa].Status;

                    var dd = new showAudioBooksRequest();
                    var cc = showAudioBooksRequest.FirstOrDefault(k => k.Name == sName);
                    if (cc != null)
                    {
                        cc.Name = sName;
                    }
                    else
                    {
                        dd.Id = sId;
                        dd.Name = sName;
                        dd.Text = sText;
                        dd.Url = sUrl;
                        dd.Subject = sSubject;
                        dd.Status = sStatus;
                        

                        showAudioBooksRequest.Add(dd);
                    }
                }
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
        public async Task<JsonResult> AddAudioBook(addAudioBookRequest request)
        {
            try
            {
                var _audio_book = new AudioBooks()
                {
                    Name = request.Name,
                    Subject = request.Subject,
                    Text = request.Text,
                    Url = request.Url,
                    Status = 1,
                };
                db.AudioBooks.Add(_audio_book);
                await db.SaveChangesAsync();

                return Json(new { Status = true, Messages = "Ekleme işlemi hatalı." });
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", Messages = ex.Message });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetAudioBooks(int sId)
        {
            var _audio_books_current = await db.AudioBooks.Where(f => f.Id == sId).Select(s => new { s.Id,s.Name, s.Subject, s.Url ,s.Text }).FirstOrDefaultAsync();
            return Json(new { Status = true, data = _audio_books_current, Messages = "Success", Code = 200 });
        }

        [HttpPost]
        public async Task<JsonResult> UpdateAudioBook(updateAudioBooksRequest request)
        {
            try
            {
                var _s_audio_book = db.AudioBooks.Where(w => w.Id == request.Id).FirstOrDefault();
                if (_s_audio_book == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                var _s_book_list = db.AudioBooks.Where(w => w.Name == _s_audio_book.Name && w.Subject == _s_audio_book.Subject && w.Url == _s_audio_book.Url).Select(s => s.Id).ToList();

                for(var _book_count = 0; _book_count < _s_book_list.Count() ; _book_count++)
                {
                    var _book_ıd = _s_book_list[_book_count];
                    var _s_book_list_current = db.AudioBooks.Where(w => w.Id == _book_ıd).FirstOrDefault();

                    _s_book_list_current.Name = request.Name;
                    _s_book_list_current.Subject = request.Subject;
                    _s_book_list_current.Text = request.Text;
                    _s_book_list_current.Url = request.Url;

                    db.AudioBooks.Update(_s_book_list_current);
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
        public async Task<JsonResult> DeleteAudioBooks(int pr)
        {
            try
            {
                var _d_audio_book = await db.AudioBooks.Where(w => w.Id == pr).FirstOrDefaultAsync();
                if (_d_audio_book == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                var _d_book_list = db.AudioBooks.Where(w => w.Name == _d_audio_book.Name && w.Subject == _d_audio_book.Subject && w.Url == _d_audio_book.Url).Select(s => s.Id).ToList();

                for (var _d_book_count = 0; _d_book_count < _d_book_list.Count(); _d_book_count++)
                {
                    var _book_ıd = _d_book_list[_d_book_count];

                    var _d_book_list_current = db.AudioBooks.Where(w => w.Id == _book_ıd).FirstOrDefault();

                    _d_book_list_current.Status = 0;//Soft Delete
                    db.AudioBooks.Update(_d_book_list_current);
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
                var _audio_books_name = db.AudioBooks.Where(w => w.Id == sId).Select(s => s.Name).FirstOrDefault();
                var _audio_books_id_list = db.AudioBooks.Where(w => w.Name == _audio_books_name).Select(s => s.UserId).ToList();

                var showUsersRequest = db.Users.Where(w => _audio_books_id_list.Contains(w.Id))
                    .Select(s => new showUsersRequest()
                    {
                        Id = s.Id,
                        UserName = s.UserName,
                        Surname = s.Surname,

                    }).ToList();

                var recordsTotal = showUsersRequest.Count();

                return Json(new
                {
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = showUsersRequest
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = true, data = "", messages = ex.Message });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetSelect(int sId)
        {
            List<showUsersRequest> ListCount;
            List<string> lıd = new List<string>();

            var _book_name = db.AudioBooks.Where(w => w.Id == sId).Select(s => s.Name).FirstOrDefault();
            lıd = db.AudioBooks.Where(w => w.Name == _book_name).Select(s => s.UserId.ToString()).ToList();


                ListCount = db.Users
               .Select(s => new showUsersRequest()
               {
                   Id = s.Id,
                   UserName = s.UserName,
                   Surname = s.Surname,

               }).ToList();
          


            return Json(new { data = ListCount, selected = lıd, status = true });








        }

    }
}
