using LMS.Domain.Entities.Instructors;
using LMS.Repository.Repositories.Instructors;
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
    }
}
