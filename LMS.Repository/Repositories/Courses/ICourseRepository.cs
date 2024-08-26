using LMS.Domain.Entities.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Repositories.Courses
{
    public interface ICourseRepository
    {
        public  Task<List<Course>> GetAll();
        
    }
}
