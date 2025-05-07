using LMS.Domain.Entities.Students;
using LMS.Domain.Entities.Users;
using LMS.Repository.Repositories.Courses;
using LMS.Repository.Repositories.Instructors;
using LMS.Repository.Repositories.Students;
using LMS.Service.DTOs.Students;
using LMS.Service.Mapper.Courses;
using LMS.Service.Mapper.Students;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Service.Services.Students
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IStudentMapper _studentMapper;

        public StudentService(
            IStudentRepository studentRepository,
            IStudentMapper studentMapper)
        {
            _studentRepository = studentRepository;
            _studentMapper = studentMapper;
        }

        public async Task CreateStudent(CreateUserDto createStudentDto)
        {
            Student student = _studentMapper.MapFromCreateStudentDtoToEntity(createStudentDto);

            await _studentRepository.CreateStudent(student);
        }
    }
}
