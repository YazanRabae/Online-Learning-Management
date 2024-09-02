using LMS.Domain.Entities.Courses;
using LMS.Domain.Entities.Users;
using LMS.Service.DTOs.UserDTOs;
using LMS.Service.Services;
using LMS.Service.Services.Courses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Register()
        {
            if (signInManager.IsSignedIn(User))
                return RedirectToAction("Dashboard", "Student");

            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await userService.Logout();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                await userService.Register(model, "Student");
                return RedirectToAction("Dashboard", "Student");

            }
            return View(model);
        }
        public IActionResult LogIn()
        {
            if (signInManager.IsSignedIn(User))
                return RedirectToAction("Dashboard", "Student");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(LogInDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user == null)
                    return RedirectToAction("AccessDenied", "Shared");

                var roles = await userManager.GetRolesAsync(user);

                var roleAssign = roles.FirstOrDefault();

                if (roleAssign == "Student")
                {
                    await userService.LogIn(model);
                    return RedirectToAction("Dashboard", "Student");
                }
                else
                    return RedirectToAction("AccessDenied", "Shared");
            }

            return View(model);
        }
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Profile()
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Register");

            var model = new RegisterDto
            {
                Name = user.UserName,
                Email = user.Email
            };

            return View(model);
        }
        [Authorize(Roles = "Student")]
        public IActionResult Dashboard()
        {

            if (signInManager.IsSignedIn(User))
                return View();

            return RedirectToAction("LogIn", "Student");
        }
        public async Task<IActionResult> GetAllCourses()
        {
            var UserId = userManager.GetUserId(User);

            var allCourses = await courseService.GetAllCourses(UserId);

            return Ok(allCourses);
        }

        [HttpPost]
        public async Task<IActionResult> AddEnrollment(int courseId)
        {
            var isEnrolled = await courseService.IsEnrolled(userManager.GetUserId(User), courseId);

            if (isEnrolled)
                return BadRequest();

            await courseService.AddEnrollment(userManager.GetUserId(User), courseId);
            return Ok();
        }
    }
}
