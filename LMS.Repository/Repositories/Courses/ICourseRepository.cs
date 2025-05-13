using LMS.Domain.Entities.Courses;
using LMS.Domain.Entities.Enrollments;
using LMS.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Repositories.Courses
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetCoursesByStudentIdAsync(int studentId);
        Task<List<Course>> GetCoursesByInstructorIdAsync(int instructorId);
        Task<bool> IsStudentEnrolledAsync(int studentId, int courseId);
        Task AddEnrollmentAsync(Enrollment enrollment);
        Task CreateCourseAsync(Course course);
        Task<int> GetInstructorIdByCourseIdAsync(int courseId);
        Task SaveChangesAsync();

        Task<IEnumerable<Course>> GetCoursesByInstructorAsync(int instructorId);
        Task<int> GetCourseCountByInstructorAsync(string userId);
        Task AddCourseAsync(Course course);
        Task<List<Course>> GetAllWithInstructorAndEnrollmentsAsync();
        Task<List<Course>> GetCourses();
        Task<List<Course>> GetCoursesByUserId(string userId);

        Task<List<Course>> GetEnrolledCoursesByStudentIdAsync(int studentId);
        Task<List<Course>> GetAvailableCoursesAsync(int studentId);
    }
}
