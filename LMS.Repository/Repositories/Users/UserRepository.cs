using LMS.Repository.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly DbLMS _context;

        public UserRepository(DbLMS context)
        {
            _context = context;
        }
    }
}
