using LMS.Domain.Entities.Users;
using LMS.Repository.Context;
using LMS.Repository.Repositories.Courses;
using LMS.Repository.Repositories.Enrollments;
using LMS.Repository.Repositories.Instructors;
using LMS.Repository.Repositories.Shared;
using LMS.Repository.Repositories.Students;
using LMS.Repository.Repositories.Users;
using LMS.Service.Common.Constants;
using LMS.Service.Mapper.Courses;
using LMS.Service.Mapper.Enrollments;
using LMS.Service.Mapper.Instructors;
using LMS.Service.Mapper.Students;
using LMS.Service.Services;
using LMS.Service.Services.Courses;
using LMS.Service.Services.Enrollments;
using LMS.Service.Services.Instructors;
using LMS.Service.Services.Shared;
using LMS.Service.Services.Students;
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

            builder.Services.AddControllersWithViews()
           .AddViewOptions(options => options.HtmlHelperOptions.ClientValidationEnabled = true);



            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<IInstructorService, InstructorService>();
            builder.Services.AddScoped<ISharedService, SharedService>();
            builder.Services.AddScoped<ICourseMapper, CourseMapper>();
            builder.Services.AddScoped<IStudentMapper, StudentMapper>();
            builder.Services.AddScoped<IInstructorMapper, InstructorMapper>();
            builder.Services.AddScoped<IEnrollmentMapper, EnrollmentMapper>();
            builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
            builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            builder.Services.AddScoped<ISharedRepository, SharedRepository>();


            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy.ManageStudents", policy =>
                    policy.RequireClaim("Manage Students", "true"));
                options.AddPolicy("AdminPolicy.ManageInstructors", policy =>
                    policy.RequireClaim("Manage Instructor", "true"));
                options.AddPolicy("AdminPolicy.ManageCourses", policy =>
                    policy.RequireClaim("Manage Courses", "true"));
                options.AddPolicy("AdminPolicy.DisableStudents", policy =>
                    policy.RequireClaim("Disable Students", "true"));
                options.AddPolicy("AdminPolicy.DisableInstructors", policy =>
                    policy.RequireClaim("Disable Instructor", "true"));
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
            string[] roleNames = { RoleConstants.Admin, RoleConstants.Instructor, RoleConstants.Student };
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
                    await userManager.AddToRoleAsync(user, RoleConstants.Admin);
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
