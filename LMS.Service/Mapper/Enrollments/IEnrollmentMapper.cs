using LMS.Domain.Entities.Enrollments;
using LMS.Service.DTOs.Enrollments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Service.Mapper.Enrollments
{
    public interface  IEnrollmentMapper
    {
        public Enrollment MapFromBookDTOoBook(EnrollmentDTO enrollmentDTO);
    }
}
