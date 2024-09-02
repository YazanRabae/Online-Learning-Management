using LMS.Domain.Entities.Enrollments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Repositories.Enrollments
{
    public interface IEnrollmentRepository
    {
        public Task Create(Enrollment enrollment);
        public Task<List<Enrollment>> GetPendingEnrollments();
        public Task Update(Enrollment enrollment);
        public Task AcceptEnrollmentAsync(int enrollmentId);
        public Task RejectEnrollmentAsync(int enrollmentId);

    }
}
