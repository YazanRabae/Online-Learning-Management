﻿
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.Service.DTOs.Courses;

    public class CourseDTO
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Course title is required.")]
    [StringLength(100, ErrorMessage = "Course title cannot exceed 100 characters.")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Course description is required.")]
    [StringLength(1000, ErrorMessage = "Course description cannot exceed 1000 characters.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Start date is required.")]
    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "End date is required.")]
    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    public DateTime EndDate { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Maximum number of students must be a non-negative integer.")]
    public int MaxStudents { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Course Time is required.")]
    public int CourseTime { get; set; }

    [Required(ErrorMessage = "Instructor ID is required.")]
    public string InstructorId { get; set; }

    public string InstructorName { get; set; }
}





