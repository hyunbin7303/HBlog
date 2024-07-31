using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using HBlog.Contract.DTOs;
using HBlog.Domain.Entities;
using HBlog.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HBlog.Infrastructure.Data
{
    public class Seed
    {
        private readonly static string _seedUserFilePath = "Data/UserSeedData.json";
        private readonly static string _seedPostFilePath = "Data/PostSeedData.json";
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
        public static async Task SeedTags(DataContext context)
        {
            if (await context.Tags.AnyAsync()) return;

            var tags = new[]
            {
                new Tag { Id =1, Name = "C#", Slug = "dotnet", Desc = "dotnet programming", },
                new Tag { Id =2, Name = "Go", Slug = "golang", Desc = "golang programming", },
                new Tag { Id =3, Name = "Python", Slug = "Python", Desc = "Python", },
                new Tag { Id =4, Name = "Azure", Slug = "azure", Desc = "Azure knowledge", },
                new Tag { Id =5, Name = "Life Journey", Slug = "life", Desc = "My life", },
                new Tag { Id =6, Name = "Book", Slug = "book", Desc = "Book I read" },
                new Tag { Id =7, Name = "Travel", Slug = "travel", Desc = "All trips" },
                new Tag { Id =8, Name = "Fundamental", Slug = "fundamental", Desc = "Basic implementation" },
            };
            foreach (var tag in tags)
            {
                await context.AddAsync(tag);
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedPostTags(DataContext context)
        {
            if (await context.PostTags.AnyAsync()) return;

            var postTags = new[]
            {
                new PostTags { PostId = 1, TagId = 1,},
                new PostTags { PostId = 1, TagId = 8,},
                new PostTags { PostId = 2, TagId = 1,},
                new PostTags { PostId = 3, TagId = 5,},
                new PostTags { PostId = 4, TagId = 7,},
                new PostTags { PostId = 5, TagId = 7,},
                new PostTags { PostId = 6, TagId = 6,},

            };
            foreach (var pt in postTags)
            {
                await context.AddAsync(pt);
                await context.SaveChangesAsync();
            }
        }
    }
}