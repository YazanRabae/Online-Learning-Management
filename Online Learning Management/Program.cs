using LMS.Domain.Entities.Users;
using LMS.Repository.Context;
using LMS.Repository.Repositories.Courses;
using LMS.Repository.Repositories.Enrollments;
using LMS.Repository.Repositories.Users;
using LMS.Service.Mapper.Courses;
using LMS.Service.Mapper.Enrollments;
using LMS.Service.Services;
using LMS.Service.Services.Courses;
using LMS.Service.Services.Enrollments;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Online_Learning_Management
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<DbLMS>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DbLMS")));

            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<DbLMS>()
                .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Shared/AccessDenied"; // Set to your correct path
            });

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<ICourseMapper, CourseMapper>();
            builder.Services.AddScoped<IEnrollmentMapper, EnrollmentMapper>();
            builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
            builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();


            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy.ManageStudents", policy =>
                    policy.RequireClaim("Manage Students", "true"));
                options.AddPolicy("AdminPolicy.ManageInstructors", policy =>
                    policy.RequireClaim("Manage Instructors", "true"));
                options.AddPolicy("AdminPolicy.ManageCourses", policy =>
                    policy.RequireClaim("Manage Courses", "true"));
                options.AddPolicy("AdminPolicy.DisableStudents", policy =>
                    policy.RequireClaim("Disable Students", "true"));
                options.AddPolicy("AdminPolicy.DisableInstructors", policy =>
                    policy.RequireClaim("Disable Instructors", "true"));
                options.AddPolicy("AdminPolicy.DisableCourses", policy =>
                    policy.RequireClaim("Disable Courses", "true"));
            });
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<User>>();
                SeedRoles(roleManager);
                await SeedUsers(userManager);
            }

            app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "Instructor", "Student" };
            foreach (var roleName in roleNames)
            {
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    var role = new IdentityRole { Name = roleName };
                    roleManager.CreateAsync(role).Wait();
                }
            }
        }

        private async static Task SeedUsers(UserManager<User> userManager)
        {
            var existingUser = await userManager.FindByEmailAsync("admin@example.com");

            if (existingUser == null)
            {
                var user = new User
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com"
                };

                var result = await userManager.CreateAsync(user, "P@ssw0rd%*");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
            else
            {
                if (!await userManager.IsInRoleAsync(existingUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(existingUser, "Admin");
                }
            }
        }
    }
}
