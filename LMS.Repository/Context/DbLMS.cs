using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LMS.Domain.Entities.Users;
using LMS.Domain.Entities.Courses;
using LMS.Domain.Entities.Enrollments;
using LMS.Domain.Entities.Students;
using LMS.Domain.Entities.Instructors; // Assuming you have a Course class in this namespace

namespace LMS.Repository.Context
{
    public class DbLMS : IdentityDbContext<User> // Use your custom User class here
    {
        public DbLMS(DbContextOptions<DbLMS> options) : base(options) { }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Instructor> Instructors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Instructor)
                .WithMany(u => u.Enrollments)
                .HasForeignKey(e => e.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(u => u.Enrollments)
                .HasForeignKey(e => e.StudentId);


            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.User)
                .WithOne(u => u.Student)
                .HasForeignKey<Student>(s => s.UserId);


            modelBuilder.Entity<Instructor>()
                .HasOne(c => c.User)
                .WithOne(u => u.Instructor)
                .HasForeignKey<Instructor>(e => e.UserId);


            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");
            });
            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.ToTable("Enrollment");
            });
            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Student");
            });
            modelBuilder.Entity<Instructor>(entity =>
            {
                entity.ToTable("Instructor");
            });
        }
    }
}
