using LMS.Domain.Entities.Users;
using LMS.Repository.Repositories.Users;
using LMS.Service.DTOs.Courses;
using LMS.Service.DTOs.UserDTOs;
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
        private readonly RoleManager<User> _roleManager;
        public UserService(UserManager<User> userManager,
           SignInManager<User> signInManager,RoleManager<User>roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }


        public async Task Register(RegisterDto model, string role)
        {
            var user = new User { UserName = model.Email, Email = model.Email };
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // Assign the role based on the signup type
                await userManager.AddToRoleAsync(user, role);

                // Sign in the user
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
