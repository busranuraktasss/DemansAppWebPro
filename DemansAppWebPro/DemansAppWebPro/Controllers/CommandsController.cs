using EsPark_WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using DemansAppWebPro.Helper.DTO.Commands;
using DemansAppWebPro.Helper.DTO.Users;
using DemansAppWebPro.Models;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet]
        public async Task<JsonResult> GetCommands(int sId)
        {
            var _commands_current = await db.Commands.Where(f => f.Id == sId).Select(s => new { s.Id, s.ProcessName}).FirstOrDefaultAsync();
            return Json(new { Status = true, data = _commands_current, Messages = "Success", Code = 200 });
        }

        [HttpPost]
        public async Task<JsonResult> UpdateCommands(updateCommandsRequest request)
        {
            try
            {
                var _s_commands = db.Commands.Where(w => w.Id == request.Id).FirstOrDefault();
                if (_s_commands == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                var _s_commands_list = db.Commands.Where(w => w.ProcessName == _s_commands.ProcessName).Select(s => s.Id).ToList();

                for (var _command_count = 0; _command_count < _s_commands_list.Count(); _command_count++)
                {
                    var _command_ıd = _s_commands_list[_command_count];
                    var _s_command_list_current = db.Commands.Where(w => w.Id == _command_ıd).FirstOrDefault();

                    _s_command_list_current.ProcessName = request.ProcessName;

                    db.Commands.Update(_s_command_list_current);
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
        public async Task<JsonResult> DeleteCommands(int pr)
        {
            try
            {
                var _d_commands = await db.Commands.Where(w => w.Id == pr).FirstOrDefaultAsync();
                if (_d_commands == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                var _d_commands_list = db.Commands.Where(w => w.ProcessName == _d_commands.ProcessName ).Select(s => s.Id).ToList();

                for (var _d_command_count = 0; _d_command_count < _d_commands_list.Count(); _d_command_count++)
                {
                    var _command_ıd = _d_commands_list[_d_command_count];

                    var _d_command_list_current = db.Commands.Where(w => w.Id == _command_ıd).FirstOrDefault();

                    db.Commands.Remove(_d_command_list_current);//Hard Delete
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
                var _commands_name = db.Commands.Where(w => w.Id == sId).Select(s => s.ProcessName).FirstOrDefault();
                var _commands_id_list = db.Commands.Where(w => w.ProcessName == _commands_name).Select(s => s.UserId).ToList();

                var showUsersRequest = db.Users.Where(w => _commands_id_list.Contains(w.Id))
                    .Select(s => new showUsersRequest()
                    {
                        Id = s.Id,
                        UserName = s.UserName,
                        Surname = s.Surname,
                        companionName = db.Companions.Where(w => w.UserId == s.Id).Select(s => s.Name).FirstOrDefault()

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

            var _command_name = db.Commands.Where(w => w.Id == sId).Select(s => s.ProcessName).FirstOrDefault();
            lıd = db.Commands.Where(w => w.ProcessName == _command_name).Select(s => s.UserId.ToString()).ToList();

            ListCount = db.Users
           .Select(s => new showUsersRequest()
           {
               Id = s.Id,
               UserName = s.UserName,
               Surname = s.Surname,

           }).ToList();

            return Json(new { data = ListCount, selected = lıd, status = true });
        }

        [HttpPost]
        public async Task<JsonResult> AddUser(int commandId, int[] userIds)
        {
            if (userIds != null)
            {
                var command = db.Commands.Where(w => w.Id == commandId).FirstOrDefault();

                for (var i = 0; i < userIds.Length; i++)
                {
                    var userId = userIds[i];
                    var _registered_user = db.Commands.Where(w => w.ProcessName == command.ProcessName && w.UserId == userId).FirstOrDefault();
                    if (_registered_user == null)
                    {
                        var _add_user = new Commands()
                        {
                            ProcessName = command.ProcessName,
                            Status = command.Status,
                            UserId = userId,
                            CompanionId = db.Companions.Where(w => w.UserId == userId).Select(w => w.Id).First(),

                        };
                        db.Commands.Add(_add_user);
                        await db.SaveChangesAsync();
                    }
                }
                return Json(new { Status = true, Messages = "Success", Code = 200 });

            }
            return Json(new { Status = false, data = "" });
        }

        [HttpGet]
        public async Task<JsonResult> DeleteUser(int pr, int commandId)
        {
            try
            {
                var _d_command_user = await db.Commands.Where(w => w.Id == commandId).FirstOrDefaultAsync();
                var _d_user = await db.Commands.Where(w => w.UserId == pr && w.ProcessName == _d_command_user.ProcessName ).FirstOrDefaultAsync();
                if (_d_user == null) return Json(new { Status = false, data = "", Messages = "Ürün bulunamadı." });

                db.Commands.Remove(_d_user);//Hard Delete
                await db.SaveChangesAsync();

                return Json(new { Status = true, data = "" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, data = "", messages = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> checkCommands(int request)
        {
            try
            {
                var _c_command =await db.Commands.Where(w => w.Id == request).FirstOrDefaultAsync();
                var _c_command_list =await db.Commands.Where(w => w.ProcessName == _c_command.ProcessName ).Select(s => new { s.Status, s.Id }).ToListAsync();

                for (var j = 0; j < _c_command_list.Count(); j++)
                {
                    var _status = _c_command_list[j].Status;
                    var _s_command_id = _c_command_list[j].Id;
                    var _s_command =await db.Commands.Where(w => w.Id == _s_command_id).FirstOrDefaultAsync();

                    if (_status == true)
                    {
                        _s_command.Status = false;

                        db.Commands.Update(_s_command);
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        _s_command.Status = true;

                        db.Commands.Update(_s_command);
                        await db.SaveChangesAsync();
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
