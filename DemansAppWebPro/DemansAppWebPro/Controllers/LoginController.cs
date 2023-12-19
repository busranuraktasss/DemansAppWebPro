using DemansAppWebPro.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DemansAppWebPro.Helper.DTO.Login;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Azure;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Http;
using DemansAppWebPro.Helper.IManager;

namespace DemansAppWebPro.Controllers
{
    public class LoginController : Controller
    {
        private readonly EntitiesContext db;
        private readonly IMedicinesManager _medicineManager;

        public LoginController(EntitiesContext _db, IMedicinesManager medicineManager)
        {
            db = _db;
            _medicineManager = medicineManager;

        }

        public IActionResult Index()
        {
             var medicine = _medicineManager.getAllMedicines();
            return View();
        }

        public async Task<ActionResult> LogOut()
        {
          
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);


            return RedirectToAction("Index", "Login", new { area = "Dashboard" });
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Index(getLoginRequest request)
        {
            if (ModelState.IsValid)
            {
                var a_response = await db.Admins.Where(w => w.Username == request._email && w.Password == request._password).FirstOrDefaultAsync();

                if (a_response != null)
                {
                    string JsonToString = JsonConvert.SerializeObject(a_response);

                    var d_claims = new List<Claim> {
                        new Claim(ClaimTypes.Name, JsonToString),
                        new Claim("Id", a_response.Id.ToString()),
                          new Claim(ClaimTypes.Role,"Admin")
                    };
                    var d_userIdentity = new ClaimsIdentity(d_claims, "login");

                    ClaimsPrincipal principal = new ClaimsPrincipal(d_userIdentity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                   

                    ViewBag.ErrorMessage = "0";
                    return Json(new { data = "Admin", Id = a_response.Id, Messages = ViewBag.ErrorMessage });
                }
                else
                {
                    var u_response = await db.Users.Where(w => w.Email == request._email && w.Password == request._password).FirstOrDefaultAsync();

                    if (u_response != null)
                    {
                        string JsonToString = JsonConvert.SerializeObject(u_response);

                        var m_claims = new List<Claim>
                        {
                            new Claim (ClaimTypes.Name, JsonToString),
                            new Claim("Id", u_response.Id.ToString()),
                              new Claim(ClaimTypes.Role,"User")
                        };
                        var m_userIdentity = new ClaimsIdentity(m_claims, "login");

                        ClaimsPrincipal principal = new ClaimsPrincipal(m_userIdentity);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                        ViewBag.ErrorMessage = "0";

                        TempData["UserId"] = u_response.Id;

                        return Json(new { data = "User", Id = u_response.Id, Messages = ViewBag.ErrorMessage });
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "1";
                        return Json(new { data = "", Messages = ViewBag.ErrorMessage });
                    }
                }
            }
            else { ViewBag.ErrorMessage = "2"; return Json(new { data = " ", Messages = ViewBag.ErrorMessage }); }

        }

        [HttpPost]
        public JsonResult getActivation(string email, string name)
        {
            MailMessage msg = new MailMessage();
            msg.Subject = "DemansApp";
            msg.From = new MailAddress("demansapplication@gmail.com", "DemansApp ONAY KODU");//Obezindex maili olunca değiştir
            msg.To.Add(new MailAddress(email, name));

            Random rastgele = new Random();
            string sayilar = "0123456789";
            var _OnayKodu = "";
            for (int i = 0; i < 6; i++)
            {
                _OnayKodu += sayilar[rastgele.Next(sayilar.Length)];
            }

            msg.IsBodyHtml = false;
            msg.Body = "Onay Kodu : " + _OnayKodu;

            msg.Priority = MailPriority.High;

            SmtpClient smtp = new SmtpClient("mail.kurumsaleposta.com", 587); //Bu alanda gönderim yapacak hizmetin smtp adresini ve size verilen portu girmelisiniz.
            NetworkCredential AccountInfo = new NetworkCredential("demansapplication@gmail.com", "7/8*9-6+");
            smtp.UseDefaultCredentials = false; //Standart doğrulama kullanılsın mı? -> Yalnızca gönderici özellikle istiyor ise TRUE işaretlenir.
            smtp.Credentials = AccountInfo;
            smtp.EnableSsl = false; //SSL kullanılarak mı gönderilsin...

            smtp.Send(msg);
            return Json(new { msg = "E-Posta başarıyla gönderildi.", status = true, data = _OnayKodu });
        }

        [HttpPost]
        public JsonResult AddUserRegister(addUserRequest rq)
        {
            try
            {
                var u_cntrl = db.Users.Where(w => w.Email == rq.u_email).FirstOrDefault();
                if (u_cntrl != null) { return Json(new { message = rq.u_email + "mail adresi sistemde kayıtlıdır.", status = false }); }
                var gender = false;//ERKEK
                if(rq.u_gender == 2)
                {
                    gender = true;//KADIN
                }
                db.Users.Add(new Users
                {
                    UserName = rq.u_name,
                    Surname = rq.u_surname,
                    Email = rq.u_email,
                    Password = rq.u_password,
                    Phone = rq.u_phone,
                    EmergencyPhone = rq.u_emergency,
                    Sex = gender,
                    Status = 1 //1:AKTİF 0:PASİF
                });
                db.SaveChanges();

                return Json(new { status = true, message = "Kullanıcı Kayıt Olma İşlemi Başarıyla Gerçekleşti." });
            }
            catch (Exception ex) { return Json(new { status = false, messages = "Kulanıcı Kayıt Olma İşlemi Gerçekleşemedi." }); }
        }
    }
}
