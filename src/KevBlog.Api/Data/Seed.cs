using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context){
            if(await context.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
            var options =  new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var users = JsonSerializer.Deserialize<List<User>>(userData);
            foreach(var user in users){
                using var hmac = new HMACSHA512();
                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                user.PasswordSalt = hmac.Key;
                var samplePost = new Post();
                Random ran = new Random();
                samplePost.Title = "Post " + ran.Next();
                user.Posts.Add(samplePost);
                context.Users.Add(user);
            }
            await context.SaveChangesAsync();
        }
        public static async Task SeedPosts(DataContext context) {
            if(await context.Posts.AnyAsync()) return;

            var postData = await File.ReadAllTextAsync("Data/PostSeedData.json");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            // var users = JsonSerializer.Deserialize<List<Post>>(p)
        }
    }
}