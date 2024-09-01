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
  
    }
}
