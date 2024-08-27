using LMS.Domain.Entities.Users;
using LMS.Repository.Context;
using LMS.Service.DTOs.UserDTOs;
using LMS.Service.Services;
using LMS.Service.Services.Courses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Online_Learning_Management.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IUserService userService;
        private readonly ICourseService _courseService;
        private readonly DbLMS _context;
        public InstructorsController(UserManager<User> userManager,
           SignInManager<User> signInManager , IUserService userService , ICourseService courseService, DbLMS context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.userService = userService;
            _courseService = courseService;
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            if (signInManager.IsSignedIn(User))
                return RedirectToAction("Dashboard", "Instructors");

            return View();
        }

        [AllowAnonymous]
        public IActionResult LogIn()
        {
            if (signInManager.IsSignedIn(User))
                return RedirectToAction("Dashboard", "Instructors");

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
                await userService.Register(model, "Instructor");
                return RedirectToAction("Dashboard", "Instructors");
            }
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn(LogInDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                var roles = await userManager.GetRolesAsync(user);

                var roleAssign = roles.FirstOrDefault();

                if (roleAssign == "Admin")
                {
                    await userService.LogIn(model);

                    return RedirectToAction("Dashboard", "Admin");
                }
                else if (roleAssign == "Instructor")
                {
                    await userService.LogIn(model);

                    return RedirectToAction("Dashboard", "Instructors");
                }
                else
                    return RedirectToAction("AccessDenied", "Shared");
            }

            return View(model);
        }

        public async Task<IActionResult> Profile()
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Register", "Instructors");

            var model = new RegisterDto
            {
                Name = user.UserName,
                Email = user.Email
            };

            return View(model);
        }

        [Authorize(Roles = "Instructor")]
        public IActionResult Dashboard()
        {
            if (signInManager.IsSignedIn(User)) { 
            var instructorUsername = User.Identity.Name;
            var courseCount = _context.Courses.Count(c => c.Instructor.UserName == instructorUsername);

            //var model = new DashboardViewModel
            //{
            //    CourseCount = courseCount
            //};
            ViewBag.CourseCount = courseCount;  
            return View();
        }

            return RedirectToAction("LogIn", "Instructors");
        }

        [Authorize(Roles = "Instructor")]
        public IActionResult GetAllEnrollments()
        {
            return View();
        }


        [Authorize(Roles = "Instructor")]
        public IActionResult Courses()
        {
            return View();
        }

        public IActionResult AddCourses()
        {
            return View(); 
        }
        [HttpGet]
   
        public IActionResult GetAllCourses()
        {
            // Get the currently logged-in instructor's username
            var instructorUsername = User.Identity.Name;

            if (string.IsNullOrEmpty(instructorUsername))
            {
                return Unauthorized(); // Or handle the unauthorized case as needed
            }

            // Fetch courses related to the logged-in instructor
            var courses = _context.Courses
                .Include(c => c.Instructor)  // Include the related Instructor data
                .Where(c => c.Instructor.UserName == instructorUsername) // Filter by logged-in instructor
                .Select(c => new
                {
                    c.Id,
                    c.Title,
                    c.Description,
                    InstructorName = c.Instructor.UserName,  // Assuming UserName is the name you want to display
                    c.StartDate,
                    c.EndDate,
                    c.MaxStudents,
                    c.Price,
                    c.CourseTime
                })
                .ToList();

            if (courses == null || !courses.Any())
            {
                return NotFound("No courses found for the current instructor.");
            }

            return Ok(courses);
        }

    }
}
     