using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Online_Learning_Management.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetStudents()
        {
            return View();
        }
        public IActionResult GetInstructors()
        {
            return View();
        }
        public IActionResult GetCourses()
        {
            return View();
        }
    }
}
