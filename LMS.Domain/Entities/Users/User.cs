using LMS.Domain.Entities.Courses;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Entities.Users;

public class User : IdentityUser
{




    public ICollection<Course> Courses { get; set; }
}

