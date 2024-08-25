using LMS.Domain.Entities.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Domain.Entities.Users;

namespace LMS.Domain.Entities.Instructors;

public class Instructor
{
    public int Id { get; set; }

    public DateTime HireDate { get; set; }

    public string Department { get; set; }

   
    public string UserId { get; set; }
    public User User { get; set; }

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}


