using LMS.Domain.Entities.Enrollments;
using LMS.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Repository.Repositories.Enrollments
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly DbLMS _context;

        public EnrollmentRepository(DbLMS context)
        {
            _context = context;
        }
        public async Task CreateAsync(Enrollment enrollment)
        {
            await _context.Enrollments.AddAsync(enrollment);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Enrollment>> GetPendingEnrollmentsAsync()
        {
            return await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Instructor)
                .Include(e => e.Course)
                .Where(e => e.Status == EnrollmentStatus.Pending)
                .ToListAsync();
        }
        public async Task UpdateAsync(Enrollment enrollment)
        {
            _context.Enrollments.Update(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task AcceptEnrollmentAsync(int enrollmentId)
        {
            var enrollment = await _context.Enrollments.FindAsync(enrollmentId);
            if (enrollment != null)
            {
                enrollment.Status = EnrollmentStatus.Accepted;
                _context.Enrollments.Update(enrollment);
                await _context.SaveChangesAsync();
            }
        }
        public async Task RejectEnrollmentAsync(int enrollmentId)
        {
            var enrollment = await _context.Enrollments.FindAsync(enrollmentId);
            if (enrollment != null)
            {
                enrollment.Status = EnrollmentStatus.Rejected;
                _context.Enrollments.Update(enrollment);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Enrollment>> GetEnrollmentsByInstructorUsernameAsync(string username)
        {
            var instructorId = await _context.Instructors.Where(u => u.Name == username).Select(i => i.Id).FirstOrDefaultAsync();
            if (instructorId < 1)
                return new List<Enrollment>();

            return await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .Where(e => e.InstructorId == instructorId)
                .ToListAsync();
        }

        public async Task<List<Enrollment>> GetAllEnrollmentsByUserId(string userId)
        {
            return await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .Include(e => e.Course.Instructor)
                .Where(e => e.Course.Instructor.UserId == userId && e.Status == EnrollmentStatus.Pending)
                .ToListAsync();
        }

        public async Task<List<Enrollment>> GetStudentsByCourse(int courseId)
        {
            return await _context.Enrollments
                .Include(e => e.Student)
                .Where(e => e.CourseId == courseId && e.Status == EnrollmentStatus.Accepted)
                .ToListAsync();
        }

        public async Task RejectEnrollmentByStudentIdAsync(int studentId)
        {
            // Find the enrollment by studentId
            var enrollment = await _context.Enrollments
                .Where(e => e.StudentId == studentId)
                .FirstOrDefaultAsync();

            if (enrollment != null)
            {
                enrollment.Status = EnrollmentStatus.Rejected;
                _context.Enrollments.Update(enrollment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
