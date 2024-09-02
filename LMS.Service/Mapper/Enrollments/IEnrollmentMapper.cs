using LMS.Domain.Entities.Courses;
using LMS.Domain.Entities.Enrollments;
using LMS.Service.DTOs.Courses;
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
        public Enrollment MapFromEnrollmentDTOtoEnrollment(EnrollmentDTO enrollmentDTO);
        public List<EnrollmentDTO> MapFromEnrollmentToEnrollmentDTO(List<Enrollment> Entrollments);
    }
}
