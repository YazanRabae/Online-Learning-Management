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

        public async Task CreateCourse(CourseDTO courseDTO, string instructorId)
        {
            byte[] imageData = null;

            // Ensure the Image is an IFormFile and convert it to a byte array
            if (courseDTO.ImageFile != null && courseDTO.ImageFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await courseDTO.ImageFile.CopyToAsync(memoryStream); // Correct usage of CopyToAsync with Stream
                    imageData = memoryStream.ToArray(); // Convert the Stream to a byte array
                }
            }

            courseDTO.ImageData = imageData;
            

            var course = courseMapper.MapFromCourseDTOToCourse(courseDTO);

            course.InstructorId = instructorId;
            course.ImageData = imageData;

            await courseRepository.Create(course);
        }
    }
}
