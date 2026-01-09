using HBlog.Domain.Common;
using HBlog.Domain.Common.Exceptions;

namespace HBlog.Domain.ValueObjects;

public sealed class PostType : IEquatable<PostType>
{
    public string Value { get; }

    public static readonly PostType Normal = new("Normal");
    public static readonly PostType Featured = new("Featured");
    public static readonly PostType Pinned = new("Pinned");

    private PostType(string value)
    {
        Value = value;
    }

    public static PostType FromString(string type)
    {
        return type switch
        {
            "Normal" => Normal,
            "Featured" => Featured,
            "Pinned" => Pinned,
            _ => throw new DomainException($"Invalid post type: '{type}'. Valid values: Normal, Featured, Pinned")
        };
    }

    public bool Equals(PostType? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is PostType other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    public static implicit operator string(PostType type) => type.Value;
    public override string ToString() => Value;
}