﻿using Microsoft.AspNetCore.Mvc;

namespace Online_Learning_Management.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
