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
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        public UserService(UserManager<IdentityUser> userManager,
           SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }


        public async Task Register(RegisterDto model, string role)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
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

    }
}
