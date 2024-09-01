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

    }
}
