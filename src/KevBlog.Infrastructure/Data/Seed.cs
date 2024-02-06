using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using KevBlog.Domain.Entities;
using KevBlog.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KevBlog.Infrastructure.Data
{
    public class Seed
    {
        private readonly static string _seedFilePath = "../KevBlog.Infrastructure/Data/UserSeedData.json";
        public static async Task SeedUsers(UserManager<User> userManager, RoleManager<AppRole> roleManager)
        {
            if(await userManager.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync(_seedFilePath);
            var options =  new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var users = JsonSerializer.Deserialize<List<User>>(userData);

            var roles = new List<AppRole>
            {
                new AppRole{ Name =  "Member"},
                new AppRole{ Name =  "Admin"},
                new AppRole{ Name =  "Moderator"},
            };
            foreach(var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach(var user in users){
                user.UserName = user.UserName.ToLower();
                await userManager.CreateAsync(user, "Testing#1234!");
                await userManager.AddToRoleAsync(user, "Member");
            }

            var admin = new User
            {
                UserName = "admin"
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] {"Admin", "Moderator" });
        }
        public static async Task SeedPosts(DataContext context) {
            if(await context.Posts.AnyAsync()) return;

            var postData = await File.ReadAllTextAsync(_seedFilePath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }
    }
}