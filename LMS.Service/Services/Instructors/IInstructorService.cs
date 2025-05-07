using LMS.Service.DTOs.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Service.Services.Instructors
{
    public interface IInstructorService
    {
        public Task CreateInstructor(CreateUserDto createInstructorDto);
    }
}
