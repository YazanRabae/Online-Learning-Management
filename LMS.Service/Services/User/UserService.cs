using LMS.Domain.Entities.Users;
using LMS.Repository.Repositories.Users;
using LMS.Service.DTOs.UserDTOs;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace LMS.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserRepository _userRepository;

        public UserService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            IUserRepository userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userRepository = userRepository;
        }

        public async Task<bool> LogIn(LogInDto model)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            return signInResult.Succeeded;
        }

        public async Task<string> GetRoleByName(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            return role;
        }

        public async Task<RegisterResultDto> Register(RegisterDto model, string role)
        {
            var user = new User { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role);
                return new RegisterResultDto()
                {
                    IsSuccess = true,
                    UserId = user.Id
                };
            }
            return new RegisterResultDto()
            {
                MessageError = string.Join(", ", result.Errors.Select(e => e.Description)),
                IsSuccess = false
            };
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<List<User>> GetInstructors()
        {
            return (await _userManager.GetUsersInRoleAsync("Instructor")).ToList();
        }

        public async Task<List<User>> GetStudents()
        {
            return (await _userManager.GetUsersInRoleAsync("Student")).ToList();
        }
        public async Task<string> GetRoleByUserId(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            return role;
        }
    }
}
