using LMS.Domain.Entities.Courses;
using LMS.Domain.Entities.Users;
using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.Entities.Enrollments
{
    public class Enrollment
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
