using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using KevBlog.Contract.DTOs;
using KevBlog.Domain.Entities;
using KevBlog.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KevBlog.Infrastructure.Data
{
    public class Seed
    {
        private readonly static string _seedUserFilePath = "../KevBlog.Infrastructure/Data/UserSeedData.json";
        private readonly static string _seedPostFilePath = "../KevBlog.Infrastructure/Data/PostSeedData.json";
        public static async Task SeedUsers(UserManager<User> userManager, RoleManager<AppRole> roleManager)
        {
            if(await userManager.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync(_seedUserFilePath);
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

            var postData = await File.ReadAllTextAsync(_seedPostFilePath);
            var posts = JsonSerializer.Deserialize<List<Post>>(postData);

            foreach(var post in posts)
            {
                await context.AddAsync(post);
                await context.SaveChangesAsync();
            }
        }
        public static async Task SeedCategories(DataContext context)
        {
            if (await context.Categories.AnyAsync()) return;

            Category category1 = new Category { Id = 1, Title = "Programming", Description = "Programming Category" };
            Category category2 = new Category { Id = 2, Title = "Devops", Description = "DevOps Knowledge" };
            Category category3 = new Category { Id = 3, Title = "Life", Description = "Life" };
            var categories = new[] { category1, category2, category3 };
            foreach (var post in categories)
            {
                await context.AddAsync(post);
                await context.SaveChangesAsync();
            }
        }
    }
}