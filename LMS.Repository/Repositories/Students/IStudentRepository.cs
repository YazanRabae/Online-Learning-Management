using LMS.Domain.Entities.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Repositories.Students
{
    public interface IStudentRepository
    {
        Task<int> GetStudentId(string userId);
        Task CreateStudent(Student student);
        Task<List<Student>> GetStudents();
        Task<Student> GetStudentById(int id);
        Task UpdateStudent(Student student);
    }
}
