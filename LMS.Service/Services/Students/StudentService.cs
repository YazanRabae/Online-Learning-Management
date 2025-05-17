using LMS.Domain.Entities.Students;
using LMS.Domain.Entities.Users;
using LMS.Repository.Repositories.Courses;
using LMS.Repository.Repositories.Instructors;
using LMS.Repository.Repositories.Students;
using LMS.Service.Common.Constants;
using LMS.Service.DTOs.Students;
using LMS.Service.Mapper.Courses;
using LMS.Service.Mapper.Students;
using LMS.Service.Services.Shared;
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
        private readonly IEmailService _emailService;

        public StudentService(
            IStudentRepository studentRepository,
            IStudentMapper studentMapper,
            IEmailService emailService)
        {
            _studentRepository = studentRepository;
            _studentMapper = studentMapper;
            _emailService = emailService;
        }

        public async Task CreateStudent(CreateUserDto createStudentDto)
        {
            Student student = _studentMapper.MapFromCreateStudentDtoToEntity(createStudentDto);

            await _studentRepository.CreateStudent(student);

            await _emailService.SendEmailAsync(
                receiverName: createStudentDto.Name,
                receiverMail: EmailTemplates.TestEmail,
                subject: EmailTemplates.CreateUserSubject(RoleConstants.Student),
                body: EmailTemplates.CreateUserBody(createStudentDto.Name,
                    createStudentDto.Email,
                    createStudentDto.Password));
        }

        public async Task<CreateUserDto> GetStudentById(int id)
        {
            Student student = await _studentRepository.GetStudentById(id);

            return new CreateUserDto()
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                UserId = student.UserId,
                RoleName = RoleConstants.Student
            };
        }

        public async Task<List<StudentDto>> GetStudents()
        {
            List<Student> students = await _studentRepository.GetStudents();

            return _studentMapper.MapFromCreateStudentEntityToDto(students);
        }

        public async Task UpdateStudent(CreateUserDto updateStudentDto)
        {
            await _studentRepository.UpdateStudent(new Student()
            {
                Id = updateStudentDto.Id,
                Name = updateStudentDto.Name,
                Email = updateStudentDto.Email,
                UserId = updateStudentDto.UserId
            });

            await _emailService.SendEmailAsync(
                receiverName: updateStudentDto.Name,
                receiverMail: EmailTemplates.TestEmail,
                subject: EmailTemplates.UpdateUserSubject(RoleConstants.Student),
                body: EmailTemplates.UpdateUserBody(updateStudentDto.Name,
                    updateStudentDto.Email,
                    updateStudentDto.Password));
        }
    }
}
