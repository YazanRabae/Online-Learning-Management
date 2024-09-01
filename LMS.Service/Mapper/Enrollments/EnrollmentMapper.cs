using LMS.Domain.Entities.Courses;
using LMS.Domain.Entities.Enrollments;
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
        public Enrollment MapFromBookDTOoBook(EnrollmentDTO enrollmentDTO)
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
    }
}
