using HBlog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HBlog.Infrastructure.Data
{
    public class BlogContext : DbContext
    {
        public BlogContext()
        {
        }

        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
        }

        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<PostTags> PostTags { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PostConfiguration());

            modelBuilder.Entity<Tag>(p =>
            {
                p.HasKey(tag => tag.Id);
                p.Property(tag => tag.Id).ValueGeneratedOnAdd();
                p.Property(tag => tag.Id).UseIdentityAlwaysColumn();
            });

            modelBuilder.Entity<PostTags>()
                .HasKey(k => new { k.PostId, k.TagId });
        }
    }
}
