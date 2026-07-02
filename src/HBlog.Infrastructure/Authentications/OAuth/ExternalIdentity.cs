namespace HBlog.Infrastructure.Authentications.OAuth;

public record ExternalIdentity(
    string Provider,
    string Subject,
    string? Email,
    bool EmailVerified,
    string? GivenName,
    string? FamilyName);
