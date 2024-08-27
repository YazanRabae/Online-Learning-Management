using LMS.Domain.Entities.Courses;
using LMS.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Repository.Repositories.Courses
{
    public class CourseRepository(DbLMS _context) : ICourseRepository
    {

        public async Task<List<Course>> GetAll()
        {
            return await _context.Courses.ToListAsync();
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

    }
}
