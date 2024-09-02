using LMS.Domain.Entities.Enrollments;
using LMS.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Repositories.Enrollments
{
    public class EnrollmentRepository(DbLMS _context) : IEnrollmentRepository
    {
        public async Task Create(Enrollment enrollment)
        {
            await _context.Enrollments.AddAsync(enrollment);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Enrollment>> GetPendingEnrollments()
        {
            return await _context.Enrollments
                                 .Include(e => e.Student)
                                 .Include(e => e.Instructor)
                                 .Include(e => e.Course)
                                 .Where(e => e.Status == EnrollmentStatus.Pending)
                                 .ToListAsync();
        }

        public async Task Update(Enrollment enrollment)
        {
            _context.Enrollments.Update(enrollment);
            await _context.SaveChangesAsync();
        }

        public async Task AcceptEnrollmentAsync(int enrollmentId)
        {
            var enrollment = await _context.Enrollments.FindAsync(enrollmentId);
            if (enrollment != null && enrollment.Status == EnrollmentStatus.Pending)
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
    }
}
