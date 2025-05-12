using LMS.Domain.Entities.Students;
using LMS.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Repositories.Students
{
    public class StudentRepository : IStudentRepository
    {
        private readonly DbLMS _context;

        public StudentRepository(DbLMS context)
        {
            _context = context;
        }

        public async Task<int> GetStudentId(string userId)
        {
            return await _context.Students
                .Where(s => s.UserId == userId)
                .Select(s => s.Id)
                .FirstOrDefaultAsync();
        }

        public async Task CreateStudent(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Student>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }
    }
}
