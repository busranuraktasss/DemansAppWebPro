using EsPark_WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using DemansAppWebPro.Helper.DTO.Commands;
using DemansAppWebPro.Helper.DTO.AudioBooks;
using DemansAppWebPro.Models;

namespace DemansAppWebPro.Controllers
{
    public class CommandsController : Controller
    {
        private readonly EntitiesContext db;

        public CommandsController(EntitiesContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ShowCommands()
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
                var showCommandsRequest = new List<showCommandsRequest>();

                var _list = db.Commands.ToList();
                for (var i = 0; i < _list.Count(); i++)
                {
                    var sName = _list[i].ProcessName;
                    var sId = _list[i].Id;
                    var sStatus = _list[i].Status;

                    var dd = new showCommandsRequest();
                    var cc = showCommandsRequest.FirstOrDefault(k => k.ProcessName == sName);
                    if (cc != null)
                    {
                        cc.ProcessName = sName;
                    }
                    else
                    {
                        dd.Id = sId;
                        dd.ProcessName = sName;
                        dd.Status = sStatus;
                        showCommandsRequest.Add(dd);
                    }
                }
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn)
                    {
                        case "ProcessName":
                            if (sortColumnDir == "desc")
                                showCommandsRequest = (from o in showCommandsRequest orderby o.ProcessName descending select o).ToList();
                            else
                                showCommandsRequest = (from o in showCommandsRequest orderby o.ProcessName ascending select o).ToList();
                            break;
                        case "Id":
                            if (sortColumnDir == "desc")
                                showCommandsRequest = (from o in showCommandsRequest orderby o.Id descending select o).ToList();
                            else
                                showCommandsRequest = (from o in showCommandsRequest orderby o.Id ascending select o).ToList();
                            break;
                        default:
                            if (sortColumnDir == "desc")
                                showCommandsRequest = (from o in showCommandsRequest orderby o.ProcessName descending select o).ToList();
                            else
                                showCommandsRequest = (from o in showCommandsRequest orderby o.ProcessName ascending select o).ToList();
                            break;
                    }
                }
                var recordsTotal = showCommandsRequest.Count();

                return Json(new
                {
                    draw = draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = showCommandsRequest
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = true, data = "", messages = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<JsonResult> AddCommands(string ProcessName)
        {
            try
            {
                var _commands = new Commands()
                {
                    ProcessName = ProcessName,
                    Status = true,
                };
                db.Commands.Add(_commands);
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
