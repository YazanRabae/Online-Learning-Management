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

        public async Task CreateInstructor(Instructor instructor)
        {
            await _context.Instructors.AddAsync(instructor);
            await _context.SaveChangesAsync();
        }

        public async Task<Instructor> GetInstructorById(int id)
        {
            return await _context.Instructors
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<int> GetInstructorId(string userId)
        {
            return await _context.Instructors
                .Where(s => s.UserId == userId)
                .Select(s => s.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Instructor>> GetInstructors()
        {
            return await _context.Instructors
                .ToListAsync();
        }

        public async Task UpdateInstructor(Instructor instructor)
        {
            _context.Instructors.Update(instructor);
            await _context.SaveChangesAsync();
        }
    }
}
