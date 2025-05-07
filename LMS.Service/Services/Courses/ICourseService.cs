using LMS.Domain.Entities.Courses;
using LMS.Service.DTOs.Courses;

namespace LMS.Service.Services.Courses
{
    public interface ICourseService
    {
        Task<List<CourseDTO>> GetAllCourses(string userId);
        Task CreateCourse(CourseDTO courseDTO, string userId);
        Task AddEnrollment(string userId, int courseId);
        Task<bool> IsEnrolled(string userId, int courseId);
        Task<int> NumberOfCourses(string userId);
        Task<List<Course>> GetCoursesByUserId(string userId);
    }
}
