using HBlog.Domain.Common;

namespace HBlog.Domain.ValueObjects;

public sealed class PostTitle : IEquatable<PostTitle>
{
    public string Value { get; }
    private const int MaxLength = 200;
    private const int MinLength = 1;

    private PostTitle(string value)
    {
        Value = value;
    }

    public static PostTitle Create(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Title cannot be empty");
            
        var trimmed = title.Trim();
        
        if (trimmed.Length > MaxLength)
            throw new DomainException($"Title cannot exceed {MaxLength} characters");
            
        if (trimmed.Length < MinLength)
            throw new DomainException($"Title must be at least {MinLength} character");

        return new PostTitle(trimmed);
    }

    public bool Equals(PostTitle? other)
    {
        if (other is null) return false;
        return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj) => obj is PostTitle other && Equals(other);
    public override int GetHashCode() => Value.ToLowerInvariant().GetHashCode();
    public static implicit operator string(PostTitle title) => title.Value;
    public override string ToString() => Value;
}