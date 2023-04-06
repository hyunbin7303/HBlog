using KevBlog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KevBlog.Infrastructure.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<User> Users {get; set; }
        public DbSet<Domain.Entities.Application> Applications { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}