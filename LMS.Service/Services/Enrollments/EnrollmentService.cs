using LMS.Domain.Entities.Enrollments;
using LMS.Repository.Repositories.Enrollments;
using LMS.Service.DTOs.Enrollments;
using LMS.Service.Mapper.Enrollments;

namespace LMS.Service.Services.Enrollments
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IEnrollmentMapper _enrollmentMapper;

        public EnrollmentService(IEnrollmentRepository enrollmentRepository, IEnrollmentMapper enrollmentMapper)
        {
            _enrollmentRepository = enrollmentRepository;
            _enrollmentMapper = enrollmentMapper;
        }

        public async Task AcceptEnrollmentAsync(int enrollmentId)
        {
            await _enrollmentRepository.AcceptEnrollmentAsync(enrollmentId);
        }

        public async Task<List<EnrollmentDTO>> GetAllPendingEnrollments()
        {
            var enrollments = await _enrollmentRepository.GetPendingEnrollmentsAsync();
            return _enrollmentMapper.MapFromEnrollmentToEnrollmentDTO(enrollments);
        }

        public async Task RejectEnrollmentAsync(int enrollmentId)
        {
            await _enrollmentRepository.RejectEnrollmentAsync(enrollmentId);
        }

        public async Task UpdateEnrollment(EnrollmentDTO enrollmentDTO)
        {
            var enrollment = _enrollmentMapper.MapFromEnrollmentDTOtoEnrollment(enrollmentDTO);
            await _enrollmentRepository.UpdateAsync(enrollment);
        }

        public async Task<List<EnrollmentDTO>> GetPendingEnrollmentsByInstructorUsernameAsync(string username)
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsByInstructorUsernameAsync(username);
            var pending = enrollments.Where(e => e.Status == EnrollmentStatus.Pending);
            return _enrollmentMapper.MapFromEnrollmentToEnrollmentDTO(pending.ToList());
        }

        public async Task<List<Enrollment>> GetAllEnrollmentsByUserId(string userId)
        {
            return await _enrollmentRepository.GetAllEnrollmentsByUserId(userId);
        }

        public async Task<List<StudentDetailsDto>> GetStudentsByCourse(int courseId)
        {
            var enrollments = await _enrollmentRepository.GetStudentsByCourse(courseId);
            return enrollments.Select( e => new StudentDetailsDto
            {
                Id = e.Student.Id,
                Name = e.Student.Name,
                Email = e.Student.Email

            }).ToList();
        }
    }
}
