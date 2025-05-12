using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.Service.DTOs.Courses
{
    public class CourseDTO
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Course title is required.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Course title must be between 5 and 100 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Course description is required.")]
        [StringLength(1000, MinimumLength = 20, ErrorMessage = "Course description must be between 20 and 1000 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        [DateNotInPast(ErrorMessage = "Start date cannot be in the past.")]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        [Display(Name = "End Date")]
        [DateGreaterThan(nameof(StartDate), ErrorMessage = "End date must be after the start date.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Maximum number of students is required.")]
        [Range(1, 1000, ErrorMessage = "Maximum students must be between 1 and 1000.")]
        public int MaxStudents { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 10000, ErrorMessage = "Price must be a positive value between 0.01 and 10,000.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Course duration (in hours) is required.")]
        [Range(1, 1000, ErrorMessage = "Course time must be between 1 and 1000 hours.")]
        [Display(Name = "Course Time (hours)")]
        public int CourseTime { get; set; }

        public string ImageData { get; set; }

        [Display(Name = "Course Image")]
        public IFormFile ImageFile { get; set; }

        public int InstructorId { get; set; }

        public string InstructorName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsEnrolled { get; set; }
    }

    // ✅ Custom validation: EndDate > StartDate
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateGreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not DateTime endDate)
                return new ValidationResult("Invalid date value.");

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null)
                return new ValidationResult($"Unknown property: {_comparisonProperty}");

            var startDateObj = property.GetValue(validationContext.ObjectInstance);
            if (startDateObj is not DateTime startDate)
                return new ValidationResult("Invalid comparison date.");

            if (endDate <= startDate)
                return new ValidationResult(ErrorMessage ?? "End date must be after the start date.");

            return ValidationResult.Success;
        }
    }

    // ✅ Custom validation: StartDate >= Today
    public class DateNotInPastAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not DateTime dateValue)
                return new ValidationResult("Invalid date format.");

            if (dateValue.Date < DateTime.Now.Date)
                return new ValidationResult(ErrorMessage ?? "Start date cannot be in the past.");

            return ValidationResult.Success;
        }
    }
}
