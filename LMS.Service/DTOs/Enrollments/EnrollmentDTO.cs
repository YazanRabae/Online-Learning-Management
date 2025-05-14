using LMS.Domain.Entities.Courses;
using LMS.Domain.Entities.Enrollments;
using LMS.Domain.Entities.Instructors;
using System.ComponentModel.DataAnnotations;
using LMS.Domain.Entities.Students;

namespace LMS.Service.DTOs.Enrollments
{
    public class EnrollmentDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Pending;

        // Foreign Key for Instructor
        [RequiredIfNotNull(nameof(Instructor), ErrorMessage = "Instructor ID is required.")]
        public int InstructorId { get; set; }
        public Instructor Instructor { get; set; }

        // Foreign Key for Course
        [Required(ErrorMessage = "Course ID is required.")]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        // Foreign Key for Student
        [RequiredIfNotNull(nameof(Student), ErrorMessage = "Student ID is required.")]
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string CourseName { get; set; }
        public DateTime AddDate { get; set; }
        public decimal Price { get; set; }
        public LMS.Domain.Entities.Students.Student Student { get; set; }
    }

    /// <summary>
    /// Custom validation to ensure a string ID is not null or empty when the related object is not null.
    /// Useful for validating foreign keys in DTOs.
    /// </summary>
    public class RequiredIfNotNullAttribute : ValidationAttribute
    {
        private readonly string _relatedProperty;

        public RequiredIfNotNullAttribute(string relatedProperty)
        {
            _relatedProperty = relatedProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var relatedPropertyInfo = validationContext.ObjectType.GetProperty(_relatedProperty);
            if (relatedPropertyInfo == null)
                return new ValidationResult($"Unknown property: {_relatedProperty}");

            var relatedValue = relatedPropertyInfo.GetValue(validationContext.ObjectInstance);
            var stringValue = value as string;

            if (relatedValue != null && string.IsNullOrWhiteSpace(stringValue))
            {
                return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} is required.");
            }

            return ValidationResult.Success;
        }
    }
}
