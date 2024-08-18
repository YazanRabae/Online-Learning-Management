using LMS.Domain.Entities.User;
using LMS.Service.DTOs.UserDTOs;
using LMS.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Online_Learning_Management.Controllers
{
    //[Authorize(Roles = "Instructor")]
    public class InstructorsController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IUserService userService;
        public InstructorsController(UserManager<IdentityUser> userManager,
           SignInManager<IdentityUser> signInManager , IUserService userService)
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
            return View();
        }
        [AllowAnonymous]
        public IActionResult LogIn()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await userService.Logout();
            return RedirectToAction("Dashboard", "Instructors");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                await userService.Register(model);
                 return RedirectToAction("Dashboard", "Instructors"); //false is session cookies ,Ture is persistent cookies
                
            }
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn(LogInDto model)
        {
            if (ModelState.IsValid)
            {
                await userService.LogIn(model);
               return RedirectToAction("Dashboard", "Instructors");
            }

            return View(model);
        }
        public async Task<IActionResult> Profile()
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Register" , "Instructors");
            }

            var model = new RegisterDto
            {
                Name = user.UserName,
                Email = user.Email
            };

            return View(model);
        }
        //public async Task<IActionResult> Logout()
        //{
        //    await userService.Logout();
        //    return RedirectToAction("index", "Home");
        //}
        [AcceptVerbs("Get", "Post")]
        //allow how are not authintecated not login
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailUsed(string Email)
        {
            var result = await userManager.FindByEmailAsync(Email);
            if (result == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"this {Email} is already in used");
            }

        }
        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult GetAllCourses()
        {
            return View();
        }


    }
}
