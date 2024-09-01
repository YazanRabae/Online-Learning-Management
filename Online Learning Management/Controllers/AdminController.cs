using LMS.Domain.Entities.Courses;
using LMS.Domain.Entities.Users;
using LMS.Repository.Context;
using LMS.Service.DTOs.UserDTOs;
using LMS.Service.Services;
using LMS.Service.Services.Courses;
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

        public AdminController(UserManager<User> userManager,
           SignInManager<User> signInManager,
           IUserService userService,
           DbLMS context, ICourseService courseService) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _context = context;
            _courseService = courseService;
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

        public async Task<IActionResult> GetStudentsAsync(string userName, string email)
        {

            var Students = await _userService.GetStudents();

            if (!string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(email))
            {
                Students = Students.Where(i => i.UserName.Contains(userName, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else if (!string.IsNullOrEmpty(email) && string.IsNullOrEmpty(userName))
            {
                Students = Students.Where(i => i.Email.Contains(email, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(email))
            {
                Students = Students.Where(i => i.UserName.Contains(userName, StringComparison.OrdinalIgnoreCase)
                                                      && i.Email.Contains(email, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return Ok(Students);
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




        public IActionResult Courses()
        {
            return View();
        }
      
        [HttpGet]
        public async Task<IActionResult> GetCourses(string courseName, string instructorId)
        {

            var courses = _context.Courses
               .Include(c => c.Instructor)  
               .Select(c => new
               {
                   c.Id,
                   c.Title,
                   c.Description,
                   InstructorName = c.Instructor.UserName,  
                   c.StartDate,
                   c.EndDate,
                   c.MaxStudents,
                   c.Price,
                   c.CourseTime
               })
               .ToList();

            if (string.IsNullOrEmpty(courseName) && !string.IsNullOrEmpty(instructorId))
            {
      
                courses = courses.Where(i => i.InstructorName.Contains(instructorId, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else if (!string.IsNullOrEmpty(courseName) && !string.IsNullOrEmpty(instructorId))
            {
         
                courses = courses.Where(i => i.Title.Contains(courseName, StringComparison.OrdinalIgnoreCase)
                                            && i.InstructorName.Contains(instructorId, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else if (!string.IsNullOrEmpty(courseName) && string.IsNullOrEmpty(instructorId))
            {
                courses = courses.Where(i => i.Title.Contains(courseName, StringComparison.OrdinalIgnoreCase)).ToList();
            }


            return Ok(courses);
        }

        public IActionResult LogIn()
        {
            return View();
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

        public async Task<IActionResult> DashboardAsync()
        {
            // Get the counts
            var courses = await _courseService.GetAllCourses();
            var students = await _userService.GetStudents();
            var instructors = await _userService.GetInstructors();

            // Getting the counts
            int courseCount = courses.Count();
            int studentCount = students.Count();
            int instructorCount = instructors.Count();
            // Store the counts in ViewBag
            ViewBag.CourseCount = courseCount;
            ViewBag.StudentCount = studentCount;
            ViewBag.InstructorCount = instructorCount;

            // Return the view
            return View();
        }


    }
}
