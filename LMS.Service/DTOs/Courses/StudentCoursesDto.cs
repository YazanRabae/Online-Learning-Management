using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Service.DTOs.Courses
{
    public class StudentCoursesDto
    {
        public List<CourseDTO> ActiveCourses { get; set; } = new();
        public List<CourseDTO> FinishedCourses { get; set; } = new();
        public List<CourseDTO> AvailableCourses { get; set; } = new();
        public List<CourseDTO> PendingCourses { get; set; } = new();
        public List<CourseDTO> RejectedCourses { get; set; } = new();
    }
}
