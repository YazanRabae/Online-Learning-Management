using LMS.Domain.Entities.Courses;
using LMS.Domain.Entities.Enrollments;
using LMS.Domain.Entities.Users;
using LMS.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Repository.Repositories.Courses
{
    public class CourseRepository : ICourseRepository
    {
        private readonly DbLMS _context;

        public CourseRepository(DbLMS context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all courses where a user is enrolled as a student.
        /// </summary>
        public async Task<List<Course>> GetCoursesByStudentIdAsync(int studentId)
        {
            return await _context.Courses
                .Include(course => course.Instructor)
                .Include(course => course.Enrollments)
                    .ThenInclude(enrollment => enrollment.Student)
                .Where(course => course.Enrollments.Any(e => e.StudentId == studentId))
                .ToListAsync();
        }

        /// <summary>
        /// Get all courses taught by a specific instructor.
        /// </summary>
        public async Task<List<Course>> GetCoursesByInstructorIdAsync(int instructorId)
        {
            return await _context.Courses
                .Where(c => c.InstructorId == instructorId)
                .Include(c => c.Enrollments)
                .ToListAsync();
        }

        /// <summary>
        /// Check if a student is already enrolled in a course.
        /// </summary>
        public async Task<bool> IsStudentEnrolledAsync(int studentId, int courseId)
        {
            return await _context.Enrollments
                .AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);
        }

        /// <summary>
        /// Enroll a student in a course.
        /// </summary>
        public async Task AddEnrollmentAsync(Enrollment enrollment)
        {
            try
            {
                await _context.Enrollments.AddAsync(enrollment);
                await SaveChangesAsync();  // Consolidated SaveChanges here
            }
            catch (Exception ex)
            {
                // You can log or wrap the exception here to provide more context
                throw new Exception("Error enrolling student", ex);
            }
        }

        /// <summary>
        /// Create a new course.
        /// </summary>
        public async Task CreateCourseAsync(Course course)
        {
            course.CreatedAt = DateTime.Now;
            await _context.Courses.AddAsync(course);
            await SaveChangesAsync(); // Saving after adding the course
        }

        /// <summary>
        /// Get instructor ID by course ID.
        /// </summary>
        public async Task<int> GetInstructorIdByCourseIdAsync(int courseId)
        {
            return await _context.Courses
                .Where(c => c.Id == courseId)
                .Select(c => c.InstructorId)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Save changes to the database.
        /// </summary>
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Course>> GetCoursesByInstructorAsync(int instructorId)
        {
            return await _context.Courses
                .Where(c => c.InstructorId == instructorId)
                .ToListAsync();
        }

        public async Task<int> GetCourseCountByInstructorAsync(string userId)
        {
            return (await _context.Instructors
                 .Include(i => i.Courses)
                 .FirstOrDefaultAsync(i => i.UserId == userId))
                 .Courses.Count();
        }

        public async Task AddCourseAsync(Course course)
        {
            await _context.Courses.AddAsync(course);
            await SaveChangesAsync(); // Saving after adding the course
        }

        public async Task<List<Course>> GetAllWithInstructorAndEnrollmentsAsync()
        {
            return await _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Enrollments)
                .ToListAsync();
        }

        public async Task<List<Course>> GetCoursesByUserId(string userId)
        {
            return await _context.Courses
                .Include(c => c.Instructor)
                .Where(c => c.Instructor.UserId == userId)
                .ToListAsync();
        }
    }
}
