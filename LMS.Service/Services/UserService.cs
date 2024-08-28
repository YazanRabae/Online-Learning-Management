using LMS.Domain.Entities.Users;
using LMS.Repository.Repositories.Courses;
using LMS.Repository.Repositories.Users;
using LMS.Service.DTOs.Courses;
using LMS.Service.DTOs.UserDTOs;
using LMS.Service.Mapper.Courses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICourseMapper courseMapper;
        private readonly ICourseRepository courseRepository;
        public UserService(UserManager<User> userManager,
           SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager,ICourseMapper courseMapper, ICourseRepository courseRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _roleManager = roleManager;
            this.courseMapper = courseMapper;
            this.courseRepository = courseRepository;
        }


        public async Task Register(RegisterDto model, string role)
        {
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
     
            };

            var result = await userManager.CreateAsync(user, model.Password);


            if (result.Succeeded)
            { 

                await userManager.AddToRoleAsync(user, role);

                await signInManager.SignInAsync(user, isPersistent: false);
            }
        }



        public async Task LogIn(LogInDto model)
        {
            await signInManager.PasswordSignInAsync(
           model.Email, model.Password, model.RememberMe, false);
        }


        public async Task Logout()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<List<User>> GetInstructors()
        {
            var instructors = await userManager.GetUsersInRoleAsync("Instructor");
            var list = instructors.ToList();
            return list;

        }
        public async Task<List<User>> GetStudents()
        {
            var Students = await userManager.GetUsersInRoleAsync("Student");
            var list = Students.ToList();
            return list;

        }







    }
}
