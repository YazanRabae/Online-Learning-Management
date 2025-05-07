using LMS.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Repositories.Users
{
    public interface IUserRepository
    {
        Task<User> GetByUsernameAsync(string username);

        Task<IEnumerable<User>> GetStudentsAsync();
        Task<IEnumerable<User>> GetInstructorsAsync();
    }
}
