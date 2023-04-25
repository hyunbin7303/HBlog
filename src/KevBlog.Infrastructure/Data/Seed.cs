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
        public static async Task SeedUsers(UserManager<User> userManger){
            if(await userManger.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("../KevBlog.Infrastructure/Data/UserSeedData.json");
            var options =  new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var users = JsonSerializer.Deserialize<List<User>>(userData);

            foreach(var user in users){
                user.UserName = user.UserName.ToLower();
                await userManger.CreateAsync(user, "Pa$$w0rd");
            }
        }
        public static async Task SeedPosts(DataContext context) {
            if(await context.Posts.AnyAsync()) return;

            var postData = await File.ReadAllTextAsync("../KevBlog.Infrastructure/Data/UserSeedData.json");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }
    }
}