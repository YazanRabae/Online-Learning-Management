using LMS.Domain.Entities.Courses;
using LMS.Domain.Entities.Users;
using LMS.Service.DTOs.Courses;
using LMS.Service.DTOs.Enrollments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Service.Services.Courses
{
    public interface ICourseService
    {
        Task<List<CourseDTO>> GetAllCourses();
        Task CreateCourse(CourseDTO courseDTO, string instructorId);
        Task AddEnrollment(string userId, int courseId);
        Task<bool> IsEnrolled(string userId, int courseId);
    }

}
