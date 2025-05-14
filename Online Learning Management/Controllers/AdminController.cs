using LMS.Domain.Entities.Courses;
using LMS.Domain.Entities.Enrollments;
using LMS.Domain.Entities.Instructors;
using LMS.Domain.Entities.Students;
using LMS.Domain.Entities.Users;
using LMS.Repository.Context;
using LMS.Service.Common.Constants;
using LMS.Service.DTOs.Courses;
using LMS.Service.DTOs.Instructors;
using LMS.Service.DTOs.Shared;
using LMS.Service.DTOs.Students;
using LMS.Service.DTOs.UserDTOs;
using LMS.Service.Mapper.Students;
using LMS.Service.Services;
using LMS.Service.Services.Courses;
using LMS.Service.Services.Enrollments;
using LMS.Service.Services.Instructors;
using LMS.Service.Services.Students;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Security.Claims;
using System.Web.WebPages.Html;
using static System.Reflection.Metadata.BlobBuilder;

namespace Online_Learning_Management.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICourseService _courseService;
        private readonly IStudentMapper _studentMapper;
        private readonly IStudentService _studentService;
        private readonly IInstructorService _instructorService;
        private readonly IEnrollmentService _enrollmentService;

        public AdminController(
           IUserService userService,
           ICourseService courseService,
           IStudentMapper studentMapper,
           IStudentService studentService,
           IInstructorService instructorService,
           IEnrollmentService enrollmentService)
        {
            _userService = userService;
            _courseService = courseService;
            _studentMapper = studentMapper;
            _studentService = studentService;
            _instructorService = instructorService;
            _enrollmentService = enrollmentService;
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Students()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents(string name, string email, int? pageNumber)
        {
            var students = await _studentService.GetStudents();

            if (!string.IsNullOrEmpty(name))
            {
                students = students.Where(i => i.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (!string.IsNullOrEmpty(email))
            {
                students = students.Where(i => i.Email.Contains(email, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var paginatedStudents = PaginatedList<StudentDto>.CreateAsync(students, pageNumber ?? 1);

            if (!paginatedStudents.Any())
            {
                paginatedStudents = PaginatedList<StudentDto>.CreateAsync(students, 1);
            }

            return Ok(new
            {
                Students = paginatedStudents,
                PageIndex = paginatedStudents.PageIndex,
                PageSize = paginatedStudents.PageSize,
                TotalPages = paginatedStudents.TotalPages,
                HasPreviousPage = paginatedStudents.HasPreviousPage,
                HasNextPage = paginatedStudents.HasNextPage
            });
        }

        public IActionResult CreateUser(string roleName)
        {
            return View(new CreateUserDto()
            {
                RoleName = roleName
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser([Bind("Name,Email,Password,ConfirmPassword,RoleName")] CreateUserDto createUserDto)
        {
            if (ModelState.IsValid)
            {
                //Regiser Student
                var result = await _userService.Register(_studentMapper.MapFromCreateStudentDtoToRegiserModel(createUserDto), createUserDto.RoleName);

                if (!result.IsSuccess)
                {
                    TempData["Password"] = createUserDto.Password;
                    TempData["ConfirmPassword"] = createUserDto.ConfirmPassword;
                    TempData["Error"] = result.MessageError;
                    return View(createUserDto);
                }

                createUserDto.UserId = result.UserId;

                if (createUserDto.RoleName == RoleConstants.Student)
                {
                    await _studentService.CreateStudent(createUserDto);
                    TempData["Success"] = createUserDto.RoleName + " Created Successfully";
                    return RedirectToAction("students", "Admin");
                }
                else
                {
                    await _instructorService.CreateInstructor(createUserDto);
                    TempData["Success"] = createUserDto.RoleName + " Created Successfully";
                    return RedirectToAction("Instructors", "Admin");
                }
            }

            TempData["Password"] = createUserDto.Password;
            TempData["ConfirmPassword"] = createUserDto.ConfirmPassword;
            return View(createUserDto);
        }

        public IActionResult Instructors()
        {
            return View();
        }



        [HttpGet]
        public async Task<IActionResult> GetInstructorsAsync(string name, string email, int? pageNumber, bool getAll = false)
        {
            var instructors = await _instructorService.GetInstructors();

            if (getAll) return Ok(new { Instructors = instructors });

            if (!string.IsNullOrEmpty(name))
            {
                instructors = instructors.Where(i => i.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (!string.IsNullOrEmpty(email))
            {
                instructors = instructors.Where(i => i.Email.Contains(email, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            var paginatedInstructors = PaginatedList<InstructorDto>.CreateAsync(instructors, pageNumber ?? 1);

            if (!paginatedInstructors.Any())
            {
                paginatedInstructors = PaginatedList<InstructorDto>.CreateAsync(instructors, 1);
            }

            return Ok(new
            {
                Instructors = paginatedInstructors,
                PageIndex = paginatedInstructors.PageIndex,
                PageSize = paginatedInstructors.PageSize,
                TotalPages = paginatedInstructors.TotalPages,
                HasPreviousPage = paginatedInstructors.HasPreviousPage,
                HasNextPage = paginatedInstructors.HasNextPage
            });
        }



        [HttpGet]
        public async Task<IActionResult> GetStudentsByCourse(int courseId)
        {
            var students = await _enrollmentService.GetStudentsByCourse(courseId);

            return Json(students);
        }

        public IActionResult Courses()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCourses(string courseName, int? instructorId, int? pageNumber)
        {
            var courses = await _courseService.GetCourses();

            if (instructorId.HasValue && instructorId > 0)
            {
                courses = courses.Where(i => i.InstructorId == instructorId).ToList();
            }
            if (!string.IsNullOrEmpty(courseName))
            {
                courses = courses.Where(i => i.Title.Contains(courseName, StringComparison.OrdinalIgnoreCase)).ToList();
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
    }
}
