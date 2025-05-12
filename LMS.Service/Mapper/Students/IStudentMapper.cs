using LMS.Domain.Entities.Students;
using LMS.Service.DTOs.Students;
using LMS.Service.DTOs.UserDTOs;

namespace LMS.Service.Mapper.Students
{
    public interface IStudentMapper
    {
        RegisterDto MapFromCreateStudentDtoToRegiserModel(CreateUserDto createStudentDto);
        Student MapFromCreateStudentDtoToEntity(CreateUserDto createStudentDto);
        List<StudentDto> MapFromCreateStudentEntityToDto(List<Student> students);
    }
}
