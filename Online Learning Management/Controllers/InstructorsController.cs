using LMS.Domain.Entities.Enrollments;
using LMS.Domain.Entities.Users;
using LMS.Repository.Context;
using LMS.Service.DTOs.Courses;
using LMS.Service.DTOs.UserDTOs;
using LMS.Service.Services;
using LMS.Service.Services.Courses;
using LMS.Service.Services.Enrollments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Online_Learning_Management.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IUserService userService;
        private readonly ICourseService _courseService;
        private readonly IEnrollmentService _enrollmentService;
        public InstructorsController(UserManager<User> userManager,
           SignInManager<User> signInManager, IUserService userService, ICourseService courseService, IEnrollmentService enrollmentService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.userService = userService;
            _courseService = courseService;
            _enrollmentService = enrollmentService;
        }


        public IActionResult Index()
        {
            return View();
        }


        [AllowAnonymous]
        public IActionResult LogIn()
        {
            if (signInManager.IsSignedIn(User))
                return RedirectToAction("Dashboard", "Instructor");

            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn(LogInDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return View(model);

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

                    return RedirectToAction("Dashboard", "Instructor");
                }
                else if (roleAssign == "Instructor")
                {
                    await userService.LogIn(model);

                    return RedirectToAction("Dashboard", "Instructor");
                }
                else
                    return View(model);
            }

            return View(model);
        }
  

        public IActionResult Register()
        {
            if (signInManager.IsSignedIn(User))
                return RedirectToAction("Dashboard", "Instructor");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                await userService.Register(model, "Instructor");
                return RedirectToAction("Dashboard", "Instructor");
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await userService.Logout();
            return RedirectToAction("LogIn", "Instructor");
        }

        public async Task<IActionResult> Profile()
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Register", "Instructor");

            var model = new RegisterDto
            {
                Name = user.UserName,
                Email = user.Email
            };

            return View(model);
        }

        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Dashboard()
        {
            if (signInManager.IsSignedIn(User))
            {
                var userId = userManager.GetUserId(User);
                ViewBag.CourseCount = await _courseService.NumberOfCourses(userId);
                return View();
            }

            return RedirectToAction("LogIn", "Instructor");
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

        public async Task<IActionResult> GetAllCourses()
        {

            var userId = userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var courses = (await _courseService.GetCoursesByUserId(userId))
                .Select(c => new
                {
                    c.Id,
                    c.Title,
                    c.Description,
                    InstructorName = c.Instructor.Name,
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCourses([Bind("Title,Description,StartDate,EndDate,Price,CourseTime,ImageFile,MaxStudents,InstructorId")] CourseDTO courseDTO)
        {
            if (ModelState.IsValid)
            {
                var userId = userManager.GetUserId(User);

                await _courseService.CreateCourse(courseDTO, userId);

                return RedirectToAction("Courses", "Instructors");
            }

            return View(courseDTO);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllPendingEnrollments()
        {
            // Get the currently authenticated instructor's username
            var userId = userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }


            var enrollments = (await _enrollmentService.GetAllEnrollmentsByUserId(userId))
                .Select(e => new
                {
                    e.Id,
                    StudentName = e.Student.Name,
                    CourseName = e.Course.Title,
                    e.AddDate,
                    e.Course.Price,
                    status = e.Status,
                })
                .ToList();

            if (enrollments == null || !enrollments.Any())
            {
                return NotFound("No pending enrollments found for the current instructor.");
            }

            return Ok(enrollments);
        }

        [HttpPost]
        public async Task<IActionResult> Accept(int id)
        {
            await _enrollmentService.AcceptEnrollmentAsync(id);
            return RedirectToAction("GetAllEnrollmentsByUserId", "Instructor");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {

            await _enrollmentService.RejectEnrollmentAsync(id);
            return RedirectToAction("GetAllEnrollmentsByUserId", "Instructor");
        }
    }

}

