using System.Text.RegularExpressions;
using HBlog.Domain.Common;

namespace HBlog.Domain.ValueObjects;

public sealed partial class Slug : IEquatable<Slug>
{
    public string Value { get; }
    private const int MaxLength = 200;

    private Slug(string value)
    {
        Value = value;
    }

    public static Slug FromString(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new DomainException("Cannot create slug from empty string");

        var slug = GenerateSlug(input);
        
        if (slug.Length > MaxLength)
            slug = slug[..MaxLength];

        return new Slug(slug);
    }

    public static Slug FromValue(string slugValue)
    {
        if (string.IsNullOrWhiteSpace(slugValue))
            throw new DomainException("Slug cannot be empty");
            
        if (!IsValidSlug(slugValue))
            throw new DomainException("Invalid slug format. Use lowercase letters, numbers, and hyphens only");

        return new Slug(slugValue);
    }

    private static string GenerateSlug(string input)
    {
        var slug = input.ToLowerInvariant();
        slug = SlugRegex().Replace(slug, "");
        slug = WhitespaceRegex().Replace(slug, "-");
        slug = slug.Trim('-');
        return slug;
    }

    private static bool IsValidSlug(string value)
    {
        return ValidSlugRegex().IsMatch(value);
    }

    [GeneratedRegex(@"[^a-z0-9\s-]")]
    private static partial Regex SlugRegex();
    
    [GeneratedRegex(@"[\s-]+")]
    private static partial Regex WhitespaceRegex();
    
    [GeneratedRegex(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")]
    private static partial Regex ValidSlugRegex();

    public bool Equals(Slug? other)
    {
        if (other is null) return false;
        return string.Equals(Value, other.Value, StringComparison.Ordinal);
    }

    public override bool Equals(object? obj) => obj is Slug other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    public static implicit operator string(Slug slug) => slug.Value;
    public override string ToString() => Value;
}