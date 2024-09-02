using LMS.Domain.Entities.Courses;
using LMS.Domain.Entities.Enrollments;
using LMS.Repository.Repositories.Courses;
using LMS.Service.DTOs.Courses;
using LMS.Service.Mapper.Courses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using LMS.Domain.Entities.Users;
using LMS.Service.DTOs.Enrollments;
using LMS.Repository.Context;

namespace LMS.Service.Services.Courses
{
    public class CourseService(ICourseRepository courseRepository ,
         ICourseMapper courseMapper,
         UserManager<User> userManager , DbLMS _context) : ICourseService
    {

        //List<CourseDTO> CoursesDTO = courseMapper.MapFromCourseToCourseDTO(Courses);
        public async Task<List<CourseDTO>> GetAllCourses(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
            }

            //List<Course> userCourses = await courseRepository.GetAll(userId);
        var courses = await _context.Courses
       .Include(course => course.Instructor)
       .Include(course => course.Enrollments)
       .Select(course => new CourseDTO
       {
           Id = course.Id,
           Title = course.Title,
           Description = course.Description,
           Price = course.Price,
           StartDate = course.StartDate,
           InstructorName = course.Instructor.UserName,
           ImageData = course.ImageData,
           IsEnrolled = course.Enrollments.Any(enrollment => enrollment.StudentId == userId)
       })
       .ToListAsync();

         return courses;
        }

        public async Task<bool> Enroll(string userId, int courseId)
        {
            var enrol =  await courseRepository.Enroll(userId, courseId);
            if (enrol)
            {
                return true;  
            }
            return false; /*The result(enrol) will be true if the student is already enrolled in the course, or false if not.*/
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


        public async Task AddEnrollment(string userId, int courseId)
        {
            var viewModel = new Enrollment
            {
                AddDate = DateTime.Now,
                CourseId = courseId,
                InstructorId = await courseRepository.GetInstructorIdByCourseIdAsync(courseId),
                StudentId = userId
            };

            await courseRepository.AddEnrollment(viewModel);
        }
        public async Task<bool> IsEnrolled(string userId, int courseId)
        {
            return await courseRepository.IsEnrolled(userId, courseId);
        }

    
    }
}

