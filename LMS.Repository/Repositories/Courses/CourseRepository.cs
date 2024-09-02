using LMS.Domain.Entities.Courses;
using LMS.Domain.Entities.Enrollments;
using LMS.Domain.Entities.Users;
using LMS.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Repository.Repositories.Courses
{
    public class CourseRepository(DbLMS _context) : ICourseRepository
    {

        public async Task<List<Course>> GetAll(string? userId)
        {
            //return await _context.Courses.Include(cors => cors.Instructor ).ToListAsync();
            //, it checks if the course has an enrollment record where the StudentId matches the provided userId.
            return await _context.Courses
             .Include(course => course.Instructor)
             .Include(course => course.Enrollments)
             .ThenInclude(enrollment => enrollment.Student)
             .Where(course => course.Enrollments.Any(enrollment => enrollment.StudentId == userId))
             .ToListAsync();
        }

       public async Task<bool> Enroll(string userId, int courseId)
        {
            var isEnrolled = _context.Enrollments
               .AnyAsync(e => e.StudentId == userId && e.CourseId == courseId);
            // trur if student is already enrolled in the course
            return await isEnrolled;
        }
          
        public async Task Create(Course course)
        {
            course.CreatedAt = DateTime.Now;
            await _context.Courses.AddAsync(course);
            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetInstructorIdByCourseIdAsync(int courseId)
        {
            return await _context.Courses
                .Where(c => c.Id == courseId)
                .Select(c => c.InstructorId)
                .FirstOrDefaultAsync();
        }

        public async Task AddEnrollment(Enrollment enrollment)
        {
            try
            {
                await _context.Enrollments.AddAsync(enrollment);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> IsEnrolled(string userId, int courseId)
        {
            return await _context.Enrollments
                .AnyAsync(e => e.StudentId == userId && e.CourseId == courseId);
        } 

 

    }
}
