using LMS.Domain.Entities.Instructors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Entities.Courses
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int MaxStudents { get; set; }

        public decimal Price { get; set; }
        public int CourseTime { get; set; }

        public int InstructorId { get; set; }

        // Navigation property
        public Instructor Instructor { get; set; }
    }
}
