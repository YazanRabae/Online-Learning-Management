using LMS.Domain.Entities.Enrollments;
using LMS.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Repositories.Shared
{
    public class SharedRepository : ISharedRepository
    {
        private readonly DbLMS _context;

        public SharedRepository(DbLMS context)
        {
            _context = context;
        }
        public async Task<(int, int, int, List<string>, List<int>)> GetCounts(string role, string userId)
        {
            int coursesCount = 0;
            int studentsCount = 0;
            int instructorCount = 0;
            List<string> courseTitles = new List<string>();
            List<int> enrollmentCounts = new List<int>();
            if (role == "Admin")
            {
                coursesCount = await _context.Courses
                    .CountAsync();

                studentsCount = await _context.Students
                    .CountAsync();

                instructorCount = await _context.Instructors
                    .CountAsync();

                var coursesData = await _context.Courses
                    .Select(course => new
                    {
                        course.Title,
                        EnrollmentCount = course.Enrollments.Count()
                    }).ToListAsync();

                courseTitles = coursesData.Select(c => c.Title).ToList();
                enrollmentCounts = coursesData.Select(c => c.EnrollmentCount).ToList();
            }


            if (role == "Instructor")
            {
                coursesCount = await _context.Courses
                    .Include(c => c.Instructor)
                    .Where(c => c.Instructor.UserId == userId)
                    .CountAsync();

                studentsCount = await _context.Students
                    .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Instructor)
                    .Where(s => s.Enrollments.Any(e => e.Instructor.UserId == userId))
                    .CountAsync();

                instructorCount = await _context.Instructors
                    .CountAsync();
                
                var coursesData = await _context.Courses
                    .Include(c => c.Instructor)
                    .Where(c => c.Instructor.UserId == userId)
                    .Select(course => new
                    {
                        course.Title,
                        EnrollmentCount = course.Enrollments.Count()
                    }).ToListAsync();

                courseTitles = coursesData.Select(c => c.Title).ToList();
                enrollmentCounts = coursesData.Select(c => c.EnrollmentCount).ToList();
            }


            if (role == "Student")
            {
                coursesCount = await _context.Courses
                    .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Student)
                    .Where(c => c.Enrollments.Any(e => e.Student.UserId == userId && e.Status == EnrollmentStatus.Accepted))
                    .CountAsync();

                studentsCount = await _context.Students
                    .CountAsync();

                instructorCount = await _context.Instructors
                    .Include(i => i.Enrollments)
                    .ThenInclude(e => e.Student)
                    .Where(i => i.Enrollments.Any(e => e.Student.UserId == userId && e.Status == EnrollmentStatus.Accepted))
                    .CountAsync();

                var coursesData = await _context.Courses
                    .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Student)
                    .Where(c => c.Enrollments.Any(e => e.Student.UserId == userId && e.Status == EnrollmentStatus.Accepted))
                    .Select(course => new
                    {
                        course.Title,
                        EnrollmentCount = course.Enrollments.Count()
                    }).ToListAsync();

                courseTitles = coursesData.Select(c => c.Title).ToList();
                enrollmentCounts = coursesData.Select(c => c.EnrollmentCount).ToList();
            }

            return (coursesCount, studentsCount, instructorCount, courseTitles, enrollmentCounts);
        }
    }
}
