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
        Task<List<Course>> GetAll(string? userId);
        Task<bool> Enroll(string userId, int courseId);
        Task Create(Course course);
        Task SaveChangesAsync();
        Task<string> GetInstructorIdByCourseIdAsync(int courseId);
        Task AddEnrollment(Enrollment enrollment);
        Task<bool> IsEnrolled(string userId, int courseId);

      
    }
}
