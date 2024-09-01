using LMS.Domain.Entities.Courses;
using LMS.Domain.Entities.Enrollments;
using Microsoft.AspNetCore.Identity;

namespace LMS.Domain.Entities.Users;

public class User : IdentityUser
{
    public ICollection<Course> Courses { get; set; }
    public ICollection<Enrollment> EnrollmentsAsStudent { get; set; } = new List<Enrollment>();
    public ICollection<Enrollment> EnrollmentsAsInstructor { get; set; } = new List<Enrollment>();  
}

