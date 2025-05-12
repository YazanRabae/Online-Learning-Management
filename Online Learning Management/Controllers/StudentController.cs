using LMS.Domain.Entities.Courses;
using LMS.Domain.Entities.Users;
using LMS.Service.DTOs.UserDTOs;
using LMS.Service.Services;
using LMS.Service.Services.Courses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Online_Learning_Management.Models;

namespace Online_Learning_Management.Controllers
{
    public class StudentController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IUserService userService;
        private readonly ICourseService courseService;
        public StudentController(UserManager<User> userManager,
           SignInManager<User> signInManager,
           IUserService userService, ICourseService courseService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.userService = userService;
            this.courseService = courseService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Profile()
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Index" , "Home");

            var model = new RegisterDto
            {
                Name = user.UserName,
                Email = user.Email
            };

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Courses()
        {
            var userId = userManager.GetUserId(User);
            return View(await courseService.GetEnrolledCoursesSplitAsync(userId));
        }

        [HttpGet]
        public async Task<IActionResult> Enroll()
        {
            var userId = userManager.GetUserId(User);
            return View(await courseService.GetAvailableCoursesAsync(userId));
        }

        [HttpPost]
        public async Task<IActionResult> AddEnrollment(int courseId)
        {
            var userId = userManager.GetUserId(User);

            if (await courseService.IsEnrolled(userId, courseId))
                return BadRequest("Already enrolled");

            await courseService.AddEnrollment(userId, courseId);
            TempData["Success"] = "Successfully enrolled!";
            return Ok();
        }

    }

}
