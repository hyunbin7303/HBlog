using HBlog.Domain.Entities;
using HBlog.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HBlog.Infrastructure.Data
{
    internal class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            // Table and key
            builder.ToTable("Posts");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Id).UseIdentityAlwaysColumn();
            
            // Value Objects with Converters (matching your DataContext style)
            var slugComparer = new ValueComparer<Slug>(
                (s1, s2) => ReferenceEquals(s1, s2) || (s1 != null && s1.Equals(s2)),
                s => s == null ? 0 : s.GetHashCode(),
                s => s == null ? null! : Slug.FromValue(s.Value)
            );
            
            builder.Property(p => p.Slug)
                .HasConversion(v => v.Value, v => Slug.FromValue(v))
                .HasMaxLength(200)
                .HasColumnName("Slug")
                .IsRequired()
                .Metadata.SetValueComparer(slugComparer);
            
            var postStatusComparer = new ValueComparer<PostStatus>(
                (p1, p2) => ReferenceEquals(p1, p2) || (p1 != null && p1.Equals(p2)),
                p => p == null ? 0 : p.GetHashCode(),
                p => p == null ? null! : PostStatus.FromString(p.ToString())
            );
            
            builder.Property(p => p.Status)
                .HasConversion(v => v.ToString(), v => PostStatus.FromString(v))
                .HasMaxLength(50)
                .HasColumnName("Status")
                .Metadata.SetValueComparer(postStatusComparer);
            
            builder.Property(p => p.Type)
                .HasConversion(v => v.ToString(), v => PostType.FromString(v))
                .HasMaxLength(50)
                .HasColumnName("Type");
            
            builder.Property(p => p.Title).IsRequired().HasMaxLength(200);
            builder.Property(p => p.Desc).HasMaxLength(500);
            builder.Property(p => p.Content).IsRequired();
            builder.Property(p => p.LinkForPost).HasMaxLength(500);
            builder.Property(p => p.Upvotes).HasDefaultValue(0);
            builder.Property(p => p.Created).IsRequired();
            builder.Property(p => p.LastUpdated).IsRequired();
            
            builder.HasMany(x => x.Tags)
                .WithMany(x => x.Posts)
                .UsingEntity<PostTags>(
                    x => x.HasOne(x => x.Tag).WithMany().HasForeignKey(pt => pt.TagId),
                    x => x.HasOne(x => x.Post).WithMany().HasForeignKey(pt => pt.PostId)
                );
            
            // Indexes
            builder.HasIndex(p => p.Slug).IsUnique();
            builder.HasIndex(p => p.UserId);
            builder.HasIndex(p => p.CategoryId);
        }
    }
}
