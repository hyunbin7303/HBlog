using HBlog.Domain.Common;
using HBlog.Domain.ValueObjects;
using HBlog.Domain.Events;
using System.ComponentModel.DataAnnotations.Schema;

namespace HBlog.Domain.Entities
{
    [Table("Posts")]
    public class Post : BaseEntity<int>
    {
        public PostTitle Title { get; private set; }
        public Slug Slug { get; private set; }
        public string Desc { get; private set; }
        public PostStatus Status { get; private set; }
        public string Content { get; private set; } = string.Empty;
        public string LinkForPost { get; private set; }
        public PostType Type { get; private set; }
        public int Upvotes { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime LastUpdated { get; private set; }
        public Guid UserId { get; private set; }
        public int CategoryId { get; private set; }
        
        public virtual User User { get; private set; }
        public virtual Category Category { get; private set; }
        private readonly List<Tag> _tags = new();
        public IReadOnlyCollection<Tag> Tags => _tags.AsReadOnly();

        // EF Core requires parameterless constructor
        private Post() { }

        // Factory method
        public static Post Create(
            string title, 
            string description, 
            string content,
            Guid userId,
            int categoryId,
            PostType type)
        {
            // Validate business rules (throws DomainException if invalid)
            if (userId == Guid.Empty)
                throw new DomainException("User ID cannot be empty");

            if (categoryId <= 0)
                throw new DomainException("Category ID must be greater than 0");

            var post = new Post
            {
                Title = PostTitle.Create(title), // Can throw
                Slug = Slug.FromString(title),    // Can throw
                Desc = description ?? string.Empty,
                Content = content ?? string.Empty,
                Status = PostStatus.Draft,
                UserId = userId,
                CategoryId = categoryId,
                Type = type ?? PostType.Normal,
                Created = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow,
                Upvotes = 0
            };
            
            post.AddDomainEvent(new PostCreatedEvent(post));
            return post;
        }

        // Business methods
        public void Publish()
        {
            if (Status.IsPublished)
                throw new DomainException("Post is already published");

            if (string.IsNullOrWhiteSpace(Content))
                throw new DomainException("Cannot publish a post without content");
                
            Status = PostStatus.Published;
            LastUpdated = DateTime.UtcNow;
            AddDomainEvent(new PostPublishedEvent(Id));
        }

        public void Archive()
        {
            if (Status.IsRemoved)
                throw new DomainException("Post is already archived");
                
            Status = PostStatus.Removed;
            LastUpdated = DateTime.UtcNow;
        }

        public void Activate()
        {
            if (Status.IsPublished)
                throw new DomainException("Cannot activate an already published post");
                
            Status = PostStatus.Active;
            LastUpdated = DateTime.UtcNow;
        }

        public void AddTag(Tag tag)
        {
            if (tag is null)
                throw new ArgumentNullException(nameof(tag));
                
            if (_tags.Any(t => t.Id == tag.Id))
                return; // Idempotent operation
                
            _tags.Add(tag);
            LastUpdated = DateTime.UtcNow;
        }

        public void RemoveTag(int tagId)
        {
            var tag = _tags.FirstOrDefault(t => t.Id == tagId);
            if (tag is null)
                throw new DomainException($"Tag with ID {tagId} is not associated with this post");

            _tags.Remove(tag);
            LastUpdated = DateTime.UtcNow;
        }

        public void Update(string title, string description, string content, int categoryId)
        {
            if (Status.IsRemoved)
                throw new DomainException("Cannot update a removed post");

            if (categoryId <= 0)
                throw new DomainException("Category ID must be greater than 0");

            Title = PostTitle.Create(title); // Can throw
            Desc = description ?? string.Empty;
            Content = content ?? string.Empty;
            CategoryId = categoryId;
            LastUpdated = DateTime.UtcNow;
            
            AddDomainEvent(new PostUpdatedEvent(Id));
        }

        public void ChangeType(PostType newType)
        {
            if (newType is null)
                throw new ArgumentNullException(nameof(newType));

            Type = newType;
            LastUpdated = DateTime.UtcNow;
        }

        public void IncrementUpvotes()
        {
            Upvotes++;
            LastUpdated = DateTime.UtcNow;
        }
    }
}