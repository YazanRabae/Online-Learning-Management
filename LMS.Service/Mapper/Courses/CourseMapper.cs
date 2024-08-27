using LMS.Domain.Entities.Courses;
using LMS.Domain.Entities.Users;
using LMS.Service.DTOs.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Service.Mapper.Courses
{
    public class CourseMapper : ICourseMapper
    {
        public List<CourseDTO> MapFromCourseToCourseDTO(List<Course> Courses)
        {
            return Courses.Select(b => new CourseDTO
            {
                Id = b.Id,
                Title = b.Title,
                Description = b.Description,
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                InstructorName = b.Instructor.UserName,
                MaxStudents = b.MaxStudents,
                Price = b.Price,
                CourseTime = b.CourseTime
            }).ToList();
        }

        public Course MapFromCourseDTOToCourse(CourseDTO courseDTO)
        {
            return new Course
            {
                Id = courseDTO.Id,
                Title = courseDTO.Title,
                Description = courseDTO.Description,
                ImageData = courseDTO.ImageData,
                StartDate = courseDTO.StartDate,
                EndDate = courseDTO.EndDate,
                Price = courseDTO.Price,
                CourseTime = courseDTO.CourseTime,
                CreatedAt = DateTime.UtcNow,
                MaxStudents= courseDTO.MaxStudents,


            };
        }
    }
}
