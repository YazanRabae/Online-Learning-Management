using LMS.Domain.Entities.Enrollments;
using LMS.Service.DTOs.Enrollments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Service.Services.Enrollments
{
    public interface  IEnrollmentService
    {
        public Task<List<EnrollmentDTO>> GetAllPendingEnrollments();
        public Task UpdateEnrollment(EnrollmentDTO enrollmentDTO);
        public Task AcceptEnrollmentAsync(int enrollmentId);
        public Task RejectEnrollmentAsync(int enrollmentId);
    }
}
