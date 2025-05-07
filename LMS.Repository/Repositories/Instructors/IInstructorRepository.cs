using LMS.Domain.Entities.Instructors;

namespace LMS.Repository.Repositories.Instructors
{
    public interface IInstructorRepository
    {
        Task<int> GetInstructorId(string userId);
        Task CreateInstructor(Instructor Instructor);
    }
}
