using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LMS.Domain.Entities.Users;
using LMS.Domain.Entities.Courses;
using LMS.Domain.Entities.Enrollments; // Assuming you have a Course class in this namespace

namespace LMS.Repository.Context
{
    public class DbLMS : IdentityDbContext<User> // Use your custom User class here
    {
        public DbLMS(DbContextOptions<DbLMS> options) : base(options) { }

        public DbSet<Course> Courses { get; set; } // Register your Course entity here
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Instructor)
                .WithMany(u => u.EnrollmentsAsInstructor)
                .HasForeignKey(e => e.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(u => u.EnrollmentsAsStudent)
                .HasForeignKey(e => e.StudentId);


            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId);



            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course"); // Ensure your Course table is configured properly
                // Add any additional configurations here
            });
            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.ToTable("Enrollment"); // Ensure your Course table is configured properly
                // Add any additional configurations here
            });

            // Additional configurations for the User entity can also be added here
        }
    }
}
