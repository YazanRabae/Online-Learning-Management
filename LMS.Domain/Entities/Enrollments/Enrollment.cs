using LMS.Domain.Entities.Courses;
using LMS.Domain.Entities.Instructors;
using LMS.Domain.Entities.Students;
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
        public int InstructorId { get; set; }
        public Instructor Instructor { get; set; } 

        // Foreign Key for Course
        public int CourseId { get; set; }
        public Course Course { get; set; } 

        // Foreign Key for Student
        public int StudentId { get; set; }
        public Student Student { get; set; } 
    }
}
