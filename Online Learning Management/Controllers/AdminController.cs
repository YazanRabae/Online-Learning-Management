using LMS.Domain.Entities.Courses;
using LMS.Domain.Entities.Enrollments;
using LMS.Domain.Entities.Users;
using LMS.Repository.Context;
using LMS.Service.Common.Constants;
using LMS.Service.DTOs.Courses;
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
using System.Security.Claims;
using System.Web.WebPages.Html;
using static System.Reflection.Metadata.BlobBuilder;

namespace Online_Learning_Management.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;

        private readonly SignInManager<User> _signInManager;
        private readonly IUserService _userService;
        private readonly DbLMS _context;
        private readonly ICourseService _courseService;
        private readonly IStudentMapper _studentMapper;
        private readonly IStudentService _studentService;
        private readonly IInstructorService _instructorService;
        private readonly IEnrollmentService _enrollmentService;

        public AdminController(UserManager<User>userManager,
           SignInManager<User> signInManager,
           IUserService userService,
           DbLMS context,
           ICourseService courseService,
           IStudentMapper studentMapper,
           IStudentService studentService,
           IInstructorService instructorService,
           IEnrollmentService enrollmentService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _context = context;
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
        public async Task<IActionResult> GetStudents(string name, string email)
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

            return Ok(students);
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
        public async Task<IActionResult> GetInstructorsAsync(string userName, string email)
        {

            var instructors = await _userService.GetInstructors();

            if (!string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(email))
            {
                instructors = instructors.Where(i => i.UserName.Contains(userName, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else if (!string.IsNullOrEmpty(email) && string.IsNullOrEmpty(userName))
            {
                instructors = instructors.Where(i => i.Email.Contains(email, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(email))
            {
                instructors = instructors.Where(i => i.UserName.Contains(userName, StringComparison.OrdinalIgnoreCase)
                                                      && i.Email.Contains(email, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return Ok(instructors);
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
        public async Task<IActionResult> GetCourses(string courseName, string instructorEmail)
        {

            var courses = _context.Courses
               .Include(c => c.Instructor)
               .Select(c => new
               {
                   c.Id,
                   c.Title,
                   c.Description,
                   InstructorName = c.Instructor.Name,
                   InstructorEmail = c.Instructor.Email, // Added
                   c.StartDate,
                   c.EndDate,
                   c.MaxStudents,
                   c.Price,
                   c.CourseTime
               })
               .ToList();

            if (!string.IsNullOrEmpty(instructorEmail))
            {
                courses = courses.Where(i => i.InstructorEmail.Contains(instructorEmail, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(courseName))
            {
                courses = courses.Where(i => i.Title.Contains(courseName, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return Ok(courses);
        }

        public async Task<IActionResult> ManageUserClaims(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            ViewBag.UserName = user.UserName;

            var existingUserClaims = await _userManager.GetClaimsAsync(user);

            var model = new UserClaimsViewModel
            {
                UserId = user.Id
            };

            foreach (var claim in ClaimsStores.AllClaims)
            {
                var userClaim = new UserClaims
                {
                    ClaimType = claim.Type
                };

                if (existingUserClaims.Any(c => c.Type == claim.Type && c.Value == "true"))
                {
                    userClaim.IsSelected = true;
                }
                else
                {
                    userClaim.IsSelected = false;
                }

                model.Claims.Add(userClaim);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.UserId} cannot be found";
                return View("NotFound");
            }

            var existingClaims = await _userManager.GetClaimsAsync(user);
            var result = await _userManager.RemoveClaimsAsync(user, existingClaims);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user's existing claims");
                return View(model);
            }

            result = await _userManager.AddClaimsAsync(user, model.Claims
                .Select(c => new Claim(c.ClaimType, c.IsSelected ? "true" : "false")));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected claims to user");
                return View(model);
            }

            return RedirectToAction("EditUser", new { Id = model.UserId });
        }
    }
}
