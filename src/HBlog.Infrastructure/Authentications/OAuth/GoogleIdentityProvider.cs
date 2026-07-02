using Google.Apis.Auth;
using HBlog.Contract.DTOs.OAuth;
using HBlog.Infrastructure.Authentications.OAuth.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HBlog.Infrastructure.Authentications.OAuth;

public class GoogleIdentityProvider : IExternalIdentityProvider
{
    public const string ProviderName = "google";
    public string Name => ProviderName;

    private readonly GoogleAuthOptions _options;
    private readonly ILogger<GoogleIdentityProvider> _logger;

    public GoogleIdentityProvider(IOptions<GoogleAuthOptions> options, ILogger<GoogleIdentityProvider> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task<ExternalIdentity?> VerifyAsync(OAuthLoginDto request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.IdToken)) return null;
        if (_options.AllowedAudiences.Count == 0)
        {
            _logger.LogError("GoogleAuth:AllowedAudiences is empty; refusing to verify token.");
            return null;
        }

        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = _options.AllowedAudiences,
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);
            return new ExternalIdentity(
                Provider: ProviderName,
                Subject: payload.Subject,
                Email: payload.Email,
                EmailVerified: payload.EmailVerified,
                GivenName: payload.GivenName,
                FamilyName: payload.FamilyName);
        }
        catch (InvalidJwtException ex)
        {
            _logger.LogWarning(ex, "Google ID token validation failed.");
            return null;
        }
    }
}
