using LMS.Service.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Service.Services
{
    public interface IUserService
    {
        public  Task Register(RegisterDto model);
        public  Task LogIn(LogInDto model); 
    }
}
