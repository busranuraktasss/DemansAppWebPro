using DemansAppWebPro.Helper.DTO.MotivationSentences;
using DemansAppWebPro.Helper.DTO.Medicines;
using DemansAppWebPro.Helper.DTO.Users;
using DemansAppWebPro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemansAppWebPro.Controllers
{
    public class MotivationSentencesController : Controller
    {
        private readonly EntitiesContext db;

        public MotivationSentencesController(EntitiesContext _db)
        {
            db = _db;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ShowMotivationSentences()
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
                var showSentenceRequest = new List<showSentenceRequest>();

                var _list = db.MotivationSentences.Where(w => w.Status == true).ToList();
                for (var i = 0; i < _list.Count(); i++)
                {
                    var sId = _list[i].Id;
                    var sName = _list[i].Name;
                    var sText = _list[i].Text;

                    var dd = new showSentenceRequest();
                    var cc = showSentenceRequest.FirstOrDefault(k => k.Name == sName);
                    if (cc != null)
                    {
                        cc.Name = sName;
                    }
                    else
                    {
                        dd.Id = sId;
                        dd.Name = sName;
                        dd.Text = sText;
                        showSentenceRequest.Add(dd);
                    }
                }
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn)
                    {
                        case "Name":
                            if (sortColumnDir == "desc")
                                showSentenceRequest = (from o in showSentenceRequest orderby o.Name descending select o).ToList();
                            else
                                showSentenceRequest = (from o in showSentenceRequest orderby o.Name ascending select o).ToList();
                            break;
                        case "Id":
                            if (sortColumnDir == "desc")
                                showSentenceRequest = (from o in showSentenceRequest orderby o.Id descending select o).ToList();
                            else
                                showSentenceRequest = (from o in showSentenceRequest orderby o.Id ascending select o).ToList();
                            break;
                        default:
                            if (sortColumnDir == "desc")
                                showSentenceRequest = (from o in showSentenceRequest orderby o.Name descending select o).ToList();
                            else
                                showSentenceRequest = (from o in showSentenceRequest orderby o.Name ascending select o).ToList();
                            break;
                    }
                }
                var recordsTotal = showSentenceRequest.Count();

                return Json(new
                {
                    draw = draw,
                    recordsFiltered = showSentenceRequest.Count(),
                    recordsTotal = showSentenceRequest.Count(),
                    data = showSentenceRequest
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = true, data = "", messages = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> AddSentence(string Name, string Text)
        {
            try
            {
                var _sentence = new MotivationSentences()
                {
                    Name = Name,
                    Text = Text,
                    Status = true,

                };
                db.MotivationSentences.Add(_sentence);
                await db.SaveChangesAsync();

                return Json(new { Status = true, Messages = "Ekleme işlemi hatalı." });
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", Messages = ex.Message });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetSentence(int sId)
        {
            var _sentence_current = await db.MotivationSentences.Where(f => f.Id == sId).Select(s => new { s.Id, s.Name, s.Text }).FirstOrDefaultAsync();
            return Json(new { Status = true, data = _sentence_current, Messages = "Success", Code = 200 });
        }

        [HttpPost]
        public async Task<JsonResult> UpdateSentence(int Id, string Name, string Text)
        {
            try
            {
                var _s_sentence = db.MotivationSentences.Where(w => w.Id == Id).FirstOrDefault();
                if (_s_sentence == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                var _s_sentence_list = db.MotivationSentences.Where(w => w.Name == _s_sentence.Name).Select(s => s.Id).ToList();

                for (var _seentence_count = 0; _seentence_count < _s_sentence_list.Count(); _seentence_count++)
                {
                    var _sentence_ıd = _s_sentence_list[_seentence_count];
                    var _s_sentence_list_current = db.MotivationSentences.Where(w => w.Id == _sentence_ıd).FirstOrDefault();

                    _s_sentence_list_current.Name = Name;
                    _s_sentence_list_current.Text = Text;

                    db.MotivationSentences.Update(_s_sentence_list_current);
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
        public async Task<JsonResult> DeleteSentence(int pr)
        {
            try
            {
                var _d_sentence = await db.MotivationSentences.Where(w => w.Id == pr).FirstOrDefaultAsync();
                if (_d_sentence == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                var _d_sentence_list = db.MotivationSentences.Where(w => w.Name == _d_sentence.Name).Select(s => s.Id).ToList();

                for (var _d_sentence_count = 0; _d_sentence_count < _d_sentence_list.Count(); _d_sentence_count++)
                {
                    var _sentence_ıd = _d_sentence_list[_d_sentence_count];

                    var _d_sentence_list_current = db.MotivationSentences.Where(w => w.Id == _sentence_ıd).FirstOrDefault();

                    _d_sentence_list_current.Status = false;

                    db.MotivationSentences.Update(_d_sentence_list_current); //Soft Delete
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
                var _sentence_name = db.MotivationSentences.Where(w => w.Id == sId).Select(s => s.Name).FirstOrDefault();
                var _sentence_id_list = db.MotivationSentences.Where(w => w.Name == _sentence_name).Select(s => s.UserId).ToList();

                var showUsersRequest = db.Users.Where(w => _sentence_id_list.Contains(w.Id) && w.Status == 1)
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

            var _sentence_name = db.MotivationSentences.Where(w => w.Id == sId).Select(s => s.Name).FirstOrDefault();
            lıd = db.MotivationSentences.Where(w => w.Name == _sentence_name).Select(s => s.UserId.ToString()).ToList();

            ListCount = db.Users.Where(w => w.Status == 1)
           .Select(s => new showUsersRequest()
           {
               Id = s.Id,
               UserName = s.UserName,
               Surname = s.Surname,

           }).ToList();

            return Json(new { data = ListCount, selected = lıd, status = true });
        }

        [HttpPost]
        public async Task<JsonResult> AddUser(int sentenceId, int[] userIds)
        {
            if (userIds != null)
            {
                var sentence = db.MotivationSentences.Where(w => w.Id == sentenceId).FirstOrDefault();

                for (var i = 0; i < userIds.Length; i++)
                {
                    var userId = userIds[i];
                    var _registered_user = db.MotivationSentences.Where(w => w.Name == sentence.Name && w.Text == sentence.Text && w.UserId == userId).FirstOrDefault();
                    if (_registered_user == null)
                    {
                        var _add_user = new MotivationSentences()
                        {
                            Name = sentence.Name,
                            UserId = userId,
                            Text = sentence.Text,
                            Status = sentence.Status,

                        };
                        db.MotivationSentences.Add(_add_user);
                        await db.SaveChangesAsync();
                    }
                }
                return Json(new { Status = true, Messages = "Success", Code = 200 });

            }
            return Json(new { Status = false, data = "" });
        }

        [HttpGet]
        public async Task<JsonResult> DeleteUser(int pr, int sentenceId)
        {
            try
            {
                var _d_sentence_user = await db.MotivationSentences.Where(w => w.Id == sentenceId).FirstOrDefaultAsync();
                var _d_user = await db.MotivationSentences.Where(w => w.UserId == pr && w.Name == _d_sentence_user.Name ).FirstOrDefaultAsync();
                if (_d_user == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                db.MotivationSentences.Remove(_d_user);//Hard Delete
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
