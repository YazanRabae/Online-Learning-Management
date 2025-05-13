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

        public async Task<IActionResult> Profile()
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Index", "Home");

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
        public IActionResult Enrollments()
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

                TempData["Success"] = "Successfully enrolled!";
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

            return Ok(enrollments);
        }

        [HttpPost]
        public async Task<IActionResult> Accept(int id)
        {
            await _enrollmentService.AcceptEnrollmentAsync(id);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {

            await _enrollmentService.RejectEnrollmentAsync(id);
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> RejectStudent(int studentId)
        {
            if (studentId <= 0)
            {
                return BadRequest("Invalid student ID.");
            }

            await _enrollmentService.RejectStudentByIdAsync(studentId);

            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetStudentsByCourse(int courseId)
        {
            var students = await _enrollmentService.GetStudentsByCourse(courseId);

            return Json(students);
        }
    }

}

