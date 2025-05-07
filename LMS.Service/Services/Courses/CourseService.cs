using LMS.Domain.Entities.Courses;
using LMS.Domain.Entities.Enrollments;
using LMS.Domain.Entities.Users;
using LMS.Repository.Repositories.Courses;
using LMS.Service.DTOs.Courses;
using LMS.Service.Mapper.Courses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LMS.Repository.Repositories.Students;
using LMS.Repository.Repositories.Instructors;
using LMS.Domain.Entities.Students;

namespace LMS.Service.Services.Courses
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IInstructorRepository _instructorRepository;
        private readonly ICourseMapper _courseMapper;
        private readonly UserManager<User> _userManager;

        public CourseService(
            ICourseRepository courseRepository,
            ICourseMapper courseMapper,
            IStudentRepository studentRepository,
            UserManager<User> userManager,
            IInstructorRepository instructorRepository)
        {
            _courseRepository = courseRepository;
            _courseMapper = courseMapper;
            _userManager = userManager;
            _studentRepository = studentRepository;
            _instructorRepository = instructorRepository;
        }

        public async Task<List<CourseDTO>> GetAllCourses(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            var courses = await _courseRepository.GetAllWithInstructorAndEnrollmentsAsync();
            var studentId = await _studentRepository.GetStudentId(userId);

            var courseDTOs = courses.Select(c => new CourseDTO
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Price = c.Price,
                StartDate = c.StartDate,
                InstructorName = c.Instructor.Name,
                ImageData = c.ImageData,
                IsEnrolled = c.Enrollments.Any(e => e.StudentId == studentId)
            }).ToList();

            return courseDTOs;
        }

        public async Task CreateCourse(CourseDTO courseDTO, string userId)
        {
            var instructorId = await _instructorRepository.GetInstructorId(userId);
            courseDTO.InstructorId = instructorId;
            byte[]? imageData = null;

            if (courseDTO.ImageFile != null && courseDTO.ImageFile.Length > 0)
            {
                using var ms = new MemoryStream();
                await courseDTO.ImageFile.CopyToAsync(ms);
                imageData = ms.ToArray();
            }

            courseDTO.ImageData = imageData;
            var course = _courseMapper.MapFromCourseDTOToCourse(courseDTO);
            course.InstructorId = instructorId;

            await _courseRepository.CreateCourseAsync(course);
        }

        public async Task AddEnrollment(string userId, int courseId)
        {
            var enrollment = new Enrollment
            {
                AddDate = DateTime.UtcNow,
                CourseId = courseId,
                InstructorId = await _courseRepository.GetInstructorIdByCourseIdAsync(courseId),
                StudentId = await _studentRepository.GetStudentId(userId),
                Status = EnrollmentStatus.Pending
            };

            await _courseRepository.AddEnrollmentAsync(enrollment);
        }

        public async Task<bool> IsEnrolled(string userId, int courseId)
        {
            return await _courseRepository.IsStudentEnrolledAsync(await _studentRepository.GetStudentId(userId), courseId);
        }

        public async Task<int> NumberOfCourses(string userId)
        {
            return await _courseRepository.GetCourseCountByInstructorAsync(userId);
        }

        public async Task<List<Course>> GetCoursesByUserId(string userId)
        {
            return await _courseRepository.GetCoursesByUserId(userId);
        }
    }
}
