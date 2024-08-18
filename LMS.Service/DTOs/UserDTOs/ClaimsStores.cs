using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Service.DTOs.UserDTOs;

public static class ClaimsStores
{
    public static List<Claim> AllClaims { get; set; } = new List<Claim>()
    {
        new Claim("Manage Students", "Manage Students"),
        new Claim("Manage Instructors", "Manage Instructors"),
        new Claim("Manage Courses", "Manage Courses"),
        new Claim("Disable Entities", "Disable Entities"),
        new Claim("Disable Students", "Disable Students"),
        new Claim("Disable Instructors", "Disable Instructors"),
        new Claim("Disable Courses", "Disable Courses")
    };
}
