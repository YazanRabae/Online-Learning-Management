using LMS.Domain.Entities.Instructors;
using LMS.Service.DTOs.Students;

namespace LMS.Service.Mapper.Instructors
{
    public class InstructorMapper : IInstructorMapper
    {
        public Instructor MapFromCreateInstructorDtoToEntity(CreateUserDto createStudentDto)
        {
            return new Instructor()
            {
                Name = createStudentDto.Name,
                Email = createStudentDto.Email,
                UserId = createStudentDto.UserId
            };
        }
    }
}
