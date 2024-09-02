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
    public class EnrollmentMapper : IEnrollmentMapper
    {
        public Enrollment MapFromEnrollmentDTOtoEnrollment(EnrollmentDTO enrollmentDTO)
        {
            return new Enrollment
            {
                 Id = enrollmentDTO.Id,
                AddDate = enrollmentDTO.AddDate,
                CourseId = enrollmentDTO.CourseId,
                //InstructorId = enrollmentDTO.InstructorId ,
                //StudentId = enrollmentDTO.StudentId 

            };
        }
        public List<EnrollmentDTO> MapFromEnrollmentToEnrollmentDTO(List<Enrollment> Enrollments)
        {
            return Enrollments.Select(b => new EnrollmentDTO
            {
                Id = b.Id,
                CourseId = b.CourseId,
                StudentId = b.StudentId,
                InstructorId = b.InstructorId,
                Status = b.Status,
                AddDate = b.AddDate,
            }).ToList();
        }
    }
}
