using LMS.Domain.Entities.Instructors;
using LMS.Domain.Entities.Students;
using LMS.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Repository.Repositories.Instructors
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly DbLMS _context;

        public InstructorRepository(DbLMS context)
        {
            _context = context;
        }

        public async Task CreateInstructor(Instructor Instructor)
        {
            await _context.Instructors.AddAsync(Instructor);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetInstructorId(string userId)
        {
            return await _context.Instructors
                .Where(s => s.UserId == userId)
                .Select(s => s.Id)
                .FirstOrDefaultAsync();
        }
    }
}
