using LMS.Domain.Entities.Enrollments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS.Repository.Repositories.Enrollments
{
    public interface IEnrollmentRepository
    {
        Task CreateAsync(Enrollment enrollment);
        Task<List<Enrollment>> GetPendingEnrollmentsAsync();
        Task UpdateAsync(Enrollment enrollment);
        Task<Enrollment> AcceptEnrollmentAsync(int enrollmentId);
        Task<Enrollment> RejectEnrollmentAsync(int enrollmentId);

        Task<IEnumerable<Enrollment>> GetEnrollmentsByInstructorUsernameAsync(string username);
        Task<List<Enrollment>> GetAllEnrollmentsByUserId(string userId);
        Task<List<Enrollment>> GetStudentsByCourse(int courseId);

        Task<Enrollment> RejectEnrollmentByStudentIdAsync(int studentId, int courseId);

    }
}
