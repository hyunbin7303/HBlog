using HBlog.Domain.Entities;
using HBlog.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
        public virtual DbSet<UserLike> Likes {get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<PostTags> PostTags { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<FileData> FileData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
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

            modelBuilder.Entity<FileStorage>()
                .HasMany(fileStorage => fileStorage.SharedUsers);

            modelBuilder.Entity<Post>(p =>
            {
                p.HasKey(post => post.Id);
                p.Property(post => post.Id).ValueGeneratedOnAdd();
                p.Property(post => post.Id).UseIdentityAlwaysColumn();
            });

            modelBuilder.Entity<Post>().HasMany(x => x.Tags)
                .WithMany(x => x.Posts)
                .UsingEntity<PostTags>(
                    x => x.HasOne(x => x.Tag)
                        .WithMany().HasForeignKey(posttag => posttag.TagId),
                    x => x.HasOne(x => x.Post)
                        .WithMany().HasForeignKey(posttag => posttag.PostId));

            var postStatusComparer = new ValueComparer<PostStatus>(
                (p1, p2) => ReferenceEquals(p1, p2) || (p1 != null && p1.Equals(p2)),
                p => p == null ? 0 : p.GetHashCode(),
                p => p == null ? null! : PostStatus.FromString(p.ToString())
            );

            modelBuilder.Entity<Post>(b =>
            {
                b.Property(p => p.Status)
                    .HasConversion(
                        v => v.ToString(),
                        v => PostStatus.FromString(v)
                    )
                    .HasMaxLength(50)
                    .HasColumnName("Status");

                b.Property(p => p.Status).Metadata.SetValueComparer(postStatusComparer);
            });

            //var postTypeComparer = new ValueComparer<PostType>(
	           // (p1, p2) => ReferenceEquals(p1, p2) || (p1 != null && p1.Equals(p2)),
	           // p => p == null ? 0 : p.GetHashCode(),
	           // p => p == null ? null! : PostType.FromString(p.ToString())
            //);

            modelBuilder.Entity<Post>(b =>
            {
	            b.Property(p => p.Type)
		            .HasConversion(
			            v => v.ToString(),
			            v => PostType.FromString(v)
		            )
		            .HasMaxLength(50)
		            .HasColumnName("Type");

	            //b.Property(p => p.Type).Metadata.SetValueComparer(postTypeComparer);
            });
			// ValueConverter + ValueComparer for Slug value object
			var slugComparer = new ValueComparer<Slug>(
                (s1, s2) => ReferenceEquals(s1, s2) || (s1 != null && s1.Equals(s2)),
                s => s == null ? 0 : s.GetHashCode(),
                s => s == null ? null! : Slug.FromValue(s.Value)
            );

            modelBuilder.Entity<Post>(b =>
            {
                b.Property(p => p.Slug)
                    .HasConversion(
                        v => v.Value,
                        v => Slug.FromValue(v)
                    )
                    .HasMaxLength(200)
                    .HasColumnName("Slug")
                    .IsRequired();

                b.Property(p => p.Slug).Metadata.SetValueComparer(slugComparer);
            });

            modelBuilder.Entity<Tag>(p =>
            {
                p.HasKey(tag => tag.Id);
                p.Property(tag => tag.Id).ValueGeneratedOnAdd();
                p.Property(tag => tag.Id).UseIdentityAlwaysColumn();
            });

            modelBuilder.Entity<PostTags>()
                .HasKey(k => new { k.PostId, k.TagId });

            modelBuilder.Entity<UserLike>()
                .HasKey(k => new { k.SourceUserId, k.TargetUserId });

            modelBuilder.Entity<UserLike>()
                .HasOne(s => s.SourceUser)
                .WithMany(l => l.LikedUsers)
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserLike>()
                .HasOne(s => s.TargetUser)
                .WithMany(l => l.LikedByUsers)
                .HasForeignKey(s => s.TargetUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(m => m.MessagesReceived)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}