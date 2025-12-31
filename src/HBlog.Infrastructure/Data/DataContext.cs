using HBlog.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HBlog.Infrastructure.Data
{
    public class DataContext : IdentityDbContext<User, AppRole, Guid,
    IdentityUserClaim<Guid>, AppUserRole, IdentityUserLogin<Guid>,
    IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public DataContext()
        {
        }
        public DataContext(DbContextOptions options) : base(options)
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

			modelBuilder.Entity<User>(b =>
            {
                b.HasMany(userRole => userRole.UserRoles)
                .WithOne(user => user.User)
                .HasForeignKey(userRole => userRole.UserId).IsRequired();
                b.ToTable("User");
            });

            modelBuilder.Entity<AppRole>(b => {
                b.HasMany(userRole => userRole.UserRoles)
                .WithOne(user => user.Role)
                .HasForeignKey(userRole => userRole.RoleId).IsRequired();
                b.ToTable("Role");
            });
            modelBuilder.Entity<AppUserRole>().ToTable("UserRole");
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaim");
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaim");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserToken");
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogin");


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