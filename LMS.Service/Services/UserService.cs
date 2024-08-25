﻿using LMS.Domain.Entities.Users;
using LMS.Repository.Repositories.Users;
using LMS.Service.DTOs.UserDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public UserService(UserManager<User> userManager,
           SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _roleManager = roleManager;
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
   
        public async Task<List<IdentityUser>> GetInstructors()
        {
            var instructors = await userManager.GetUsersInRoleAsync("Instructor");
            var lista = instructors.ToList();
            return lista;

        }
        public async Task<List<IdentityUser>> GetStudents()
        {
            var Students = await userManager.GetUsersInRoleAsync("Student");
            var lista = Students.ToList();
            return lista;

        }

    }
}
