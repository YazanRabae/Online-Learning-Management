using LMS.Domain.Entities.Users;
using LMS.Repository.Context;
using LMS.Repository.Repositories.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

public class UserRepository : IUserRepository
{
    private readonly DbLMS _context;
    private readonly UserManager<User> _userManager;

    public UserRepository(DbLMS context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }


    public async Task<User> GetByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
    }

    public async Task<IEnumerable<User>> GetStudentsAsync()
    {
        return await _userManager.GetUsersInRoleAsync("Student");
    }

    public async Task<IEnumerable<User>> GetInstructorsAsync()
    {
        return await _userManager.GetUsersInRoleAsync("Instructor");
    }
}
