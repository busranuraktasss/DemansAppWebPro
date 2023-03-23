using Microsoft.AspNetCore.Mvc;

namespace DemansAppWebPro.Controllers
{
    public class MedicinesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
