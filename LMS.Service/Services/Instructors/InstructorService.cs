using LMS.Domain.Entities.Instructors;
using LMS.Domain.Entities.Students;
using LMS.Repository.Repositories.Instructors;
using LMS.Service.Common.Constants;
using LMS.Service.DTOs.Instructors;
using LMS.Service.DTOs.Students;
using LMS.Service.Mapper.Instructors;
using LMS.Service.Services.Shared;

namespace LMS.Service.Services.Instructors
{
    public class InstructorService : IInstructorService
    {
        private readonly IInstructorRepository _InstructorRepository;
        private readonly IInstructorMapper _InstructorMapper;
        private readonly IEmailService _emailService;

        public InstructorService(
            IInstructorRepository InstructorRepository,
            IInstructorMapper InstructorMapper,
            IEmailService emailService)
        {
            _InstructorRepository = InstructorRepository;
            _InstructorMapper = InstructorMapper;
            _emailService = emailService;
        }
        public async Task CreateInstructor(CreateUserDto createInstructorDto)
        {
            Instructor Instructor = _InstructorMapper.MapFromCreateInstructorDtoToEntity(createInstructorDto);

            await _InstructorRepository.CreateInstructor(Instructor);

            await _emailService.SendEmailAsync(
                receiverName: createInstructorDto.Name,
                receiverMail: EmailTemplates.TestEmail,
                subject: EmailTemplates.CreateUserSubject(RoleConstants.Instructor),
                body: EmailTemplates.CreateUserBody(createInstructorDto.Name,
                    createInstructorDto.Email,
                    createInstructorDto.Password));
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

        public async Task UpdateInstructor(CreateUserDto updateInstructorDto)
        {
            await _InstructorRepository.UpdateInstructor(new Instructor()
            {
                Id = updateInstructorDto.Id,
                Name = updateInstructorDto.Name,
                Email = updateInstructorDto.Email,
                UserId = updateInstructorDto.UserId
            });

            await _emailService.SendEmailAsync(
                receiverName: updateInstructorDto.Name,
                receiverMail: EmailTemplates.TestEmail,
                subject: EmailTemplates.UpdateUserSubject(RoleConstants.Instructor),
                body: EmailTemplates.UpdateUserBody(updateInstructorDto.Name,
                    updateInstructorDto.Email,
                    updateInstructorDto.Password));
        }
    }
}
