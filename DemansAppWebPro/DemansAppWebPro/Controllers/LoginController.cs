using DemansAppWebPro.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DemansAppWebPro.Helper.DTO.Login;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DemansAppWebPro.Controllers
{
    public class LoginController : Controller
    {
        private readonly EntitiesContext db;

        public LoginController(EntitiesContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
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
                var response = await db.Admins.Where(w => w.Username == request.UserName && w.Password == request.Password).FirstOrDefaultAsync();

                if (response != null)
                {
                    string JsonToString = JsonConvert.SerializeObject(response);

                    var claims = new List<Claim> {
                        new Claim(ClaimTypes.Name, JsonToString),
                        new Claim("UserId", response.Id.ToString())
                    };
                    var userIdentity = new ClaimsIdentity(claims, "login");

                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    ViewBag.ErrorMessage = "";

                    return RedirectToAction("Index", "Users");
                }
                else
                {
                    ViewBag.ErrorMessage = "1";

                    ModelState.AddModelError("Error", "1");
                }
            }
            else
            {
                ViewBag.ErrorMessage = "2";

                ModelState.AddModelError("Info", "2");
            }

            return View();
        }

    }
}
