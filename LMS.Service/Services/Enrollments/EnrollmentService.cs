using LMS.Domain.Entities.Enrollments;
using LMS.Repository.Repositories.Enrollments;
using LMS.Service.Common.Constants;
using LMS.Service.DTOs.Enrollments;
using LMS.Service.Mapper.Enrollments;
using LMS.Service.Services.Shared;

namespace LMS.Service.Services.Enrollments
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IEnrollmentMapper _enrollmentMapper;
        private readonly IEmailService _emailService;

        public EnrollmentService(IEnrollmentRepository enrollmentRepository, IEnrollmentMapper enrollmentMapper, IEmailService emailService)
        {
            _enrollmentRepository = enrollmentRepository;
            _enrollmentMapper = enrollmentMapper;
            _emailService = emailService;
        }

        public async Task AcceptEnrollmentAsync(int enrollmentId)
        {
            var enrollment = await _enrollmentRepository.AcceptEnrollmentAsync(enrollmentId);
            await SendEnrollmentStatus(enrollment, true);
        }

        public async Task<List<EnrollmentDTO>> GetAllPendingEnrollments()
        {
            var enrollments = await _enrollmentRepository.GetPendingEnrollmentsAsync();
            return _enrollmentMapper.MapFromEnrollmentToEnrollmentDTO(enrollments);
        }

        public async Task RejectEnrollmentAsync(int enrollmentId)
        {
            var enrollment = await _enrollmentRepository.RejectEnrollmentAsync(enrollmentId);
            await SendEnrollmentStatus(enrollment, false);
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

        public async Task<List<EnrollmentDTO>> GetAllEnrollmentsByUserId(string userId)
        {
            var enrollments = await _enrollmentRepository.GetAllEnrollmentsByUserId(userId);

            return enrollments.Select(e => new EnrollmentDTO
            {
                Id = e.Id,
                StudentName = e.Student.Name,
                CourseName = e.Course.Title,
                AddDate = e.AddDate,
                Price = e.Course.Price,
                Status = e.Status,
            }).ToList();
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

        public async Task RejectStudentByIdAsync(int studentId, int courseId)
        {
            var enrollment = await _enrollmentRepository.RejectEnrollmentByStudentIdAsync(studentId, courseId);

            await _emailService.SendEmailAsync(
                receiverName: enrollment.Student.Name,
                receiverMail: EmailTemplates.TestEmail,
                subject: EmailTemplates.RemovedFromCourseSubject(enrollment.Course.Title),
                body: EmailTemplates.RemovedFromCourseBody(enrollment.Student.Name,
                    enrollment.Course.Title,
                    enrollment.Instructor.Name));
        }

        private async Task SendEnrollmentStatus(Enrollment enrollment, bool isAccepted)
        {
            await _emailService.SendEmailAsync(
                receiverName: enrollment.Student.Name,
                receiverMail: EmailTemplates.TestEmail,
                subject: EmailTemplates.EnrollmentStatusSubject(false, enrollment.Course.Title),
                body: EmailTemplates.EnrollmentStatusBody(false,
                    enrollment.Student.Name,
                    enrollment.Course.Title));
        }
    }
}
