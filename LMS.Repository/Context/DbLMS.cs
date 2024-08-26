using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LMS.Domain.Entities.Users;
using LMS.Domain.Entities.Courses;
using Microsoft.AspNetCore.Identity; // Assuming you have a Course class in this namespace

namespace LMS.Repository.Context
{
    public class DbLMS : IdentityDbContext<User> // Use your custom User class here
    {
        public DbLMS(DbContextOptions<DbLMS> options) : base(options) { }

        public DbSet<Course> Courses { get; set; } // Register your Course entity here

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure your entities here
            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course"); // Ensure your Course table is configured properly
                // Add any additional configurations here
            });

            // Additional configurations for the User entity can also be added here
        }
    }
}
