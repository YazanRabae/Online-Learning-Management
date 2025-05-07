using LMS.Domain.Entities.Students;
using LMS.Service.DTOs.Students;
using LMS.Service.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Service.Mapper.Students
{
    public class StudentMapper : IStudentMapper
    {
        public Student MapFromCreateStudentDtoToEntity(CreateUserDto createStudentDto)
        {
            return new Student()
            {
                Name = createStudentDto.Name,
                Email = createStudentDto.Email,
                UserId = createStudentDto.UserId
            };
        }

        public RegisterDto MapFromCreateStudentDtoToRegiserModel(CreateUserDto createStudentDto)
        {
            return new RegisterDto()
            {
                Name = createStudentDto.Name,
                Email = createStudentDto.Email,
                Password = createStudentDto.Password,
                ConfirmPassword = createStudentDto.ConfirmPassword
            };
        }
    }
}
