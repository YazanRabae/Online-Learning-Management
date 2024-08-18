using LMS.Repository.Context;
using LMS.Repository.Repositories.Users;
using LMS.Service.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Online_Learning_Management
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<DbLMS>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DbLMS")));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<DbLMS>();

       

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");





            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                
                
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                    SeedRoles(roleManager);
                    SeedUsers(userManager);

            }
            app.Run();
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "Instructor", "Student" };
            foreach (var roleName in roleNames)
            {
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    var role = new IdentityRole();
                    role.Name = roleName;
                    roleManager.CreateAsync(role).Wait();
                }
            }
        }

        private static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            if (userManager.FindByNameAsync("admin@example.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "Admin",
                    Email = "admin@example.com"
                };

                IdentityResult result = userManager.CreateAsync(user, "P@ssw0rd%*").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }

        }
    }
}

