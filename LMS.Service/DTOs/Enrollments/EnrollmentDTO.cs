using LMS.Domain.Entities.Courses;
using LMS.Domain.Entities.Enrollments;
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

        public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Pending;

        // Foreign Key for Instructor
        public string InstructorId { get; set; }
        public User Instructor { get; set; }

        // Foreign Key for Course
        public int CourseId { get; set; }
        public Course Course { get; set; }

        // Foreign Key for Student
        public string StudentId { get; set; }
        public User Student { get; set; }
    }

    }

