using LMS.Domain.Entities.Users;
using LMS.Service.DTOs.UserDTOs;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace LMS.Service.Services
{
    public interface IUserService
    {
        Task<bool> LogIn(LogInDto model);
        Task<string> GetRoleByName(string email);
        Task<RegisterResultDto> Register(RegisterDto model, string role);
        Task Logout();
        Task<List<User>> GetInstructors();
        Task<List<User>> GetStudents();
    }
}
