using LMS.Domain.Entities.Courses;
using LMS.Domain.Entities.Enrollments;
using LMS.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Repository.Repositories.Courses
{
    public class CourseRepository(DbLMS _context) : ICourseRepository
    {

        public async Task<List<Course>> GetAll()
        {
            return await _context.Courses.Include(cors => cors.Instructor).ToListAsync();
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
