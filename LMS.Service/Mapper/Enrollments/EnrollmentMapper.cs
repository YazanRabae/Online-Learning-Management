using LMS.Domain.Entities.Enrollments;
using LMS.Service.DTOs.Enrollments;


namespace LMS.Service.Mapper.Enrollments
{
    public class EnrollmentMapper : IEnrollmentMapper
    {
        public Enrollment MapFromEnrollmentDTOtoEnrollment(EnrollmentDTO dto)
        {
            return new Enrollment
            {
                Id = dto.Id,
                AddDate = dto.AddDate,
                Status = dto.Status,
                InstructorId = dto.InstructorId,
                StudentId = dto.StudentId,
                CourseId = dto.CourseId
            };
        }

        public List<EnrollmentDTO> MapFromEnrollmentToEnrollmentDTO(List<Enrollment> enrollments)
        {
            return enrollments.Select(e => new EnrollmentDTO
            {
                Id = e.Id,
                AddDate = e.AddDate,
                Status = e.Status,
                InstructorId = e.InstructorId,
                StudentId = e.StudentId,
                CourseId = e.CourseId,
                Instructor = e.Instructor,
                Student = e.Student,
                Course = e.Course
            }).ToList();
        }
    }
}
