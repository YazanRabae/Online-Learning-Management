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
using LMS.Service.Common.Constants;
using LMS.Service.Services.Shared;

namespace LMS.Service.Services.Courses
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IInstructorRepository _instructorRepository;
        private readonly ICourseMapper _courseMapper;
        private readonly IEmailService _emailService;

        public CourseService(
            ICourseRepository courseRepository,
            ICourseMapper courseMapper,
            IStudentRepository studentRepository,
            IInstructorRepository instructorRepository,
            IEmailService emailService)
        {
            _courseRepository = courseRepository;
            _courseMapper = courseMapper;
            _studentRepository = studentRepository;
            _instructorRepository = instructorRepository;
            _emailService = emailService;
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
                ImageData = c.ImageData == null ? null : Convert.ToBase64String(c.ImageData),
                IsEnrolled = c.Enrollments.Any(e => e.StudentId == studentId)
            }).ToList();

            return courseDTOs;
        }

        public async Task CreateCourse(CourseDTO courseDTO, string userId)
        {
            var instructorId = await _instructorRepository.GetInstructorId(userId);
            courseDTO.InstructorId = instructorId;
            byte[] imageData = null;
            string imageName = null;

            if (courseDTO.ImageFile != null && courseDTO.ImageFile.Length > 0)
            {
                using var ms = new MemoryStream();
                await courseDTO.ImageFile.CopyToAsync(ms);
                imageData = ms.ToArray();
                imageName = courseDTO.ImageFile.FileName;
            }

            courseDTO.ImageData = imageData == null ? null : Convert.ToBase64String(imageData);
            courseDTO.ImageName = imageName;
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

            var enroll = await _courseRepository.AddEnrollmentAsync(enrollment);

            await _emailService.SendEmailAsync(
                receiverName: enroll.Instructor.Name,
                receiverMail: EmailTemplates.TestEmail,
                subject: EmailTemplates.StudentEnrollSubject(enroll.Course.Title),
                body: EmailTemplates.StudentEnrollBody(enroll.Instructor.Name,
                    enroll.Student.Name,
                    enroll.Course.Title));
        }

        public async Task<bool> IsEnrolled(string userId, int courseId)
        {
            return await _courseRepository.IsStudentEnrolledAsync(await _studentRepository.GetStudentId(userId), courseId);
        }

        public async Task<int> NumberOfCourses(string userId)
        {
            return await _courseRepository.GetCourseCountByInstructorAsync(userId);
        }

        public async Task<List<CourseDTO>> GetCoursesByUserId(string userId)
        {
            var courses = await _courseRepository.GetCoursesByUserId(userId);

            return courses.Select(c => new CourseDTO
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                InstructorName = c.Instructor.Name,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                MaxStudents = c.MaxStudents,
                Price = c.Price,
                CourseTime = c.CourseTime
            }).ToList();
        }

        public async Task<StudentCoursesDto> GetEnrolledCoursesSplitAsync(string userId)
        {
            var studentId = await _studentRepository.GetStudentId(userId);
            var courses = await _courseRepository.GetEnrolledCoursesByStudentIdAsync(studentId);

            var now = DateTime.UtcNow;
            var studentCourses = new StudentCoursesDto();

            foreach (var course in courses)
            {
                var studentCourse = new CourseDTO
                {
                    Id = course.Id,
                    Title = course.Title,
                    Description = course.Description,
                    Price = course.Price,
                    StartDate = course.StartDate,
                    InstructorName = course.Instructor?.Name,
                    ImageData = course.ImageData == null ? null : Convert.ToBase64String(course.ImageData),
                    IsEnrolled = true,
                    Status = course.Enrollments.FirstOrDefault(s => s.StudentId == studentId).Status.ToString()
                };

                var studentCourseStatus = course.Enrollments.First(e => e.StudentId == studentId).Status;

                if (studentCourseStatus == EnrollmentStatus.Pending)
                    studentCourses.PendingCourses.Add(studentCourse);
                else if (studentCourseStatus == EnrollmentStatus.Rejected)
                    studentCourses.RejectedCourses.Add(studentCourse);
                else if (course.EndDate < now)
                    studentCourses.FinishedCourses.Add(studentCourse);
                else
                    studentCourses.ActiveCourses.Add(studentCourse);
            }

            studentCourses.FinishedCourses.ForEach(s => s.Status = "Finished");
            studentCourses.ActiveCourses.ForEach(s => s.Status = "Active");
            return studentCourses;
        }

        public async Task<StudentCoursesDto> GetAvailableCoursesAsync(string userId)
        {
            var studentId = await _studentRepository.GetStudentId(userId);
            var availableCourses = await _courseRepository.GetAvailableCoursesAsync(studentId);
            var studentCourses = new StudentCoursesDto();

            studentCourses.AvailableCourses = availableCourses.Select(c => new CourseDTO
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Price = c.Price,
                StartDate = c.StartDate,
                InstructorName = c.Instructor?.Name,
                ImageData = c.ImageData == null ? null : Convert.ToBase64String(c.ImageData),
                IsEnrolled = false
            }).ToList();

            return studentCourses;
        }

        public async Task<List<CourseDTO>> GetCourses()
        {
            var courses = await _courseRepository.GetCourses();

            var courseDTOs = courses.Select(c => new CourseDTO
            {
                Id = c.Id,
                Title = c.Title,
                InstructorName = c.Instructor.Name,
                InstructorEmail = c.Instructor.Email,
                InstructorId = c.InstructorId,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                MaxStudents = c.MaxStudents,
                Price = c.Price,
                CourseTime = c.CourseTime,
                Description = c.Description,
            }).ToList();

            return courseDTOs;
        }

        public async Task<CourseDTO> GetCourseById(int id)
        {
            Course course = await _courseRepository.GetCourseById(id);

            return new CourseDTO()
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                StartDate = course.StartDate,
                EndDate = course.EndDate,
                MaxStudents = course.MaxStudents,
                Price = course.Price,
                CourseTime = course.CourseTime,
                CreatedAt = course.CreatedAt,
                InstructorId = course.InstructorId,
                ExistingFile = course.ImageData == null ? null : Convert.ToBase64String(course.ImageData),
                ImageName = course.ImageName
            };
        }

        public async Task UpdateCourse(CourseDTO courseDTO)
        {
            byte[]? imageData = null;
            string imageName = null;

            if (courseDTO.ImageFile != null && courseDTO.ImageFile.Length > 0)
            {
                using var ms = new MemoryStream();
                await courseDTO.ImageFile.CopyToAsync(ms);
                imageData = ms.ToArray();
                imageName = courseDTO.ImageFile.FileName;
            }
            else
            {
                imageData = courseDTO.ExistingFile == null ? null : Convert.FromBase64String(courseDTO.ExistingFile);
                imageName = courseDTO.ImageName;
            }

            await _courseRepository.UpdateCourse(new Course()
            {
                Id = courseDTO.Id,
                Title = courseDTO.Title,
                Description = courseDTO.Description,
                StartDate = courseDTO.StartDate,
                EndDate = courseDTO.EndDate,
                MaxStudents = courseDTO.MaxStudents,
                Price = courseDTO.Price,
                CourseTime = courseDTO.CourseTime,
                CreatedAt = courseDTO.CreatedAt,
                InstructorId = courseDTO.InstructorId,
                ImageData = imageData,
                ImageName = imageName
            });
        }
    }
}
