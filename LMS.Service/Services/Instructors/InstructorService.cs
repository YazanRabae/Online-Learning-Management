using LMS.Domain.Entities.Instructors;
using LMS.Domain.Entities.Students;
using LMS.Repository.Repositories.Instructors;
using LMS.Service.Common.Constants;
using LMS.Service.DTOs.Instructors;
using LMS.Service.DTOs.Students;
using LMS.Service.Mapper.Instructors;

namespace LMS.Service.Services.Instructors
{
    public class InstructorService : IInstructorService
    {
        private readonly IInstructorRepository _InstructorRepository;
        private readonly IInstructorMapper _InstructorMapper;

        public InstructorService(
            IInstructorRepository InstructorRepository,
            IInstructorMapper InstructorMapper)
        {
            _InstructorRepository = InstructorRepository;
            _InstructorMapper = InstructorMapper;
        }
        public async Task CreateInstructor(CreateUserDto createInstructorDto)
        {
            Instructor Instructor = _InstructorMapper.MapFromCreateInstructorDtoToEntity(createInstructorDto);

            await _InstructorRepository.CreateInstructor(Instructor);
        }

        public async Task<CreateUserDto> GetInstructorById(int id)
        {
            Instructor instructor = await _InstructorRepository.GetInstructorById(id);

            return new CreateUserDto()
            {
                Id = instructor.Id,
                Name = instructor.Name,
                Email = instructor.Email,
                UserId = instructor.UserId,
                RoleName = RoleConstants.Instructor
            };
        }

        public async Task<List<InstructorDto>> GetInstructors()
        {
            List<Instructor> instructors = await _InstructorRepository.GetInstructors();

            return _InstructorMapper.MapFromInstructorEntityToDto(instructors);
        }

        public async Task UpdateInstructor(CreateUserDto updateStudentDto)
        {
            await _InstructorRepository.UpdateInstructor(new Instructor()
            {
                Id = updateStudentDto.Id,
                Name = updateStudentDto.Name,
                Email = updateStudentDto.Email,
                UserId = updateStudentDto.UserId
            });
        }
    }
}
