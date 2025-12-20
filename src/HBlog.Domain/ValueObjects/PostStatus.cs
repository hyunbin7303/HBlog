using HBlog.Domain.Common;

namespace HBlog.Domain.ValueObjects;

public sealed class PostStatus : IEquatable<PostStatus>
{
    public string Value { get; }

    public static readonly PostStatus Draft = new("Draft");
    public static readonly PostStatus Active = new("Active");
    public static readonly PostStatus Published = new("Published");
    public static readonly PostStatus Removed = new("Removed");

    private PostStatus(string value)
    {
        Value = value;
    }

    public static PostStatus FromString(string status)
    {
        return status switch
        {
            "Draft" => Draft,
            "Active" => Active,
            "Published" => Published,
            "Removed" => Removed,
            _ => throw new DomainException($"Invalid post status: '{status}'. Valid values: Draft, Active, Published, Removed")
        };
    }

    public bool IsDraft => Equals(Draft);
    public bool IsPublished => Equals(Published);
    public bool IsRemoved => Equals(Removed);

    public bool Equals(PostStatus? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is PostStatus other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    public static implicit operator string(PostStatus status) => status.Value;
    public override string ToString() => Value;
}