using LMS.Domain.Entities.Users;
using LMS.Service.DTOs.UserDTOs;
using LMS.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Online_Learning_Management.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IUserService userService;
        public InstructorsController(UserManager<User> userManager,
           SignInManager<User> signInManager, IUserService userService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.userService = userService;
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
            if (signInManager.IsSignedIn(User))
                return View();

            return RedirectToAction("LogIn", "Instructors");
        }

        [Authorize(Roles = "Instructor")]
        public IActionResult GetAllCourses()
        {
            return View();
        }

        public IActionResult GetAllEnrollments()
        {
            return View();
        }



        public IActionResult AddCourses()
        {
            return View(); 
        }
    }
}

