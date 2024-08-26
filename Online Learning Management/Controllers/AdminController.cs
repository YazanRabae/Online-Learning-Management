using LMS.Domain.Entities.Users;
using LMS.Repository.Context;
using LMS.Service.DTOs.UserDTOs;
using LMS.Service.Services;
using LMS.Service.Services.Courses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Online_Learning_Management.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        
        private readonly SignInManager<User> _signInManager;
        private readonly IUserService _userService;
        private readonly DbLMS _context; // Inject DbContext
        private readonly ICourseService _courseService;

        public AdminController(UserManager<User> userManager,
           SignInManager<User> signInManager,
           IUserService userService,
           DbLMS context , ICourseService courseService) // Add DbContext parameter
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _context = context; // Assign DbContext
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

            if (!string.IsNullOrEmpty(userName))
            {
                Students = Students.Where(i => i.UserName.Contains(userName, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (!string.IsNullOrEmpty(email))
            {
                Students = Students.Where(i => i.Email.Contains(email, StringComparison.OrdinalIgnoreCase)).ToList();
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
 
            var instructors = await _userService.GetInstructors(); // Assuming this returns a Task<List<Instructor>>

      
            if (!string.IsNullOrEmpty(userName))
            {
                instructors = instructors.Where(i => i.UserName.Contains(userName, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (!string.IsNullOrEmpty(email))
            {
                instructors = instructors.Where(i => i.Email.Contains(email, StringComparison.OrdinalIgnoreCase)).ToList();
            }

  
            return Ok(instructors);
        }


        public IActionResult Courses()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            var courses = await  _courseService.GetAllCourses();
            return Json(courses);
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

        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
