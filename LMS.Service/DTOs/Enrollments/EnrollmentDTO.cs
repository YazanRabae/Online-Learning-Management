using LMS.Domain.Entities.Courses;
using LMS.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Service.DTOs.Enrollments
{
    public class EnrollmentDTO
    {
            [Key]
            public int Id { get; set; }
            public DateTime AddDate { get; set; }

            public int InstructorId { get; set; }

            public int CourseId { get; set; }

            public int StudentId { get; set; }
        }

    }

