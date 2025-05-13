using LMS.Domain.Entities.Instructors;
using LMS.Service.DTOs.Instructors;
using LMS.Service.DTOs.Students;

namespace LMS.Service.Mapper.Instructors
{
    public interface IInstructorMapper
    {
        Instructor MapFromCreateInstructorDtoToEntity(CreateUserDto createStudentDto);
        List<InstructorDto> MapFromInstructorEntityToDto(List<Instructor> instructor);
    }
}
