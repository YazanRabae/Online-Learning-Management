using LMS.Domain.Entities.Courses;
using LMS.Repository.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Repositories.Courses
{
    public class CourseRepository(DbLMS _context) : ICourseRepository
    {
      
        public async Task<List<Course>> GetAll()
        {
            return await _context.Courses.ToListAsync();
        }
    }
}
