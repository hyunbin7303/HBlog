using HBlog.Contract.DTOs.OAuth;

namespace HBlog.Infrastructure.Authentications.OAuth;

public interface IExternalIdentityProvider
{
    string Name { get; }
    Task<ExternalIdentity?> VerifyAsync(OAuthLoginDto request, CancellationToken ct);
}
