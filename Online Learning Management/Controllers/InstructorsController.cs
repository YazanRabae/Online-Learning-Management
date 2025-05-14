using LMS.Domain.Entities.Enrollments;
using LMS.Domain.Entities.Instructors;
using LMS.Domain.Entities.Users;
using LMS.Repository.Context;
using LMS.Service.DTOs.Courses;
using LMS.Service.DTOs.Enrollments;
using LMS.Service.DTOs.Instructors;
using LMS.Service.DTOs.Shared;
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

        public async Task<IActionResult> GetAllCourses(string title,
            string startDateFrom,
            string startDateTo,
            string endDateFrom,
            string endDateTo,
            int? pageNumber)
        {
            var userId = userManager.GetUserId(User);
            var courses = await _courseService.GetCoursesByUserId(userId);

            if (!string.IsNullOrEmpty(title))
            {
                courses = courses.Where(i => i.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (!string.IsNullOrEmpty(startDateFrom) && !string.IsNullOrEmpty(startDateTo))
            {
                DateTime dateFrom = DateTime.Parse(startDateFrom);
                DateTime dateTo = DateTime.Parse(startDateTo);
                courses = courses.Where(i => dateFrom <= i.StartDate && i.StartDate <= dateTo).ToList();
            }
            if (!string.IsNullOrEmpty(endDateFrom) && !string.IsNullOrEmpty(endDateTo))
            {
                DateTime dateFrom = DateTime.Parse(endDateFrom);
                DateTime dateTo = DateTime.Parse(endDateTo);
                courses = courses.Where(i => dateFrom <= i.EndDate && i.EndDate <= dateTo).ToList();
            }

            var paginatedCourses = PaginatedList<CourseDTO>.CreateAsync(courses, pageNumber ?? 1);

            if (!paginatedCourses.Any())
            {
                paginatedCourses = PaginatedList<CourseDTO>.CreateAsync(courses, 1);
            }

            return Ok(new
            {
                Courses = paginatedCourses,
                PageIndex = paginatedCourses.PageIndex,
                PageSize = paginatedCourses.PageSize,
                TotalPages = paginatedCourses.TotalPages,
                HasPreviousPage = paginatedCourses.HasPreviousPage,
                HasNextPage = paginatedCourses.HasNextPage
            });
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
        public async Task<IActionResult> GetAllPendingEnrollments(
            string courseName,
            string studentName,
            int? pageNumber
            )
        {
            var userId = userManager.GetUserId(User);

            var enrollments = (await _enrollmentService.GetAllEnrollmentsByUserId(userId));

            if (!string.IsNullOrEmpty(courseName))
            {
                enrollments = enrollments.Where(i => i.CourseName.Contains(courseName, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (!string.IsNullOrEmpty(studentName))
            {
                enrollments = enrollments.Where(i => i.StudentName.Contains(studentName, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var paginatedEnrollments = PaginatedList<EnrollmentDTO>.CreateAsync(enrollments, pageNumber ?? 1);

            if (!paginatedEnrollments.Any())
            {
                paginatedEnrollments = PaginatedList<EnrollmentDTO>.CreateAsync(enrollments, 1);
            }

            return Ok(new
            {
                Enrollments = paginatedEnrollments,
                PageIndex = paginatedEnrollments.PageIndex,
                PageSize = paginatedEnrollments.PageSize,
                TotalPages = paginatedEnrollments.TotalPages,
                HasPreviousPage = paginatedEnrollments.HasPreviousPage,
                HasNextPage = paginatedEnrollments.HasNextPage
            });
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
        public async Task<IActionResult> RejectStudent(int studentId, int courseId)
        {
            if (studentId <= 0 || courseId <= 0)
            {
                return BadRequest("Invalid student ID.");
            }

            await _enrollmentService.RejectStudentByIdAsync(studentId, courseId);

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

