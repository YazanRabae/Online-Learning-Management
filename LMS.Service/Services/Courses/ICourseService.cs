using LMS.Service.DTOs.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Service.Services.Courses
{
   public interface  ICourseService
    {
        public Task<List<CourseDTO>> GetAllCourses();
        Task CreateCourse(CourseDTO courseDTO, string instructorId);
    }

}
