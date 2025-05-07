using LMS.Service.Common.Constants;
using LMS.Service.DTOs.Students;
using LMS.Service.DTOs.UserDTOs;
using LMS.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Online_Learning_Management.Models;
using System.Diagnostics;

namespace Online_Learning_Management.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;

        public HomeController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult LogIn()
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await _userService.Logout();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn(LogInDto model)
        {
            if (ModelState.IsValid)
            {

                var loginSuccussed = await _userService.LogIn(model);
                if (!loginSuccussed)
                {
                    TempData["Error"] = "Login Failed";
                    TempData["Password"] = model.Password;
                    return View(model);
                }
                var role = await _userService.GetRoleByName(model.Email);

                TempData["Success"] = "Login Successfully";
                if (role == RoleConstants.Admin)
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                else if (role == RoleConstants.Instructor)
                {
                    return RedirectToAction("Dashboard", "Instructors");
                }
                else if (role == RoleConstants.Student)
                {
                    return RedirectToAction("Dashboard", "Student");
                }

                TempData["Password"] = model.Password;
                return View(model);
            }
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode)
        {
            return View("AccessDenied");
        }
    }
}
