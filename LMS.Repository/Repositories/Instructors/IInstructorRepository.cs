using LMS.Domain.Entities.Instructors;

namespace LMS.Repository.Repositories.Instructors
{
    public interface IInstructorRepository
    {
        Task<int> GetInstructorId(string userId);
        Task<List<Instructor>> GetInstructors();
        Task CreateInstructor(Instructor Instructor);
    }
}
