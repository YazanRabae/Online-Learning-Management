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
        Task<List<Enrollment>> GetAllEnrollmentsByUserId(string userId);

    }
}
