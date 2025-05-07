using LMS.Domain.Entities.Courses;
using LMS.Domain.Entities.Enrollments;
using LMS.Domain.Entities.Instructors;
using LMS.Domain.Entities.Students;
using Microsoft.AspNetCore.Identity;

namespace LMS.Domain.Entities.Users;

public class User : IdentityUser
{
    // Foreign Key for Instructor
    public int InstructorId { get; set; }
    public Instructor Instructor { get; set; }

    // Foreign Key for Student
    public int StudentId { get; set; }
    public Student Student { get; set; }
}

