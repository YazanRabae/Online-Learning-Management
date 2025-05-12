using LMS.Domain.Entities.Users;
using LMS.Repository.Context;
using LMS.Service.Common.Constants;
using LMS.Service.DTOs.Students;
using LMS.Service.DTOs.UserDTOs;
using LMS.Service.Services;
using LMS.Service.Services.Courses;
using LMS.Service.Services.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Learning_Management.Models;
using System.Diagnostics;

namespace Online_Learning_Management.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly ISharedService _sharedService;


        public HomeController(IUserService userService, ICourseService courseService, UserManager<User> userManager, ISharedService sharedService)
        {
            _userService = userService;
            _userManager = userManager;
            _sharedService = sharedService;

        }
        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> Dashboard()
        {
            var userId = _userManager.GetUserId(User);
            var role = await _userService.GetRoleByUserId(userId);

            // Get the counts
            var (coursesCount, studentsCount, instructorsCount, courseTitles, enrollmentCounts) = await _sharedService.GetCounts(role, userId);

            // Store the counts in ViewBag
            ViewBag.CourseCount = coursesCount;
            ViewBag.StudentCount = studentsCount;
            ViewBag.InstructorCount = instructorsCount;
            ViewBag.CourseTitles = courseTitles;
            ViewBag.EnrollmentCounts = enrollmentCounts;

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
                return RedirectToAction("Dashboard", "Home");
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
