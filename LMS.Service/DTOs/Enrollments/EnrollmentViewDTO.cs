using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Service.DTOs.Enrollments
{
    class EnrollmentViewDTO
    {
        public int Id { get; set; }
        public string StudentName { get; set; }
        public string CourseName { get; set; }
        public DateTime AddDate { get; set; }
        public decimal Price { get; set; }
        public int Status { get; set; } // You can cast from EnrollmentStatus
    }
}
