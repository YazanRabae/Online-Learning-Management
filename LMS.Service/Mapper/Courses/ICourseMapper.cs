using LMS.Domain.Entities.Courses;
using LMS.Service.DTOs.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages;

namespace LMS.Service.Mapper.Courses
{
    public interface ICourseMapper
    {
        public List<CourseDTO> MapFromCourseToCourseDTO(List<Course> Courses);
    }
}
