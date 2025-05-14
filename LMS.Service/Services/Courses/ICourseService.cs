using LMS.Domain.Entities.Courses;
using LMS.Service.DTOs.Courses;

namespace LMS.Service.Services.Courses
{
    public interface ICourseService
    {
        Task<List<CourseDTO>> GetAllCourses(string userId);
        Task<List<CourseDTO>> GetCourses();
        Task CreateCourse(CourseDTO courseDTO, string userId);
        Task<CourseDTO> GetCourseById(int id);
        Task UpdateCourse(CourseDTO courseDTO);
        Task AddEnrollment(string userId, int courseId);
        Task<bool> IsEnrolled(string userId, int courseId);
        Task<int> NumberOfCourses(string userId);
        Task<List<CourseDTO>> GetCoursesByUserId(string userId);

        Task<StudentCoursesDto> GetEnrolledCoursesSplitAsync(string userId);
        Task<StudentCoursesDto> GetAvailableCoursesAsync(string userId);

    }
}
