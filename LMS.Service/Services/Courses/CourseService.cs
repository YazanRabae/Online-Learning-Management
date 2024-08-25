using LMS.Domain.Entities.Courses;
using LMS.Repository.Repositories.Courses;
using LMS.Service.DTOs.Courses;
using LMS.Service.Mapper.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Service.Services.Courses
{
    public class CourseService(ICourseRepository courseRepository ,
         ICourseMapper courseMapper ) : ICourseService
    {
        public async Task<List<CourseDTO>> GetAllCourses()
        {
            //Get 
            List<Course> Courses = await courseRepository.GetAll();

            //Map
            List<CourseDTO> CoursesDTO = courseMapper.MapFromCourseToCourseDTO( Courses );

            return CoursesDTO;
        }
    }
}
