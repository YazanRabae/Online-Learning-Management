using LMS.Domain.Entities.Enrollments;
using LMS.Service.DTOs.Enrollments;

namespace LMS.Service.Services.Enrollments
{
    public interface IEnrollmentService
    {
        Task<List<EnrollmentDTO>> GetAllPendingEnrollments();
        Task UpdateEnrollment(EnrollmentDTO enrollmentDTO);
        Task AcceptEnrollmentAsync(int enrollmentId);
        Task RejectEnrollmentAsync(int enrollmentId);
        Task<List<EnrollmentDTO>> GetPendingEnrollmentsByInstructorUsernameAsync(string instructorUsername);
        Task<List<EnrollmentDTO>> GetAllEnrollmentsByUserId(string userId);
        Task<List<StudentDetailsDto>> GetStudentsByCourse(int courseId);
        Task RejectStudentByIdAsync(int studentId, int courseId);

    }
}
