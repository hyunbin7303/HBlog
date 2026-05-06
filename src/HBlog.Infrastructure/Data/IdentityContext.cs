using HBlog.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HBlog.Infrastructure.Data
{
    public class IdentityContext : IdentityDbContext<User, AppRole, Guid,
        IdentityUserClaim<Guid>, AppUserRole, IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public IdentityContext()
        {
        }
        
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(b =>
            {
                b.HasMany(userRole => userRole.UserRoles)
                    .WithOne(user => user.User)
                    .HasForeignKey(userRole => userRole.UserId)
                    .IsRequired();
                b.ToTable("User");
            });

            modelBuilder.Entity<AppRole>(b =>
            {
                b.HasMany(userRole => userRole.UserRoles)
                    .WithOne(user => user.Role)
                    .HasForeignKey(userRole => userRole.RoleId)
                    .IsRequired();
                b.ToTable("Role");
            });
            
            modelBuilder.Entity<AppUserRole>().ToTable("UserRole");
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaim");
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaim");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserToken");
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogin");
        }
    }
}
