using LMS.Domain.Entities.Instructors;
using LMS.Domain.Entities.Students;

namespace LMS.Repository.Repositories.Instructors
{
    public interface IInstructorRepository
    {
        Task<int> GetInstructorId(string userId);
        Task<List<Instructor>> GetInstructors();
        Task CreateInstructor(Instructor instructor);
        Task<Instructor> GetInstructorById(int id);
        Task UpdateInstructor(Instructor instructor);
    }
}
