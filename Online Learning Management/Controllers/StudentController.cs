using LMS.Service.DTOs.UserDTOs;
using LMS.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Online_Learning_Management.Models;

namespace Online_Learning_Management.Controllers
{

    public class StudentController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IUserService userService;
        public StudentController(UserManager<IdentityUser> userManager,
           SignInManager<IdentityUser> signInManager ,
           IUserService userService)
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
                return RedirectToAction("Dashboard", "Student");

            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await userService.Logout();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto model )
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
            if(signInManager.IsSignedIn(User))
                return RedirectToAction("Dashboard", "Student");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(LogInDto model)
        {
            if (ModelState.IsValid)
            {
                await userService.LogIn(model);
                if (User.IsInRole("Admin"))
                    return RedirectToAction("Index", "Home");
                else
                    return RedirectToAction("Dashboard", "Student");
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
    }
}
