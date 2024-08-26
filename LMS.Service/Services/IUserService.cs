
using LMS.Domain.Entities.Users;
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
    public interface IUserService
    {
        public  Task Register(RegisterDto model,string role);
        public  Task LogIn(LogInDto model);
        public Task Logout();
        public Task<List<User>> GetInstructors();

        public Task<List<User>> GetStudents();

    }
}
