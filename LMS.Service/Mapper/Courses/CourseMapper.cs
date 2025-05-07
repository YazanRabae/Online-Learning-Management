using LMS.Domain.Entities.Courses;
using LMS.Service.DTOs.Courses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Service.Mapper.Courses
{
    public class CourseMapper : ICourseMapper
    {
        public IEnumerable<CourseDTO> MapFromCourseToCourseDTO(IEnumerable<Course> courses)
        {
            return courses.Select(course => new CourseDTO
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                StartDate = course.StartDate,
                EndDate = course.EndDate,
                InstructorId = course.InstructorId,
                InstructorName = course.Instructor?.Name,
                MaxStudents = course.MaxStudents,
                Price = course.Price,
                CourseTime = course.CourseTime,
                ImageData = course.ImageData,
                CreatedAt = course.CreatedAt,
            }).ToList();
        }

        public Course MapFromCourseDTOToCourse(CourseDTO courseDTO)
        {
            var course = new Course
            {
                Id = courseDTO.Id,
                Title = courseDTO.Title,
                Description = courseDTO.Description,
                StartDate = courseDTO.StartDate,
                EndDate = courseDTO.EndDate,
                MaxStudents = courseDTO.MaxStudents,
                Price = courseDTO.Price,
                CourseTime = courseDTO.CourseTime,
                CreatedAt = courseDTO.CreatedAt == default ? DateTime.UtcNow : courseDTO.CreatedAt,
                InstructorId = courseDTO.InstructorId,
            };

            if (courseDTO.ImageFile != null && courseDTO.ImageFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    courseDTO.ImageFile.CopyTo(ms);
                    course.ImageData = ms.ToArray();
                }
            }
            else
            {
                course.ImageData = courseDTO.ImageData; // retain existing image if not updating
            }

            return course;
        }
    }
}
