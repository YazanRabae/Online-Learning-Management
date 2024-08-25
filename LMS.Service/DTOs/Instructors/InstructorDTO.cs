using LMS.Domain.Entities.Courses;
using System.ComponentModel.DataAnnotations;

namespace LMS.Service.DTOs.Instructors;

public class InstructorDTO
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(100)]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public DateTime HireDate { get; set; }

    
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}

