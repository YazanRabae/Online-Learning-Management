using LMS.Service.DTOs.Instructors;
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
        public Task<List<InstructorDto>> GetInstructors();
        public Task<CreateUserDto> GetInstructorById(int id);
        public Task UpdateInstructor(CreateUserDto updateInstructorDto);
    }
}
